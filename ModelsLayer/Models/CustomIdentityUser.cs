using Microsoft.AspNetCore.Identity;
using ModelLayer.Models.Course;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Models
{
    public class CustomIdentityUser:IdentityUser
    {
        [Required,MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        public string? CustomerId { get; set; }
        public virtual ICollection<UserCourseEnrollment> UserCourses { get; set; }
        public virtual ICollection<CourseComment> Comments { get; set; }
        public virtual ICollection<UserCourseProgress> UserCourseProgresses { get; set; }
        public CustomIdentityUser() 
        {
            UserCourses = new List<UserCourseEnrollment>();
            Comments = new List<CourseComment>();
            UserCourseProgresses = new List<UserCourseProgress>();
        }
    }
}
