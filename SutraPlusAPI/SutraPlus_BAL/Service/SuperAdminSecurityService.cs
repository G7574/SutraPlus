
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_BAL.Service
{
    public class SuperAdminSecurityService
    {
        public SuperAdminSecurityRepository _superAdminRepository = null;
        public CommonRepository _commonRepository = null;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        //  private MasterDBContext _masterDBContext;

        public SuperAdminSecurityService(MasterDBContext masterDB, IConfiguration _configuration, ILogger logger)
        {
            _logger = logger;
            _superAdminRepository = new SuperAdminSecurityRepository(masterDB, _configuration, _logger);
            _commonRepository = new CommonRepository(masterDB, _logger);
        }
        public JObject Authenticate(JObject Data)
        {
            try
            {
                _logger.LogDebug("Authenticate User");
                var login = JsonConvert.DeserializeObject<dynamic>(Data["UserDetails"].ToString());
                var customerFinancialYearId = Convert.ToString(login["CustomerFinancialYearId"]);
                var userEmailId = Convert.ToString(login["UserEmailId"]);
                var password = Convert.ToString(login["Password"]);
                return _superAdminRepository.Authenticate(userEmailId, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject ForgotPassword(JObject Data)
        {
            try
            {
                _logger.LogDebug("Super Admin Forgot Password Service");
                var data = JsonConvert.DeserializeObject<dynamic>(Data["EmailData"].ToString());
                var Email = Convert.ToString(data["Email"]);
                return _superAdminRepository.ForgotPassword(Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }

        }

    }
}
