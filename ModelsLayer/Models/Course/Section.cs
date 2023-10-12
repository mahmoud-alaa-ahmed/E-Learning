

namespace ModelLayer.Models.Course
{
    public class Section:BaseModel
    {
        public Section()
        {
            Lessons = new List<Lesson>();
        }
        public int OrderInCourse { get; set; }
        public int CourseId { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
