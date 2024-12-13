using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> GetVendorDetails()
        {
            var vendorDetails = await _context.Vendorlists.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VendorList_DTO>>(vendorDetails));
            
        }

        [HttpPost]
        public async Task<IActionResult> PostVendorDetails(VendorList_DTO vendorData)
        {
            var existingVendorDetail = await _context.Vendorlists.Where(x => x.Vendorid == vendorData.Vendorid).FirstOrDefaultAsync();

            try
            {
                if (existingVendorDetail != null)
                {
                    var vendorDatum = new Vendorlist
                    {
                        Id = existingVendorDetail.Id,
                        Vendorid = existingVendorDetail.Vendorid,
                        Vendorname = existingVendorDetail.Vendorname,
                        Vendorlocation = existingVendorDetail.Vendorlocation,
                        Vendortype = vendorData.Vendortype,
                        Dataenteredby = "Admin",
                        Dataenteredon = DateTime.UtcNow.ToLocalTime(),
                        Gstnumber = vendorData.Gstnumber,
                        Ownername = vendorData.Ownername,
                        Ownernumber = vendorData.Ownernumber,
                    };

                    _context.Entry(vendorDatum).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var vendorDatum = new Vendorlist
                    {
                        Vendorid = vendorData.Vendorid,
                        Vendorname = vendorData.Vendorname,
                        Vendorlocation = vendorData.Vendorlocation,
                        Vendortype = vendorData.Vendortype,
                        Dataenteredby = "Admin",
                        Dataenteredon = DateTime.UtcNow.ToLocalTime(),
                        Gstnumber = vendorData.Gstnumber,
                        Ownername = vendorData.Ownername,
                        Ownernumber = vendorData.Ownernumber,
                    };

                    _context.Vendorlists.Add(vendorDatum);
                    await _context.SaveChangesAsync();

                }

                return Ok("Success");
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

    }
}
