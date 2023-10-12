using AuthModel.Filters;
using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.CourseDtos.CommentDTOs;
using ModelLayer.Models.Course;
using System.Security.Claims;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseCommentController : ControllerBase
    {
        #region Fields
        private readonly IUnit unitOfWork;
        private readonly IAuth auth;
        #endregion

        #region Constructor
        public CourseCommentController(IUnit unitOfWork, IAuth auth)
        {
            this.unitOfWork = unitOfWork;
            this.auth = auth;
        }
        #endregion

        #region Get  Course Comments
        //[Authorize]
        //[SubscribtionValidation]
        [HttpGet]
        public async Task<IActionResult> GetCourseComments(int id)
        {
            var item = await unitOfWork.Course.GetByIdAsync(a => a.Id == id);
            if (item is null)
                return NotFound("The Course Not Found");
            var response = item.CourseComments.Select(async a => new CommentResponseDTO()
            {
                Comment = a.commentTxt,
                CreatedTime = DateTime.Now,
                UserName = await auth.GetUserNameById(a.UserId)
            });
            return Ok(response);
        }
        #endregion

        #region Add comment To Course
        
        [Authorize]
       // [SubscribtionValidation]
        [HttpPost]
        public async Task<IActionResult> Comment(CommentRequestDTO model)
        {
           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return BadRequest();
            if (await unitOfWork.UserCourseEnrollment.FindAnyAsync(a => a.CourseId == model.CourseId && a.UserId == userId) is null)
                return BadRequest(new { message = "you should Enroll The Course First" });
            var course = await unitOfWork.Course.GetByIdAsync(c=>c.Id==model.CourseId);
            if (course is null)
                return NotFound(new {message ="The Course You Try To Comment In Not Found"});
            var comment =await unitOfWork.CourseComments.AddAsync(new CourseComment()
            {
                commentTxt =model.CommentTxt,
                 CourseId =course.Id ,
                 UserId =userId,
                 CreatedTime =DateTime.Now,
            });
            await unitOfWork.SaveDataAsync();
            return Ok(new { comment = comment.commentTxt });
        }
        #endregion
    }
}
