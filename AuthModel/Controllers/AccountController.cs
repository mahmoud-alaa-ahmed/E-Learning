using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ModelLayer.DTO.AuthDtos;
using DataLayer.Services.Interfaces;
using System.Security.Claims;

namespace AuthModel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Fields
        private readonly IAuth auth;
        #endregion

        #region Constructor
        public AccountController(IAuth auth)
        {
            this.auth = auth;
        }
        #endregion


        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = ModelState });
            var res = await auth.OnRegisterAsync(registerDTO);
            if (res.IsAuthenticated is null)
                return BadRequest(res.Message);
            return Ok(res);
        }
        #endregion


        #region Login(GetToken)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var res = await auth.OnLoginAsync(loginInfo);
            if (res.IsAuthenticated is null)
                return NotFound(new { message = res.Message });
            return Ok(res);
        }
        #endregion


        #region add role
        [Authorize(Roles = "Admin")]
        [HttpPost("setRole")]
        public async Task<IActionResult> SetRole(RoleSetModelDTO roleRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var res = await auth.OnSetRoleAsync(roleRequest);
            if (res != string.Empty)
                return NotFound(new { message = res });
            return Ok(new { message = "Role Added" });
        }
        #endregion

        #region[RefreshToken
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RefeshUserToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest();
            var user=await auth.FindUserAsync(userId);
            if (user is null)
                return NotFound(new { message = "Invalid User" });
           var token=await auth.CreateJWTAsync(user);
            return Ok(new {token=token});
        }

        #endregion


        #region Forget Password
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody][Required] string email)
        {
            if (email == null)
                return BadRequest();
            var request = await auth.ForgotPasswordAsync(email);
            if (!request)
                return NotFound(new { message = "Invalid Email" });
            return Ok(new { message = "Check Your Email to reset password" });
        }
        #endregion

        #region Reset Password
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody][Required] ResetPasswordModelDTO resetInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await auth.ResetPasswordAsync(resetInfo);
            if (!result)
                return BadRequest(new { message = "Invalid Email or Token" });
            return Ok(new { message = "Password Has been reset" });
        }

        #endregion

        #region Send Email
        [HttpPost("sendemail")]
        public IActionResult SendEmail([FromBody] EmailDTO emailInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            auth.SendEmail(emailInfo);
            return Ok(new { Message = "Sent" });
        }
        #endregion


    }
}
