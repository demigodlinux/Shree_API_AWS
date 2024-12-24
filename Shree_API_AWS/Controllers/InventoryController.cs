using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : Controller
    {
        private readonly ShreeDbContext_Postgres _context;
        private IMapper _mapper { get; set; }

        public InventoryController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet("Inventory")]
        public async Task<ActionResult<IEnumerable<InventoryList_DTO>>> GetInventoryList()
        {
            var inventory = await _context.Inventorylists.OrderBy(x => x.Materialid).ToListAsync();

            var vendorDetails = await _context.Vendorlists.ToListAsync();

            var clientDetails = await _context.ClientDetails.ToListAsync();

            return Ok(new { Inventory = _mapper.Map<IEnumerable<InventoryList_DTO>>(inventory), 
                            Vendor = _mapper.Map<IEnumerable<VendorList_DTO>>(vendorDetails),
                            Client = _mapper.Map<IEnumerable<ClientDetails_DTO>>(clientDetails)
            });
        }

        // GET: api/Employees
        [HttpGet("LogInventory")]
        public async Task<ActionResult<IEnumerable<LogInventory_DTO>>> GetLogInventoryList()
        {
            var logInventory = await _context.Loginventorydata.OrderBy(x => x.Materialid).ToListAsync();

            return Ok(_mapper.Map<IEnumerable<LogInventory_DTO>>(logInventory));
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostInventoryData(List<LogInventory_DTO> inventoryTransactions)
        {
            foreach(var inventoryData in inventoryTransactions)
            {
                var existingInventory = await _context.Inventorylists
                .Where(x => x.Materialname == inventoryData.Materialname &&
                    (inventoryData.Typeoftransaction.Equals("Inward") || inventoryData.Typeoftransaction.Equals("Outwards")))
                .FirstOrDefaultAsync();
                try
                {
                    if (existingInventory != null)
                    {
                        // Update the existing inventory
                        existingInventory.Materialpriceperunit =
                            (existingInventory.Materialpriceperunit + inventoryData.Currentmaterialpriceperunit) / 2;

                        existingInventory.Dateenteredby = inventoryData.Dateenteredby;
                        existingInventory.Dateenteredon = DateTime.UtcNow.ToLocalTime();

                        existingInventory.Quantity = inventoryData.Typeoftransaction.Equals("Inward") ?
                        existingInventory.Quantity + inventoryData.Quantity : existingInventory.Quantity - inventoryData.Quantity;

                        existingInventory.Typeofmaterial = inventoryData.Typeofmaterial;
                        existingInventory.Vendorid = inventoryData.Vendorid ?? null;

                        // Save changes for the existing entity
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Add new inventory if it doesn't exist
                        var inventoryAdd = new Inventorylist()
                        {
                            Materialname = inventoryData.Materialname,
                            Materialdescription = inventoryData.Materialdescription,
                            Materialid = inventoryData.Materialid,
                            Materialpriceperunit = inventoryData.Currentmaterialpriceperunit,
                            Measuringunit = inventoryData.Measuringunit,
                            Dateenteredby = inventoryData.Dateenteredby,
                            Dateenteredon = DateTime.UtcNow.ToLocalTime(),
                            Quantity = inventoryData.Quantity,
                            Typeofmaterial = inventoryData.Typeofmaterial,
                            Vendorid = inventoryData.Vendorid ?? null,
                        };

                        _context.Inventorylists.Add(inventoryAdd);
                        await _context.SaveChangesAsync();
                    }

                    // Add a log entry
                    var logInventory = new Loginventorydatum()
                    {
                        Typeoftransaction = inventoryData.Typeoftransaction,
                        Dateoftransaction = DateTime.UtcNow.ToLocalTime(),
                        Materialname = inventoryData.Materialname,
                        Materialdescription = inventoryData.Materialdescription,
                        Materialid = inventoryData.Materialid,
                        Currentmaterialpriceperunit = inventoryData.Currentmaterialpriceperunit,
                        Priceofunittransacted = inventoryData.Priceofunittransacted,
                        Measuringunit = inventoryData.Measuringunit,
                        Dateenteredby = inventoryData.Dateenteredby,
                        Dateenteredon = DateTime.UtcNow.ToLocalTime(),
                        Quantity = inventoryData.Quantity,
                        Typeofmaterial = inventoryData.Typeofmaterial,
                        Vendorid = inventoryData.Vendorid ?? null,
                        Clientid = inventoryData.Clientid ?? null,
                    };

                    _context.Loginventorydata.Add(logInventory);
                    await _context.SaveChangesAsync();

                    return Ok("Success");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return Ok("Success");
        }

    }
}
