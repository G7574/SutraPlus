using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Data
{
    internal class DBContextHandler
    {
        private static TenantDBContext _tenantdbContext = null;
        private DBContextHandler()
        {
            //singleton class. i prefer this way.
            // openWriter();
        }

        ~DBContextHandler()
        {
            //closeWriter();
        }


        //check & create dbContext for TenantDB
        public static TenantDBContext GetDBContext(string connectionString)
        {
            if (null == _tenantdbContext)
            {
                _tenantdbContext = new TenantDBContext(connectionString);
            }
            else
            {
                _tenantdbContext = new TenantDBContext(connectionString);
            }

            return _tenantdbContext;
        }

    }

}
