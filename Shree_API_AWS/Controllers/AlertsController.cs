using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlertsController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;
        private IMapper _mapper { get; set; }

        public AlertsController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<AlertsController>
        [HttpGet]
        public async Task<IEnumerable<AlertNotification_DTO>> Get()
        {
            return _mapper.Map<IEnumerable<AlertNotification_DTO>>(await _context.AlertNotifications.ToListAsync());
        }

        // GET api/<AlertsController>/5
        [HttpGet("{id}")]
        public async Task<List<Tuple<AlertNotification_DTO, TimesheetEmployee_DTO>>> Get(string id)
        {
            var alerts = await _context.AlertNotifications
                .Where(x => x.Employeeid == id && x.Isnotificationactive == true && x.Isemployeenotification == true)
                .ToListAsync();

            var timesheetEmployeeList = await _context.TimesheetdetailsEmployees
                .Where(x => x.Employeeid == id)
                .ToListAsync();

            var alertData = new List<Tuple<AlertNotification_DTO, TimesheetEmployee_DTO>>();

            foreach (var alert in alerts)
            {
                var matchingTimesheet = timesheetEmployeeList.FirstOrDefault(t => t.Id == alert.TimesheetId);

                if (matchingTimesheet != null)
                {
                    // Assuming AlertNotification_DTO and TimesheetEmployee_DTO are mapped from the entities
                    var alertDTO = _mapper.Map<AlertNotification_DTO>(alert);
                    var attendanceDTO = alertDTO.IsSentback == false ? null : _mapper.Map<TimesheetEmployee_DTO>(matchingTimesheet);

                    // Add the pair to the list
                    alertData.Add(new Tuple<AlertNotification_DTO, TimesheetEmployee_DTO>(alertDTO, attendanceDTO));
                }
            }

            return alertData;
        }


        [HttpGet("Admin")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<AlertNotification_DTO>>> GetAlertsForAdmin()
        {
            var alertData = await _context.AlertNotifications.Where(x => x.Isadminnotification == true && x.Isnotificationactive == true).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<AlertNotification_DTO>>(alertData));
        }

        [HttpPost("Admin/Approve")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<string>> ApproveTimesheetAlert(AlertNotification_DTO alert)
        {
            var timesheetData = getTimesheetData(alert.TimesheetId);
            var exactTimesheetData = JsonConvert.DeserializeObject<Timesheet>(timesheetData.Result.TimesheetDetails);

            DateTime parsedDateTime;
            DateTime.TryParseExact(exactTimesheetData?.TimeEntry, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime);

            List<AlertNotification> existingDatas = await _context.AlertNotifications
            .Where(x => x.TimesheetId == alert.TimesheetId)
            .ToListAsync();
            if (existingDatas.Count > 0)
            {
                existingDatas.ForEach(existingData => {
                    existingData.Isnotificationactive = false;
                    _context.Entry(existingData).State = EntityState.Modified;
                });

                await _context.SaveChangesAsync();
            }


            var alertData = new AlertNotification()
            {
                Dataenteredby = "Admin",
                Dataenteredon = DateTime.Now,
                Employeeid = alert.EmployeeId,
                Notificationmessage = $"Timesheet for #{parsedDateTime.ToString("dd-MM-yyyy")}# for the site #{exactTimesheetData.SiteName}# at #{exactTimesheetData.SiteLocation}# has been Approved by Admin.",
                Isadminnotification = false,
                Isemployeenotification = true,
                Isapproved = true,
                Issentback = false,
                Isnotificationactive = true,
                TimesheetId = alert.TimesheetId,
            };
            _context.AlertNotifications.Add(alertData);

            await _context.SaveChangesAsync();

            return Ok("Success");

        }

        [HttpPost("Admin/Sendback")]
        [Authorize(Roles = "user,admin")]
        [EncryptResponse]
        public async Task<ActionResult<string>> SendbackTimesheetAlert(AlertNotification_DTO alert)
        {
            var timesheetData = getTimesheetData(alert.TimesheetId);
            var exactTimesheetData = JsonConvert.DeserializeObject<Timesheet>(timesheetData.Result.TimesheetDetails);

            DateTime parsedDateTime;
            DateTime.TryParseExact(exactTimesheetData?.TimeEntry, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime);

            List<AlertNotification> existingDatas = await _context.AlertNotifications
            .Where(x => x.TimesheetId == alert.TimesheetId)
            .ToListAsync();
            if (existingDatas.Count > 0)
            {
                existingDatas.ForEach(existingData => {
                    existingData.Isnotificationactive = false;
                    _context.Entry(existingData).State = EntityState.Modified;
                });

                await _context.SaveChangesAsync();
            }

            var alertData = new AlertNotification()
            {
                Dataenteredby = "Admin",
                Dataenteredon = DateTime.Now,
                Employeeid = alert.EmployeeId,
                Notificationmessage = $"Timesheet for #{parsedDateTime.ToString("dd-MM-yyyy")}# for the site #{exactTimesheetData.SiteName}# at #{exactTimesheetData.SiteLocation}# has been sent back by Admin for corrections. Please make the necessary changes and sendback the timesheet.",
                Isadminnotification = false,
                Isemployeenotification = true,
                Isapproved = false,
                Issentback = true,
                Isnotificationactive = true,
                TimesheetId = alert.TimesheetId,
            };
            _context.AlertNotifications.Add(alertData);
            await _context.SaveChangesAsync();

            return Ok("Success");

        }

        private async Task<TimesheetEmployee_DTO> getTimesheetData(int? timesheetID)
        {
            return (_mapper.Map<TimesheetEmployee_DTO>(await _context.TimesheetdetailsEmployees.FindAsync(timesheetID)));
        }

    }
}
