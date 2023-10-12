using ModelLayer.DTO.CourseDtos.CourseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
    public class SectionResponseDTO
    {
        public string SectionTitle { get; set; }
        public int OrderInCourse { get; set; }
        public List<LessonResponseDTO> Lessons { get; set; }
    }
}
