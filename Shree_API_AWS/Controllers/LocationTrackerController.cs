using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationTrackerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ShreeDbContext_Postgres _context;
        public LocationTrackerController(IMapper mapper, ShreeDbContext_Postgres shreeDbContext)
        {
            _mapper = mapper;
            _context = shreeDbContext;

        }
        // GET: api/<LocationTrackerController>
        [HttpGet]
        public async Task<IEnumerable<LocationTracker_DTO>> GetLocationsOfAllEmployee()
        {
            return _mapper.Map<IEnumerable<LocationTracker_DTO>>(await _context.Locationtrackers.ToListAsync());
        }

        // POST api/<LocationTrackerController>
        [HttpPost]
        public async Task<IActionResult> PostLocationRealTime([FromBody] LocationTracker_DTO trackedvalue)
        {
            var existingRecord = _context.Locationtrackers
            .Where(x => x.Employeeid == trackedvalue.Employeeid && DateOnly.FromDateTime(x.Dateenteredon) == DateOnly.FromDateTime(DateTime.UtcNow))
            .FirstOrDefault();

            var employee = _context.Employees.Where(x => x.Employeeid == trackedvalue.Employeeid).FirstOrDefault();

            if (existingRecord != null)
            {
                // Detach the existing entity to avoid conflicts
                _context.Entry(existingRecord).State = EntityState.Detached;

                List<LocationLog> log = JsonConvert.DeserializeObject<List<LocationLog>>(trackedvalue.Locationlog);
                if(log != null)
                {
                   List<LocationLog> existingLog = JsonConvert.DeserializeObject<List<LocationLog>>(existingRecord.Locationlog);
                    existingLog.Add(log[0]);
                   trackedvalue.Locationlog = JsonConvert.SerializeObject(existingLog);
                   trackedvalue.Id = existingRecord.Id;
                   var trackedValue = new Locationtracker()
                   {
                        Dateenteredon = DateTime.UtcNow,
                        Dateenteredby = employee.Firstname + " " + employee.Lastname,
                        Employeeid = trackedvalue.Employeeid,
                        Id = trackedvalue.Id,
                        Isactive = trackedvalue.Isactive,
                        Locationlog = trackedvalue.Locationlog
                   };
                    _context.Entry(trackedValue).State = EntityState.Modified;
                }
            }
            else
            {
                var trackedValue = new Locationtracker()
                {
                    Dateenteredon = DateTime.UtcNow,
                    Dateenteredby = employee.Firstname + " " + employee.Lastname,
                    Employeeid = trackedvalue.Employeeid,
                    Id = trackedvalue.Id,
                    Isactive = trackedvalue.Isactive,
                    Locationlog = trackedvalue.Locationlog
                };
                _context.Locationtrackers.Add(trackedValue);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
