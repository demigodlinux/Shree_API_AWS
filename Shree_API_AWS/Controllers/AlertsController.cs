using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlertsController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private IMapper _mapper { get; set; }

        public AlertsController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<AlertsController>
        [HttpGet]
        public async Task<IEnumerable<AlertNotification_DTO>> Get()
        {
            return _mapper.Map<IEnumerable<AlertNotification_DTO>>(await _context.AlertNotifications.ToListAsync());
        }

        // GET api/<AlertsController>/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<AlertNotification_DTO>> Get(string id)
        {
            return _mapper.Map<IEnumerable<AlertNotification_DTO>>(await _context.AlertNotifications.Where(x => x.Employeeid == id).ToListAsync());
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<AlertNotification_DTO>>> GetAlertsForAdmin()
        {
            var alertData = await _context.AlertNotifications.Where(x => x.Isadminnotification == true && x.Isnotificationactive == true).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<AlertNotification_DTO>>(alertData));
        }

    }
}
