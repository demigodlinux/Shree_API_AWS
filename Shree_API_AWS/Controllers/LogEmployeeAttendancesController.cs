using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogEmployeeAttendancesController : ControllerBase
    {
        private readonly ShreedbContext _context;

        public LogEmployeeAttendancesController(ShreedbContext context)
        {
            _context = context;
        }

        // GET: api/LogEmployeeAttendances
        [HttpGet]
        [HttpGet]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<LogEmployeeAttendance>>> GetLogEmployeeAttendances()
        {
            return Ok(await _context.LogEmployeeAttendances.ToListAsync());
        }

        // GET: api/LogEmployeeAttendances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogEmployeeAttendance>> GetLogEmployeeAttendance(int id)
        {
            var logEmployeeAttendance = await _context.LogEmployeeAttendances.FindAsync(id);

            if (logEmployeeAttendance == null)
            {
                return NotFound();
            }

            return logEmployeeAttendance;
        }

        // PUT: api/LogEmployeeAttendances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogEmployeeAttendance(int id, LogEmployeeAttendance logEmployeeAttendance)
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

        // POST: api/LogEmployeeAttendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogEmployeeAttendance>> PostLogEmployeeAttendance(LogEmployeeAttendance logEmployeeAttendance)
        {
            _context.LogEmployeeAttendances.Add(logEmployeeAttendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogEmployeeAttendance", new { id = logEmployeeAttendance.Id }, logEmployeeAttendance);
        }

        // DELETE: api/LogEmployeeAttendances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogEmployeeAttendance(int id)
        {
            var logEmployeeAttendance = await _context.LogEmployeeAttendances.FindAsync(id);
            if (logEmployeeAttendance == null)
            {
                return NotFound();
            }

            _context.LogEmployeeAttendances.Remove(logEmployeeAttendance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogEmployeeAttendanceExists(int id)
        {
            return _context.LogEmployeeAttendances.Any(e => e.Id == id);
        }
    }
}
