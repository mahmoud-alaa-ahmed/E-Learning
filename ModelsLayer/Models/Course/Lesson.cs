using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
   
    public class Lesson:BaseModel
    {
        public int OrderInSection { get; set; }
        public string VideoUrl { get; set; }
        public int SectionId { get; set; }
        public virtual ICollection<UserCourseProgress> UserCourseProgresses { get; set; }
        public Lesson ()
        {
            UserCourseProgresses = new List<UserCourseProgress> ();
        }
       
    }
}
