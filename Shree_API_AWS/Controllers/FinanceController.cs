using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private IMapper _mapper { get; set; }
        public FinanceController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("SalaryAdvDetails")]
        public async Task<ActionResult<IEnumerable<EmployeeSalaryAdvanceDetails_DTO>>> GetEmployeeSalaryDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeSalaryAdvanceDetails_DTO>>(await _context.Employeesalaryadvancedetails
                .Where(x => x.Isadvancededucted == false && x.Isactive == true).ToListAsync()));
        }

        [HttpGet("MiscDetails")]
        public async Task<ActionResult<IEnumerable<EmployeeMiscDetails_DTO>>> GetEmployeeMiscDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeMiscDetails_DTO>>(await _context.Employeemiscdetails
                .Where(x => x.Iscashdeducted == false && x.Isactive == true).ToListAsync()));
        }

        [HttpGet("PettyCashDetails")]
        public async Task<ActionResult<IEnumerable<EmployeePettyCashDetails_DTO>>> GetEmployeePettyCashDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeePettyCashDetails_DTO>>(await _context.Employeepettycashdetails
                .Where(x => x.Iscashdeducted == false && x.Isactive == true).ToListAsync()));
        }

        [HttpGet("HistoricSalaryAdvDetails")]
        public async Task<ActionResult<IEnumerable<EmployeeSalaryAdvanceDetails_DTO>>> GetHistoricEmployeeSalaryDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeSalaryAdvanceDetails_DTO>>(await _context.Employeesalaryadvancedetails.ToListAsync()));
        }

        [HttpGet("HistoricMiscDetails")]
        public async Task<ActionResult<IEnumerable<EmployeeMiscDetails_DTO>>> GetHistoricEmployeeMiscDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeMiscDetails_DTO>>(await _context.Employeemiscdetails.ToListAsync()));
        }

        [HttpGet("HistoricPettyCashDetails")]
        public async Task<ActionResult<IEnumerable<EmployeePettyCashDetails_DTO>>> GetHistoricEmployeePettyCashDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeePettyCashDetails_DTO>>(await _context.Employeepettycashdetails.ToListAsync()));
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostEmployeeFinanceDetail([FromBody]string json, string service, string? paidCheck = "NOTPAID")
        {
            try
            {
                if (service.Equals("SalaryAdvance"))
                {
                    EmployeeSalaryAdvanceDetails_DTO salaryAdvanceDetail = JsonConvert.DeserializeObject<EmployeeSalaryAdvanceDetails_DTO>(json);
                    if (paidCheck == "PAID")
                    {
                        var existingSalaryAdvanceRecord = await _context.Employeesalaryadvancedetails.FindAsync(salaryAdvanceDetail?.Id);

                        existingSalaryAdvanceRecord.Isadvancededucted = true;
                        existingSalaryAdvanceRecord.Isactive = false;
                        existingSalaryAdvanceRecord.Remarks = $"Old Remarks: {existingSalaryAdvanceRecord.Remarks} - Paid on {DateTime.Now.ToString("f")}";
                        _context.SaveChangesAsync();

                        var updatedRecord = new Employeesalaryadvancedetail();
                        updatedRecord.Employeeid = existingSalaryAdvanceRecord.Employeeid;
                        updatedRecord.Amountprocessedon = DateOnly.FromDateTime(DateTime.Now);
                        updatedRecord.Dataenteredon = DateTime.UtcNow.ToLocalTime();
                        updatedRecord.Dataenteredby = "Admin";
                        updatedRecord.Advanceamount = existingSalaryAdvanceRecord.Advanceamount;
                        updatedRecord.Isadvancededucted = true;
                        updatedRecord.Isactive = true;
                        updatedRecord.Transactiontype = "INWARD";
                        updatedRecord.Remarks = $"Admin Note: {salaryAdvanceDetail.Remarks} - Received on {DateTime.Now.ToString("f")}";

                        _context.Employeesalaryadvancedetails.Add(updatedRecord);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var newRecord = _mapper.Map<Employeesalaryadvancedetail>(salaryAdvanceDetail);
                        _context.Employeesalaryadvancedetails.Add(newRecord);
                        await _context.SaveChangesAsync();
                    }
                    return Ok("Success");
                }
                else if (service.Equals("Misc"))
                {
                    EmployeeMiscDetails_DTO miscDetail = JsonConvert.DeserializeObject<EmployeeMiscDetails_DTO>(json);
                    if (paidCheck == "PAID")
                    {
                        var existingMiscRecord = await _context.Employeemiscdetails.FindAsync(miscDetail?.Id);

                        existingMiscRecord.Isactive = false;
                        existingMiscRecord.Remarks = $"Old Remarks: {existingMiscRecord.Remarks} - Paid on {DateTime.Now.ToString("f")}";
                        await _context.SaveChangesAsync();

                        var updatedRecord = new Employeemiscdetail();
                        updatedRecord.Employeeid = existingMiscRecord.Employeeid;
                        updatedRecord.Amountprocessedon = DateOnly.FromDateTime(DateTime.Now);
                        updatedRecord.Dataenteredon = DateTime.UtcNow.ToLocalTime();
                        updatedRecord.Dataenteredby = "Admin";
                        updatedRecord.Misccashamount = existingMiscRecord.Misccashamount;
                        updatedRecord.Iscashdeducted = true;
                        updatedRecord.Isactive = true;
                        updatedRecord.Transactiontype = "INWARD";
                        updatedRecord.Remarks = $"Admin Note: {miscDetail.Remarks} - Received on {DateTime.Now.ToString("f")}";

                        _context.Employeemiscdetails.Add(updatedRecord);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var newRecord = _mapper.Map<Employeemiscdetail>(miscDetail);
                        _context.Employeemiscdetails.Add(newRecord);
                        await _context.SaveChangesAsync();
                    }
                    return Ok("Success");
                }
                else if (service.Equals("PettyCash"))
                {
                    EmployeePettyCashDetails_DTO pettyCashDetail = JsonConvert.DeserializeObject<EmployeePettyCashDetails_DTO>(json);
                    if (paidCheck == "PAID")
                    {
                        var existingPettyCashRecord = await _context.Employeepettycashdetails.FindAsync(pettyCashDetail?.Id);

                        existingPettyCashRecord.Isactive = false;
                        existingPettyCashRecord.Remarks = $"Old Remarks: {existingPettyCashRecord.Remarks} - Paid on {DateTime.Now.ToString("f")}";
                        await _context.SaveChangesAsync();

                        var updatedRecord = new Employeepettycashdetail();
                        updatedRecord.Employeeid = existingPettyCashRecord.Employeeid;
                        updatedRecord.Amountprocessedon = DateOnly.FromDateTime(DateTime.Now);
                        updatedRecord.Dataenteredon = DateTime.UtcNow.ToLocalTime();
                        updatedRecord.Dataenteredby = "Admin";
                        updatedRecord.Pettycashamount = existingPettyCashRecord.Pettycashamount;
                        updatedRecord.Iscashdeducted = true;
                        updatedRecord.Isactive = true;
                        updatedRecord.Transactiontype = "INWARD";
                        updatedRecord.Remarks = $"Admin Note: {pettyCashDetail.Remarks} - Received on {DateTime.Now.ToString("f")}";

                        _context.Employeepettycashdetails.Add(updatedRecord);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var newRecord = _mapper.Map<Employeepettycashdetail>(pettyCashDetail);
                        _context.Employeepettycashdetails.Add(newRecord);
                        await _context.SaveChangesAsync();
                    }
                    return Ok("Success");
                }

                return Ok("Failed");
            }catch(Exception ex)
            {
                return Ok($"Error - {ex.Message}");
            }
        }

    }
}
