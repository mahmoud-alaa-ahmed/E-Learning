using ModelLayer.DTO.CourseDtos.CourseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.UserDetailsDtos
{
   public class UserDetailsResponse
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<UserEnrollCourseResponse> Courses { get; set; }
        //public List<CourseDto> UserCourses { get; set; }
    }
}
