using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SutraPlus_BAL.Service
{
    public class UserSecurityService
    {
        public IConfiguration _configuration { get; }
        public UserSecurityRepository _securityRepository = null;
        public CommonRepository _commonRepository = null;
        public MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public UserSecurityService(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _masterDBContext = masterDBContext;
            _logger = logger;
            _configuration = configuration;
            _commonRepository = new CommonRepository(masterDBContext, logger);
            _securityRepository = new UserSecurityRepository(tenantID, masterDBContext, _configuration, _logger);
        }
        public JObject Authenticate(JObject Data)
        {
            try
            {
                var login = JsonConvert.DeserializeObject<dynamic>(Data["UserDetails"].ToString());
                var userEmailId = Convert.ToString(login["UserEmailId"]);
                var password = Convert.ToString(login["Password"]);
                return _securityRepository.Authenticate(userEmailId, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public bool UpdatePassword(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["UserDetails"].ToString());
                var userEmailId = Convert.ToString(data["UserEmailId"]);
                var newPassword = Convert.ToString(data["Password"]);
                return _securityRepository.UpdatePassword(userEmailId, newPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject Forgot(JObject Data)
        {
            try
            {
                var response = new JObject();
                var data = JsonConvert.DeserializeObject<dynamic>(Data["EmailData"].ToString());
                var Email = Convert.ToString(data["Email"]);
                response = _securityRepository.Forgot(Email);
                if (response != null)
                {
                    OTPTrans oTPTrans = new OTPTrans
                    {
                        CustomerId = data["CustomerCode"],
                        OTP = Convert.ToString(response["OTP"]),
                        CreatedDate = DateTime.Now,
                        ExpireDate = DateTime.Now.AddMinutes(5),
                        IsActive = true
                    };
                    var result = _masterDBContext.OTPTrans.Add(oTPTrans);
                    _masterDBContext.SaveChanges();
                    return response;
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public Boolean ValidateOTP(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["OTPValidate"].ToString());
                string customerId = Convert.ToString(data["CustomerId"]);
                string OTP = Convert.ToString(data["OTP"]);
                var result = _securityRepository.ValidateOTP(customerId, OTP);
                //TODO check mail is working or not
                EmailSender.SendMailMessage_Mail_OTP(_commonRepository.GetEmailConfig(),Convert.ToString(data["NewMail"]), "New Mail Id OTP", "", "");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool Admin_ChangePassword(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["UserDetails"].ToString());
                var userEmailId = Convert.ToString(data["Email"]);
                var newPassword = Convert.ToString(data["NewPassword"]);
                return _securityRepository.Admin_ChangePassword(userEmailId, newPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}
