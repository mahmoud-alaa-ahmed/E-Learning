using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.QuizDto
{
    public class OptionRequest
    {
        [MinLength(1)]
        public string OptionName { get; set; }
        [MinLength(4)]
        public bool IsCorrect { get; set; }
    }
}
