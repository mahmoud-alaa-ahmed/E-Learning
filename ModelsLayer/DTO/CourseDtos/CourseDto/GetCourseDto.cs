using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CourseDto
{
    public class GetCourseDto
    {
       
        public string CourseName { get; set; }
       
        public string Level { get; set; }
       
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Requirement { get; set; }
     
        public string Image { get; set; }
 
        public string Language { get; set; }
      
        public string InstructorName { get; set; }
        public string InstructorImage { get; set; }
        public string  InstructorAbout { get; set; }

        public string CategoryName { get; set; }
        public List<SectionDto> Sections { get; set; }
    }
}
