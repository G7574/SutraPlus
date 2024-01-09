using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SutraPlus_DAL.Common;
using Microsoft.Extensions.Logging;
using SutraPlus.Models;
using Microsoft.Extensions.Configuration;

namespace SutraPlus_BAL.Service
{
    public class TenantDBCommonService
    {
        
        public TenantDBCommonRepository _tenantDBCommonRepository = null;
        public CommonRepository _commonRepository = null;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public IConfiguration _configuration { get; }
        private EmailSender _emailSender = null;
        public TenantDBCommonService(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _masterDBContext = masterDBContext;
            _logger = logger;
            _configuration = configuration;
            _emailSender = new EmailSender(_configuration);
            _tenantDBCommonRepository = new TenantDBCommonRepository(tenantID, masterDBContext, _configuration, _logger);
            _commonRepository = new CommonRepository(masterDBContext, logger);
        }
        //Sudhir Development 18-4-23

        public JObject UnitsDropDown()
        {
            try
            {
                var response = new JObject();
                response = _tenantDBCommonRepository.UnitsDropDown();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject LedgerTypeDropDown()
        {
            try
            {
                var response = new JObject();
                response = _tenantDBCommonRepository.LedgerTypeDropDown();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject DealerTypeDropDown()
        {
            try
            {
                var response = new JObject();
                response = _tenantDBCommonRepository.DealerTypeDropDown();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject AccounitngGroupsDropDown()
        {
            try
            {
                var response = new JObject();
                response = _tenantDBCommonRepository.AccounitngGroupsDropDown();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject OtherAccounitngGroupsDropDown()
        {
            try
            {
                var response = new JObject();
                response = _tenantDBCommonRepository.OtherAccounitngGroupsDropDown();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject GetVoucherTypeList()
        {
            try
            {
                var response = new JObject();
                response = _tenantDBCommonRepository.VoucherTypeList();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }


        public JObject UserTypeDropDown()
        {
            try
            {
                var response = new JObject();
                response = _tenantDBCommonRepository.UserTypeDropDown();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        //Shivaji Development 19-4-23
        public Boolean LedgerAdd(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["LedgerData"].ToString());
                string Leadername = data["LedgerName"] ?? "";
                string PlaceName = data["Place"] ?? "";
                if (_tenantDBCommonRepository._tenantDBContext.Ledgers.Any(A => A.LedgerName == Leadername && A.Place == PlaceName))
                {
                    return false;
                }
                else
                {
                    Ledger ledger = new Ledger
                    {
                        CompanyId = data["CompanyId"],
                        LedgerName = data["LedgerName"],
                        LedgerType = data["LedgerType"],
                        DealerType = data["DealerType"],
                        Address1 = data["Address1"],
                        Address2 = data["Address2"],
                        Place = data["Place"],
                        State = data["State"], //value of drop down
                        Gstn = data["Gstn"],
                        ContactDetails = data["ContactDetails"],
                        Country = data["Country"], //name of drop down
                        AccountingGroupId = data["AccountingGroupId"], //id value of drop down
                        CellNo = data["CellNo"],
                        EmailId = data["EmailId"],
                        Fssai = data["Fssai"],
                        Tdsdeducted = data["Tdsdeducted"],
                        BankName = data["BankName"],
                        Ifsc = data["Ifsc"],
                        AccountNo = data["AccountNo"],
                        Pan = data["Pan"],
                        OpeningBalance = data["OpeningBalance"],
                        CrDr = data["CrDr"],
                        ManualBookPageNo = data["ManualBookPageNo"],
                        CreatedBy = data["CreatedBy"],
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        LedType = "Sales Ledger" //it should be Party Ledger future problem same ledger can be utlilized in purchase invoice. 6-6-23
                    };
                    return _tenantDBCommonRepository.AddLedger(ledger);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public Boolean LedgerOtherAdd(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["LedgerData"].ToString());
                Ledger ledger = new Ledger
                {
                    CompanyId = data["CompanyId"],
                    LedgerName = data["LedgerName"],
                    Place = data["Place"],
                    AccountingGroupId = data["AccountingGroupId"], //id value of drop down
                    CreatedBy = data["CreatedBy"],
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    LedType = "Sales Other Ledger" //data["LedType"] //it should be Other Ledger 
                };
                return _tenantDBCommonRepository.AddLedger(ledger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
            //  _logger.LogDebug("TenantDBCommonService : User Exist");
        }
        public pagination<Ledger> GetLedgerList(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data["SearchText"]);
                var page = JsonConvert.DeserializeObject<pagination<Ledger>>(Convert.ToString(Data["Page"]));
                var data = JsonConvert.DeserializeObject<dynamic>(Data["LedgerData"].ToString());
               
                int companyId = data["CompanyId"];
                string LedgerType = data["LedgerType"];
                string Country = data["Country"];


                var result = _tenantDBCommonRepository.GetLedger(companyId, page, searchText,LedgerType, Country);
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
        public JObject GetLedger(JObject Data)
        {
            var response = new JObject();
            var data = JsonConvert.DeserializeObject<dynamic>(Data["LedgerData"].ToString());
            response = _tenantDBCommonRepository.GetLedger(Int32.Parse(Convert.ToString(data["CompanyId"])), Int32.Parse(Convert.ToString(data["LedgerId"])));
            return response;
        }

        public Boolean UptLedger(JObject Data)
        {
            try
            {
                return _tenantDBCommonRepository.UpdateLed(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public Boolean SetLedger(JObject Data)
        {
            try
            {
                return _tenantDBCommonRepository.UpdateLedger(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public Boolean UpdateOtherLedger(JObject Data)
        {
            try
            {
                return _tenantDBCommonRepository.UpdateOtherLedger(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        //UpdateOtherLedger
        //Sudhir Development 19-4-23
        public string Add(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["ItemProduct"].ToString());
                Commodity commodity = new Commodity
                {
                    CommodityName = data["CommodityName"],
                    HSN = data["HSN"],
                    CompanyId = data["CompanyId"],
                    Mou = data["MOU"],
                    IGST = data["IGST"],
                    SGST = data["SGST"],
                    CGST = data["CGST"],
                    OpeningStock = data["OpeningStock"], 
                    Obval = data["OBVAL"],
                    IsTrading = data["IsTrading"],
                    DeductTds = data["DeductTDS"],
                    IsVikriCommodity = data["DeductItem"],
                    IsService = data["IsService"],
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                return _tenantDBCommonRepository.Add(commodity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetSingle(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["ItemProduct"].ToString());
                string _id = Convert.ToString(data["_Id"]);
                return _tenantDBCommonRepository.GetSingle(Int32.Parse(_id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public pagination<Commodity> Get(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data["SearchText"]);
                var page = JsonConvert.DeserializeObject<pagination<Commodity>>(Convert.ToString(Data["Page"]));
                var result = _tenantDBCommonRepository.Get(page, searchText);
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
        public string Update(JObject Data)
        {
            try
            {
                return _tenantDBCommonRepository.Update(Data);
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
                _tenantDBCommonRepository.Delete(Data);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        //Sudhir 20-4-23
        /// <summary>
        /// Add User in Master/ Create User Page
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public string AddUser(JObject Data)
        {
            string response = "";
            try
            {
                _logger.LogDebug("TenantDBCommonService : Add User()");
                var data = JsonConvert.DeserializeObject<dynamic>(Data["UserCreate"].ToString());
                string Code = "", UserName = "";
                UserName = data["UserName"];
                string Password = GetOTP.Get_OTP(); //get unique otp (6 digits)
                User user = new User
                {
                    UserType = data["UserType"],
                    UserName = data["UserName"],
                    Password = Password,
                    FirstName = data["FirstName"],
                    LastName = data["LastName"],
                    PhoneNo = data["PhoneNo"],
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                response = _tenantDBCommonRepository.AddUser(user);
                if (response == "E-Mail id already present...!")
                {
                    _logger.LogDebug("TenantDBCommonService : User Exist");
                    return response;
                }
                else if (response == "Something get wrong...!")
                {
                    _logger.LogDebug("TenantDBCommonService : Error occured while adding user");
                    return response;
                }
                else
                {
                    var Userdata = _tenantDBCommonRepository._tenantDBContext.Users.Where(x => x.UserName == user.UserName).Select(x=> x.UserId).FirstOrDefault();
                    //add access data in access master
                    string[] formId = JsonConvert.DeserializeObject<string[]>(Data["AccessData"].ToString());
                    foreach (var item in formId)
                    {
                        UserAccesMaster userAcces = new UserAccesMaster()
                        {
                            FormId = Convert.ToInt32(item),
                            UserId = Convert.ToInt32(Userdata.ToString()),
                            IsAccess = true,
                            PreparedBy = data["PreparedBy"],
                            PreparedDate = DateTime.Now,
                            IsActive = true
                        };
                        _tenantDBCommonRepository.AddAccess(userAcces);
                    }
                    //send mail to user (Admin / User)
                    _emailSender.SendMailMessage(_commonRepository.GetEmailConfig(),UserName, "User Created Successfully...!", "", Password);
                    // send mail to all admin for same company about creation of user or admin role
                    _logger.LogDebug("TenantDBCommonService : Mail sent to user about creations");
                    var adminall = _tenantDBCommonRepository._tenantDBContext.Users.Where(u => u.UserType == "Admin").ToList();
                    string date = DateTime.Now.ToString();
                    foreach (var item in adminall)
                    {
                        string AdminUsermail = item.UserName;
                        string firstName = item.FirstName;
                        string lastname = item.LastName;
                        string FLName = firstName + " " + lastname;
                        EmailSender.SendMailMessageAdmin(_commonRepository.GetEmailConfig(),UserName, AdminUsermail, "User Created Successfully...!", "", Password, FLName, date);
                        _logger.LogDebug("TenantDBCommonService : Mail sent to all admin for user created");
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        
        /// <summary>
        /// Get user single based on Id in Master  CreateUser
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public JObject GetUserSingle(JObject Data)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(Data["CreateUser"].ToString());
            string id = Convert.ToString(data["Id"]);
            return _tenantDBCommonRepository.GetUserSingle(Int32.Parse(id));
        }
        /// <summary>
        /// get user  all List in Master DB  
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public pagination<User> GetUserList(JObject Data)
        {
            try
            {
                _logger.LogDebug("TenantDBCommonService : User Search");
                var searchText = Convert.ToString(Data["SearchText"]);
                var page = JsonConvert.DeserializeObject<pagination<User>>(Convert.ToString(Data["Page"]));
                var result = _tenantDBCommonRepository.GetUserList(page, searchText);
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
        /// <summary>
        /// Update  single User  based on   id
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public Boolean UpdateUser(JObject Data)
        {
            try
            {
                return _tenantDBCommonRepository.UpdateUser(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool DeleteUser(JObject Data)
        {
            try
            {
                return _tenantDBCommonRepository.DeleteUser(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetFormList()
        {
            return _tenantDBCommonRepository.GetFormList();
        }
        public JObject GetUsrFormList()
        {
            return _tenantDBCommonRepository.GetUserFormList();
        }
        public Boolean UserAccess(JObject Data)
        {
            _logger.LogDebug("TenantDBCommonService : UserAccess()");
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["FormAccess"].ToString());
                UserAccesMaster userAcces = new UserAccesMaster()
                {
                    FormId = data["FormId"],
                    UserId = data["UserId"],
                    IsAccess = true,
                    PreparedBy = data["UpdatedBy"],
                    PreparedDate = DateTime.Now,
                    IsActive = true
                };
                return _tenantDBCommonRepository.Access(Data, userAcces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetUserAccess(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["UserDetails"].ToString());
                string userId = data["UserId"];
                return _tenantDBCommonRepository.GetUserAccess(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}
