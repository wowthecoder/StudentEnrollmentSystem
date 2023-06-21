using AutoMapper;
using Microsoft.AspNetCore.Identity;
using StudentEnrollmentSystem.Authentication;
using StudentEnrollmentSystem.Models;

namespace StudentEnrollmentSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            //Map from student to studentDTO object
            CreateMap<Student, StudentDTO>();
            //Map from RegisterModel to AdminDTO object
            CreateMap<IdentityUser, AdminDTO>();
        }
    }
}
