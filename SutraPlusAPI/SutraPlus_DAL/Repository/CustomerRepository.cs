
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.LayoutRenderers;
using SutraPlus.Models;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;


namespace SutraPlus_DAL.Repository
{
    public class CustomerRepository
    {
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public IConfiguration _configuration { get; }
        public CustomerRepository(MasterDBContext masterDB, ILogger logger ,IConfiguration Configuration )
        {
            _logger = logger;   
            _masterDBContext = masterDB;
            _configuration = Configuration;
        }
        
        public JObject GetYearList(string CustomerId)
        {
            var response = new JObject();
            try
            {
                var result = (from c in _masterDBContext.Customers
                              join fy in _masterDBContext.CustomerFinancialYears
                              on c.Code equals fy.CustomerId
                              where c.Code == CustomerId && c.IsActive == true
                              select new {fy.Id, c.Name, fy.Description,fy.StartDate, fy.EndDate,  fy.Year, fy.ThemeCode, fy.BackCode }).OrderBy(fy => fy.Description);
                if (result != null)
                {
                    response.Add("YearList", JArray.FromObject(result));
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public int Add(Customer customer)
        {
            try
            {
                if (GetCustomerByEmail(customer.Email))
                {
                    _logger.LogDebug("Duplicate Customer found ");
                    return 0;
                }
                else
                {
                    //TODO : check debugging without using concept if work remove using 
                    using (var dbContext = new MasterDBContext())
                    {
                        dbContext.Add(customer);
                        dbContext.SaveChanges();
                        _logger.LogDebug("Add Customer Successfully");
                    }
                    return customer.Id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public int Add(CustomerFinancialYear customerFinancialYear)
        {
            try
            {
                _logger.LogDebug("Add CustomerFinancialYear" );
                _masterDBContext.Add(customerFinancialYear);
                _masterDBContext.SaveChanges();
                return customerFinancialYear.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }

        }

        public pagination<Customer> List(pagination<Customer> page, string searchText)
        {
            IEnumerable<Customer> customer_List = null;
            var list = new List<Customer>();

            if (!String.IsNullOrEmpty(searchText))
            {
                customer_List = _masterDBContext.Customers.Where(e => (e.Name.ToLower().Contains(searchText.ToLower()) && e.IsActive == true)).OrderBy(c => c.Name).ToList();
                page.TotalCount = customer_List.Count();
            }
            else
            {
                customer_List = this._masterDBContext.Customers.Where(e => e.IsActive == true).OrderBy(e => e.Name).ToList();
                page.TotalCount = customer_List.Count();
            }
            foreach (var obj in customer_List.ToList())
            {
                var customerList = new Customer();
                customerList.Id = obj.Id;
                customerList.Name = obj.Name;
                customerList.Code = obj.Code;
                customerList.Address = obj.Address;
                customerList.City = obj.City;
                customerList.State = obj.State;
                customerList.Pin = obj.Pin;
                customerList.Mobile = obj.Mobile;
                customerList.Email = obj.Email;
                customerList.ContactPerson = obj.ContactPerson;
                customerList.FirstName = obj.FirstName;
                customerList.LastName = obj.LastName;
                customerList.GSTNo = obj.GSTNo;
                customerList.CreatedDate = obj.CreatedDate;
                customerList.UpdatedDate = obj.CreatedDate;
                customerList.IsActive = obj.IsActive;
                list.Add(customerList);
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                list.OrderBy(s => s.CreatedDate).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
                page.Records = list;
            }
            else
            {
                page.Records = list.OrderBy(s => s.CreatedDate).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
            }
            return page;
        }
        public Boolean GetCustomerByEmail(string email)
        {
            var response = new JObject();
            try
            {
                var result = (from c in _masterDBContext.Customers
                              where c.Email == email && c.IsActive == true
                              select new { c.Id, c.Name, c.Code, c.Address, c.City, c.State, c.Pin, c.Mobile, c.Email, c.ContactPerson, c.FirstName, c.LastName, c.GSTNo, }).ToList();
                if (result.Count > 0)
                {
                    response.Add("CustomerSingleList", JArray.FromObject(result));
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject Get(int CustomerId)
        {
            var response = new JObject();
            try
            {
                var result = (from c in _masterDBContext.Customers
                              where c.Id == CustomerId && c.IsActive == true
                              select new { c.Id, c.Name, c.Code, c.Address, c.City, c.State, c.Pin, c.Mobile, c.Email, c.ContactPerson, c.FirstName, c.LastName, c.GSTNo, }).ToList();
                if (result != null)
                {
                    string code = (from c in _masterDBContext.Customers
                                where c.Id == CustomerId && c.IsActive == true
                                select c.Code).SingleOrDefault();
                    response.Add("CustomerSingleList", JArray.FromObject(result));
                   
                    var Description = (from fy in _masterDBContext.CustomerFinancialYears
                                      
                                       where fy.CustomerId.Equals(code) && fy.IsActive == true
                                       select new
                                       {
                                          fy.Description
                                       }).OrderBy(fy => fy.Description);
                    if (Description != null)
                        response.Add("FinancialYear", JArray.FromObject(Description));
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
        public bool Update(JObject Data) // update not for all fields also not for email
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["UpdateCustomer"].ToString());
                int id = data["Id"];
                _logger.LogDebug("Update Customer with few fields for : " );
                if (data != null)
                {
                    using (var context = new MasterDBContext())
                    {
                        var entity = context.Customers.FirstOrDefault(item => item.Id == id);
                        if (entity != null)
                        {
                            entity.Address = data["Address"];
                            entity.Mobile = data["Mobile"];
                            entity.FirstName = data["FirstName"];
                            entity.LastName = data["LastName"];
                            entity.IsActive = true;
                            entity.UpdatedDate = DateTime.Now;
                            context.SaveChanges();
                            context.Update(entity);
                            _logger.LogDebug("Update Customer successed: " + id.ToString());
                        }
                    }
                }
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
                var data = JsonConvert.DeserializeObject<dynamic>(Data.ToString());
                int id = data["Id"];
                _logger.LogDebug("Delete Customer done : " );
                if (data != null)
                {
                    using (var context = new MasterDBContext())
                    {
                        var entity = context.Customers.SingleOrDefault(item => item.Id == id);
                        if (entity != null)
                        {
                            entity.IsActive = false;
                            context.SaveChanges();
                            context.Update(entity);
                        }
                    }
                }
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
                var data = JsonConvert.DeserializeObject<dynamic>(Data.ToString());
                int id = data["Id"];
                _logger.LogDebug("Update Customer Done");
                if (data != null)
                {
                    using (var context = new MasterDBContext())
                    {
                        var entity = context.Customers.SingleOrDefault(item => item.Id == id);
                        if (entity != null)
                        {
                            entity.IsActive = true;
                            context.SaveChanges();
                            context.Update(entity);
                        }
                    }
                }
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
            var finyear = new JObject();
            try
            {
                //when March month get started the next year will be added
                if (DateTime.Today.Month >= 3)
                {
                    for (int i = DateTime.Today.Year - 10; i < DateTime.Today.Year + 1; i++)
                    {
                        int first = i;
                        int section = i + 1;
                        string drop = Convert.ToString(first + "-" + section);
                        finyear.Add(i.ToString(), drop);
                    }
                }
                else
                {
                    for (int i = DateTime.Today.Year - 11; i < DateTime.Today.Year; i++)
                    {
                        int first = i;
                        int section = i + 1;
                        string drop = Convert.ToString(first + "-" + section);
                        finyear.Add(i.ToString(), drop);
                    }
                }
                if (finyear != null)
                {
                    return finyear;
                }

                return finyear;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public string GenerateOTP(OTPTrans oTP)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("GenerateOTP for mail change");
                var result = _masterDBContext.OTPTrans.Add(oTP);
                _masterDBContext.SaveChanges();
                return oTP.OTP;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public Boolean ValidateOTP(string customerId, string OTP, string newMail)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("ValidateOTP for Done ");
                var result = _masterDBContext.OTPTrans.FirstOrDefault(t => t.OTP == OTP && t.CustomerId == customerId && DateTime.Now <= t.ExpireDate);
                if (result != null)
                {
                    using (var context = new MasterDBContext())
                    {
                        var entity = context.Customers.FirstOrDefault(item => item.Id.ToString() == customerId);
                        if (entity != null)
                        {
                            entity.Email = newMail;
                            context.SaveChanges();
                            context.Update(entity);
                        }
                        _logger.LogDebug("ValidateOTP for Customer : Mail change successed");
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        //when dynamic database creation is required, user this method
        public bool CreateDatabase(int UserID, string databaseName, string UserName, string Password,string FirstName, string LastName, string Mobile)
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("CreateDatabase Initiated ");
                string TableScriptPath = _configuration.GetSection("DatabaseScriptPath:TableScript").Value.ToString();
                string connection = _configuration.GetSection("ConnectionStrings:DefaultConnection").Value.ToString();
                string script = "Use Master Create Database " + databaseName;
                string TableScript = "USE " + databaseName + " " + File.ReadAllText(TableScriptPath);
                SqlConnection conn = new SqlConnection(connection);
                Server server = new Server(new ServerConnection(conn));
                server.ConnectionContext.ExecuteNonQuery(script);
                server.ConnectionContext.ExecuteNonQuery(TableScript);
                string DBConnection =  _configuration.GetSection("DatabaseScriptPath:DatabaseName").Value.ToString() + ";Database={" + databaseName + "};uid=" + _configuration.GetSection("DatabaseScriptPath:DatabaseUser").Value.ToString() + ";password=" + _configuration.GetSection("DatabaseScriptPath:DatabasePassword").Value.ToString() + ";TrustServerCertificate=True;";
                string UserId = UserID.ToString();
                Boolean IsActive = true;            
                var dateTime = DateTime.Now;
                var date = DateOnly.FromDateTime(dateTime);
                _logger.LogDebug("Create User Login");
                string Query = "Use " + databaseName + " INSERT INTO Users(UserId,UserType,UserName ,Password,FirstName,LastName,PhoneNo,CreatedDate,UpdatedDate,IsActive)" +
                                                     " Values ('" + UserId + "','Admin','" + UserName + "','" + Password + "','" + FirstName + "','" + LastName + "','" + Mobile + "'," + date + "," + date + ",1) SET IDENTITY_INSERT Users OFF ";
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlConnection cnn = new SqlConnection(DBConnection);
                try
                {
                    cnn.Open();
                    adapter.InsertCommand = new SqlCommand(Query, cnn);
                    adapter.InsertCommand.ExecuteNonQuery();
                    _logger.LogDebug("User Created successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);
                    throw ex;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public string GetThemeCode(string Year,string value)
        {
            
            try
            {
                if (value == "Theme")
                {
                    return _masterDBContext.ThemCodes.Where(c => c.Year == Year).Select(c => c.ThemeCode).SingleOrDefault().ToString();
                }
                else if(value == "Back")
                {
                    return _masterDBContext.ThemCodes.Where(c => c.Year == Year).Select(c => c.BackCode).SingleOrDefault().ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject SearchCustomer(string searchvalue)
        {
            var response = new JObject();
            try
            {
                var result = (from c in _masterDBContext.Customers
                              where c.Name.Contains(searchvalue) || c.City.Contains(searchvalue) || c.Pin.Contains(searchvalue) || c.Mobile.Contains(searchvalue)
                              || c.Email.Contains(searchvalue) || c.FirstName.Contains(searchvalue) || c.LastName.Contains(searchvalue) || c.GSTNo.Contains(searchvalue)
                              select new
                              {
                                  c.Id,c.Name,c.Code,c.Address,c.City,c.State,c.Pin,c.Mobile,c.Email,c.ContactPerson,
                                  c.FirstName,c.LastName,c.GSTNo,c.IsActive
                              }).ToList();
                if (result != null)
                {
                    response.Add("SearchList", JArray.FromObject(result));
                    return response;
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}
