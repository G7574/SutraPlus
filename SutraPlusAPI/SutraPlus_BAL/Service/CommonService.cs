using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_BAL.Service
{
    public class CommonService
    {
        public CommonRepository _commonRepository = null;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public CommonService(MasterDBContext masterDB, ILogger logger)
        {
            _logger = logger;   
            _commonRepository = new CommonRepository(masterDB, _logger);
            _masterDBContext = masterDB;
        }
        public JObject GetStates()
        {
            var result = new JObject();
            try
            {
                _logger.LogDebug("inside getstatus service");
                 result = _commonRepository.GetStates();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
            return result;
        }
        //TODO: Change per GetStates (Done)
        public JObject GetFinancialYears()
        {
            var result = new JObject();
            try
            {
                result = _commonRepository.GetFinancialYears();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
            return result;
        }
        //TODO: Change per GetStates (Done)
        public JObject GetCounties()
        {
            var result = new JObject();
            try
            {
                result = _commonRepository.GetCounties();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
            return result;
        }


        public JObject SaveEmailConfig(string fromEmail, string password, string emailServerHost, string emailServerPort)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("Save Email Config");
                //var existingConfig = _masterDBContext.EmailConfig.FirstOrDefault();

                var existingConfig = (from s in _masterDBContext.EmailConfig
                                      where s.IsActive == true select s ).FirstOrDefault();

                if (existingConfig != null)
                {
                    // Update existing configuration
                    existingConfig.FromEmail = fromEmail;
                    existingConfig.Password = password;
                    existingConfig.EmailServerHost = emailServerHost;
                    existingConfig.EmailServerPort = emailServerPort;

                    _masterDBContext.EmailConfig.Update(existingConfig);
                }
                else
                {
                    // Create new configuration
                    var newConfig = new EmailConfig
                    {
                        FromEmail = fromEmail,
                        Password = password,
                        EmailServerHost = emailServerHost,
                        EmailServerPort = emailServerPort,
                        PreparedDate = DateTime.Now,
                        IsActive=true
                    };
                    _masterDBContext.EmailConfig.Add(newConfig);
                }

                _masterDBContext.SaveChanges();

                response.Add("FromEmail", fromEmail);
                response.Add("Password", password);
                response.Add("EmailServerHost", emailServerHost);
                response.Add("EmailServerPort", emailServerPort);
                response.Add("IsSuccess", true);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject GetEmailConfig()
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("Get Email Config");
                var result = (from s in _masterDBContext.EmailConfig
                                      where s.IsActive == true
                                      select s).FirstOrDefault();

                if (result != null)
                {
                    response.Add("FromEmail", result.FromEmail);
                    response.Add("Password", result.Password);
                    response.Add("EmailServerHost", result.EmailServerHost);
                    response.Add("EmailServerPort", result.EmailServerPort);
                    response.Add("IsSuccess", true);
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
    }
}
