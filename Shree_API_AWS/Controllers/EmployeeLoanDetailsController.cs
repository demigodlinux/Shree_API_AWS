using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<EmployeeLoanDetail_DTO>>> GetEmployeeLoanDetails()
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeLoanDetail_DTO>>(await _context.Employeeloandetails.ToListAsync()));
        }

        // GET: api/Employeeloandetails/5
        [HttpGet("LoanDetailsLog")]
        public async Task<ActionResult<EmployeeLoanDetails_Log_DTO>> GetEmployeeLoanDetail(string empId)
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeLoanDetails_Log_DTO>>(await _context.Employeeloandetailslogs.ToListAsync()));
        }


        // POST: api/Employeeloandetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employeeloandetail>> PostEmployeeLoanDetail(EmployeeLoanDetail_DTO employeeLoanDetail)
        {
            try
            {
                Employeeloandetail loanDetails = EmployeeLoanDetailExists(employeeLoanDetail);

                if (loanDetails == null)
                {
                    loanDetails = new Employeeloandetail();

                    loanDetails = _mapper.Map<Employeeloandetail>(employeeLoanDetail);

                    _context.Employeeloandetails.Add(loanDetails);
                    await _context.SaveChangesAsync();

                    loanDetails = EmployeeLoanDetailExists(employeeLoanDetail);
                }
                else
                {
                    loanDetails.Lastloancollecteddate = DateTime.Now;

                    await _context.SaveChangesAsync();
                }


                var loggedLoanDetails = new Employeeloandetailslog()
                {
                    Isloanamountdeducted = true,
                    Dataenteredby = "Admin",
                    Dataenteredon = DateTime.Now,
                    Employeeid = employeeLoanDetail.EmployeeId,
                    Employeeloanid = loanDetails.Id,
                    Loanamount = employeeLoanDetail.LoanAmount,
                    Partofpaymentremaining = LogEmployeeLoanDetailExists(loanDetails) ? employeeLoanDetail.PartsOfRepayment - 1 : employeeLoanDetail.PartsOfRepayment,
                    Transactiondate = DateTime.Now,
                    Transactiontype = LogEmployeeLoanDetailExists(loanDetails) ? "CREDIT" : "DEBIT",
                };

                _context.Employeeloandetailslogs.Add(loggedLoanDetails);
                await _context.SaveChangesAsync();

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        private Employeeloandetail? EmployeeLoanDetailExists(EmployeeLoanDetail_DTO employeeLoanDetail)
        {
            return _context.Employeeloandetails.Where(e => e.Employeeid == employeeLoanDetail.EmployeeId 
            && e.Loanprocessedon == employeeLoanDetail.LoanProcessedOn && e.Loanamount == employeeLoanDetail.LoanAmount).FirstOrDefault() ?? null;
        }

        private bool LogEmployeeLoanDetailExists(Employeeloandetail employeeLoanDetail)
        {
            return _context.Employeeloandetailslogs.Any(e => e.Logid == employeeLoanDetail.Id && e.Employeeid == employeeLoanDetail.Employeeid
            && e.Transactiondate == employeeLoanDetail.Loanprocessedon && e.Loanamount == employeeLoanDetail.Loanamount);
        }

    }
}
