using ModelLayer.Models;
using ModelLayer.Models.Course;

namespace DataLayer.Services.Interfaces
{
    public interface IUnit:IDisposable
    {
        IEntity<Course> Course { get; }
        IEntity<Section> Section { get; }
        IEntity<Lesson> Lesson { get; }
        IEntity<Quiz> Quiz { get; } 
        IEntity<Question> Question { get; }
        IEntity<Option> Option { get; }
        IEntity<UserSubscribtion> UserSubscribtion { get; }
        //IEntity<Subscribtion> Subscribtion { get; }
        IEntity<UserCourseEnrollment> UserCourseEnrollment { get; }
        IEntity<Instructor> Instructor { get; }
        IEntity<Category> Category { get; }
        IEntity<CourseComment> CourseComments { get; }

        Task<int> SaveDataAsync();
        
    }
}
