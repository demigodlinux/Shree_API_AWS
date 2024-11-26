using AutoMapper;
using Shree_API_AWS.DataTransferObjects;
using Shree_API_AWS.Models;

namespace LearningAPI.Mapper
{
    public class Model_Mapper: Profile
    {
        public Model_Mapper()
        {
            CreateMap<Employee, Employee_DTO>().ReverseMap();
            CreateMap<Employeeattendance, EmployeeAttendance_DTO>().ReverseMap();
            CreateMap<Employeeloandetail, EmployeeLoanDetail_DTO>().ReverseMap();
            CreateMap<LogEmployeeattendance, LogEmployeeAttendance_DTO>().ReverseMap();
            CreateMap<Overtimeworking, OvertimeWorking_DTO>().ReverseMap();
            CreateMap<ClientDetail, ClientDetails_DTO>().ReverseMap();
            CreateMap<TimesheetdetailsAdmin, TimesheetAdmin_DTO>().ReverseMap();
            CreateMap<TimesheetdetailsEmployee, TimesheetEmployee_DTO>().ReverseMap();
            CreateMap<AlertNotification, AlertNotification_DTO>().ReverseMap();
            CreateMap<Locationtracker, LocationTracker_DTO>().ReverseMap();
        }
    }
}
