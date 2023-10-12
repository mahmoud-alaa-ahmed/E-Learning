using ModelLayer.DTO.UserDetailsDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models.Course
{
   public class Course:BaseModel
    {
        public Course()
        {
            Sections = new List<Section>();
            UserCourses = new List<UserCourseEnrollment>();
            CourseComments = new List<CourseComment>();
            UserCourseProgresses = new List<UserCourseProgress>();
        }
        [MaxLength(50)]
        public string Level { get; set; }
        [MaxLength(350)]
        public string Description { get; set; }
        [MaxLength(400)]
        public string Requirement { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Image { get; set; }
        [MaxLength(30)]
        public string Language { get; set; }
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<UserCourseEnrollment> UserCourses { get; set; }
        public virtual ICollection<CourseComment> CourseComments { get; set; }
        public virtual ICollection<UserCourseProgress> UserCourseProgresses { get; set; }

        public static implicit operator Course(UserEnrollCourseResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
