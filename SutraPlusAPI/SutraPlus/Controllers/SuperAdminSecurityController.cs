using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NLog;
using System.Collections.Generic;
using System;
namespace SutraPlus.Controllers
{
    //[EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Controller")]
    [ApiController]
    [Route("[controller]")]
    public class SuperAdminSecurityController : ControllerBase
    {
        private SuperAdminSecurityService _superAdminService;
        private CommonService _commonService;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private IConfiguration _configuration { get; }
        public SuperAdminSecurityController(MasterDBContext MasterDBContext, IConfiguration configuration, ILogger<SuperAdminSecurityController> logger)
        {
            _masterDBContext = MasterDBContext;
            _logger = logger;
            _configuration= configuration;
            _superAdminService = new SuperAdminSecurityService(_masterDBContext, _configuration, _logger);
            _commonService = new CommonService(_masterDBContext, _logger);

        }
        //Super Admin Section using master database
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate([FromBody] JObject Data)
        {
            try
            {
                var result = _superAdminService.Authenticate(Data);
                var login = JsonConvert.DeserializeObject<dynamic>(Data["UserDetails"].ToString());
                var encodedJwt = this.GenerateToken(result["UserEmailId"].ToString(), "SuperAdmin");
                var response = new { token = encodedJwt, result = result, Status = true };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }


        private async Task<IActionResult> GenerateToken(string userName, string roleCode)
        {
            try
            {
                if (userName != null && roleCode != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName", userName),
                        new Claim("UserRole", roleCode),
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
                    _logger.LogError("User Not Found (Password/Email wrong");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// forgot password will send the existing password if E-mail is correct
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] JObject Data)
        {
            try
            {
                var result = _superAdminService.ForgotPassword(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost("SaveEmailConfig")]
        [AllowAnonymous]
        public async Task<ActionResult> SaveEmailConfig([FromBody] JObject Data)
        {
            try
            {
                var config = JsonConvert.DeserializeObject<dynamic>(Data["EmailConfig"].ToString());
                var fromEmail = Convert.ToString(config["FromEmail"]);
                var password = Convert.ToString(config["Password"]);
                var emailServerHost = Convert.ToString(config["EmailServerHost"]);
                var emailServerPort = Convert.ToString(config["EmailServerPort"]);

                var result = _commonService.SaveEmailConfig(fromEmail, password, emailServerHost, emailServerPort);
                var response = new { result = result, Status = true };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        [HttpGet("GetEmailConfig")]
        [AllowAnonymous]
        public async Task<ActionResult> GetEmailConfig()
        {
            try
            {
                var result = _commonService.GetEmailConfig();
                var response = new { result = result, Status = true };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

    }
}
