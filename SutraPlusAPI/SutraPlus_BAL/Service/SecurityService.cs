using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_BAL.Service
{
    public class SecurityService
    {
        public SecurityRepository _securityRepository = null;
        private MasterDBContext _masterDBContext;

        public SecurityService(DBContext DBContext, MasterDBContext MasterDBContext)
        {
            _masterDBContext = MasterDBContext;
            //_securityRepository = new CustomerRepository(DBContext, _masterDBContext);
        }
        public JObject Authenticate(JObject responseData)
        {
            var login = JsonConvert.DeserializeObject<dynamic>(responseData["UserDetails"].ToString());
            var customerFinancialYearId = Convert.ToString(login["CustomerFinancialYearId"]);
            var userEmailId = Convert.ToString(login["UserEmailId"]);
            var password = Convert.ToString(login["Password"]);
            GetTenantDB(int.Parse(customerFinancialYearId));
            return _securityRepository.Authenticate(userEmailId, password);
        }
        public void GetTenantDB(int customerFinancialYearId)
        {
            var connectionString = _masterDBContext.CustomerFinancialYears.Where(x => x.Id == customerFinancialYearId && x.IsActive == true).Select(c => c.DatabaseUri).FirstOrDefault();
            _securityRepository = new SecurityRepository(connectionString);
        }
    }
}
