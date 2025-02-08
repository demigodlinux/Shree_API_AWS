using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeLoanDetailsController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private IMapper _mapper { get; set; }
        public EmployeeLoanDetailsController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Employeeloandetails
        [HttpGet("LoanDetails")]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<EmployeeLoanDetail_DTO>>> GetEmployeeLoanDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeLoanDetail_DTO>>(await _context.Employeeloandetails.ToListAsync()));
        }

        // GET: api/Employeeloandetails/5
        [HttpGet("LoanDetailsLog/{id}")]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<ActionResult<EmployeeLoanDetails_Log_DTO>> GetEmployeeLoanDetail(int id)
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeLoanDetails_Log_DTO>>(await _context.Employeeloandetailslogs.Where(x => x.Employeeloanid == id).ToListAsync()));
        }


        // POST: api/Employeeloandetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<ActionResult<Employeeloandetail>> PostEmployeeLoanDetail(EmployeeLoanDetail_DTO employeeLoanDetail)
        {
            try
            {
                Employeeloandetail loanDetails = EmployeeLoanDetailExists(employeeLoanDetail);
                var logloanDetails = loanDetails == null ? null : LogEmployeeLoanDetailExists(loanDetails);

                if (loanDetails == null)
                {
                    employeeLoanDetail.LoanProcessedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                    employeeLoanDetail.DataEnteredOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                    var newloanDetails = _mapper.Map<Employeeloandetail>(employeeLoanDetail);

                    _context.Employeeloandetails.Add(newloanDetails);
                    await _context.SaveChangesAsync();

                    loanDetails = EmployeeLoanDetailExists(employeeLoanDetail);
                }
                else
                {
                    loanDetails.Lastloancollecteddate = DateTime.UtcNow.ToLocalTime();

                    await _context.SaveChangesAsync();
                }

 
                var loggedLoanDetails = new Employeeloandetailslog()
                {
                    Isloanamountdeducted = logloanDetails == null ? false : true,
                    Dataenteredby = "Admin",
                    Dataenteredon = DateTime.UtcNow.ToLocalTime(),
                    Employeeid = employeeLoanDetail.EmployeeId,
                    Employeeloanid = loanDetails.Id,
                    Loanamount = employeeLoanDetail.LoanAmount,
                    Partofpaymentremaining = logloanDetails == null ? employeeLoanDetail.PartsOfRepayment : logloanDetails.Partofpaymentremaining - 1,
                    Transactiondate = DateTime.UtcNow.ToLocalTime(),
                    Transactiontype = logloanDetails == null ? "DEBIT" : "CREDIT",
                };

                _context.Employeeloandetailslogs.Add(loggedLoanDetails);
                await _context.SaveChangesAsync();

                if(loggedLoanDetails.Partofpaymentremaining == 0)
                {
                    loanDetails.Isactive = false;

                    await _context.SaveChangesAsync();
                }

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            
        }

        private Employeeloandetail? EmployeeLoanDetailExists(EmployeeLoanDetail_DTO employeeLoanDetail)
        {
            return _context.Employeeloandetails.Where(e => e.Employeeid == employeeLoanDetail.EmployeeId 
            && e.Loanprocessedon == employeeLoanDetail.LoanProcessedOn && e.Loanamount == employeeLoanDetail.LoanAmount).FirstOrDefault() ?? null;
        }

        private Employeeloandetailslog? LogEmployeeLoanDetailExists(Employeeloandetail employeeLoanDetail)
        {
            return _context.Employeeloandetailslogs.Where(e => e.Employeeloanid == employeeLoanDetail.Id && e.Employeeid == employeeLoanDetail.Employeeid && e.Loanamount == employeeLoanDetail.Loanamount).OrderBy(x => x.Dataenteredon).LastOrDefault() ?? null;
        }

    }
}
