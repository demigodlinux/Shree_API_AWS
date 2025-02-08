using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogEmployeeAttendancesController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private readonly IMapper _mapper;
        public LogEmployeeAttendancesController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LogEmployeeattendances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogEmployeeattendance>>> GetLogEmployeeAttendances()
        {
            return Ok(await _context.LogEmployeeattendances.ToListAsync());
        }

        // GET: api/LogEmployeeattendances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<LogEmployeeAttendance_DTO>>> GetLogEmployeeAttendance(string id)
        {
            var logEmployeeAttendance = await _context.LogEmployeeattendances.Where(x => x.Employeeid == id && x.Checkintiming != null).ToListAsync();

            if (logEmployeeAttendance == null)
            {
                return Ok(null);
            }

            return Ok(_mapper.Map<IEnumerable<LogEmployeeAttendance_DTO>>(logEmployeeAttendance));
        }

        // PUT: api/LogEmployeeattendances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogEmployeeAttendance(int id, LogEmployeeattendance logEmployeeAttendance)
        {
            if (id != logEmployeeAttendance.Id)
            {
                return BadRequest();
            }

            _context.Entry(logEmployeeAttendance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogEmployeeAttendanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostLogEmployeeAttendance(UserCheckin userData)
        {
            try
            {
                string currentMonth = DateTime.UtcNow.ToString("MMMM yyyy");

                // Convert user timestamp to Unspecified kind for database compatibility
                var userDateOnly = DateOnly.FromDateTime(DateTime.SpecifyKind(userData.Timestamp, DateTimeKind.Unspecified));

                // Check if the employee has already logged attendance for today
                var logEmployeeAttendance = await _context.LogEmployeeattendances
                    .Where(x => x.Employeeid == userData.EmployeeId &&
                                x.Entryformonth == currentMonth)
                    .ToListAsync();

                if (logEmployeeAttendance.Any(x => DateOnly.FromDateTime(x.Dataenteredon) == userDateOnly))
                {
                    // Update checkout timing for existing log
                    return await UpdateCheckoutTiming(logEmployeeAttendance, userDateOnly, userData);
                }

                // Determine if the check-in is late
                bool isLateCheck = userData.Timestamp.ToUniversalTime().TimeOfDay > new TimeSpan(9, 30, 0);

                // Retrieve employee data
                var employeeData = await _context.Employees
                    .FirstOrDefaultAsync(x => x.Employeeid == userData.EmployeeId);

                if (employeeData == null)
                    return BadRequest("Employee not found");

                // Handle attendance update or creation
                var employeeAttendance = await GetOrCreateAttendance(userData, employeeData, isLateCheck, logEmployeeAttendance);

                // Log the employee's attendance for today
                await LogAttendance(userData, employeeData, employeeAttendance, isLateCheck);

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return Ok("Failed: " + ex.Message); // Include error message for debugging
            }
        }

        // Method to update checkout timing
        private async Task<ActionResult<string>> UpdateCheckoutTiming(List<LogEmployeeattendance> logEmployeeAttendance, DateOnly userDateOnly, UserCheckin userData)
        {
            var logData = logEmployeeAttendance.FirstOrDefault(x => DateOnly.FromDateTime(x.Dataenteredon) == userDateOnly);
            if (logData != null)
            {
                // Ensure DateTimeKind is Unspecified
                //logData.Checkouttiming = DateTime.SpecifyKind(userData.Timestamp, DateTimeKind.Unspecified);

                //_context.Entry(logData).State = EntityState.Modified;
                //await _context.SaveChangesAsync();
            }
            return Ok("Checkout updated");
        }


        private async Task<Employeeattendance> GetOrCreateAttendance(UserCheckin userData, Employee employeeData, bool isLateCheck, List<LogEmployeeattendance> logEmployeeAttendance)
        {
            string currentMonth = DateTime.Now.ToString("yyyy-MM");

            var employeeAttendance = await _context.Employeeattendances
                .FirstOrDefaultAsync(x => x.Employeeid == userData.EmployeeId && x.Entryformonth == currentMonth);

            if (employeeAttendance == null)
            {
                employeeAttendance = new Employeeattendance
                {
                    Employeeid = userData.EmployeeId,
                    Dataenteredby = $"{employeeData.Firstname} {employeeData.Lastname}",
                    Dataenteredon = DateTime.UtcNow, // Ensure UTC time is used
                    Entryformonth = currentMonth,
                    Isabsent = false,
                    Islate = isLateCheck,
                    Lastlateday = isLateCheck ? userData.Timestamp.ToUniversalTime() : null, // Convert to UTC if necessary
                    Ispresent = true,
                    Lastpresentdate = userData.Timestamp.ToUniversalTime(), // Convert to UTC
                    Totaldayspresent = 1,
                    Totaldayslateday = isLateCheck ? 1 : 0
                };
                _context.Employeeattendances.Add(employeeAttendance);
            }
            else
            {
                employeeAttendance.Islate = isLateCheck;
                employeeAttendance.Lastlateday = isLateCheck ? userData.Timestamp.ToUniversalTime() : employeeAttendance.Lastlateday;
                employeeAttendance.Lastpresentdate = userData.Timestamp.ToUniversalTime();
                employeeAttendance.Totaldayspresent++;
                employeeAttendance.Totaldayslateday += isLateCheck ? 1 : 0;
                _context.Entry(employeeAttendance).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return employeeAttendance;
        }



        private async Task LogAttendance(UserCheckin userData, Employee employeeData, Employeeattendance employeeAttendance, bool isLateCheck)
        {
            //var timestampUnspecified = DateTime.SpecifyKind(userData.Timestamp, DateTimeKind.Unspecified);

            //var loggedAttendance = new LogEmployeeattendance
            //{
            //    Employeeid = userData.EmployeeId,
            //    Entryformonth = DateTime.UtcNow.ToString("yyyy-MM"),
            //    Employeeattendid = employeeAttendance.Id,
            //    Attendancedate = userData.Timestamp.ToUniversalTime(), // Ensure Unspecified kind
            //    Checkintiming = userData.IsCheckedIn ? userData.Timestamp.ToUniversalTime() : null,
            //    Checkouttiming = userData.IsCheckedOut ? userData.Timestamp.ToUniversalTime() : null,
            //    Dataenteredby = $"{employeeData.Firstname} {employeeData.Lastname}",
            //    Dataenteredon = DateTime.UtcNow, // Ensure Unspecified kind
            //    Islate = isLateCheck,
            //    Ispresent = true
            //};
            //_context.LogEmployeeattendances.Add(loggedAttendance);
            //await _context.SaveChangesAsync();
        }



        // DELETE: api/LogEmployeeattendances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogEmployeeAttendance(int id)
        {
            var logEmployeeAttendance = await _context.LogEmployeeattendances.FindAsync(id);
            if (logEmployeeAttendance == null)
            {
                return NotFound();
            }

            _context.LogEmployeeattendances.Remove(logEmployeeAttendance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogEmployeeAttendanceExists(int id)
        {
            return _context.LogEmployeeattendances.Any(e => e.Id == id);
        }
    }
}
