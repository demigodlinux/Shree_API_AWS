using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class PublicHolidayController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private readonly IMapper _mapper;
        public PublicHolidayController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/LogEmployeeattendances
        [HttpGet]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<IndianPublicHolidays_DTO>>> GetPublicHolidays()
        {
            return Ok(_mapper.Map<IEnumerable<IndianPublicHolidays_DTO>>(await _context.Indianholidays2025s.ToListAsync()));
        }
    }
}
