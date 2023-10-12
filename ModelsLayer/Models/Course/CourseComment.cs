using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
    [PrimaryKey(nameof(UserId),nameof(CourseId),nameof(CreatedTime))]
    public class CourseComment
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string commentTxt { get; set; }
        public virtual Course Course { get; set; }
        public virtual CustomIdentityUser User { get; set; }
    }
}
