using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Web;
using SutraPlus.Utilities;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IHttpContextAccessor = Microsoft.AspNetCore.Http.IHttpContextAccessor;

namespace SutraPlus.Controllers
{
    [Route("UserSecurity")]
    [ApiController]
    public class UserSecurityController : CommonTenantController
    {
        private UserSecurityService _securityService;
        private MasterDBContext _masterDBContext;
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        //constructor contains IHttpContextAccessor used to access header value
        public UserSecurityController(MasterDBContext MasterDBContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,ILogger <UserSecurityController> logger) : base(httpContextAccessor)
        {
            _masterDBContext = MasterDBContext;
            _configuration = configuration;
            _logger = logger;
            _securityService = new UserSecurityService(Convert.ToInt32(base.tenantId), _masterDBContext, _configuration,_logger);
        }
        
        //For tenante DB connection
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromHeader] int tenantId, [FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.Authenticate(Data);
                if (result.TryGetValue("IsSuccess", out var isSuccessObj) && (bool)isSuccessObj)
                {
                    // Your code here if authentication is successful
                    var userEmailId = result.GetValue("UserEmailId").ToString();
                    var userType = result.GetValue("UserType").ToString();
                    var userId = result.GetValue("UserId").ToString();

                    var login = JsonConvert.DeserializeObject<dynamic>(Data["UserDetails"].ToString());
                    var encodedJwt = this.GenerateToken(result["UserEmailId"].ToString(), result["UserType"].ToString(), result["UserId"].ToString());
                    var response = new { token = encodedJwt, result = result, Status = true };
                    return Ok(response);
                }
                else {

                    var response = new { result = result, Status = false };
                    return Ok(response);
                    }
                
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> GenerateToken(string userName, string roleCode, string UserID)
        {
            if (userName != null && roleCode != null)
            {
                var claims = new[] {
                                     new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                     new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                     new Claim("UserName", userName),
                                     new Claim("UserRole", roleCode),
                                     new Claim("UserID", UserID),
                                     };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Change password with username, current password, new password
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Update")] //previous was Set
        [AllowAnonymous]
        public async Task<ActionResult> UpdatePassword([FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.Authenticate(Data); //check user exist or no
                if (result != null)
                {
                    var response = _securityService.UpdatePassword(Data);
                    return Ok(true);
                }
                else
                {
                    return Ok("User Not Found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// forgot password will send the existing password if E-mail is correct
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> ForGotPassword([FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.Forgot(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
 
        /// <summary>
        /// Validate OTP & update email id of customer
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("ValidateOTP")]
        [AllowAnonymous]
        public async Task<ActionResult> ValidateOTP([FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.ValidateOTP(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Admin / User can change password
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword([FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.Admin_ChangePassword(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangePasswordforUser")]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePasswordforUser([FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.User_ChangePassword(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("GetUserData")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser([FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.GetUser(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateUserData")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUserData([FromBody] JObject Data)
        {
            try
            {
                var result = _securityService.UpdateUser(Data);
                  
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

    }
}