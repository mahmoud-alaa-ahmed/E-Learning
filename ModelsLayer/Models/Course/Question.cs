using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
    public class Question:BaseModel
    {
        public Question()
        {
            Options = new List<Option>();
        }
        public int QuizId { get; set; }
        public virtual ICollection<Option> Options { get; set; }
    }
}
