using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly MasterContext _context;
        private IMapper _mapper { get; set; }

        public EmployeesController(MasterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        [Authorize(Roles = "guest,user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<Employee_DTO>>> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<Employee_DTO>>(employees));
        }

        // GET: api/Employees/5
        [HttpGet("{empId}")]
        [Authorize(Roles = "guest,user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<Employee_DTO>> GetEmployee(string empId)
        {
            var employee = await _context.Employees.Where(x => x.EmployeeId == empId).FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return _mapper.Map<Employee_DTO>(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{empId}")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<IActionResult> PutEmployee(string empId, [FromBody] Employee_DTO employee)
        {
            var employeeDetail = new Employee()
            {
                Id = employee.Id,
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                MobileNumber = employee.MobileNumber,
                Ecname = employee.Ecname,
                Ecnumber = employee.Ecnumber,
                Position = employee.Position,
                HireDate = employee.HireDate,
                NativePlace = employee.NativePlace,
                Salary = employee.Salary,
                Basic = employee.Basic,
                Hra = employee.Hra,
                SpecialAllowance = employee.SpecialAllowance,
                IsActive = employee.IsActive,
            };

            _context.Entry(employeeDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(empId))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<Employee_DTO>> PostEmployee(Employee_DTO employee)
        {
            // Check if the record already exists
            var existingRecord = await _context.Employees
                .FirstOrDefaultAsync(ow => ow.EmployeeId == employee.EmployeeId);

            if (existingRecord != null)
            {
                return Conflict("Data already exists for the specified EmployeeId and EntryForMonth.");
            }

            var employeeDetail = new Employee()
            {   
                Id = employee.Id,
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                MobileNumber = employee.MobileNumber,
                Ecname = employee.Ecname,
                Ecnumber = employee.Ecnumber,
                Position = employee.Position,
                HireDate = employee.HireDate,
                NativePlace = employee.NativePlace,
                Salary = employee.Salary,
                Basic = employee.Basic,
                Hra = employee.Hra,
                SpecialAllowance = employee.SpecialAllowance,
                IsActive = employee.IsActive,
            };

            _context.Employees.Add(employeeDetail);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(ow => ow.EmployeeId == id);

            if (employee == null)
            {
                return NoContent();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
