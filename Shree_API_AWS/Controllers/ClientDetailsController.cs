using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class ClientDetailsController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;

        private IMapper _mapper { get; set; }

        public ClientDetailsController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ClientDetails
        [HttpGet]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<ClientDetails_DTO>>> GetClientDetails()
        {
            return Ok(_mapper.Map<IEnumerable<ClientDetails_DTO>>(await _context.ClientDetails.OrderBy(x => x.Clientid).ToListAsync()));
        }

        // GET: api/ClientDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDetails_DTO>> GetClientDetail(int id)
        {
            var clientDetail = await _context.ClientDetails.FindAsync(id);

            if (clientDetail == null)
            {
                return NotFound();
            }

            return _mapper.Map<ClientDetails_DTO>(clientDetail);
        }

        // PUT: api/ClientDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutClientDetail(ClientDetails_DTO clientDetail)
        {
            // Retrieve the existing entity from the database
            var existingData = await _context.ClientDetails
                .Where(x => x.Id == clientDetail.Id)
                .FirstOrDefaultAsync();

            if (existingData != null)
            {
                // Map DTO properties to the existing entity
                existingData.Clientid = clientDetail.ClientId;
                existingData.Dataenteredon = DateTime.Now;
                existingData.Dataenteredby = "Admin";
                existingData.Clientname = clientDetail.ClientName;
                existingData.Clientaddress = clientDetail.ClientAddress;
                existingData.Clientlocation = clientDetail.ClientLocation;
                existingData.Clientservicetype = clientDetail.ClientServiceType;
                existingData.Sitemanagername = clientDetail.SiteManagerName;
                existingData.Sitemanagercontact = clientDetail.SiteManagerContact;
                existingData.Numberoflifts = clientDetail.NumberOfLifts;
                existingData.Contractstartdate = clientDetail.ContractStartDate;
                existingData.Contractenddate = clientDetail.ContractEndDate;
                existingData.Contractperiod = clientDetail.ContractPeriod;
                existingData.Contractamount = clientDetail.ContractAmount;
                existingData.Termsofpayment = clientDetail.TermsOfPayment;
                existingData.Isgstincluded = clientDetail.IsGstIncluded;
                existingData.Isactive = clientDetail.IsActive;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientDetailExists(clientDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok("Success");
            }
            else
            {
                return NotFound("No records found");
            }
        }



        // POST: api/ClientDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostClientDetail(ClientDetails_DTO clientDetail)
        {
            try
            {
                // Map DTO to Entity
                var newClient = new ClientDetail
                {
                    Clientid = clientDetail.ClientId,
                    Dataenteredon = DateTime.Now,
                    Dataenteredby = "Admin",
                    Clientname = clientDetail.ClientName,
                    Clientaddress = clientDetail.ClientAddress,
                    Clientlocation = clientDetail.ClientLocation,
                    Clientservicetype = clientDetail.ClientServiceType,
                    Sitemanagername = clientDetail.SiteManagerName,
                    Sitemanagercontact = clientDetail.SiteManagerContact,
                    Numberoflifts = clientDetail.NumberOfLifts,
                    Contractstartdate = clientDetail.ContractStartDate,
                    Contractenddate = clientDetail.ContractEndDate,
                    Contractperiod = clientDetail.ContractPeriod,
                    Contractamount = clientDetail.ContractAmount,
                    Termsofpayment = clientDetail.TermsOfPayment,
                    Isgstincluded = clientDetail.IsGstIncluded,
                    Isactive = clientDetail.IsActive
                };

                // Add the new client to the database
                _context.ClientDetails.Add(newClient);
                await _context.SaveChangesAsync();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
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
