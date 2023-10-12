using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.QuizDto
{
    public class QuizQuestionsViewDto
    {
        public string QuestionHead { get; set; }
        public List<OptionRequest> Options { get; set; }
    }
}
