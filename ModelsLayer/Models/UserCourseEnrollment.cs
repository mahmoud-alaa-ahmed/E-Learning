using Castle.Components.DictionaryAdapter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    [PrimaryKey(nameof(CourseId ) ,nameof(UserId))]
    public class UserCourseEnrollment
    {
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public virtual Course.Course Course { get; set; }
        public virtual CustomIdentityUser User { get; set; }
    }
}
