using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.Models;
using Shree_API_AWS.Repository.Interface;
using Shree_API_AWS.Repository.Service;
using System.Runtime.InteropServices;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IFileUploader _fileUploader;
        private readonly IMapper _mapper;
        private readonly ShreeDbContext_Postgres _context;
        public ReportsController(IFileUploader fileUploader, IMapper mapper, ShreeDbContext_Postgres context_Postgres) {
            _fileUploader = fileUploader;
            _mapper = mapper;
            _context = context_Postgres;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "user")]
        [EncryptResponse]
        public async Task<IActionResult> UploadFile([FromForm] FetchBlobModel blob, [FromForm] IFormFile file)
        {
            var employeeData = _context.Employees.Where(x => x.Employeeid == blob.EmployeeId).FirstOrDefault();

            if (employeeData == null)
            {
                return BadRequest("No employees found");
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            string uploadPath = "";
            string employeeName = employeeData.Firstname + " " + employeeData.Lastname;

            if (RuntimeInformation.OSDescription.ToString().Contains("Windows"))
            {
                uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Reports/");
                if (!Directory.Exists(uploadPath + employeeName))
                {
                    uploadPath = pathGenerator(uploadPath, employeeName);
                    Directory.CreateDirectory(uploadPath);
                }
                else
                {
                    uploadPath = pathGenerator(uploadPath, employeeName);
                }
            }
            else
            {
                uploadPath = "/home/shree/Shree_Elevator/Uploads/Reports/";
                if (!Directory.Exists(uploadPath + employeeName))
                {
                    uploadPath = pathGenerator(uploadPath, employeeName);
                    Directory.CreateDirectory(uploadPath);
                }
                else
                {
                    uploadPath = pathGenerator(uploadPath, employeeName);
                }

            }

            try
            {
                string response = await _fileUploader.UploadFileAsync(uploadPath, blob, file, employeeName);
                return response == "Success" ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates path to file upload based on the given root path
        /// </summary>
        /// <param name="uploadPathRoot"></param>
        /// <param name="employeeName"></param>
        /// <returns></returns>
        private string pathGenerator(string uploadPathRoot, string employeeName)
        {
            return uploadPathRoot + employeeName + "/" + DateTime.Now.ToString("dd MMMM yyyy") + "/";
        }
    }
}
