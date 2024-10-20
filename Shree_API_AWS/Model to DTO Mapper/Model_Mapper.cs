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
            CreateMap<EmployeeAttendance, EmployeeAttendance_DTO>().ReverseMap();
            CreateMap<EmployeeLoanDetail, EmployeeLoanDetail_DTO>().ReverseMap();
            CreateMap<LogEmployeeAttendance, LogEmployeeAttendance_DTO>().ReverseMap();
            CreateMap<OvertimeWorking, OvertimeWorking_DTO>().ReverseMap();


        }
    }
}
