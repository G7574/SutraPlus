using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus.Models;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Repository
{
    public class TenantDBCommonRepository : BaseRepository
    {
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private MasterDBContext _masterDBContext;

        public TenantDBCommonRepository(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger) : base(tenantID, masterDBContext)
        {
            _logger = logger;
            _configuration = configuration;
            _masterDBContext = masterDBContext;
        }
        //Sudhir development 18-4-23
        public JObject UnitsDropDown()
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.Commodities
                              where c.IsActive == true
                              select new { c.CommodityId, c.Mou }).ToList().DistinctBy(c => new { c.Mou });
                if (result != null)
                {
                    response.Add("UnitDropDownList", JArray.FromObject(result));
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
            //  
        }

        public JObject LedgerTypeDropDown()
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.Ledger_Types
                              where c.IsActive == true && c.LedgerType != null
                              select new { c.LedgerType, c.Id }).ToList().DistinctBy(c => new { c.LedgerType });
                if (result != null)
                {
                    response.Add("LederTypeDDList", JArray.FromObject(result));
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
        public JObject DealerTypeDropDown()
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.Dealer_Types
                              where c.IsActive == true && c.DealerType != null
                              select new { c.DealerType, c.Id }).ToList().DistinctBy(c => new { c.DealerType });
                if (result != null)
                {
                    response.Add("DealerTypeDDList", JArray.FromObject(result));
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

        public JObject AccounitngGroupsDropDown()
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.AccounitngGroups
                              where c.IsActive == true && (c.AccontingGroupId == 21 || c.AccontingGroupId == 22) && c.GroupName != null
                              select new { c.AccontingGroupId, c.GroupName }).ToList().DistinctBy(c => new { c.GroupName });
                if (result != null)
                {
                    response.Add("AccounitngGroupsDDList", JArray.FromObject(result));
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
        public JObject OtherAccounitngGroupsDropDown()
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.AccounitngGroups
                              where c.IsActive == true && c.GroupName != null
                              select new { c.AccontingGroupId, c.GroupName }).ToList().DistinctBy(c => new { c.GroupName });
                if (result != null)
                {
                    response.Add("AccounitngGroupsDDList", JArray.FromObject(result));
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


        public JObject VoucherTypeList()
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.VoucherTypes
                              where c.VoucherId > 15 && c.VoucherId < 25
                              select new { c.VoucherName, c.VoucherId }).ToList().DistinctBy(c => new { c.VoucherName });
                if (result != null)
                {
                    response.Add("VoucherTypes", JArray.FromObject(result));
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

        public JObject UserTypeDropDown()
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.Users
                              where c.IsActive == true
                              select new { c.UserId, c.UserType }).ToList().DistinctBy(c => new { c.UserType });
                if (result != null)
                {
                    response.Add("UserTypeDDList", JArray.FromObject(result));
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

        //Shivaji Development 19-4-23
        public bool AddLedger(Ledger ledger)
        {
            try
            {
                //get next id for ledgerId field
                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                _tenantDBContext.Add(ledger);
                _tenantDBContext.SaveChanges();
                _logger.LogDebug("TenantDBCommonRepo : Ledger Added");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Get Ledger List based on company Id, pagesize & page records per page
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="page"></param>
        /// <param name="ledgerName"></param>
        /// <param name="ledgertype"></param>
        /// <returns></returns>
        public pagination<Ledger> GetLedger(int companyId, pagination<Ledger> page, string SearchText, string LedgerType ,string Country) // get all list of ledger / party based on company code
        {

            IEnumerable<Ledger> objList1 = null;
            var list = new List<Ledger>();
            try
            {
                if (SearchText != null && SearchText != string.Empty)
                {
                    if (Country != "")
                    {
                        if (Country == "Export")
                        {
                            objList1 = _tenantDBContext.Ledgers.Where(e => (e.LedgerName.ToLower().Contains(SearchText.ToLower()) || e.LedgerType.ToLower().Contains(SearchText.ToLower())) && e.CompanyId == companyId && e.IsActive == true && e.LedType == LedgerType && e.Country != "India").OrderBy(L => L.LedgerName).ToList();
                            page.TotalCount = _tenantDBContext.Ledgers.Where(e => (e.LedType == LedgerType && e.IsActive == true)).Count();
                            //page.TotalCount = objList1.Count();
                        }
                        else
                        {
                            objList1 = _tenantDBContext.Ledgers.Where(e => (e.LedgerName.ToLower().Contains(SearchText.ToLower()) || e.LedgerType.ToLower().Contains(SearchText.ToLower())) && e.CompanyId == companyId && e.IsActive == true && e.AccountingGroupId==24).OrderBy(L => L.LedgerName).ToList();
                            page.TotalCount = _tenantDBContext.Ledgers.Where(e => (e.LedType == LedgerType && e.IsActive == true)).Count();
                            //page.TotalCount = objList1.Count();
                        }
                    }
                    else
                    {
                        objList1 = _tenantDBContext.Ledgers.Where(e => (e.LedgerName.ToLower().Contains(SearchText.ToLower())) 
                        //|| (e.AccountingGroupId>20 && e.AccountingGroupId<22) && 
                        || (e.AccountingGroupId>21 && e.AccountingGroupId<22) && 
                        e.CompanyId == companyId && e.IsActive == true).OrderBy(L => L.LedgerName).ToList();
                        page.TotalCount = _tenantDBContext.Ledgers.Where(e => (e.LedType == LedgerType && e.IsActive == true)).Count();
                       // page.TotalCount = objList1.Count();
                    }
                }
                else
                {
                    if (Country!="")
                    {
                        objList1 = _tenantDBContext.Ledgers.Where(e => e.CompanyId == companyId && e.IsActive == true && e.LedType == LedgerType && e.Country == Country).OrderBy(e => e.LedgerName).ToList();
                        page.TotalCount = _tenantDBContext.Ledgers.Where(e => (e.LedType == LedgerType && e.IsActive == true)).Count();
                        //page.TotalCount = objList1.Count();
                    }
                    else
                    {
                        objList1 = _tenantDBContext.Ledgers.Where(e => e.CompanyId == companyId && e.IsActive == true && e.AccountingGroupId != 21 && e.AccountingGroupId != 22 ).OrderBy(e => e.LedgerName).ToList();
                        page.TotalCount = _tenantDBContext.Ledgers.Where(e => e.CompanyId == companyId && e.IsActive == true && e.AccountingGroupId != 21 && e.AccountingGroupId != 22).OrderBy(e => e.LedgerName).Count();
                        //page.TotalCount = objList1.Count();
                    }
                }
                foreach (var obj in objList1.ToList())
                {
                    var ledgerList = new Ledger();
                    ledgerList._Id = obj._Id;
                    ledgerList.CompanyId = obj.CompanyId;
                    ledgerList.LedgerId = obj.LedgerId;
                    ledgerList.LedgerName = obj.LedgerName;
                    ledgerList.LedgerType = obj.LedgerType;
                    ledgerList.DealerType = obj.DealerType;
                    ledgerList.Address1 = obj.Address1;
                    ledgerList.Address2 = obj.Address2;
                    ledgerList.Place = obj.Place;
                    ledgerList.State = obj.State;
                    ledgerList.Gstn = obj.Gstn;
                    ledgerList.ContactDetails = obj.ContactDetails;
                    ledgerList.Country = obj.Country;
                    ledgerList.AccountingGroupId = obj.AccountingGroupId;
                    ledgerList.CellNo = obj.CellNo;
                    ledgerList.EmailId = obj.EmailId;
                    ledgerList.Fssai = obj.Fssai;
                    ledgerList.Tdsdeducted = obj.Tdsdeducted;
                    ledgerList.BankName = obj.BankName;
                    ledgerList.Ifsc = obj.Ifsc;
                    ledgerList.AccountNo = obj.AccountNo;
                    ledgerList.Pan = obj.Pan;
                    ledgerList.ManualBookPageNo = obj.ManualBookPageNo;
                    ledgerList.CreatedBy = obj.CreatedBy;
                    ledgerList.CreatedDate = obj.CreatedDate;
                    ledgerList.OpeningBalance = obj.OpeningBalance;
                    ledgerList.CrDr = obj.CrDr;
                    ledgerList.LedType = obj.LedType;
                    ledgerList.IsActive = obj.IsActive;
                    ledgerList.LegalName = obj.LegalName;
                    ledgerList.Pin = obj.Pin;
                    list.Add(ledgerList);
                }
                if (!string.IsNullOrEmpty(SearchText))
                {
                    list.OrderBy(s => s.LedgerName).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
                    page.Records = list;
                }
                else
                {
                    page.Records = list.OrderBy(s => s.LedgerName).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
                }
                return page;
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetLedger(int companyId, int ledgerId) // get single ledger / pary based on company id & ledger id
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.Ledgers
                    .Where(c => c.CompanyId == companyId && c.LedgerId == ledgerId && c.IsActive == true )
                     .Select(c => new                    
                              {
                                  c.CompanyId,
                                  c.LedgerId,
                                  c.LedgerName,
                                  c.LedgerType,
                                  c.DealerType,
                                  c.Address1,
                                  c.Address2,
                                  c.Place,
                                  c.State,
                                  c.Gstn,
                                  c.ContactDetails,
                                  c.Country,
                                  c.AccountingGroupId,
                                  c.CellNo,
                                  c.EmailId,
                                  c.Fssai,
                                  c.Tdsdeducted,
                                  c.BankName,
                                  c.Ifsc,
                                  c.AccountNo,
                                  c.Pan,
                                  c.ManualBookPageNo,
                                  c.CreatedBy,
                                  c.CreatedDate,
                                  c.OpeningBalance,
                                  c.CrDr,
                                  c.LedType,
                                  c.IsActive                                 
                              }).ToList();
                if (result != null)
                {
                    response.Add("LedgerList", JArray.FromObject(result));
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

        public bool UpdateLed(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["LedgerData"].ToString());
                if (data != null)
                {
                    //int Companyid = data["CompanyId"];
                    int Ledgerid = data["ledgerId"];

                    var entity = _tenantDBContext.Ledgers.FirstOrDefault(item => item.LedgerId == Ledgerid);
                    if (entity != null)
                    {                        
                        entity.Address1 = data["address1"];
                        entity.Address2 = data["address2"];                       
                        entity.State = data["state"];  
                        entity.Pin = data["pin"];
                        entity.CellNo = data["cellNo"];
                        entity.EmailId = data["emailId"];
                        entity.LegalName = data["legalName"];                        
                        entity.Pan = data["pan"];                       
                        entity.UpdatedDate = DateTime.Now;                        
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entity);
                        _logger.LogDebug("TenantDBCommonRepo : Ledger Updated");
                    }
                    return true;
                }
                else
                {
                    _logger.LogDebug("TenantDBCommonRepo : Ledger Updated Failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public bool UpdateLedger(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["LedgerData"].ToString());
                if (data != null)
                {
                    int Companyid = data["CompanyId"];
                    int Ledgerid = data["LedgerId"];

                    var entity = _tenantDBContext.Ledgers.FirstOrDefault(item => item.CompanyId == Companyid && item.LedgerId == Ledgerid && item.LedType == "Sales Ledger");
                    if (entity != null)
                    {
                        entity.CompanyId = data["CompanyId"];
                        entity.LedgerName = data["LedgerName"];
                        entity.LedgerType = data["LedgerType"];
                        entity.DealerType = data["DealerType"];
                        entity.Address1 = data["Address1"];
                        entity.Address2 = data["Address2"];
                        entity.Place = data["Place"];
                        entity.State = data["State"];
                        entity.Gstn = data["Gstn"];
                        entity.ContactDetails = data["ContactDetails"];
                        entity.Country = data["Country"];
                        entity.AccountingGroupId = data["AccountingGroupId"];
                        entity.CellNo = data["CellNo"];
                        entity.EmailId = data["EmailId"];
                        entity.Fssai = data["Fssai"];
                        entity.Tdsdeducted = data["Tdsdeducted"];
                        entity.BankName = data["BankName"];
                        entity.Ifsc = data["Ifsc"];
                        entity.AccountNo = data["AccountNo"];
                        entity.Pan = data["Pan"];
                        entity.ManualBookPageNo = data["ManualBookPageNo"];
                        entity.CreatedBy = data["CreatedBy"];
                        entity.CreatedDate = DateTime.Now;
                        entity.OpeningBalance = data["OpeningBalance"];
                        entity.CrDr = data["CrDr"];
                        entity.LedType = "Sales Ledger";
                        entity.IsActive = true;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entity);
                        _logger.LogDebug("TenantDBCommonRepo : Ledger Updated");
                    }
                    return true;
                }
                else
                {
                    _logger.LogDebug("TenantDBCommonRepo : Ledger Updated Failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public bool UpdateOtherLedger(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["LedgerData"].ToString());
                if (data != null)
                {
                    int Companyid = data["CompanyId"];
                    int Ledgerid = data["LedgerId"];

                    var entity = _tenantDBContext.Ledgers.FirstOrDefault(item => item.CompanyId == Companyid && item.LedgerId == Ledgerid && item.LedType == "Sales Other Ledger");
                    if (entity != null)
                    {
                        entity.LedgerName = data["LedgerName"];
                        entity.Place = data["Place"];
                        entity.AccountingGroupId = data["AccountingGroupId"];
                        entity.CreatedBy = data["CreatedBy"];
                        entity.CreatedDate = DateTime.Now;
                        entity.LedType = "Sales Other Ledger";
                        entity.IsActive = true;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entity);
                        _logger.LogDebug("TenantDBCommonRepo : Other Ledger Updated");
                    }
                    return true;
                }
                else
                {
                    _logger.LogDebug("TenantDBCommonRepo : Other Ledger Updated Failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        //Sudhir Development 19-4-23
        public string Add(Commodity commodity)
        {
            try
            {   
                var entity = _tenantDBContext.Commodities.FirstOrDefault(item => item.CommodityName == commodity.CommodityName && item.CommodityId != commodity.CommodityId);

                if (entity == null)
                {
                    commodity.CommodityId = _tenantDBContext.Commodities.Select(x => x.CommodityId).ToList().Max() + 1;
                    _tenantDBContext.Add(commodity);
                    _tenantDBContext.SaveChanges();
                    _logger.LogDebug("TenantDBCommonRepo : Commodity added");
                    return "Added";
                }
                else
                {
                    return "Invalid Item/Service Name.Already a Item/Service Already Exists with this same name";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetSingle(int Id)
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.Commodities.Where(a => a._Id == Id).ToList();
                if (result != null)
                {
                    response.Add("ProductIte", JArray.FromObject(result));
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
        /// <summary>
        /// Get Commodity (Product) based on CompanyId, page no, total records per page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public pagination<Commodity> Get(pagination<Commodity> page, string searchText) //Get List of all products
        {
            IEnumerable<Commodity> objList1 = null;
            var list = new List<Commodity>();
            try
            {
                if (searchText != null && searchText != string.Empty)
                {
                    objList1 = this._tenantDBContext.Commodities.Where(e => (e.CommodityName.ToLower().Contains(searchText.ToLower()) && e.IsActive == true)).OrderBy(c => c.CommodityName).ToList();
                    page.TotalCount = _tenantDBContext.Commodities.Where(e => (e.CommodityName.ToLower().Contains(searchText.ToLower()) && e.IsActive == true)).Count();
                }
                else
                { 
                    objList1 = this._tenantDBContext.Commodities.Where(e => e.IsActive == true).OrderBy(e => e.CommodityName).ToList();
                    page.TotalCount = objList1.Count();
                }
                foreach (var obj in objList1.ToList())
                {
                    var commodityList = new Commodity();
                    commodityList._Id = obj._Id;
                    commodityList.CommodityName = obj.CommodityName;
                    commodityList.CommodityId = obj.CommodityId;
                    commodityList.CompanyId = obj.CompanyId;
                    commodityList.HSN = obj.HSN;
                    commodityList.Mou = obj.Mou;
                    commodityList.IGST = obj.IGST;
                    commodityList.SGST = obj.SGST;
                    commodityList.CGST = obj.CGST;
                    commodityList.OpeningStock = obj.OpeningStock;
                    commodityList.Obval = obj.Obval;
                    commodityList.IsTrading = obj.IsTrading;
                    commodityList.DeductTds = obj.DeductTds;
                    commodityList.IsVikriCommodity = obj.IsVikriCommodity;
                    commodityList.IsService = obj.IsService;
                    commodityList.CreatedDate = obj.CreatedDate;
                    commodityList.UpdatedDate = obj.UpdatedDate;
                    commodityList.IsActive = obj.IsActive;
                    list.Add(commodityList);
                }
                if (!string.IsNullOrEmpty(searchText))
                {
                    list.OrderBy(s => s.CommodityName).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
                    page.Records = list;
                }
                else
                {
                    page.Records = list.OrderBy(s => s.CommodityName).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
                }
                return page;
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Update single product
        /// </summary>
        /// <param name="responseData"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public string Update(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["ItemProduct"].ToString());
                if (data != null)
                {
                    int id = data["_Id"];
                    var entity = _tenantDBContext.Commodities.FirstOrDefault(item => item._Id == id);

                    if (entity != null)
                    {
                        entity.CommodityName = data["CommodityName"];
                        entity.HSN = data["HSN"];
                        entity.CompanyId = data["CompanyId"];
                        entity.Mou = data["MOU"];
                        entity.IGST = data["IGST"];
                        entity.SGST = data["SGST"];
                        entity.CGST = data["CGST"];
                        entity.OpeningStock = data["OpeningStock"];
                        entity.Obval = data["OBVAL"];
                        entity.IsTrading = data["IsTrading"];
                        entity.DeductTds = data["DeductTDS"];
                        entity.IsVikriCommodity = data["DeductItem"];

                        entity.IsService = data["IsService"];
                        entity.CreatedDate = DateTime.Now;
                        entity.IsActive = true;



                        var alreadyExistsentity = _tenantDBContext.Commodities.FirstOrDefault(item => item._Id != id & item.CommodityName == entity.CommodityName);

                        if (alreadyExistsentity == null)
                        {
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(entity);
                            _logger.LogDebug("TenantDBCommonRepo : commodity/product Updated");
                            return "Updated";
                        }
                        else
                        {
                            return "Invalid Item/Service Name.Already a Item/Service Already Exists with this same name";
                        }
                    }

                    return "";
                }
                else
                {
                    _logger.LogDebug("TenantDBCommonRepo : commodity/product Updated Failed");
                    return "commodity/product Updated Failed";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool Delete(JObject Data) //product delete
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["ItemProduct"].ToString());
                int id = data["_Id"];
                if (data != null)
                {
                    using (var context = new MasterDBContext())
                    {
                        var activeentity = _tenantDBContext.Commodities.SingleOrDefault(item => item._Id == id && item.IsActive == true);
                        var deactivateentity = _tenantDBContext.Commodities.SingleOrDefault(item => item._Id == id && item.IsActive == false);
                        if (activeentity != null)
                        {
                            activeentity.IsActive = false;
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(activeentity);
                            _logger.LogDebug("TenantDBCommonRepo : Product Deactivated");
                        }
                        else if (deactivateentity != null)
                        {
                            deactivateentity.IsActive = true;
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(deactivateentity);
                            _logger.LogDebug("TenantDBCommonRepo : Product Activated");
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

        /// <summary>
        /// Add User In Master
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string AddUser(Models.User user)
        {
            try
            {
                user.UserId = _tenantDBContext.Users.Select(x => x.UserId).ToList().Max() + 1;
                //check existing user present with given email id
                var rsult = _tenantDBContext.Users.Where(x => x.UserName == user.UserName).ToList();
                if (rsult.Count > 0)
                {
                    _logger.LogDebug("TenantDBCommonRepo : User Exist");
                    return "E-Mail id already present...!";
                }
                else
                {
                    _tenantDBContext.Add(user);
                    _tenantDBContext.SaveChanges();
                    _logger.LogDebug("TenantDBCommonRepo : User Added");
                    return "User Added Successfully...!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Get Single user based on Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JObject GetUserSingle(int UserId)
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.Users.Where(a => a.UserId == UserId).ToList();
                if (result.Count > 0)
                {
                    response.Add("UserData", JArray.FromObject(result));
                    var accessdata = _tenantDBContext.UserAccesMasters.Where(u => u.UserId == UserId).ToList();
                    response.Add("Accessdata", JArray.FromObject(accessdata));
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
        /// <summary>
        /// GetList of all Users
        /// </summary>
        /// <returns></returns>
        public pagination<Models.User> GetUserList(pagination<Models.User> page, string searchText)
        {
            IEnumerable<Models.User> objList1 = null;
            var list = new List<Models.User>();
            try
            {
                if (searchText != null && searchText != string.Empty)
                {
                    objList1 = this._tenantDBContext.Users.Where(e => (e.UserName.ToLower().Contains(searchText.ToLower()) && e.IsActive == true)).OrderBy(c => c.UserName).ToList();
                    page.TotalCount = objList1.Count();
                }
                else
                {
                    objList1 = this._tenantDBContext.Users.Where(e => e.IsActive == true).OrderBy(e => e.UserName).ToList();
                    page.TotalCount = objList1.Count();
                }
                foreach (var obj in objList1.ToList())
                {
                    var userlist = new Models.User();
                    userlist.Id = obj.Id;
                    userlist.UserId = obj.UserId;
                    userlist.UserType = obj.UserType;
                    userlist.UserName = obj.UserName;
                    userlist.Password = obj.Password;
                    userlist.CreatedDate = obj.CreatedDate;
                    userlist.UpdatedDate = obj.UpdatedDate;
                    userlist.FirstName = obj.FirstName;
                    userlist.LastName = obj.LastName;
                    userlist.IsActive = obj.IsActive;
                    list.Add(userlist);
                }
                if (!string.IsNullOrEmpty(searchText))
                {
                    list.OrderByDescending(s => s.CreatedDate).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
                    page.Records = list;
                }
                else
                {
                    page.Records = list.OrderByDescending(s => s.CreatedDate).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToArray().ToList();
                }
                return page;
                //page.Records = list.OrderBy(s => s.CreatedDate).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();
                //return page;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool UpdateUser(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["UserCreate"].ToString());
                if (data != null)
                {
                    int id = data["UserId"];
                    var entity = _tenantDBContext.Users.FirstOrDefault(item => item.UserId == id);
                    if (entity != null)
                    {
                        entity.UserType = data["UserType"];
                        entity.UserName = data["UserName"];
                        entity.FirstName = data["FirstName"];
                        entity.LastName = data["LastName"];
                        entity.PhoneNo = data["PhoneNo"];
                        entity.UpdatedDate = DateTime.Now;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entity);
                        _logger.LogDebug("TenantDBCommonRepo : Product Deactivated");
                    }
                    //remove all access
                    var entityAccess = _tenantDBContext.UserAccesMasters.Where(item => item.UserId == id).ToList();
                    if (entity != null)
                    {
                        foreach (var item in entityAccess)
                        {
                            item.IsAccess = false;
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(item);
                            _logger.LogDebug("TenantDBCommonRepo : Access Removed");
                        }
                    }
                    //add access one by one
                    string[] formId = JsonConvert.DeserializeObject<string[]>(Data["AccessData"].ToString());
                    foreach (var item in formId)
                    {
                        UserAccesMaster userAcces = new UserAccesMaster()
                        {
                            FormId = Convert.ToInt32(item),
                            UserId = Convert.ToInt32(id),
                            IsAccess = true,
                            UpdatedBy = data["PreparedBy"],
                            UpdatedDate = DateTime.Now,
                            IsActive = true
                        };
                        //check exist or not
                        int frmId = Int32.Parse(item);
                        var entity_Access = _tenantDBContext.UserAccesMasters.FirstOrDefault(item => item.UserId == id && item.FormId == frmId);
                        if (entity_Access != null)
                        {
                            entity_Access.IsAccess = true;
                            entity_Access.UpdatedDate = DateTime.Now;
                            entity_Access.UpdatedBy = data["PreparedBy"];
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(entity_Access);
                            _logger.LogDebug("TenantDBCommonRepo : access added");
                        }
                        else//add new entry
                        {
                            userAcces.PreparedBy = data["PreparedBy"];
                            userAcces.PreparedDate = DateTime.Now;
                            userAcces.UpdatedDate = null;
                            _tenantDBContext.Add(userAcces);
                            _tenantDBContext.SaveChanges();
                            _logger.LogDebug("TenantDBCommonRepo : Access Added");
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool DeleteUser(JObject Data) //User Active/De-Active  delete
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["DeleteUser"].ToString());
                int id = data["Id"];
                if (data != null)
                {
                    var activeentity = _tenantDBContext.Users.SingleOrDefault(item => item.Id == id && item.IsActive == true);
                    var deactivateentity = _tenantDBContext.Users.SingleOrDefault(item => item.Id == id && item.IsActive == false);
                    if (activeentity != null)
                    {
                        activeentity.IsActive = false;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(activeentity);
                        _logger.LogDebug("TenantDBCommonRepo : User Deactivated");
                    }
                    else if (deactivateentity != null)
                    {
                        deactivateentity.IsActive = true;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(deactivateentity);
                        _logger.LogDebug("TenantDBCommonRepo : User Activated");
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
        public JObject GetFormList()
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.FormMasters.Where(a => a.IsActive == true).ToList();
                if (result != null)
                {
                    response.Add("FormList", JArray.FromObject(result));
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

        public JObject GetUserFormList()
        {
            var response = new JObject();
            try
            {
                var result = (from t1 in _tenantDBContext.FormMasters
                                  join t2 in _tenantDBContext.UserAccesMasters on t1.FormId equals t2.FormId
                                  where t2.IsAccess == true
                                  select new
                                  {
                                      FormId = t1.FormId,
                                      GroupName = t1.GroupName,
                                      FormName=t1.FormName,
                                      UserId = t2.UserId,
                                      IsAccess=t2.IsAccess

                                  }).ToList();

                //var result = _tenantDBContext.FormMasters.Where(a => a.IsActive == true).ToList();
                if (result != null)
                {
                    response.Add("FormList", JArray.FromObject(result));
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

        //shivaji 27-2-23 this is added in add user hence it is no more in use TODO: delete this after user module finalize
        public bool AddAccess(UserAccesMaster userAccesMaster) //add access
        {
            try
            {
                _tenantDBContext.Add(userAccesMaster);
                _tenantDBContext.SaveChanges();
                _logger.LogDebug("TenantDBCommonRepo : User Access Added");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool Access(JObject Data, UserAccesMaster userAccesMaster) //User Active/De-Active  delete
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["FormAccess"].ToString());
                int formId = data["FormId"];
                int userId = data["UserId"];
                int updatedBy = data["UpdatedBy"];
                if (data != null)
                {
                    var activeentity = _tenantDBContext.UserAccesMasters.SingleOrDefault(item => item.FormId == formId && item.UserId == userId && item.IsAccess == true && item.IsActive == true);
                    var deactivateentity = _tenantDBContext.UserAccesMasters.SingleOrDefault(item => item.FormId == formId && item.UserId == userId && item.IsAccess == false && item.IsActive == false);
                    if (activeentity != null)
                    {
                        activeentity.IsActive = false;
                        activeentity.IsAccess = false;
                        activeentity.UpdatedBy = updatedBy;
                        activeentity.UpdatedDate = DateTime.Now;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(activeentity);
                        _logger.LogDebug("TenantDBCommonRepo : User Activated");
                    }
                    else if (deactivateentity != null)
                    {
                        deactivateentity.IsActive = true;
                        deactivateentity.IsAccess = true;
                        deactivateentity.UpdatedBy = updatedBy;
                        deactivateentity.UpdatedDate = DateTime.Now;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(deactivateentity);
                        _logger.LogDebug("TenantDBCommonRepo : User Deactivated");
                    }
                    else
                    {
                        _tenantDBContext.Add(userAccesMaster);
                        _tenantDBContext.SaveChanges();
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
        public JObject GetUserAccess(string UserId)
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.UserAccesMasters
                              where c.IsActive == true && c.IsAccess == true
                              select new { c.FormId, c.IsAccess, c.UserId }).ToList().OrderBy(c => new { c.FormId });
                if (result != null)
                {
                    response.Add("UserAccess", JArray.FromObject(result));
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
    }
}
