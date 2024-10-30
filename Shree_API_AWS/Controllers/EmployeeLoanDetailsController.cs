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
    public class EmployeeLoanDetailsController : ControllerBase
    {
        private readonly MasterContext _context;

        public EmployeeLoanDetailsController(MasterContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeLoanDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeLoanDetail>>> GetEmployeeLoanDetails()
        {
            return await _context.EmployeeLoanDetails.ToListAsync();
        }

        // GET: api/EmployeeLoanDetails/5
        [HttpGet("{empId}")]
        public async Task<ActionResult<EmployeeLoanDetail>> GetEmployeeLoanDetail(string empId)
        {
            var employeeLoanDetail = await _context.EmployeeLoanDetails.Where(x => x.EmployeeId == empId).FirstOrDefaultAsync();

            if (employeeLoanDetail == null)
            {
                return NotFound();
            }

            return employeeLoanDetail;
        }

        // PUT: api/EmployeeLoanDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{empId}")]
        public async Task<IActionResult> PutEmployeeLoanDetail(string empId, EmployeeLoanDetail employeeLoanDetail)
        {
            _context.Entry(employeeLoanDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeLoanDetailExists(empId))
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

        // POST: api/EmployeeLoanDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeLoanDetail>> PostEmployeeLoanDetail(EmployeeLoanDetail employeeLoanDetail)
        {
            _context.EmployeeLoanDetails.Add(employeeLoanDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeLoanDetail", new { id = employeeLoanDetail.Id }, employeeLoanDetail);
        }

        // DELETE: api/EmployeeLoanDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeLoanDetail(int id)
        {
            var employeeLoanDetail = await _context.EmployeeLoanDetails.FindAsync(id);
            if (employeeLoanDetail == null)
            {
                return NotFound();
            }

            _context.EmployeeLoanDetails.Remove(employeeLoanDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeLoanDetailExists(string id)
        {
            return _context.EmployeeLoanDetails.Any(e => e.EmployeeId == id);
        }
    }
}
