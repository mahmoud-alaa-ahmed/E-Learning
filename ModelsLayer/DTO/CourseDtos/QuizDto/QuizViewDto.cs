using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO.CourseDtos.QuizDto
{
    public class QuizViewDto
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int NoOfQuestions { get; set; }
    }
}
