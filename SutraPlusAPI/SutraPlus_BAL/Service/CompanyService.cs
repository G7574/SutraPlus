using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
    public class CompanyService
    {
        public CompanyRepository _companyRepository = null;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public IConfiguration _configuration { get; }
        public string CompanyId ="0";
        public CompanyService(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _masterDBContext = masterDBContext;
            _logger = logger;
            _configuration = configuration;
            _companyRepository = new CompanyRepository(tenantID, masterDBContext, _configuration, _logger);
        }
        /// <summary>
        /// Common method to connect database
        /// </summary>
        /// <param name="Data"></param>
       
        public JObject List()
        {
            try
            {
                _logger.LogDebug("Company Service : Get()");
                return _companyRepository.List();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Get database name based on financial year and customer code
        /// </summary>
        /// <param name="customerFinancialYearId"></param>
        /// <param name="CustomerCode"></param>
       
        public Boolean Add(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["CompanyData"].ToString());
                _logger.LogDebug("Company Service : Add company data");
                Company company = new Company
                {
                    KannadaName = data["KannadaName"],
                    Shree = data["Shree"],
                    CompanyName = data["CompanyName"],
                    Title = data["Title"],
                    AddressLine1 = data["AddressLine1"],
                    Pan = data["Pan"],
                    Tan = data["Tan"],
                    Place = data["Place"],
                    Fln = data["Fln"],
                    Bin = data["Bin"],
                    District = data["District"],
                    Email = data["Email"],
                    State = data["State"],
                    CellPhone = data["CellPhone"],
                    ContactDetails = data["ContactDetails"],
                    Gstin = data["Gstin"],
                    FirmCode = data["FirmCode"],
                    Apmccode = data["Apmccode"],
                    Iec = data["Iec"],
                    Bank1 = data["Bank1"],
                    Ifsc1 = data["Ifsc1"],
                    AccountNo1 = data["AccountNo1"],
                    Bank2 = data["Bank2"],
                    Ifsc2 = data["Ifsc2"],
                    AccountNo2 = data["AccountNo2"],
                    Bank3 = data["Bank3"],
                    Ifsc3 = data["Ifsc3"],
                    Account3 = data["Account3"],
                    CreatedDate = DateTime.Now,
                    Logo = data["Logo"],
                    IsActive = true
                };
                return _companyRepository.Add(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetInvoiceKey(int companyId)
        {
            return _companyRepository.GetInvoiceKey(companyId);
        }
        public JObject Get(int companyId)
        {
            return _companyRepository.Get(companyId);
        }
        public Boolean Update(JObject Data)
        {
            return _companyRepository.Update(Data); 
        }
        public JObject GetCompanyEInvoice(int companyId)
        {
            return _companyRepository.GetCompanyEInvoice(companyId); 
        }
        public Boolean SaveEInvoice(JObject Data)
        {
            return _companyRepository.SaveEInvoice(Data); 
        }

        public JObject Search(JObject searchvalue)
        {
            var response = new JObject();
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(searchvalue["searchcriteria"].ToString());
                string serchvalue = data["searchvalue"];
                response = _companyRepository.Search(serchvalue);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public Boolean SaveAkadaEntry(JObject Data)
        {
            return _companyRepository.SaveAkadaEntry(Data);
        }
         public Boolean SaveOptionSettings(JObject Data)
        {
            return _companyRepository.SaveOptionSettings(Data);
        }
        public JObject GetOptionSettings(JObject Data)
        {
            return _companyRepository.GetOptionSettings(Data);
        }

    }
}
