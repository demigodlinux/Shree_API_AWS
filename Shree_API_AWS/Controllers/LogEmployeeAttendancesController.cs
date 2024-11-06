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
        private readonly ShreeDbContext_Postgres _context;

        public LogEmployeeAttendancesController(ShreeDbContext_Postgres context)
        {
            _context = context;
        }

        // GET: api/LogEmployeeattendances
        [HttpGet]
        [HttpGet]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<LogEmployeeattendance>>> GetLogEmployeeAttendances()
        {
            return Ok(await _context.LogEmployeeattendances.ToListAsync());
        }

        // GET: api/LogEmployeeattendances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogEmployeeattendance>> GetLogEmployeeAttendance(int id)
        {
            var logEmployeeAttendance = await _context.LogEmployeeattendances.FindAsync(id);

            if (logEmployeeAttendance == null)
            {
                return NotFound();
            }

            return logEmployeeAttendance;
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

        // POST: api/LogEmployeeattendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogEmployeeattendance>> PostLogEmployeeAttendance(LogEmployeeattendance logEmployeeAttendance)
        {
            _context.LogEmployeeattendances.Add(logEmployeeAttendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogEmployeeAttendance", new { id = logEmployeeAttendance.Id }, logEmployeeAttendance);
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
