using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
    public class Option:BaseModel
    {
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}
