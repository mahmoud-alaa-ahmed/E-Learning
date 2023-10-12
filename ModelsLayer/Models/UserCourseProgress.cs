using Microsoft.EntityFrameworkCore;
using ModelLayer.Models.Course;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    [PrimaryKey(nameof(UserId) ,nameof(LessonID),nameof(CourseId))]
    public class UserCourseProgress
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        [ForeignKey(nameof(Lesson))]
        public int LessonID { get; set; }
        public virtual CustomIdentityUser User { get; set; }
        public virtual Course.Course Course { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
