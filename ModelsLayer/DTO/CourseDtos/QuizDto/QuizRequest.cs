using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.QuizDto
{
    public class QuizRequest
    {
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        public List<QuestionRequest> QuestionRequests { get; set; }
    }
}
