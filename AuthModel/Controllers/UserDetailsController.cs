using DataLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.UserDetailsDtos;
using System.Security.Claims;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
		#region Fields
		private readonly IUnit _unit;
        private readonly IAuth _auth;
		#endregion

		#region Constructor
		public UserDetailsController(IUnit unit,IAuth auth)
        {
            _unit = unit;
            _auth=auth;
        }
		#endregion

		[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return BadRequest();
            var user=await _auth.FindUserAsync(userId);
            if(user == null) return NotFound();
            
            var userDetails = new UserDetailsResponse()
            {
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
               Courses=user.UserCourses.Select(c=>new UserEnrollCourseResponse()
               {
                   CourseId=c.Course.Id,
                   CourseImage=c.Course.Image,
                   CourseName=c.Course.Name,
               }).ToList()  
            };
            return Ok(userDetails);
        }
       
    }
}
