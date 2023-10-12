using ModelLayer.DTO.CourseDtos.CourseDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.UserDetailsDtos
{
    public class UserEnrollCourseResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseImage { get; set; }
    }
}
