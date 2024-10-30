using Microsoft.AspNetCore.Authorization;
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
    public class ClientDetailsController : ControllerBase
    {
        private readonly MasterContext _context;

        public ClientDetailsController(MasterContext context)
        {
            _context = context;
        }

        // GET: api/ClientDetails
        [HttpGet]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<ClientDetail>>> GetClientDetails()
        {
            return Ok(await _context.ClientDetails.ToListAsync());
        }

        // GET: api/ClientDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDetail>> GetClientDetail(int id)
        {
            var clientDetail = await _context.ClientDetails.FindAsync(id);

            if (clientDetail == null)
            {
                return NotFound();
            }

            return clientDetail;
        }

        // PUT: api/ClientDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientDetail(int id, ClientDetail clientDetail)
        {
            if (id != clientDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(clientDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientDetailExists(id))
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

        // POST: api/ClientDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClientDetail>> PostClientDetail(ClientDetail clientDetail)
        {
            _context.ClientDetails.Add(clientDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClientDetail", new { id = clientDetail.Id }, clientDetail);
        }

        // DELETE: api/ClientDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientDetail(int id)
        {
            var clientDetail = await _context.ClientDetails.FindAsync(id);
            if (clientDetail == null)
            {
                return NotFound();
            }

            _context.ClientDetails.Remove(clientDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientDetailExists(int id)
        {
            return _context.ClientDetails.Any(e => e.Id == id);
        }
    }
}
