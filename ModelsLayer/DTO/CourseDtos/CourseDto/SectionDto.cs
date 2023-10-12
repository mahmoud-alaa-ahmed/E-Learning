using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CourseDto
{
    public class SectionDto
    {
        public string SectionTitle { get; set; }
        public int OrderInCourse { get; set; }
        public List<LessonDto> Lessons { get; set; }
    }
}
