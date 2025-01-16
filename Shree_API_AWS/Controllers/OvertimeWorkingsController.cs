using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;

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
            return Ok(_mapper.Map<IEnumerable<OvertimeWorking_DTO>>(await _context.Overtimeworkings.ToListAsync()));
        }
    }
}
