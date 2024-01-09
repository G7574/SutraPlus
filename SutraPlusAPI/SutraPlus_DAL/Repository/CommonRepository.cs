using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Repository
{
    public class CommonRepository
    {
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public CommonRepository(MasterDBContext masterDBContext, ILogger logger)
        {
            this._masterDBContext = masterDBContext;
            _logger = logger;
        }
        public JObject GetStates()
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("inside getstatus repository");
                var result = (from s in _masterDBContext.StateMaster
                              where s.IsActive == true
                              select new { s.Id, s.Statecode, s.Statename }).OrderBy(s => s.Statename).ToList();
                if (result != null)
                {
                    response.Add("StateList", JArray.FromObject(result));
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        //TODO: Change this method as per GetStates (done)
        public JObject GetFinancialYears()
        {
            var response = new JObject();
            try
            {
                var result = (from s in _masterDBContext.YearMaster
                              where s.IsActive == true
                              select new { s.Id, s.Year, s.FinYear }).OrderBy(s => s.Year).ToList();
                if (result != null)
                {
                    response.Add("FinancialYear", JArray.FromObject(result));
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        //TODO:: Change this method as per GetStates (done)
        public JObject GetCounties()
        {
            var response = new JObject();
            //var context = new MasterDBContext();
            try
            {
                var result = (from c in _masterDBContext.Countries
                              where c.IsActive == true
                              select new { c._Id, c.CountryName }).ToList().DistinctBy(c => new { c.CountryName });

                if (result != null)
                {
                    response.Add("CountryDDList", JArray.FromObject(result));
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }


        public JObject SaveEmailConfig(string fromEmail, string password, string emailServerHost, string emailServerPort)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("Save Email Config");
                var existingConfig = _masterDBContext.EmailConfig.FirstOrDefault();

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
                        EmailServerPort = emailServerPort
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
                var result = _masterDBContext.EmailConfig.FirstOrDefault();

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

