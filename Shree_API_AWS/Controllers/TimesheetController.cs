using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimesheetController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private IMapper _mapper { get; set; }
        public JObject TimesheetDetails { get; set; }

        public TimesheetController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Timesheet
        [HttpGet("Employee")]
        public async Task<ActionResult<IEnumerable<TimesheetEmployee_DTO>>> GetEmployeeTimesheets()
        {
            var timesheet = await _context.TimesheetdetailsEmployees.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TimesheetEmployee_DTO>>(timesheet));
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<TimesheetAdmin_DTO>>> GetAdminTimesheetTable()
        {
            var timesheet = await _context.TimesheetdetailsAdmins.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TimesheetAdmin_DTO>>(timesheet));
        }

        // GET: api/Timesheet/5
        [HttpGet("Employee/{id}")]
        [Authorize(Roles = "user,admin")]
        public async Task<ActionResult<TimesheetEmployee_DTO>> GetTimesheet(int id)
        {
            var timesheet = await _context.TimesheetdetailsEmployees.FindAsync(id);

            if (timesheet == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TimesheetEmployee_DTO>(timesheet));
        }

        [HttpGet("Admin/{date}")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<TimesheetAdmin_DTO>>> GetAdminTimesheetView(string date)
        {
            // Parse the input date string to a DateTime object (no time component required)
            DateTime parsedSelectedDateTime;
            bool isDateParsed = DateTime.TryParse(date, out parsedSelectedDateTime);

            if (!isDateParsed)
            {
                // Return a bad request if the date format is invalid.
                return BadRequest("Invalid date format.");
            }

            // First, retrieve the data from the database (without complex operations)
            var timesheetEmployeeList = await _context.TimesheetdetailsEmployees.ToListAsync();

            // Now, filter the list in memory
            var timesheetEmployee = timesheetEmployeeList
                .Where(x =>
                {
                    var timesheetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Timesheet>(x?.Timesheetdetails);
                    if (timesheetData == null) return false; // Skip if deserialization fails

                    // Parse the TimeEntry to DateTime
                    DateTime parsedDateTime;
                    bool isParsed = DateTime.TryParseExact(timesheetData?.TimeEntry, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime);

                    // Filter based on the date part of TimeEntry matching the selected date
                    return isParsed && parsedDateTime.Date == parsedSelectedDateTime.Date;
                })
                .ToList();
            var timesheetEmpList = _mapper.Map<IEnumerable<TimesheetEmployee_DTO>>(timesheetEmployee);

            var timesheetAdmin = await _context.TimesheetdetailsAdmins.ToListAsync();
            var timesheetList = _mapper.Map<IEnumerable<TimesheetAdmin_DTO>>(timesheetAdmin);

            var filteredTimesheetAdminList = timesheetList
            .Where(admin =>
            {
                // Find matching EmployeeTimesheet
                var matchingEmployeeTimesheet = timesheetEmpList
                    .Where(employee => employee.Id == admin.TimesheetDetailId) // Filter matching employee timesheets
                    .FirstOrDefault();

                if (matchingEmployeeTimesheet != null)
                {
                    admin.EmployeeTimesheet = matchingEmployeeTimesheet; // Assign the matching list
                    return true;
                }
                else
                {
                    return false;
                }
            })
            .ToList();

            if (timesheetAdmin == null)
            {
                return NotFound();
            }

            return Ok(filteredTimesheetAdminList);
        }

        // POST: api/Timesheet
        [HttpPost("Employee")]
        [Authorize(Roles = "guest,user,admin")]
        public async Task<ActionResult<IEnumerable<AlertNotification_DTO>>> CreateTimesheet(TimesheetEmployee_DTO timesheet)
        {
            var employee = await _context.Employees.Where(x => x.Employeeid == timesheet.EmployeeId).FirstOrDefaultAsync();

            var timesheetDetails = new TimesheetdetailsEmployee()
            {
                Employeeid = timesheet.EmployeeId,
                Dataenteredby = employee?.Firstname + " " + employee?.Lastname,
                Dataenteredon = DateTime.Now,
                Timesheetdetails = timesheet.TimesheetDetails
            };

            try
            {
                

                _context.TimesheetdetailsEmployees.Add(timesheetDetails);
                await _context.SaveChangesAsync();

                int timesheetID = _context.TimesheetdetailsEmployees.OrderByDescending(x => x.Dataenteredon).FirstOrDefault()?.Id ?? 1;

                var timesheetAdminDetails = new TimesheetdetailsAdmin()
                {
                    Employeeid = timesheet.EmployeeId,
                    Dataenteredby = employee?.Firstname + " " + employee?.Lastname,
                    Dataenteredon = DateTime.Now,
                    Timesheetdetailid = timesheetID
                };

                _context.TimesheetdetailsAdmins.Add(timesheetAdminDetails);
                await _context.SaveChangesAsync();

                var timesheetData = Newtonsoft.Json.JsonConvert.DeserializeObject<Timesheet>(timesheet.TimesheetDetails);

                // Parse the string to DateTime
                DateTime parsedDateTime;
                DateTime.TryParseExact(timesheetData?.TimeEntry, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime);

                var employeeNotification = new AlertNotification()
                {
                    Employeeid = timesheet.EmployeeId,
                    Dataenteredby = employee?.Firstname + " " + employee?.Lastname,
                    Dataenteredon = DateTime.Now,
                    Notificationmessage = $"Timesheet for #{parsedDateTime.ToString("dd-MM-yyyy")}# for the site #{timesheetData?.SiteName}# at #{timesheetData?.SiteLocation}# has been submitted successfully and is sent for approval.",
                    Isadminnotification = false,
                    Isemployeenotification = true,
                    Isapproved = false,
                    Issentback = false,
                    Isnotificationactive = true,
                    TimesheetId = timesheetID
                };

                _context.AlertNotifications.Add(employeeNotification);
                await _context.SaveChangesAsync();

                var adminNotification = new AlertNotification()
                {
                    Employeeid = timesheet.EmployeeId,
                    Dataenteredby = employee?.Firstname + " " + employee?.Lastname,
                    Dataenteredon = DateTime.Now,
                    Notificationmessage = $"Timesheet for #{parsedDateTime.ToString("dd-MM-yyyy")}# for the site #{timesheetData?.SiteName}# at #{timesheetData?.SiteLocation}# has been submitted by {employee?.Firstname + " " + employee?.Lastname}. Please submit your response.",
                    Isadminnotification = true,
                    Isemployeenotification = false,
                    Isapproved = false,
                    Issentback = false,
                    Isnotificationactive = true,
                    TimesheetId = timesheetID
                };

                _context.AlertNotifications.Add(adminNotification);
                await _context.SaveChangesAsync();

                var alerts = await _context.AlertNotifications.Where(x => x.Employeeid == timesheet.EmployeeId).ToListAsync();
                var mappedAlerts = _mapper.Map<IEnumerable<AlertNotification_DTO>>(alerts);
                return Ok(mappedAlerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred: " + ex.Message });
            }
        }

        // PUT: api/Timesheet/5
        [HttpPut]
        public async Task<IActionResult> UpdateTimesheet(TimesheetEmployee_DTO timesheet)
        {
            var employee = await _context.Employees.Where(x => x.Employeeid == timesheet.EmployeeId).FirstOrDefaultAsync();

            var timesheetDetails = new TimesheetdetailsEmployee()
            {
                Id = timesheet.Id,
                Employeeid = timesheet.EmployeeId,
                Dataenteredby = employee?.Firstname + " " + employee?.Lastname,
                Dataenteredon = DateTime.Now,
                Timesheetdetails = timesheet.TimesheetDetails
            };

            _context.Entry(timesheet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimesheetExists(timesheet.EmployeeId))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Timesheet/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimesheet(string id)
        {
            var timesheet = await _context.TimesheetdetailsEmployees.Where(x => x.Employeeid == id).FirstOrDefaultAsync();
            if (timesheet == null)
            {
                return NotFound();
            }

            _context.TimesheetdetailsEmployees.Remove(timesheet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TimesheetExists(string id)
        {
            return _context.TimesheetdetailsEmployees.Any(e => e.Employeeid == id);
        }
    }
}
