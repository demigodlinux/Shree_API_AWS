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
        private readonly ShreeDbContext_Postgres _context;

        public OvertimeWorkingsController(ShreeDbContext_Postgres context)
        {
            _context = context;
        }

        // GET: api/Overtimeworkings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Overtimeworking>>> GetOvertimeWorkings()
        {
            return await _context.Overtimeworkings.ToListAsync();
        }

        // GET: api/Overtimeworkings/5
        [HttpGet("{empId}")]
        public async Task<ActionResult<Overtimeworking>> GetOvertimeWorking(string empId)
        {
            var overtimeWorking = await _context.Overtimeworkings
                .Where(ow => ow.Employeeid == empId).FirstOrDefaultAsync();

            if (overtimeWorking == null)
            {
                return NotFound();
            }

            return overtimeWorking;
        }

        // PUT: api/Overtimeworkings/{empId}
        [HttpPut("{empId}/{entryForMonth}")]
        public async Task<IActionResult> PutOvertimeWorking(string empId, string entryForMonth, Overtimeworking overtimeWorking)
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


        // POST: api/Overtimeworkings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Overtimeworking>> PostOvertimeWorking(Overtimeworking overtimeWorking)
        {
            // Check if the record already exists
            var existingRecord = await _context.Overtimeworkings
                .FirstOrDefaultAsync(ow => ow.Employeeid == overtimeWorking.Employeeid && ow.Entryformonth == overtimeWorking.Entryformonth);

            if (existingRecord != null)
            {
                return Conflict("Data already exists for the specified Employeeid and EntryForMonth.");
            }

            _context.Overtimeworkings.Add(overtimeWorking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOvertimeWorking", new { empId = overtimeWorking.Employeeid }, overtimeWorking);
        }

        // DELETE: api/Overtimeworkings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOvertimeWorking(int id)
        {
            var overtimeWorking = await _context.Overtimeworkings.FindAsync(id);
            if (overtimeWorking == null)
            {
                return NotFound();
            }

            _context.Overtimeworkings.Remove(overtimeWorking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OvertimeWorkingExists(string id)
        {
            return _context.Overtimeworkings.Any(e => e.Employeeid == id);
        }
    }
}
