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
        private readonly ShreeDbContext_Postgres _context;

        public EmployeeLoanDetailsController(ShreeDbContext_Postgres context)
        {
            _context = context;
        }

        // GET: api/Employeeloandetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employeeloandetail>>> GetEmployeeLoanDetails()
        {
            return await _context.Employeeloandetails.ToListAsync();
        }

        // GET: api/Employeeloandetails/5
        [HttpGet("{empId}")]
        public async Task<ActionResult<Employeeloandetail>> GetEmployeeLoanDetail(string empId)
        {
            var employeeLoanDetail = await _context.Employeeloandetails.Where(x => x.Employeeid == empId).FirstOrDefaultAsync();

            if (employeeLoanDetail == null)
            {
                return NotFound();
            }

            return employeeLoanDetail;
        }

        // PUT: api/Employeeloandetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{empId}")]
        public async Task<IActionResult> PutEmployeeLoanDetail(string empId, Employeeloandetail employeeLoanDetail)
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

        // POST: api/Employeeloandetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employeeloandetail>> PostEmployeeLoanDetail(Employeeloandetail employeeLoanDetail)
        {
            _context.Employeeloandetails.Add(employeeLoanDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeLoanDetail", new { id = employeeLoanDetail.Id }, employeeLoanDetail);
        }

        // DELETE: api/Employeeloandetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeLoanDetail(int id)
        {
            var employeeLoanDetail = await _context.Employeeloandetails.FindAsync(id);
            if (employeeLoanDetail == null)
            {
                return NotFound();
            }

            _context.Employeeloandetails.Remove(employeeLoanDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeLoanDetailExists(string id)
        {
            return _context.Employeeloandetails.Any(e => e.Employeeid == id);
        }
    }
}
