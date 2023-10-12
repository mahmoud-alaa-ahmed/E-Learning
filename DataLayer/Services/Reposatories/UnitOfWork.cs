using DataLayer.Data;
using DataLayer.Services.Interfaces;
using ModelLayer.Models;
using ModelLayer.Models.Course;

namespace DataLayer.Services.Reposatories
{
    public class UnitOfWork : IUnit
    {
       private readonly AppDbContext _context;
        public IEntity<Course> Course { get;private set; }
        public IEntity<Section> Section { get; private set; }
        public IEntity<Lesson> Lesson { get; private set; }
        public IEntity<Quiz> Quiz { get; private set; }
        public IEntity<Question> Question { get; private set; }
        public IEntity<Option> Option { get; private set; }
        public IEntity<UserSubscribtion> UserSubscribtion { get; private set; }
        //public IEntity<Subscribtion> Subscribtion { get; private set; }
        public IEntity<UserCourseEnrollment> UserCourseEnrollment { get; private set; }
        public IEntity<Instructor> Instructor { get; private set; }
        public IEntity<Category> Category { get; private set; }
        public IEntity<CourseComment> CourseComments { get; private set; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Course=new EntityRepository<Course>(_context);
            Section=new EntityRepository<Section>( _context);
            Lesson=new EntityRepository<Lesson>( _context);
            Quiz=new EntityRepository<Quiz>( _context);
            Question=new EntityRepository<Question>( _context);
            Option=new EntityRepository<Option>( _context);
            //Subscribtion =new EntityRepository<Subscribtion>( _context);
            UserSubscribtion = new EntityRepository<UserSubscribtion>( _context);
            UserCourseEnrollment = new EntityRepository<UserCourseEnrollment>( _context);
            Instructor = new EntityRepository<Instructor>( _context);
            Category = new EntityRepository<Category>( _context);
            CourseComments = new EntityRepository<CourseComment>( _context);
        }

        public void  Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveDataAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
