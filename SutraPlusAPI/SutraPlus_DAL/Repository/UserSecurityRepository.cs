using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus.Common;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Repository
{
    public class UserSecurityRepository : BaseRepository
    {
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private MasterDBContext _masterDBContext; 

        private readonly CommonRepository _commonRepository;

        public UserSecurityRepository(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger) : base(tenantID, masterDBContext)
        {
            _logger = logger;
            _configuration = configuration;
            _masterDBContext = masterDBContext;
            _commonRepository = new CommonRepository(masterDBContext, logger);
        }
        public JObject Authenticate(string userEmail, string password)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("User Authenticate (Login)");
                var result = _tenantDBContext.Users.Where(a => a.UserName == userEmail && a.Password == password && a.IsActive == true).FirstOrDefault();
                if (result != null)
                {
                    response.Add("UserEmailId", result.UserName);
                    response.Add("UserType", result.UserType);
                    response.Add("IsSuccess", true);
                    response.Add("UserId", result.UserId);
                    return response;
                }
                response.Add("IsSuccess", false);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetUser(string userEmail)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("User Authenticate (Login)");
                var result = _tenantDBContext.Users.Where(a => a.UserName == userEmail && a.IsActive == true).FirstOrDefault();
                if (result != null)
                {
                    response.Add("FirstName", result.FirstName);
                    response.Add("LastName", result.LastName);
                    response.Add("IsSuccess", true);
                    response.Add("PhoneNo", result.PhoneNo);
                    response.Add("ProfileImage", result.ProfileImage);
                    return response;
                }
                response.Add("IsSuccess", false);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public bool UpdatePassword(string userEmail, string password)
        {
            try
            {
                User user;
                user = _tenantDBContext.Users.Where(u => u.UserName == userEmail).First();
                user.Password = password;
                _tenantDBContext.SaveChanges();
                _tenantDBContext.Update(user);
                _logger.LogDebug("User Change Password Done");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public bool UpdateUser(string userEmail, string firstname,string lastname,string mobile, IFormFile profileImage)
        {
            try
            {
                User user;
                user = _tenantDBContext.Users.Where(u => u.UserName == userEmail).First();  
                
                user.FirstName = firstname;
                user.LastName = lastname;
                user.PhoneNo = mobile; 

                if (profileImage != null && profileImage.Length > 0)
                {
                    var directoryPath = "userProfileImages";

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var path = Path.Combine(directoryPath, profileImage.FileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        profileImage.CopyToAsync(stream);
                        stream.Close();
                    }

                    user.ProfileImage = "userProfileImages/" + profileImage.FileName;
                }

                _tenantDBContext.SaveChanges();
                _tenantDBContext.Update(user);
                _logger.LogDebug("User Update Done");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                //throw ex;
                return false;
            }
             
        }
        
        public JObject getUserProfilePicture(string email)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("User Authenticate (Login)");
                var result = _tenantDBContext.Users.Where(a => a.UserName == email && a.IsActive == true).FirstOrDefault();
                if (result != null)
                { 
                    response.Add("ProfileImage", result.ProfileImage);
                    return response;
                }
                response.Add("IsSuccess", false);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject Forgot(string Email)
        {
            var response = new JObject();
            try
            {
                Random rnd = new Random();
                string randomNumber = (rnd.Next(100000, 999999)).ToString();
                var result = _tenantDBContext.Users.Where(a => a.UserName == Email).FirstOrDefault();
                _masterDBContext.EmailConfig.FirstOrDefault();
                if (result != null)
                {
                    EmailSender.SendMailMessage_Admi_ForgotPass_OTP(_commonRepository.GetEmailConfig(),Email, "Your OTP", "", randomNumber);
                    response.Add("UserName", Email);
                    response.Add("OTP", randomNumber);
                    response.Add("IsSuccess", true);
                    _logger.LogDebug("User Change Password OTP Send Successfully to User's mail");
                    return response;
                }
                response.Add("IsSuccess", false);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public Boolean ValidateOTP(string customerId, string OTP)
        {
            MasterDBContext _masterDBContext = new MasterDBContext();
            var response = new JObject();
            try
            {
                var result = _masterDBContext.OTPTrans.FirstOrDefault(t => t.OTP == OTP && t.CustomerId == customerId && DateTime.Now <= t.ExpireDate);
                if (result != null)
                {
                    _logger.LogDebug("OTP Validation Succeed");
                    return true;
                }
                else
                {
                    _logger.LogDebug("OTP Validation Not-Succeed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public bool Admin_ChangePassword(string userEmail, string password)
        {
            try
            {
                var entity = _tenantDBContext.Users.Where(u => u.UserName == userEmail).FirstOrDefault();
                if (entity != null)
                {
                    if(entity.Password == password)
                    entity.Password = password;
                    _tenantDBContext.SaveChanges();
                    _tenantDBContext.Update(entity);
                    _logger.LogDebug("Admin Change Password Succeed");
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }

        }
        public bool User_ChangePassword(string userEmail, string password,string oldPassword)
        {
            try
            {
                var entity = _tenantDBContext.Users.Where(u => u.UserName == userEmail).FirstOrDefault();
                if (entity != null)
                {
                    if (entity.Password == oldPassword)
                    {
                        entity.Password = password;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entity);
                        _logger.LogDebug("Admin Change Password Succeed");
                        return true;

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }

        }
    }
}
