using DataLayer.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using ModelLayer.DTO.AuthDtos;
using ModelLayer.Helper;
using ModelLayer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataLayer.Services.Reposatories
{
    public class AuthenticationServices : IAuth
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private IConfiguration _conf;
        private readonly IUnit _unit;

        public AuthenticationServices(UserManager<CustomIdentityUser> userManager, IConfiguration conf, RoleManager<IdentityRole> roleManager,IUnit unit)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _conf = conf;
            _unit = unit;
        }

        public async Task<IdentityResult> UpdataUserData(CustomIdentityUser user)
        {
            var res = await _userManager.UpdateAsync(user);
            return res;
        }
        public async Task<string> GetUserNameById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user is null ? null : user.UserName;
        }
        public async Task<CustomIdentityUser> FindUserByEmailAsync(string email)
        {
            var user =await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<CustomIdentityUser> FindUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<AuthModel> OnRegisterAsync(RegisterDTO regInfo)
        {
            AuthModel result = new AuthModel();
            var ExistUser = await _userManager.FindByEmailAsync(regInfo.Email);
            var emailPattern = EmailValidation.CheckEmailRegex(regInfo.Email);
            if (!emailPattern)
            {
                result.Message = "Invalid Email Address";
                return result;
            }
            if (ExistUser is not null)
            {
                result.Message = "Email Already Taken";
                return result;
            }
           
            CustomIdentityUser user = new CustomIdentityUser()
            {
                FirstName = regInfo.FirstName,
                LastName = regInfo.LastName,
                UserName = regInfo.UserName,
                Email = regInfo.Email,
            };
            IdentityResult res = await _userManager.CreateAsync(user, regInfo.Password);
            if (!res.Succeeded)
            {
                StringBuilder str = new StringBuilder();
                foreach (var err in res.Errors)
                {
                    str.Append(err.Description + Environment.NewLine);
                }
                return new AuthModel { Message = str.ToString() };
            }
            await _userManager.AddToRoleAsync(user, "User");
            var userRoles = await _userManager.GetRolesAsync(user);
            return new AuthModel
            {
                Message = "Created Successfully",
                IsAuthenticated = true,
                Roles = userRoles.ToList(),
                Email = user.Email,
                UserName = user.UserName,
                Token = await CreateJWTAsync(user)
        };

        }
      
        public async Task<AuthModel> OnLoginAsync(LoginDTO logInfo)
        {
            var result = new AuthModel();
            var user = await _userManager.FindByEmailAsync(logInfo.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, logInfo.Password))
            {
                result.Message = "Incorrect Email or Password";
                return result;
            }
            var userToken = await CreateJWTAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            return new AuthModel()
            {
                Message = "login Success",
                Email = user.Email,
                UserName = user.UserName,
                Roles = userRoles.ToList(),
                IsAuthenticated = true,
                Token = userToken
            };
        }
      
        public async Task<string> CreateJWTAsync(CustomIdentityUser user)
        {
            string Status = "false";
            DateTime? EndDate = DateTime.Now.AddDays(7);
            var subscripedUser = await _unit.UserSubscribtion.GetByIdAsync(u => u.CustomerId == user.CustomerId);
            if(subscripedUser != null && subscripedUser.Status== "active")
            {
                Status = subscripedUser.Status;
                EndDate = subscripedUser.EndDate;
            };
            
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = new List<Claim>()
         {
             new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
             new Claim(ClaimTypes.NameIdentifier,$"{user.Id}"),
             new Claim("Status",Status),
         };
            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var identity = new ClaimsIdentity(userClaims);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_conf.GetSection("JWT:Key").Value));

            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = EndDate ,   // DateTime.Now.AddDays(double.Parse(_conf.GetSection("JWT:DurationInDays").Value)),
                SigningCredentials = credintials,
                
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> OnSetRoleAsync(RoleSetModelDTO requestInfo)
        {
            var existUser = await _userManager.FindByEmailAsync(requestInfo.Email);
            if (existUser is null || !await _roleManager.RoleExistsAsync(requestInfo.Role))
            {
                return "Invalid User Or Role!";
            }
            if (await _userManager.IsInRoleAsync(existUser, requestInfo.Role))
                return $"User Already Have The Role {requestInfo.Role} ";
            var res = await _userManager.AddToRoleAsync(existUser, requestInfo.Role);
            return res.Succeeded ? string.Empty : "Request Failed";
        }

        public void SendEmail(EmailDTO emailObj)
        {
            var email = new MimeMessage();
            email.To.Add(MailboxAddress.Parse(emailObj.Email));
            email.From.Add(MailboxAddress.Parse(_conf.GetSection("EmailConfig:email").Value));
            email.Subject = emailObj.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailObj.Body };
            using var smtp = new SmtpClient();
            smtp.Connect(_conf.GetSection("EmailConfig:smtp").Value,
                int.Parse(_conf.GetSection("EmailConfig:port").Value), SecureSocketOptions.StartTls);
            smtp.Authenticate(_conf.GetSection("EmailConfig:email").Value, _conf.GetSection("EmailConfig:pw").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return false;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var encodedToken = Encoding.UTF8.GetBytes(token);
            //var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            //var url = $"{_conf.GetSection("Url").Value}/resetpassword?{email}&token={validToken}";
            EmailDTO emailRequest = new EmailDTO()
            {
                Email = user.Email,
                Subject = "Reset Password",
                Body = $"<h4>Use This Token To Reset your password:\n{token}</h4>"
            };
            SendEmail(emailRequest);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModelDTO modelInfo)
        {
            var user = await _userManager.FindByEmailAsync(modelInfo.Email);
            if (user == null) return false;
            var res = await _userManager.ResetPasswordAsync(user, modelInfo.Token, modelInfo.Password);
            if (!res.Succeeded)
                return false;
            return true;
        }
    }
}
