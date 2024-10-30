using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Shree_API_AWS.Context;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OvertimeWorkingsController : ControllerBase
    {
        private readonly MasterContext _context;

        public OvertimeWorkingsController(MasterContext context)
        {
            _context = context;
        }

        // GET: api/OvertimeWorkings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OvertimeWorking>>> GetOvertimeWorkings()
        {
            return await _context.OvertimeWorkings.ToListAsync();
        }

        // GET: api/OvertimeWorkings/5
        [HttpGet("{empId}")]
        public async Task<ActionResult<OvertimeWorking>> GetOvertimeWorking(string empId)
        {
            var overtimeWorking = await _context.OvertimeWorkings
                .Where(ow => ow.EmployeeId == empId).FirstOrDefaultAsync();

            if (overtimeWorking == null)
            {
                return NotFound();
            }

            return overtimeWorking;
        }

        // PUT: api/OvertimeWorkings/{empId}
        [HttpPut("{empId}/{entryForMonth}")]
        public async Task<IActionResult> PutOvertimeWorking(string empId, string entryForMonth, OvertimeWorking overtimeWorking)
        {
            // Set the entity state to modified
            _context.Entry(overtimeWorking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OvertimeWorkingExists(empId))
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


        // POST: api/OvertimeWorkings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OvertimeWorking>> PostOvertimeWorking(OvertimeWorking overtimeWorking)
        {
            // Check if the record already exists
            var existingRecord = await _context.OvertimeWorkings
                .FirstOrDefaultAsync(ow => ow.EmployeeId == overtimeWorking.EmployeeId && ow.EntryForMonth == overtimeWorking.EntryForMonth);

            if (existingRecord != null)
            {
                return Conflict("Data already exists for the specified EmployeeId and EntryForMonth.");
            }

            _context.OvertimeWorkings.Add(overtimeWorking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOvertimeWorking", new { empId = overtimeWorking.EmployeeId }, overtimeWorking);
        }

        // DELETE: api/OvertimeWorkings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOvertimeWorking(int id)
        {
            var overtimeWorking = await _context.OvertimeWorkings.FindAsync(id);
            if (overtimeWorking == null)
            {
                return NotFound();
            }

            _context.OvertimeWorkings.Remove(overtimeWorking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OvertimeWorkingExists(string id)
        {
            return _context.OvertimeWorkings.Any(e => e.EmployeeId == id);
        }
    }
}
