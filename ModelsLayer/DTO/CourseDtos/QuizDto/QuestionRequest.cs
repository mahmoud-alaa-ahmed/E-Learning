using ModelLayer.Models.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.QuizDto
{
    public class QuestionRequest
    {
        [MinLength(10)]
        public string Head { get; set; }
        [Required]
        public List<OptionRequest> QuestionOptions { get; set; }
    }
}
