using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VendorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ShreeDbContext_Postgres _context;
        public VendorController(IMapper mapper, ShreeDbContext_Postgres shreeDbContext)
        {
            _mapper = mapper;
            _context = shreeDbContext;

        }
        // GET: api/<VendorController>
        [HttpGet]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<IActionResult> GetVendorDetails()
        {
            var vendorDetails = await _context.Vendorlists.OrderBy(x => x.Vendorid).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VendorList_DTO>>(vendorDetails));
            
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<IActionResult> PostVendorDetails(VendorList_DTO vendorData)
        {
            try
            {
                var existingVendorDetail = await _context.Vendorlists
                    .FirstOrDefaultAsync(x => x.Vendorid == vendorData.Vendorid);

                if (existingVendorDetail != null)
                {
                    // Update existing vendor details
                    existingVendorDetail.Vendortype = vendorData.Vendortype;
                    existingVendorDetail.Gstnumber = vendorData.Gstnumber;
                    existingVendorDetail.Ownername = vendorData.Ownername;
                    existingVendorDetail.Ownernumber = vendorData.Ownernumber;
                    existingVendorDetail.Isactive = vendorData.IsActive;
                    existingVendorDetail.Dataenteredby = "Admin";
                    existingVendorDetail.Dataenteredon = DateTime.UtcNow.ToLocalTime();

                    _context.Entry(existingVendorDetail).State = EntityState.Modified;
                }
                else
                {
                    // Add new vendor details
                    var newVendorId = vendorData.Vendorid ?? Guid.NewGuid().ToString(); // Generate ID if null
                    var vendorDatum = new Vendorlist
                    {
                        Vendorid = newVendorId,
                        Vendorname = vendorData.Vendorname,
                        Vendorlocation = vendorData.Vendorlocation,
                        Vendortype = vendorData.Vendortype,
                        Gstnumber = vendorData.Gstnumber,
                        Ownername = vendorData.Ownername,
                        Ownernumber = vendorData.Ownernumber,
                        Isactive = vendorData.IsActive,
                        Dataenteredby = "Admin",
                        Dataenteredon = DateTime.UtcNow.ToLocalTime()
                    };

                    _context.Vendorlists.Add(vendorDatum);
                }

                await _context.SaveChangesAsync();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
