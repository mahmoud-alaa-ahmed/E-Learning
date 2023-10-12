using Microsoft.AspNetCore.Http;
using ModelLayer.Models.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.CourseDto
{
   public class LessonDto
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public int OrderInSection { get; set; }
        public string Video { get; set; }

    }
}
