using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.Models;
using System.Data;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeAttendancesController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;

        public EmployeeAttendancesController(ShreeDbContext_Postgres context)
        {
            _context = context;
        }

        // GET: api/EmployeeAttendances
        [HttpGet]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<Employeeattendance>>> GetEmployeeAttendances()
        {
            return Ok(await _context.Employeeattendances.ToListAsync());
        }

        // GET: api/EmployeeAttendances/5
        [HttpGet("{empId}")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<Employeeattendance>> GetEmployeeAttendance(string empId)
        {
            var employeeAttendance = await _context.Employeeattendances
                .Where(x => x.Employeeid == empId).FirstOrDefaultAsync();

            if (employeeAttendance == null)
            {
                return NotFound();
            }

            return Ok(employeeAttendance);
        }

        // PUT: api/EmployeeAttendances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{empId}")]
        public async Task<IActionResult> PutEmployeeAttendance(string empId, Employeeattendance employeeAttendance)
        {
            var existingRecord = await _context.Employeeattendances
                .FirstOrDefaultAsync(ow => ow.Employeeid == employeeAttendance.Employeeid);

            if (existingRecord == null)
            {
                return NotFound();
            }

            try
            {
                // Create DataTable
                DataTable attendanceTable = new DataTable();
                // Define columns according to the structure
                attendanceTable.Columns.Add("Employeeid", typeof(string));
                attendanceTable.Columns.Add("Entryformonth", typeof(string));
                attendanceTable.Columns.Add("DataEnteredOn", typeof(DateTime));
                attendanceTable.Columns.Add("DataEnteredBy", typeof(string));
                attendanceTable.Columns.Add("IsPresent", typeof(bool));
                attendanceTable.Columns.Add("LastPresentDate", typeof(DateTime));
                attendanceTable.Columns.Add("TotalDaysPresent", typeof(int));
                attendanceTable.Columns.Add("IsAbsent", typeof(bool));
                attendanceTable.Columns.Add("LastAbsentDate", typeof(DateTime));
                attendanceTable.Columns.Add("TotalDaysAbsent", typeof(int));
                attendanceTable.Columns.Add("IsPaidLeave", typeof(bool));
                attendanceTable.Columns.Add("LastPaidLeaveDate", typeof(DateTime));
                attendanceTable.Columns.Add("IsLate", typeof(bool));
                attendanceTable.Columns.Add("LastLateDay", typeof(DateTime));
                attendanceTable.Columns.Add("TotalDaysLateDay", typeof(int));
                attendanceTable.Columns.Add("IsHalfDay", typeof(bool));
                attendanceTable.Columns.Add("LastHalfDayDate", typeof(DateTime));
                attendanceTable.Columns.Add("TotalDaysHalfDays", typeof(int));

                // Populate DataTable with values
                attendanceTable.Rows.Add(employeeAttendance);

                // Create parameters for the stored procedure
                var actionParameter = new SqlParameter
                {
                    ParameterName = "@Action",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50, // Set size if needed
                    Value = "UPDATE"
                };
                var EmpAttndparameter = new SqlParameter
                {
                    ParameterName = "@EmployeeAttendance",
                    SqlDbType = SqlDbType.Structured,
                    Value = attendanceTable,
                    TypeName = "EmployeeAttendanceType" // Make sure this matches your created type
                };

                // Execute the stored procedure
                await _context.Database.ExecuteSqlRawAsync("SELECT usp_set_attendance_records(@Action, @EmployeeAttendance)", actionParameter, EmpAttndparameter);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAttendanceExists(empId))
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

        // POST: api/EmployeeAttendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employeeattendance>> PostEmployeeAttendance(Employeeattendance employeeAttendance)
        {
            // Check if the record already exists
            var existingRecord = await _context.Employeeattendances
                .FirstOrDefaultAsync(ow => ow.Employeeid == employeeAttendance.Employeeid && ow.Entryformonth == employeeAttendance.Entryformonth);

            if (existingRecord != null)
            {
                return Conflict("Data already exists for the specified Employeeid and Entryformonth.");
            }

            // Create DataTable
            DataTable attendanceTable = new DataTable();
            // Define columns according to the structure
            attendanceTable.Columns.Add("Employeeid", typeof(string));           
            attendanceTable.Columns.Add("Entryformonth", typeof(string));        
            attendanceTable.Columns.Add("DataEnteredOn", typeof(DateTime));      
            attendanceTable.Columns.Add("DataEnteredBy", typeof(string));         
            attendanceTable.Columns.Add("IsPresent", typeof(bool));              
            attendanceTable.Columns.Add("LastPresentDate", typeof(DateTime));     
            attendanceTable.Columns.Add("TotalDaysPresent", typeof(int));        
            attendanceTable.Columns.Add("IsAbsent", typeof(bool));                
            attendanceTable.Columns.Add("LastAbsentDate", typeof(DateTime));      
            attendanceTable.Columns.Add("TotalDaysAbsent", typeof(int));         
            attendanceTable.Columns.Add("IsPaidLeave", typeof(bool));             
            attendanceTable.Columns.Add("LastPaidLeaveDate", typeof(DateTime));   
            attendanceTable.Columns.Add("IsLate", typeof(bool));                  
            attendanceTable.Columns.Add("LastLateDay", typeof(DateTime));         
            attendanceTable.Columns.Add("TotalDaysLateDay", typeof(int));        
            attendanceTable.Columns.Add("IsHalfDay", typeof(bool));               
            attendanceTable.Columns.Add("LastHalfDayDate", typeof(DateTime));    
            attendanceTable.Columns.Add("TotalDaysHalfDays", typeof(int));       

            // Populate DataTable with values
            attendanceTable.Rows.Add(employeeAttendance);

            // Create parameters for the stored procedure
            var actionParameter = new SqlParameter
            {
                ParameterName = "@Action",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50, // Set size if needed
                Value = "INSERT"
            };
            var EmpAttndparameter = new SqlParameter
            {
                ParameterName = "@EmployeeAttendance",
                SqlDbType = SqlDbType.Structured,
                Value = attendanceTable,
                TypeName = "EmployeeAttendanceType" // Make sure this matches your created type
            };

            // Execute the stored procedure
            await _context.Database.ExecuteSqlRawAsync("SELECT usp_set_attendance_records(@Action, @EmployeeAttendance)", actionParameter, EmpAttndparameter);
            return CreatedAtAction("GetEmployeeAttendance", new { empId = employeeAttendance.Employeeid }, employeeAttendance);
        }

        // DELETE: api/EmployeeAttendances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAttendance(int id)
        {
            var employeeAttendance = await _context.Employeeattendances.FindAsync(id);
            if (employeeAttendance == null)
            {
                return NotFound();
            }

            _context.Employeeattendances.Remove(employeeAttendance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeAttendanceExists(string id)
        {
            return _context.Employeeattendances.Any(e => e.Employeeid == id);
        }
    }
}
