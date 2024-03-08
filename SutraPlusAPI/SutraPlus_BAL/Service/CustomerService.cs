using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus.Models;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_BAL.Service
{
    public class CustomerService
    {
        public IConfiguration _configuration { get; }
        public CustomerRepository _customerRepository = null;
        public CommonRepository _commonRepository = null;
        private MasterDBContext _masterDBContext;
        private EmailSender _sender;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public CustomerService(MasterDBContext masterDB, ILogger logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _masterDBContext = masterDB;
            _sender = new EmailSender(_configuration);
            _customerRepository = new CustomerRepository(masterDB, _logger, _configuration);
            _commonRepository = new CommonRepository(masterDB, _logger);

        }

        public JObject GetYearList(string CustomerId)
        {
            var result = _customerRepository.GetYearList(CustomerId);
            return result;
        }

        public int GetMax()
        {
            try
            {
                return _masterDBContext.Customers.Select(x => x.Id).ToList().Max();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public string Add(JObject Data)
        {
            try
            {
                _logger.LogDebug("Customer Service : Adding new customer");
                var data = JsonConvert.DeserializeObject<dynamic>(Data["Customer"].ToString());
                dynamic yeardata = JsonConvert.DeserializeObject<dynamic>(Data["YearData"].ToString());
                var Id = GetMax();
                int LinkId = Id + 1;
                string Name = "", Email = "", Mob = "", FirstName = "", LastName = "", PhoneNo = "";
                Name = data["Name"];
                Email = data["Email"];
                Mob = data["Mobile"];
                string Password = _configuration.GetSection("TenantServer:InitialPass").Value.ToString() + Mob.Substring(0, 5);
                FirstName = Convert.ToString(data["FirstName"]);
                LastName = Convert.ToString(data["LastName"]);
                PhoneNo = Convert.ToString(data["Mobile"]);
                Customer customer = new Customer
                {
                    Name = data["Name"],
                    Code = data["Code"],
                    Address = data["Address"],
                    City = data["City"],
                    State = data["State"],
                    Pin = data["Pin"],
                    Mobile = data["Mobile"],
                    Email = data["Email"],
                    GSTNo = data["GSTNo"],
                    ContactPerson = data["ContactPerson"],
                    FirstName = data["FirstName"],
                    LastName = data["LastName"],
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                //get id for new inserted customer 
                int Cust_Id = _customerRepository.Add(customer);

                if (Cust_Id == 0)
                {
                    return "E-mail already exist...!";
                }
                else
                {
                    // insert loop data in CustomerFinancialYear
                    string webUrl = "";
                    foreach (var item in yeardata)
                    {
                        //string DbName = "", finyear = "", startYear = "", endYear = "";
                        string finyear = "", startYear = "", endYear = "";
                        string DbName = "K" + Convert.ToString(item["FinYear"]).Substring(2, 2) + Convert.ToString(item["FinYear"]).Substring(7, 2) + data["Code"];

                        //CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2)

                        //if (item["FinYear"])

                        string themeCode = "", backCode = "";
                        themeCode = _customerRepository.GetThemeCode(Convert.ToString(item["Year"]), "Theme");
                        backCode = _customerRepository.GetThemeCode(Convert.ToString(item["Year"]), "Back");
                        webUrl = _configuration.GetSection("TenantServer:WebUrl").Value.ToString() + data["Code"];
                        //webUrl = webUrl + data["Code"];
                        finyear = item["FinYear"];
                        startYear = finyear.Substring(2, 2);
                        endYear = finyear.Substring(7, 2);
                        //DbName = _configuration.GetSection("TenantServer:DatabaseInitail").Value.ToString() + startYear + endYear + data["Code"];
                        DateTime dtfrm, dtto;
                        string startDate = "", endDate = "";
                        startDate = item["Year"] + _configuration.GetSection("TenantServer:StartDate").Value.ToString();
                        endDate = (Convert.ToInt64(Convert.ToString(item["Year"])) + 1) + _configuration.GetSection("TenantServer:EndDate").Value.ToString();
                        dtfrm = DateTime.Parse(startDate);
                        dtto = DateTime.Parse(endDate);
                        CustomerFinancialYear customerFinancialYear = new CustomerFinancialYear
                        {
                            //CustomerId = Cust_Id.ToString(),
                            CustomerId = data["Code"],
                            Year = item["Year"],
                            StartDate = dtfrm,
                            EndDate = dtto,
                            Description = item["FinYear"],
                            DatabaseUri = _configuration.GetSection("TenantServer:ServerName").Value.ToString() + ";Database=" + DbName + ";User Id=" + _configuration.GetSection("TenantServer:DatabaseUser").Value.ToString() + ";Password=" + _configuration.GetSection("TenantServer:DatabasePassword").Value.ToString() + ";TrustServerCertificate=True;",
                            ThemeCode = themeCode,
                            BackCode = backCode,
                            WebUrl = webUrl,
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        };
                        var findata = _customerRepository.Add(customerFinancialYear);
                        //commented line will create all databases which are present in the financial year
                        //_customerRepository.CreateDatabase(Cust_Id, DbName, Email, Password,FirstName,LastName,PhoneNo);
                    }
                    //loop each above table & create multi databases in master database
                    //insert admon user login details for each database created above

                    //_sender.SendMailMessage(_commonRepository.GetEmailConfig(),Email, "Customer Login Creation...", "", customer, webUrl, Email, Password);
                    return "Customer Added Successfully...!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
            //_logger.LogDebug("Company Service : Add company data");

        }

        public pagination<Customer> List(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data["SearchText"]);
                var page = JsonConvert.DeserializeObject<pagination<Customer>>(Convert.ToString(Data["Page"]));
                var result = _customerRepository.List(page, searchText);

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject Get(int Customer)
        {
            try
            {
                var result = _customerRepository.Get(Customer);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new JObject();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool Update(JObject Data)
        {
            try
            {
                _customerRepository.Update(Data);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool Delete(JObject Data)
        {
            try
            {
                _customerRepository.Delete(Data);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool Activate(JObject Data)
        {
            try
            {
                _customerRepository.Activate(Data);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject Get()
        {
            try
            {
                return _customerRepository.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public string GenerateOTP(JObject Data)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(Data["OTPGenerate"].ToString());
            Random rnd = new Random();
            string randomNumber = (rnd.Next(100000, 999999)).ToString();
            OTPTrans oTPTrans = new OTPTrans
            {
                CustomerId = data["CustomerId"],
                OTP = randomNumber,
                CreatedDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMinutes(5),
                IsActive = true
            };
            var result = _customerRepository.GenerateOTP(oTPTrans);
            ;
            //TODO Email template for send OTP 
            EmailSender.SendMailMessage_Mail_OTP(_commonRepository.GetEmailConfig(), Convert.ToString(data["NewMail"]), "New Mail Id OTP", "", randomNumber);
            _logger.LogDebug("Customer Service : Customer creation mail sent for OTP");
            return result;
        }

        public Boolean ValidateOTP(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["OTPValidate"].ToString());
                string customerId = data["CustomerId"];
                var result = _customerRepository.ValidateOTP(customerId, Convert.ToString(data["OTP"]), Convert.ToString(data["NewMail"]));
                //EmailSender.SendMailMessage_Mail_OTP(Convert.ToString(data["NewMail"]), "New Mail Id OTP", "", "");
                _logger.LogDebug("Customer Service : OTP Validated");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject SearchCustomer(JObject searchvalue)
        {
            var response = new JObject();
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(searchvalue["searchcriteria"].ToString());
                string serchvalue = data["searchvalue"];
                response = _customerRepository.SearchCustomer(serchvalue);
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
