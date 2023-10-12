using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CourseDto
{
    public class LessonViewDto
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public int OrderInSection { get; set; }
        public string VideoUrl { get; set; }

    }
}
