using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Implementations
{
    public class Authentication
    {
        public string MVApiKey = AuthValues.MVApiKey;
        public string MVSecretKey = AuthValues.MVSecretKey;
        public string gstin = AuthValues.gstin;
        public string eInvoiceUserName = AuthValues.eInvoiceUserName;
        public string eInvoicePassword = AuthValues.eInvoicePassword;
    }

    public class AuthentictionResponse
    {
        public string Status { get; set; }
        public object ErrorMessage { get; set; }
        public object ErrorCode { get; set; }
        public string AuthToken { get; set; }
        public string Sek { get; set; }
        public DateTime TokenExpiry { get; set; }

    }
}