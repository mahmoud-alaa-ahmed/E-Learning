using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Models;
using ModelLayer.Models.Course;

namespace DataLayer.Data
{
    public class AppDbContext:IdentityDbContext<CustomIdentityUser>
    {
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        //publ virtual c DbSet<Video> Videos { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        //public virtual DbSet<Subscribtion> Subscribtions { get; set; }
        public virtual DbSet<UserSubscribtion> UserSubscribtions { get; set; }
        public virtual DbSet<UserCourseEnrollment> UserCourseEnrollments { get; set; }
        public virtual DbSet<CourseComment> CourseComments { get; set; }  
        public virtual DbSet<UserCourseProgress> UserCourseProgress { get; set; }
        public  AppDbContext(DbContextOptions opts):base(opts)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Course>().Property(c => c.ReleaseDate).HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },  new IdentityRole() { Id = "2", Name = "User", NormalizedName = "USER" } );
            //builder.Entity<Subscribtion>().HasData(new Subscribtion() {Id=1, Name = "Three Months Subscribtion", DurationInDays = 30 }, new Subscribtion() {Id=2, Name = "Three Months Subscribtion", DurationInDays = 90 },new Subscribtion() {Id=3, Name="One Year Subscribtion " ,DurationInDays =360});
            //builder.Entity<UserSubscribtion>().HasKey(pk => new { pk.UserId, pk.SubscribtionId });
        }
    }
}
