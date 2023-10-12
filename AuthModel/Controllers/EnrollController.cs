using AuthModel.Filters;
using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.CourseDtos.UserCourses;
using ModelLayer.Models;
using System.Security.Claims;


namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollController : ControllerBase
    {
        #region Fields
        private readonly IUnit unitOfWork;
        #endregion

        #region Constructor
        public EnrollController(IUnit unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        } 
        #endregion
        
        #region Enroll
        [Authorize]
        //[SubscribtionValidation]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            //get the current user id
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return BadRequest();
            //check if the course id exists
            var course = await unitOfWork.Course.GetByIdAsync(a => a.Id == courseId);
            if (course is null) return BadRequest(new { message = "The Course You Try To Enroll Not Exists" });
            //check if the user already enrolled in this course before
            if (await unitOfWork.UserCourseEnrollment.FindAnyAsync(a => a.UserId == userId && a.CourseId == courseId) is not null)
                return BadRequest(new { message = "The User Already Enroll In This Course" });
            var savedItem = await unitOfWork.UserCourseEnrollment.AddAsync(new UserCourseEnrollment()
            {
                UserId = userId,
                CourseId = courseId,
                StartDate = DateTime.Now,
            });
            await unitOfWork.SaveDataAsync(); 
            return Ok(new { message = "Enrolled Successfully" });
        }
        #endregion
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return BadRequest();
            var userCourses = await unitOfWork.UserCourseEnrollment.GetAsync(c => c.UserId == userId);
            if (userCourses is null) return NotFound();
            var userCoursez=new List<UserEnrolledCoursesIdsDto>();
            foreach (var course in userCourses)
            {
                var userCourse = new UserEnrolledCoursesIdsDto();
                userCourse.CourseId = course.CourseId;
                userCoursez.Add(userCourse);
            }
            return Ok(userCoursez);
        }
    }
}
