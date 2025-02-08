using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OvertimeWorkingsController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private IMapper _mapper { get; set; }
        public OvertimeWorkingsController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("OTDetails")]
        public async Task<ActionResult<IEnumerable<OvertimeWorking_DTO>>> GetEmployeeOTDetails()
        {
            try
            {
                // Format the current date as "MMMM yyyy" for comparison
                var currentMonthYear = DateTime.Now.ToString("MMMM yyyy");

                // Filter records where Entryfor matches the current month and year
                var data = await _context.Overtimeworkings
                    .Where(x => x.Entryfor == currentMonthYear)
                    .OrderBy(x => x.Employeeid)
                    .ThenBy(x => x.Dateofotworking)
                    .ToListAsync();

                // Map the data to DTOs and return the result
                return Ok(_mapper.Map<IEnumerable<OvertimeWorking_DTO>>(data));
            }
            catch (Exception ex)
            {
                // Return a 500 status code with the error message
                return Ok(ex.Message);
            }
        }

        [HttpGet("OTDetails/{empID}")]
        public async Task<ActionResult<IEnumerable<OvertimeWorking_DTO>>> GetEmployeeOTDetails(string empID)
        {
            try
            {
                // Format the current date as "MMMM yyyy" for comparison
                var currentMonthYear = DateTime.Now.ToString("MMMM yyyy");

                // Filter records where Entryfor matches the current month and year
                var data = await _context.Overtimeworkings
                    .Where(x => x.Employeeid == empID)
                    .OrderBy(x => x.Employeeid)
                    .ThenBy(x => x.Dateofotworking)
                    .ToListAsync();

                // Map the data to DTOs and return the result
                return Ok(_mapper.Map<IEnumerable<OvertimeWorking_DTO>>(data));
            }
            catch (Exception ex)
            {
                // Return a 500 status code with the error message
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostEmployeeOTDetail(OvertimeWorking_DTO overtimeWorking)
        {
            try
            {
                var existingOTRecord = await _context.Overtimeworkings.Where(x => x.Id == overtimeWorking.Id && x.Dateofotworking == overtimeWorking.DateOfOTWorking).FirstOrDefaultAsync();

                if (existingOTRecord == null)
                {
                    overtimeWorking.DataEnteredOn = DateTime.UtcNow.ToLocalTime();
                    var mappedOTDetails = _mapper.Map<Overtimeworking>(overtimeWorking);
                    _context.Overtimeworkings.Add(mappedOTDetails);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    existingOTRecord.Othoursworked = overtimeWorking.OTHoursWorked;
                    await _context.SaveChangesAsync();
                }
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return Ok("Error :- " + ex.Message);
            }
            
        }
    }
}
