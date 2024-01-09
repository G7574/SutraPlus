using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Data
{
    public class BaseRepository
    {
        public TenantDBContext _tenantDBContext;
        //private DBContext _DBContext;
        private MasterDBContext _masterDBContext;
        public BaseRepository(string connectionString, MasterDBContext masterDBContext)
        {
            if (null == _tenantDBContext)
            {
                _tenantDBContext = DBContextHandler.GetDBContext(connectionString);
            }
          //  _DBContext = dBContext;
            _masterDBContext = masterDBContext;
        }
        public BaseRepository(int tenantID, MasterDBContext masterDBContext)
        {
           // _DBContext = dBContext;
            _masterDBContext = masterDBContext;
            if (null == _tenantDBContext)
            {
                var customerFinancialYear = _masterDBContext.CustomerFinancialYears;
                //var userSession = _masterDBContext.UserSessions;

                var customerFinDetails = 
                        from cf in customerFinancialYear
                        where cf.Id == tenantID
                        select cf;

                var connectionString = customerFinDetails.FirstOrDefault() != null ? customerFinDetails.FirstOrDefault().DatabaseUri : null;
                if(connectionString != null)
                {
                    _tenantDBContext = DBContextHandler.GetDBContext(connectionString);
                }
            }
        }

    }

}
