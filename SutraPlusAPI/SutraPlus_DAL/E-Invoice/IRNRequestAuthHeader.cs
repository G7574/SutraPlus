using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Implementations
{
    public class IRNRequestAuthHeader
    {
        public String MVApiKey = AuthValues.MVApiKey;
        public String MVSecretKey = AuthValues.MVSecretKey;
        public String gstin = AuthValues.gstin;
        public String eInvoiceUserName = AuthValues.eInvoiceUserName;
        public String eInvoicePassword = AuthValues.eInvoicePassword;
        public String authToken;
        public String MonthYear = "ex. 05-2021";
        public String ContentType = "application/json";
    }
}