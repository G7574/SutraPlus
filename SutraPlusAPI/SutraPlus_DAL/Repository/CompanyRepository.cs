using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SutraPlus_DAL.Repository
{
    public class CompanyRepository : BaseRepository
    {
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private MasterDBContext _masterDBContext;
        public CompanyRepository(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger) : base(tenantID, masterDBContext)
        {
            _logger = logger;
            _configuration = configuration;
            _masterDBContext = masterDBContext;
        }
        /// <summary>
        /// Get all company list 
        /// </summary>
        /// <returns></returns>
        public JObject List()
        {
            var response = new JObject();
            try
            {
                _logger.LogDebug("Company Repo : List Companies");

                var result = (from a in _tenantDBContext.Companies                              
                            where a.IsActive == true                            
                            select new
                            {
                                CompanyId = a.CompanyId,
                                CompanyName = a.CompanyName,
                                AddressLine1= a.AddressLine1,
                                AddressLine2=a.AddressLine2,
                                AddressLine3=a.AddressLine3,
                                Place=a.Place,
                                Gstin=a.Gstin,
                                State=a.State,
                                ContactDetails=a.ContactDetails

                            }).ToList();
               
                if (result != null)
                {
                    response.Add("CompanyList", JArray.FromObject(result));
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

        public int GetMax()
        {
            try
            {
                return _tenantDBContext.Companies.Select(x => x.CompanyId).ToList().Max();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool Add(Company company)
        {
            try
            {
                company.CompanyId = GetMax() + 1;
                _tenantDBContext.Add(company);
                _tenantDBContext.SaveChanges();
                _logger.LogDebug("Company Repo : Add Company Successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject GetInvoiceKey(int companyId)
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.Companies.Where(a => a.CompanyId == companyId).Select(a => a.EinvoiceKey).First();
                if (result != null)
                {
                    response.Add("CompanyList",result);
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

        public JObject Get(int companyId)
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.Companies.Where(a => a.CompanyId == companyId).ToList();
                if (result != null)
                {
                    response.Add("CompanyList", JArray.FromObject(result));
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
        
        public bool Update(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["CompanyData"].ToString());
                if (data != null)
                {
                    int id = data["CompanyId"];
                    var entity = _tenantDBContext.Companies.FirstOrDefault(item => item.CompanyId == id);
                    if (entity != null)
                    {
                        entity.KannadaName = data["KannadaName"];
                        entity.Shree = data["Shree"];
                        entity.CompanyName = data["CompanyName"];
                        entity.Title = data["Title"];
                        entity.AddressLine1 = data["AddressLine1"];
                        entity.Pan = data["Pan"];
                        entity.Tan = data["Tan"];
                        entity.Place = data["Place"];
                        entity.Fln = data["Fln"];
                        entity.Bin = data["Bin"];
                        entity.District = data["District"];
                        entity.Email = data["Email"];
                        entity.State = data["State"];
                        entity.CellPhone = data["CellPhone"];
                        entity.ContactDetails = data["ContactDetails"];
                        entity.Gstin = data["Gstin"];
                        entity.FirmCode = data["FirmCode"];
                        entity.Apmccode = data["Apmccode"];
                        entity.Iec = data["Iec"];
                        entity.Bank1 = data["Bank1"];
                        entity.Ifsc1 = data["Ifsc1"];
                        entity.AccountNo1 = data["AccountNo1"];
                        entity.Bank2 = data["Bank2"];
                        entity.Ifsc2 = data["Ifsc2"];
                        entity.AccountNo2 = data["AccountNo2"];
                        entity.Bank3 = data["Bank3"];
                        entity.Ifsc3 = data["Ifsc3"];
                        entity.Account3 = data["Account3"];
                        entity.CreatedDate = DateTime.Now;
                        entity.Logo = data["Logo"];
                       
                        entity.IsActive = true;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entity);
                    }
                    _logger.LogDebug("Company Repo : Companies Updated");
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

        public JObject GetCompanyEInvoice(int companyId)
        {
            try
            {
                var entity = _tenantDBContext.Companies.Where(item => item.CompanyId == companyId).FirstOrDefault();
                if (entity != null)
                {
                    _logger.LogDebug("Company Repo : Company Retrieved");
                    var result = new JObject
                    {
                        ["EinvoicePassword"] = entity.EinvoicePassword ?? string.Empty,
                        ["EinvoiceUserName"] = entity.EinvoiceUserName ?? string.Empty,
                        ["EinvoiceKey"] = entity.EinvoiceKey ?? string.Empty,
                        ["EinvoiceSkey"] = entity.EinvoiceSkey ?? string.Empty,
                        ["PortalUserName"] = entity.PortaluserName ?? string.Empty,
                        ["PortalPw"] = entity.PortalPw ?? string.Empty,
                        ["PortalEmail"] = entity.PortalEmail ?? string.Empty,
                        ["Pan"] = entity.Pan ?? string.Empty,
                        ["AddressLine2"] = entity.AddressLine2 ?? string.Empty,
                        ["CellPhone"] = entity.CellPhone ?? string.Empty,
                        ["EInvoiceReq"] = entity.EinvoiceReq ?? 0,
                        ["Pin"] = entity.Pin ?? string.Empty
                    };
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

        public bool SaveEInvoice(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["CompanyData"].ToString());
                if (data != null)
                {
                    int id = data["CompanyId"];
                    var entity = _tenantDBContext.Companies.FirstOrDefault(item => item.CompanyId == id);
                if (entity != null)
                {
                        entity.EinvoicePassword = data["EinvoicePassword"];
                        entity.EinvoiceUserName = data["EinvoiceUserName"];
                        entity.EinvoiceKey = data["EinvoiceKey"];
                        entity.EinvoiceSkey = data["EinvoiceSkey"];
                        entity.PortaluserName = data["PortalUserName"];
                        entity.PortalPw = data["PortalPw"];
                        entity.PortalEmail = data["PortalEmail"];
                        entity.Pan = data["Pan"];
                        entity.AddressLine2 = data["AddressLine2"];
                        entity.CellPhone = data["CellPhone"];
                        entity.EinvoiceReq = data["EInvoiceReq"];
                        entity.Pin = data["Pin"];

                        entity.IsActive = true;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entity);
                    }
                    _logger.LogDebug("Company Repo : Companies Updated");
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
        public bool SaveAkadaEntry(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    var entity = new Inventory
                    {
                        CompanyId = data["CompanyId"],
                        LotNo = data["LotNo"],
                        Rate = data["Rate"],
                        NoOfBags = data["NoOfBags"],
                        TotalWeight = data["TotalWeight"],
                        Amount = data["Amount"],
                        Mark = data["Mark"],
                        CommodityId = data["CommodityId"],
                        CommodityName = data["CommodityName"],
                        IsActive = true,
                        LedgerId = data["LedgerId"],
                        //TranctDate = data["TranctDate"],
                    };

                    _tenantDBContext.Add(entity);
                    _tenantDBContext.SaveChanges();

                    this.SaveBagWeights(Data);
                    _logger.LogDebug("Company Repo : New bag weight added");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new inventory");
                throw;
            }
        }

        public bool SaveBagWeights(JObject data)
        {
            try
            {
                var akadaData = JsonConvert.DeserializeObject<dynamic>(data["AkadaData"].ToString());
                if (akadaData != null)
                {
                    // Assuming dgvBags is a DataGridView
                    foreach (var gridEntry in akadaData["GridEntries"])
                    {
                        double? bagWeight = Convert.ToDouble(gridEntry);

                        var bagWeightObject = new BagWeight
                        {
                            CompanyId = akadaData["CompanyId"],
                            VoucherId = akadaData["VoucherId"],
                            VoucherNumber = akadaData["VoucherNumber"],
                            LedgerId = akadaData["LedgerId"],
                            TranctDate = akadaData["TranctDate"],
                            LotNo = akadaData["LotNo"],
                            BagWeight1 = bagWeight
                        };

                        _tenantDBContext.Add(bagWeightObject);
                        _tenantDBContext.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new inventory");
                throw;
            }
            
        }

        public JObject Search(string searchvalue)
        {
            var response = new JObject();
            try
            {
                var result = (from c in _tenantDBContext.Companies
                              where (c.CompanyName.Contains(searchvalue) || c.Place.Contains(searchvalue) || c.Pan.Contains(searchvalue) || c.Email.Contains(searchvalue)
                              || c.Tan.Contains(searchvalue) || c.District.Contains(searchvalue) || c.CellPhone.Contains(searchvalue) || c.Gstin.Contains(searchvalue))
                              select new
                              {
                                  c._Id,
                                  c.KannadaName,
                                  c.Shree,
                                  c.CompanyName,
                                  c.AddressLine1,
                                  c.Pin,
                                  c.Tan,
                                  c.Apmccode,
                                  c.Place,
                                  c.Fln,
                                  c.Bin,
                                  c.District,
                                  c.Email,
                                  c.State,
                                  c.CellPhone,
                                  c.ContactDetails,
                                  c.Gstin,
                                  c.FirmCode,
                                  c.IsShopSent,
                                  c.Iec,
                                  c.IsActive
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

        public bool SaveOptionSettings(JObject data)
        {
            try
            {
                if (data != null && data.ContainsKey("CompanyId"))
                {
                    int companyId = data.Value<int>("CompanyId");
                    var companyEntity = _tenantDBContext.Companies.FirstOrDefault(item => item.CompanyId == companyId);
                    var ledgerEntity = _tenantDBContext.Ledgers.FirstOrDefault(item => item.CompanyId == companyId);

                    if (companyEntity != null && data.ContainsKey("Update"))
                    {
                        string updateType = data.Value<string>("Update");

                        switch (updateType)
                        {
                            case "TCS":
                                UpdateTCSOptions(companyEntity, data);
                                break;

                            case "TDS":
                                UpdateTDSOptions(companyEntity, ledgerEntity, data);
                                break;

                            // Handle other update types, if needed

                            default:
                                break;
                        }

                        _logger.LogDebug("Company Repo: Companies Updated");
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }


        public JObject GetOptionSettings(JObject data)
        {
            JObject result = new JObject();
            try
            {
                if (data != null && data.ContainsKey("CompanyId"))
                {
                    int id = data.Value<int>("CompanyId");
                    var companyEntity = _tenantDBContext.Companies.FirstOrDefault(item => item.CompanyId == id);
                    var ledgerEntity = _tenantDBContext.Ledgers.FirstOrDefault(item => item.CompanyId == id);

                    if (companyEntity != null)
                    {
                        result["AskIndtcs"] = (ledgerEntity != null) ? ledgerEntity.AskIndtcs : false;
                        result["AskTcs"] = companyEntity.AskTcs;
                        result["Tcsreq"] = companyEntity.Tcsreq;
                        result["TcsreqinReceipt"] = companyEntity.TcsreqinReceipt;
                    }
                    else
                    {
                        result["AskIndtcs"] = false;
                        result["AskTcs"] = false;
                        result["Tcsreq"] = false;
                        result["TcsreqinReceipt"] = false;
                    }
                }
                else
                {
                    result["AskIndtcs"] = false;
                    result["AskTcs"] = false;
                    result["Tcsreq"] = false;
                    result["TcsreqinReceipt"] = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                result["error"] = ex.Message;
                throw ex;
            }
        }
        private void UpdateTCSOptions(Company companyEntity, JObject data)
        {
            // New fields for TCS
            companyEntity.Tcsreq = data.Value<int>("TCSREQ");
            companyEntity.TcsreqinReceipt = data.Value<int>("TCSREQinReceipt");
            companyEntity.IsActive = true;

            _tenantDBContext.SaveChanges();
        }
        private void UpdateTDSOptions(Company companyEntity, Ledger ledgerEntity, JObject data)
        {
            companyEntity.AskTcs = data.Value<int>("AskTCS");

            if (ledgerEntity != null)
            {
                ledgerEntity.AskIndtcs = data.Value<int>("AskIndTCS");
                _tenantDBContext.SaveChanges();
            }

            companyEntity.IsActive = true;
            _tenantDBContext.SaveChanges();
        }


    }
}
