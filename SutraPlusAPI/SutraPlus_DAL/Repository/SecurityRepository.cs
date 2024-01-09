using Newtonsoft.Json;
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
    public class SecurityRepository : BaseRepository
    {
        public SecurityRepository(string connectionString) : base(connectionString, null)
        {
        }
        public JObject Authenticate(string userEmail, string password)
        {
            var result = _tenantDBContext.Users.Where(a => a.UserName == userEmail && a.Password == password && a.IsActive == true).FirstOrDefault();
            var response = new JObject();
            if (result != null)
            {
                response.Add("UserEmailId", result.UserName);
                response.Add("UserType", result.UserType);
                response.Add("IsSuccess", true);
                return response;
            }
            response.Add("UserEmailId", result.UserName);
            response.Add("IsSuccess", false);
            return response;
        }
    }
}
