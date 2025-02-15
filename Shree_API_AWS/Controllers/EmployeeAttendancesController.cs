using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Shree_API_AWS.Attributes;
using Shree_API_AWS.Context;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;
using System.Data;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeAttendancesController : ControllerBase
    {
        private readonly ShreeDbContext_Postgres _context;

        private IMapper _mapper { get; set; }
        public EmployeeAttendancesController(ShreeDbContext_Postgres context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/EmployeeAttendances
        [HttpGet]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<ActionResult<IEnumerable<Employeeattendance>>> GetEmployeeAttendances()
        {
            return Ok(await _context.Employeeattendances.ToListAsync());
        }

        // GET: api/EmployeeAttendances/5
        [HttpGet("{empId}")]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<ActionResult<Employeeattendance>> GetEmployeeAttendance(string empId)
        {
            var employeeAttendance = await _context.Employeeattendances
                .Where(x => x.Employeeid == empId).FirstOrDefaultAsync();

            if (employeeAttendance == null)
            {
                return NotFound();
            }

            return Ok(employeeAttendance);
        }


        // POST: api/EmployeeAttendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        [EncryptResponse]
        public async Task<ActionResult<string>> PostEmployeeAttendance(AttendanceDetails employeeAttendance)
        {
            try
            {
                TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                // Convert DateTime? to formatted string with microseconds and time zone
                var checkInTime = employeeAttendance.CheckinTiming.HasValue
                    ? TimeZoneInfo.ConvertTimeFromUtc(employeeAttendance.CheckinTiming.Value, indianTimeZone).ToString("yyyy-MM-dd HH:mm:ss.ffffffzzz")
                    : null;

                var checkOutTime = employeeAttendance.CheckoutTiming.HasValue
                    ? TimeZoneInfo.ConvertTimeFromUtc(employeeAttendance.CheckoutTiming.Value, indianTimeZone).ToString("yyyy-MM-dd HH:mm:ss.ffffffzzz")
                    : null;

                // Check if the record already exists
                var existingRecord = _context.Employeeattendances
                    .Where(ow => ow.Employeeid == employeeAttendance.EmployeeAttendanceDetails.EmployeeId
                                 && ow.Entryformonth == employeeAttendance.EmployeeAttendanceDetails.EntryForMonth)
                    .AsEnumerable()
                    .FirstOrDefault(ow => CheckSameDates(ow, employeeAttendance));

                if (existingRecord != null)
                {
                    var existingLogRecord = await _context.LogEmployeeattendances
                        .Where(x => x.Employeeattendid == existingRecord.Id)
                        .FirstOrDefaultAsync();

                    if (existingLogRecord != null)
                    {
                        existingLogRecord.Checkintiming = checkInTime;
                        existingLogRecord.Checkouttiming = checkOutTime;
                        existingLogRecord.Ispresent = employeeAttendance.EmployeeAttendanceDetails.IsPresent;
                        existingLogRecord.Isabsent = employeeAttendance.EmployeeAttendanceDetails.IsAbsent;
                        existingLogRecord.Ispaidleave = employeeAttendance.EmployeeAttendanceDetails.IsPaidLeave;
                        existingLogRecord.Ishalfday = employeeAttendance.EmployeeAttendanceDetails.IsHalfDay;
                        existingLogRecord.Islate = employeeAttendance.EmployeeAttendanceDetails.IsLate;
                        existingLogRecord.Isonduty = employeeAttendance.EmployeeAttendanceDetails.LastSundayDutyDate == null
                                                        && employeeAttendance.EmployeeAttendanceDetails.LastPublicHolidayDutyDate != null
                                                        ? employeeAttendance.EmployeeAttendanceDetails.IsPublicHolidayDuty
                                                        : employeeAttendance.EmployeeAttendanceDetails.IsSundyDuty;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return "Failed";
                    }

                    existingRecord.Ispresent = employeeAttendance.EmployeeAttendanceDetails.IsPresent;
                    existingRecord.Isabsent = employeeAttendance.EmployeeAttendanceDetails.IsAbsent;
                    existingRecord.Ispaidleave = employeeAttendance.EmployeeAttendanceDetails.IsPaidLeave;
                    existingRecord.Ishalfday = employeeAttendance.EmployeeAttendanceDetails.IsHalfDay;
                    existingRecord.Islate = employeeAttendance.EmployeeAttendanceDetails.IsLate;
                    existingRecord.Issundyduty = employeeAttendance.EmployeeAttendanceDetails.IsSundyDuty;
                    existingRecord.Ispublicholidayduty = employeeAttendance.EmployeeAttendanceDetails.IsPublicHolidayDuty;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    employeeAttendance.EmployeeAttendanceDetails.DataEnteredOn = DateTime.UtcNow;

                    if (employeeAttendance.EmployeeAttendanceDetails.LastPresentDate.HasValue)
                        employeeAttendance.EmployeeAttendanceDetails.LastPresentDate =
                            DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastPresentDate.Value, DateTimeKind.Utc);

                    if (employeeAttendance.EmployeeAttendanceDetails.LastAbsentDate.HasValue)
                        employeeAttendance.EmployeeAttendanceDetails.LastAbsentDate =
                            DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastAbsentDate.Value, DateTimeKind.Utc);

                    if (employeeAttendance.EmployeeAttendanceDetails.LastPaidLeaveDate.HasValue)
                        employeeAttendance.EmployeeAttendanceDetails.LastPaidLeaveDate =
                            DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastPaidLeaveDate.Value, DateTimeKind.Utc);

                    if (employeeAttendance.EmployeeAttendanceDetails.LastHalfDayDate.HasValue)
                        employeeAttendance.EmployeeAttendanceDetails.LastHalfDayDate =
                            DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastHalfDayDate.Value, DateTimeKind.Utc);

                    if (employeeAttendance.EmployeeAttendanceDetails.LastLateDay.HasValue)
                        employeeAttendance.EmployeeAttendanceDetails.LastLateDay =
                            DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastLateDay.Value, DateTimeKind.Utc);

                    if (employeeAttendance.EmployeeAttendanceDetails.LastSundayDutyDate.HasValue)
                        employeeAttendance.EmployeeAttendanceDetails.LastSundayDutyDate =
                            DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastSundayDutyDate.Value, DateTimeKind.Utc);

                    if (employeeAttendance.EmployeeAttendanceDetails.LastPublicHolidayDutyDate.HasValue)
                        employeeAttendance.EmployeeAttendanceDetails.LastPublicHolidayDutyDate =
                            DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastPublicHolidayDutyDate.Value, DateTimeKind.Utc);


                    employeeAttendance.EmployeeAttendanceDetails.TotalDaysPresent += employeeAttendance.EmployeeAttendanceDetails.IsPresent == true ? 1 : 0;
                    employeeAttendance.EmployeeAttendanceDetails.TotalDaysAbsent += employeeAttendance.EmployeeAttendanceDetails.IsAbsent == true ? 1 : 0;
                    employeeAttendance.EmployeeAttendanceDetails.TotalDaysLateDay += employeeAttendance.EmployeeAttendanceDetails.IsLate == true ? 1 : 0;
                    employeeAttendance.EmployeeAttendanceDetails.TotalDaysHalfDays += employeeAttendance.EmployeeAttendanceDetails.IsHalfDay == true ? 1 : 0;
                    employeeAttendance.EmployeeAttendanceDetails.TotalSundayDutyDays += employeeAttendance.EmployeeAttendanceDetails.IsSundyDuty == true ? 1 : 0;
                    employeeAttendance.EmployeeAttendanceDetails.TotalPublicHolidayDutyDays += employeeAttendance.EmployeeAttendanceDetails.IsPublicHolidayDuty == true ? 1 : 0;

                    var mappedAttendanceDetails = _mapper.Map<Employeeattendance>(employeeAttendance.EmployeeAttendanceDetails);
                    _context.Employeeattendances.Add(mappedAttendanceDetails);
                    await _context.SaveChangesAsync();

                    var loggedAttendanceDetails = new LogEmployeeattendance()
                    {
                        Employeeattendid = mappedAttendanceDetails.Id,
                        Employeeid = mappedAttendanceDetails.Employeeid,
                        Entryformonth = mappedAttendanceDetails.Entryformonth,
                        Attendancedate = employeeAttendance.CheckinTiming != null ? DateTime.SpecifyKind(employeeAttendance.CheckinTiming.Value, DateTimeKind.Utc) 
                                            : employeeAttendance.EmployeeAttendanceDetails.IsAbsent == true ? DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastAbsentDate.Value, DateTimeKind.Utc) 
                                            : employeeAttendance.EmployeeAttendanceDetails.IsPaidLeave == true ? DateTime.SpecifyKind(employeeAttendance.EmployeeAttendanceDetails.LastPaidLeaveDate.Value, DateTimeKind.Utc) : null,
                        Checkintiming = checkInTime,
                        Checkouttiming = checkOutTime,
                        Dataenteredby = mappedAttendanceDetails.Dataenteredby,
                        Dataenteredon = DateTime.UtcNow,
                        Isabsent = mappedAttendanceDetails.Isabsent,
                        Ispresent = mappedAttendanceDetails.Ispresent,
                        Islate = mappedAttendanceDetails.Islate,
                        Ishalfday = mappedAttendanceDetails?.Ishalfday,
                        Isonduty = mappedAttendanceDetails.Lastsundaydutydate == null
                                                        && mappedAttendanceDetails.Lastpublicholidaydutydate != null                                                        
                                                        ? mappedAttendanceDetails.Ispublicholidayduty
                                                        : mappedAttendanceDetails.Issundyduty,
                        Ispaidleave = mappedAttendanceDetails.Ispaidleave,
                    };
                    _context.LogEmployeeattendances.Add(loggedAttendanceDetails);
                    await _context.SaveChangesAsync();
                }

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        private static bool CheckSameDates(Employeeattendance attendance, AttendanceDetails postData)
        {
            if (postData.CheckinTiming.HasValue && postData.CheckoutTiming.HasValue)
            {
                DateOnly checkinDate = DateOnly.FromDateTime(postData.CheckinTiming.Value);
                DateOnly checkoutDate = DateOnly.FromDateTime(postData.CheckoutTiming.Value);

                // List of all LastDate fields in Employeeattendance
                var lastDates = new List<DateTime?>()
                {
                    attendance.Lastpresentdate,
                    attendance.Lastabsentdate,
                    attendance.Lastpaidleavedate,
                    attendance.Lastlateday,
                    attendance.Lasthalfdaydate,
                    attendance.Lastsundaydutydate,
                    attendance.Lastpublicholidaydutydate
                };

                // Check if any of the last dates match both checkin and checkout date
                return lastDates.Any(date => date.HasValue &&
                                             DateOnly.FromDateTime(date.Value) == checkinDate &&
                                             DateOnly.FromDateTime(date.Value) == checkoutDate);
            }

            return false;
        }

    }
}
