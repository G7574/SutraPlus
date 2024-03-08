using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.HadrData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.events.IndexEvents;
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

        public JObject GetOrderByMark(JObject Data)
        {
            var res = new JObject();
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedCompanyId = data["CompanyId"];
                    DateTime selectedDate = data["TranctDate"];
                    var result = _tenantDBContext.Inventory
                        .Where(n=>n.CompanyId == selectedCompanyId && n.NoOfBags > 0 && n.IsTender == 1 && n.IsActive == true && n.TranctDate == selectedDate)
                                                           .AsEnumerable().GroupBy(n=>n.Mark).ToList();

                    if(result != null)
                    {
                        res.Add("Mark", JArray.FromObject(result));
                    }

                }
                return res;
            } catch(Exception e)
            {
                throw e;
            }
        }

        public void TransferTo(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedLedgerId = data["LedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];
                    DateTime TransDate = data["TransDate"];
                    DateTime NewTransDate = data["NewTransDate"];

                    var inventoryToUpdate = _tenantDBContext.Inventory.Where(i => i.CompanyId == selectedCompanyId && i.VochType == 2 && i.VochNo == 0 && i.TranctDate == TransDate).ToList();

                    if (inventoryToUpdate != null)
                    {
                        foreach (var item in inventoryToUpdate)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    var bagWeightToUpdate = _tenantDBContext.BagWeights.Where(bw => bw.CompanyId == selectedCompanyId && bw.VoucherId == 2 && bw.VoucherNumber == 0 && bw.TranctDate == TransDate).ToList();

                    if (bagWeightToUpdate != null)
                    {
                        foreach (var item in bagWeightToUpdate)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    var voucherToUpdate = _tenantDBContext.Vouchers.Where(v => v.CompanyId == selectedCompanyId && v.VoucherId == 2 && v.VoucherNo == 0 && v.TranctDate == TransDate).ToList();

                    if (voucherToUpdate != null)
                    {
                        foreach (var item in voucherToUpdate)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    var billSummaryToUpdate = _tenantDBContext.BillSummaries.Where(bs => bs.CompanyId == selectedCompanyId && bs.VochType == 2 && bs.VochNo == 0 && bs.TranctDate == TransDate).ToList();

                    if (billSummaryToUpdate != null)
                    {
                        foreach (var item in billSummaryToUpdate)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    _tenantDBContext.SaveChanges();

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteAllAkadaEntry(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedLedgerId = data["LedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];
                    DateTime TransDate = data["TransDate"];

                    var toShowInventory = _tenantDBContext.Inventory
                        .Where(i => i.IsActive == true &&
                                    i.VochType == 2 &&
                                    i.VochNo == 0 &&
                                    i.CompanyId == selectedCompanyId &&
                                    i.LedgerId == selectedLedgerId &&
                                    i.CommodityId == selectedCommodityId).ToList();

                    if (toShowInventory != null)
                    {
                        foreach (var item in toShowInventory)
                        {
                            item.IsActive = false;
                        }
                    }

                    var bagWeightsToUpdate = _tenantDBContext.BagWeights.Where(bw => bw.CompanyId == selectedCompanyId && bw.LedgerId == selectedLedgerId && bw.TranctDate == TransDate).ToList();

                    foreach (var bagWeightToUpdate in bagWeightsToUpdate)
                    {
                        bagWeightToUpdate.IsActive = 0;
                    }

                    var vouchersToUpdate = _tenantDBContext.Vouchers.Where(v => v.CompanyId == selectedCompanyId && v.VoucherId == 2 && v.VoucherNo == 0 && v.TranctDate == TransDate).ToList();

                    foreach (var voucherToUpdate in vouchersToUpdate)
                    {
                        voucherToUpdate.IsActive = false;
                    }

                    var billSummariesToUpdate = _tenantDBContext.BillSummaries.Where(bs => bs.CompanyId == selectedLedgerId && bs.VochType == 2 && bs.VochNo == 0 && bs.TranctDate == TransDate).ToList();

                    foreach (var billSummaryToUpdate in billSummariesToUpdate)
                    {
                        billSummaryToUpdate.IsActive = false;
                    }


                    _tenantDBContext.SaveChanges();

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
         
        public JObject GetDatabaseNameFromConnectionString()
        {
            var res = new JObject();

            var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
            string DataSource = builder.DataSource;
            string UserID = builder.UserID;
            string Password = builder.Password;
            string dataBaseName = builder.InitialCatalog;

            res.Add("dataBaseName",dataBaseName);
            res.Add("DataSource", DataSource);
            res.Add("UserID", UserID);
            res.Add("Password", Password); 

            return res;
        }

        public void UpdatePartyInvoiceNumber(JObject Data)
        { 
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedLedgerId = data["LedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];
                    long LotNo = data["LotNo"]; 
                    string PartyInvoiceNumber = data["PartyInvoiceNumber"];

                    var result = _tenantDBContext.Inventory.Where(n => n.LedgerId == selectedLedgerId && n.CommodityId == selectedCommodityId &&
                                                            n.CompanyId == selectedCompanyId && n.LotNo == LotNo).FirstOrDefault();

                    if (result != null)
                    {
                        result.PartyInvoiceNumber = PartyInvoiceNumber;
                    }
                    _tenantDBContext.SaveChanges();
                }

                } catch(Exception e)
            {

            }

        }


        public void DeleteAkadaEntry(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedLedgerId = data["LedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];
                    DateTime TransDate = data["TransDate"];
                    long LotNo = data["LotNo"];
                    long Id = data["Id"];

                    var result = _tenantDBContext.Inventory.Where(n => n.LedgerId == selectedLedgerId && n.CommodityId == selectedCommodityId &&
                                                            n.CompanyId == selectedCompanyId && n.LotNo == LotNo && n.Id == Id).FirstOrDefault();

                    if (result != null)
                    {
                        result.IsActive = false;
                    }

                    var bagWeightsToUpdate = _tenantDBContext.BagWeights.Where(bw => bw.CompanyId == selectedCompanyId && bw.LedgerId == selectedLedgerId && bw.LotNo == LotNo && bw.TranctDate == TransDate).ToList();

                    foreach (var bagWeightToUpdate in bagWeightsToUpdate)
                    {
                        bagWeightToUpdate.IsActive = 0;
                    }

                    var vouchersToUpdate = _tenantDBContext.Vouchers.Where(v => v.CompanyId == selectedCompanyId && v.VoucherId == 2 && v.VoucherNo == 0 && v.TranctDate == TransDate).ToList();

                    foreach (var voucherToUpdate in vouchersToUpdate)
                    {
                        voucherToUpdate.IsActive = false;
                    }

                    var billSummariesToUpdate = _tenantDBContext.BillSummaries.Where(bs => bs.CompanyId == selectedLedgerId && bs.VochType == 2 && bs.VochNo == 0 && bs.TranctDate == TransDate).ToList();

                    foreach (var billSummaryToUpdate in billSummariesToUpdate)
                    {
                        billSummaryToUpdate.IsActive = false;
                    }


                    _tenantDBContext.SaveChanges();

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateAkadaParty(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedLedgerId = data["LedgerId"];
                    long NewLedgerId = data["NewLedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];
                    DateTime TransDate = data["TransDate"];

                    var result = _tenantDBContext.Inventory.Where(n => n.LedgerId == selectedLedgerId && n.CommodityId == selectedCommodityId &&
                                                            n.CompanyId == selectedCompanyId).ToList();

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.LedgerId = NewLedgerId;
                        }
                    }

                    var BagWeights = _tenantDBContext.BagWeights.Where(n => n.CompanyId == selectedCompanyId && n.LedgerId == selectedLedgerId).ToList();

                    if (BagWeights != null)
                    {
                        foreach (var item in BagWeights)
                        {
                            item.LedgerId = int.Parse(NewLedgerId.ToString());
                        }
                    }

                    var voucher = _tenantDBContext.Vouchers.Where(n => n.CompanyId == selectedCompanyId && n.LedgerId == selectedLedgerId && n.TranctDate == TransDate).ToList();
                    if(voucher != null)
                    {
                        foreach (var item in voucher)
                        {
                            item.LedgerId = NewLedgerId;
                        }
                    }

                    var billSummary = _tenantDBContext.BillSummaries.Where(n => n.CompanyId == selectedCompanyId && n.LedgerId == selectedLedgerId && n.TranctDate == TransDate).ToList();
                    if (billSummary != null)
                    {
                        foreach (var item in billSummary)
                        {
                            item.LedgerId = NewLedgerId;
                        }
                    }

                    _tenantDBContext.SaveChanges();

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateAkadaTransDate(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedLedgerId = data["LedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];
                    DateTime TransDate = data["TransDate"];
                    DateTime NewTransDate = data["NewTransDate"];

                    var result = _tenantDBContext.Inventory.Where(n => n.LedgerId == selectedLedgerId && n.CommodityId == selectedCommodityId &&
                                                            n.CompanyId == selectedCompanyId).ToList();

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    var BagWeights = _tenantDBContext.BagWeights.Where(n => n.CompanyId == selectedCompanyId && n.LedgerId == selectedLedgerId).ToList();

                    if (BagWeights != null)
                    {
                        foreach (var item in BagWeights)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    var voucher = _tenantDBContext.Vouchers.Where(n => n.CompanyId == selectedCompanyId && n.VoucherId == 2 && n.VoucherNo == 0 && n.TranctDate == TransDate).ToList();

                    if (voucher != null)
                    {
                        foreach (var item in voucher)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    var billSummary = _tenantDBContext.BillSummaries.Where(n => n.CompanyId == selectedCompanyId && n.VochType == 0 && n.VochNo == 0 && n.TranctDate == TransDate).ToList();

                    if (billSummary != null)
                    {
                        foreach (var item in billSummary)
                        {
                            item.TranctDate = NewTransDate;
                        }
                    }

                    _tenantDBContext.SaveChanges();
                     
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateAkadaEntry(JObject Data) { 
         
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long selectedLedgerId = data["LedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];
                    long Rate = data["Rate"];
                    long LotNo = data["LotNo"];
                    long Amount = data["Amount"];
                    long TotalWeight = data["TotalWeight"];
                    long Id = data["Id"];

                    var result = _tenantDBContext.Inventory.Where(n=>n.LedgerId == selectedLedgerId && n.CommodityId == selectedCommodityId &&
                                                            n.CompanyId == selectedCompanyId && n.LotNo == LotNo && n.Id == Id).FirstOrDefault();

                    if(result != null)
                    {
                        result.Rate = Rate;
                        result.TotalWeight = TotalWeight;
                        result.Amount = Amount;
                        _tenantDBContext.SaveChanges();
                    }

                    var akadaData = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                    if (akadaData != null)
                    {
                        var bagWeightsToUpdate = _tenantDBContext.BagWeights
                            .Where(b => b.CompanyId == selectedCompanyId && b.VoucherId == 2 &&
                                        b.LedgerId == selectedLedgerId && b.LotNo == LotNo)
                            .ToList();

                        var gridEntries = akadaData["GridEntries"];

                        for (int i = 0; i < gridEntries.Count; i++)
                        {
                            var gridEntry = gridEntries[i];
                            if (!string.IsNullOrEmpty(gridEntry.ToString()))
                            {
                                double? bagWeight = Convert.ToDouble(gridEntry);

                                if (i < bagWeightsToUpdate.Count)
                                {
                                    var existingBagWeight = bagWeightsToUpdate[i];
                                    existingBagWeight.BagWeight1 = bagWeight;
                                }
                            }
                        }

                        _tenantDBContext.SaveChanges();
                    }
   
                }

            } catch (Exception e)
            {
                throw e;
            }

        }

        public JObject SaveAkadaEntry(JObject Data)
        {
            var res = new JObject();

            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    Double PackingRate = 0;
                    Double HamaliRate = 0;
                    Double WeighManFeeRate = 0;
                    Double DalaliRate = 0;
                    Double CessRate = 0;

                    Double IGST = 0;
                    Double SGST = 0;
                    Double CGST = 0;

                    long selectedLedgerId = data["LedgerId"];
                    long selectedCommodityId = data["CommodityId"];
                    long selectedCompanyId = data["CompanyId"];

                    long selectedTotalNoOfbags = data["NoOfBags"];
                    long selectedTotalAmount = data["Amount"];

                    var ledgerData = _tenantDBContext.Ledgers.Where(n => n.LedgerId == selectedLedgerId && n.CompanyId == selectedCompanyId).FirstOrDefault();
                    if (ledgerData != null)
                    {
                        PackingRate = Math.Round(Convert.ToDouble(ledgerData.PackingRate), 2);
                        HamaliRate = Math.Round(Convert.ToDouble(ledgerData.HamaliRate), 2);
                        WeighManFeeRate = Math.Round(Convert.ToDouble(ledgerData.WeighManFeeRate), 2);
                        DalaliRate = Math.Round(Convert.ToDouble(ledgerData.DalaliRate), 2);
                        CessRate = Math.Round(Convert.ToDouble(ledgerData.CessRate), 2);
                    }

                    var commodityData = _tenantDBContext.Commodities.Where(n => n.CommodityId == selectedCommodityId).FirstOrDefault();
                    if (commodityData != null)
                    {
                        IGST = Math.Round(Convert.ToDouble(commodityData.IGST), 2);
                        SGST = Math.Round(Convert.ToDouble(commodityData.SGST), 2);
                        CGST = Math.Round(Convert.ToDouble(commodityData.CGST), 2);
                    }

                    var Packing = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * PackingRate), 2);
                    var Hamali = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * HamaliRate), 2);
                    var Weighman_Fee = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * WeighManFeeRate), 2);
                    var Commission = Math.Round((Convert.ToDouble(selectedTotalAmount * DalaliRate / 100)), 2);
                    var Cess = Math.Round((Convert.ToDouble(selectedTotalAmount * CessRate) / 100), 2);
                    var Taxable_Value = Math.Round(Convert.ToDouble(selectedTotalAmount + Packing + Hamali + Weighman_Fee + Commission + Cess), 2);

                    var SGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * SGST) / 100), 2);
                    var CGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * CGST) / 100), 2);


                    var entity = new Inventory
                    {
                        CompanyId = data["CompanyId"],
                        TranctDate = data["TranctDate"],
                        LotNo = data["LotNo"],
                        Rate = data["Rate"],
                        NoOfBags = data["NoOfBags"] ?? 0,
                        TotalWeight = data["TotalWeight"] ?? 0,
                        Amount = data["Amount"] ?? 0,
                        Mark = data["Mark"] ?? 0,
                        CommodityId = data["CommodityId"] ?? 0,
                        CommodityName = data["CommodityName"] ?? 0,
                        IsActive = true,
                        LedgerId = data["LedgerId"] ?? 0,
                        VochType = 2,
                        VochNo = 0,
                        IsTender = 1,

                        PartyInvoiceNumber = "", 
                        InvoiceType = "",

                        NetAmount = data["Amount"],
                        SGST = Convert.ToDecimal(SGST_Tot),
                        CGST = Convert.ToDecimal(CGST_Tot),
                        IGST = 0,
                        NoOfDocra = 0,  
                        CreatedDate = DateTime.Now,
                        FreeQty = 0,
                        IGSTRate = Convert.ToDecimal(IGST),
                        SGSTRate = Convert.ToDecimal(SGST),
                        CGSTRate = Convert.ToDecimal(CGST),
                        Taxable = Convert.ToDecimal(Taxable_Value),




                        //TranctDate = data["TranctDate"],
                    };

                    _tenantDBContext.Add(entity);
                    _tenantDBContext.SaveChanges();
                     
 
                        return this.SaveBagWeights(Data);
                   
                    _logger.LogDebug("Company Repo : New bag weight added");
                     
                }
                else
                    return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new inventory");
                throw;
            }
        }

        public JObject GetLotNoData(JObject Data)
        {
            var res = new JObject();

            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long lotNo = data["LotNo"];
                    DateTime date = data["TranctDate"];
                    long CompanyId = data["CompanyId"];
                    long ledgerId = data["LedgerId"];
                    long commodityId = data["CommodityId"];

                    var existingInventory = _tenantDBContext.Inventory
                                            .Where(i => i.CompanyId == CompanyId &&
                                                        i.VochType == 2 &&
                                                        i.VochNo == 0 &&
                                                        i.LedgerId == ledgerId &&
                                                        i.IsActive == true &&
                                                        i.CommodityId == commodityId)
                                            .OrderBy(n=>n.LotNo).ToList();

                    if (existingInventory != null)
                    {
                        res.Add("existingInventory", JArray.FromObject(existingInventory));
                    }

                    return res;

                }
                return res;

                } catch(Exception ex)
            {
                throw ex;
            }

        }

        public JObject GetBills(JObject Data)
        {
            var res = new JObject();

            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long CompanyId = data["CompanyId"];
                    long ledgerId = 0;

                    /* var query = from a in _tenantDBContext.Inventory
                                 join b in _tenantDBContext.Ledgers on a.LedgerId equals b.LedgerId
                                 where a.VochNo == 0 && a.IsTender == 1 && a.IsActive == true
                                 group new { a, b } by b.LedgerName into g
                                 select new
                                 {
                                     tranctdate = g.Max(x => x.a.TranctDate),
                                     ledgerid = g.Key,
                                     total_no_of_bags = g.Sum(x => x.a.NoOfBags),
                                     total_weight = g.Sum(x => x.a.TotalWeight),
                                     total_amount = g.Sum(x => x.a.Amount),
                                     total_taxable_amount = g.Sum(x => x.a.Taxable),
                                     total_inclusive_amount = g.Sum(x => x.a.Taxable + x.a.SGST + x.a.CGST),
                                     legder_name = g.Key,
                                     rate = g.Max(x => x.a.Rate),
                                     lotNo = g.Max(x => x.a.LotNo),
                                     mark = g.Max(x => x.a.Mark),
                                     commodityId = g.Max(x => x.a.CommodityId),
                                     ledger_id = g.Max(x => x.a.LedgerId),
                                     gstin = g.Max(x => x.b.Gstn)
                                 };*/

                    var query = from a in _tenantDBContext.Inventory
                                join b in _tenantDBContext.Ledgers on a.LedgerId equals b.LedgerId
                                where a.VochNo == 0 && a.IsTender == 1 && a.IsActive == true && a.CompanyId == CompanyId
                                group new { a, b } by new { a.TranctDate, b.LedgerName } into g
                                select new
                                {
                                    tranctdate = g.Key.TranctDate,
                                    ledger_name = g.Key.LedgerName,
                                    total_no_of_bags = g.Sum(x => x.a.NoOfBags),
                                    total_weight = g.Sum(x => x.a.TotalWeight),
                                    total_amount = g.Sum(x => x.a.Amount),
                                    total_taxable_amount = g.Sum(x => x.a.Taxable),
                                    total_inclusive_amount = g.Sum(x => x.a.Taxable + x.a.SGST + x.a.CGST),
                                    rate = g.Max(x => x.a.Rate),

                                    lotNo = g.Max(x => x.a.LotNo),
                                    mark = g.Max(x => x.a.Mark),
                                    commodityId = g.Max(x => x.a.CommodityId),
                                    ledger_id = g.Max(x => x.a.LedgerId),
                                    gstin = g.Max(x => x.b.Gstn)
                                };


                    /* var query = from a in _tenantDBContext.Inventory
                                 join b in _tenantDBContext.Ledgers on a.LedgerId equals b.LedgerId
                                 where a.VochNo == 0 && a.IsTender == 1
                                 group new { a, b } by new { b.LedgerName, a.TranctDate, a.LotNo, a.Mark, a.CommodityId, a.LedgerId, b.Gstn, a.Rate } into g
                                 select new
                                 {
                                     tranctdate = g.Key.TranctDate,
                                     ledgerid = g.Key.LedgerId,
                                     total_no_of_bags = g.Sum(x => x.a.NoOfBags),
                                     total_weight = g.Sum(x => x.a.TotalWeight),
                                     total_amount = g.Sum(x => x.a.Amount),
                                     total_taxable_amount = g.Sum(x => x.a.Taxable),
                                     total_inclusive_amount = g.Sum(x => x.a.Taxable + x.a.SGST + x.a.CGST),
                                     legder_name = g.Key.LedgerName,
                                     rate = g.Key.Rate,
                                     lotNo = g.Key.LotNo,
                                     mark = g.Key.Mark,
                                     commodityId = g.Key.CommodityId,
                                     ledger_id = g.Key.LedgerId,
                                     gstin = g.Key.Gstn
                                 };*/

                    var commodityData = _tenantDBContext.Commodities.Where(n => n.CompanyId == CompanyId).ToList();
                    if (commodityData != null)
                    {
                        res.Add("commodityData", JArray.FromObject(commodityData));
                    }

                    var resutl = query.ToList();
                    res.Add("Data",JArray.FromObject(resutl));
  
                }
                return res;

            }catch(Exception ex) {
                throw ex;
            }

        }

        public JObject GetEditVerifyBillData(JObject data)
        {
            var response = new JObject();

            try
            {

                var inventory = JsonConvert.DeserializeObject<dynamic>(data["AkadaData"].ToString());

                if(inventory != null)
                {

                    Double PackingRate = 0;
                    Double HamaliRate = 0;
                    Double WeighManFeeRate = 0;
                    Double DalaliRate = 0;
                    Double CessRate = 0;

                    Double IGST = 0;
                    Double SGST = 0;
                    Double CGST = 0;

                    long selectedLedgerId = inventory["LedgerId"];
                    long selectedCommodityId = inventory["CommodityId"];
                    long selectedCompanyId = inventory["CompanyId"];

                    long selectedTotalNoOfbags = inventory["NoOfBags"];
                    long selectedTotalAmount = inventory["Amount"];
                     
                    var ledgerData = _tenantDBContext.Ledgers.Where(n => n.LedgerId == selectedLedgerId && n.CompanyId == selectedCompanyId).FirstOrDefault();
                    if (ledgerData != null)
                    {
                        PackingRate = Math.Round(Convert.ToDouble(ledgerData.PackingRate), 2);
                        HamaliRate = Math.Round(Convert.ToDouble(ledgerData.HamaliRate), 2);
                        WeighManFeeRate = Math.Round(Convert.ToDouble(ledgerData.WeighManFeeRate), 2);
                        DalaliRate = Math.Round(Convert.ToDouble(ledgerData.DalaliRate), 2);
                        CessRate = Math.Round(Convert.ToDouble(ledgerData.CessRate), 2);
                    }

                    var commodityData = _tenantDBContext.Commodities.Where(n => n.CommodityId == selectedCommodityId).FirstOrDefault();
                    if (commodityData != null)
                    {
                        IGST = Math.Round(Convert.ToDouble(commodityData.IGST), 2);
                        SGST = Math.Round(Convert.ToDouble(commodityData.SGST), 2);
                        CGST = Math.Round(Convert.ToDouble(commodityData.CGST), 2);
                    }

                    var Packing = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * PackingRate), 2);
                    var Hamali = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * HamaliRate), 2);
                    var Weighman_Fee = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * WeighManFeeRate), 2);
                    var Commission = Math.Round((Convert.ToDouble(selectedTotalAmount * DalaliRate / 100)), 2);
                    var Cess = Math.Round((Convert.ToDouble(selectedTotalAmount * CessRate) / 100), 2);
                    var Taxable_Value = Math.Round(Convert.ToDouble(selectedTotalAmount + Packing + Hamali + Weighman_Fee + Commission + Cess), 2);
                    var SGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * SGST) / 100), 2);
                    var CGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * CGST) / 100), 2);
                    var OtherExpenses = 0;
                    var Grand_Total = Taxable_Value + SGST_Tot + CGST_Tot + OtherExpenses;
                    if (Grand_Total == null)
                    {
                        Grand_Total = 0;
                    }
                    var roundOff = Math.Round(Math.Round(Convert.ToDouble(Grand_Total), 0) - Convert.ToDouble(Grand_Total), 2);

                    Grand_Total = Math.Round(Convert.ToDouble(Grand_Total + roundOff), 2);
                     
                    DateTime selectedDate = inventory["TranctDate"];


                    var toShowInventory = _tenantDBContext.Inventory
                        .Where(i => i.IsActive == true &&
                                    i.VochType == 2 &&
                                    i.VochNo == 0 && 
                                    i.CompanyId == selectedCompanyId &&
                                    i.LedgerId == selectedLedgerId &&
                                    i.CommodityId == selectedCommodityId)
                        .FirstOrDefault();

                    if (toShowInventory != null)
                    {

                        response.Add("VochType", toShowInventory.VochType);
                        response.Add("VochNo", toShowInventory.VochNo);
                        response.Add("LedgerId", toShowInventory.LedgerId);
                        response.Add("commodityId", toShowInventory.CommodityId);
                        response.Add("commodityName", toShowInventory.CommodityName);
                        response.Add("partyInvoiceNumber", toShowInventory.PartyInvoiceNumber);
                        response.Add("lotNo", toShowInventory.LotNo);
                        response.Add("noOfBags", toShowInventory.NoOfBags);
                        response.Add("rate", toShowInventory.Rate);
                        response.Add("totalWeight", toShowInventory.TotalWeight);
                        response.Add("amount", toShowInventory.Amount);
                        response.Add("mark", toShowInventory.Mark);
                        response.Add("trancdate", toShowInventory.TranctDate);

                        response.Add("packing", Packing);
                        response.Add("hamali", Hamali);
                        response.Add("weightManFee", Weighman_Fee);
                        response.Add("commission", Commission);
                        response.Add("cess", Cess);
                        response.Add("taxableValue", Taxable_Value);
                        response.Add("sgst", SGST_Tot);
                        response.Add("cgst", CGST_Tot);
                        response.Add("grandTotal", Grand_Total);
                        response.Add("roundOff", roundOff);

                    }

                    var toShowLedger = _tenantDBContext.Ledgers.FirstOrDefault(n => n.LedgerId == selectedLedgerId);

                    if (toShowLedger != null)
                    {
                        response.Add("PartyName", toShowLedger.LedgerName);
                        response.Add("gstin", toShowLedger.Gstin);
                    }
                      
                }
                   
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public JObject GetBagWeight(JObject Data)
        {
            var response = new JObject();
            try
            {
                var dt = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (dt != null)
                {  
                    long ledgerId = dt["LedgerId"];
                    long companyId = dt["companyId"];
                    long LotNo = dt["LotNo"];
                    DateTime trans = dt["TransDate"];
                    long Id = dt["Id"];
                    long commodityId = dt["commodityId"];
                    var bagData = _tenantDBContext.BagWeights.Where(n => n.LotNo == LotNo && n.CompanyId == companyId &&
                                            n.LedgerId == ledgerId && n.VoucherId == 2 && n.VoucherNumber == 0 && n.TranctDate == trans).ToList();

                    if (bagData != null)
                    {
                        response.Add("bagData", JArray.FromObject(bagData));
                    }

                    var result = _tenantDBContext.Inventory.Where(n => n.LedgerId == ledgerId && n.CommodityId == commodityId &&
                                                            n.CompanyId == companyId && n.LotNo == LotNo && n.Id == Id).FirstOrDefault();

                    if (result != null)
                    { 
                       response.Add("TranctDate", result.TranctDate);
                    }
                }
                 
                return response;

            } catch(Exception ex) {
                throw ex;
            }
        }

        public JObject GetLastlyAddedRecord(JObject Data)
        {
            var res = new JObject();

            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                { 
                    long CompanyId = data["CompanyId"]; 

                    var existingInventory = _tenantDBContext.Inventory
                                            .OrderByDescending(x => x.Id)  
                                  .FirstOrDefault();

                    if (existingInventory != null)
                    {
                        var d = _tenantDBContext.Ledgers.Where(n => n.LedgerId == existingInventory.LedgerId).FirstOrDefault();
                        if(d != null)
                        {
                            res.Add("PartyName", d.LedgerName);
                        }
                        res.Add("TranctDate", existingInventory.TranctDate);
                        res.Add("LotNo", existingInventory.LotNo);
                        res.Add("NoOfBags", existingInventory.NoOfBags);
                    }

                    return res;

                }
                return res;

                } catch(Exception ex)
            {
                throw ex;
            }

        }

        public Boolean GetLotNO(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["AkadaData"].ToString());
                if (data != null)
                {
                    long lotNo = Convert.ToInt64(data["lotNo"]);
                    DateTime date = Convert.ToDateTime(data["date"]);
                    long CompanyId = Convert.ToInt64(data["CompanyId"]);
                    long ledgerId = Convert.ToInt64(data["ledgerId"]);
                    long commodityId = Convert.ToInt64(data["commodityId"]);

                    var existingInventory = _tenantDBContext.Inventory
                   .Where(i => i.LotNo == lotNo &&
                               i.TranctDate == date &&
                               i.CompanyId == CompanyId &&
                               i.VochType == 2 &&
                               i.VochNo == 0 &&
                               i.LedgerId == ledgerId &&
                               i.IsActive == true &&
                               i.CommodityId == commodityId)
                   .FirstOrDefault();
                       
                    if (existingInventory != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new inventory");
                throw;
            }
        }

          
        public JObject SaveBagWeights(JObject data)
        {
            var res = new JObject();
  
            try
            {
                var akadaData = JsonConvert.DeserializeObject<dynamic>(data["AkadaData"].ToString());
                if (akadaData != null)
                {
                    // Assuming dgvBags is a DataGridView
                    foreach (var gridEntry in akadaData["GridEntries"])
                    {
                       
                        if (gridEntry != "" && gridEntry != null)
                        {
                            double? bagWeight = Convert.ToDouble(gridEntry);
                            var bagWeightObject = new BagWeight
                            {
                                CompanyId = akadaData["CompanyId"],
                                VoucherId = 2,
                                VoucherNumber = 0,
                                LedgerId = akadaData["LedgerId"],
                                TranctDate = akadaData["TranctDate"],
                                LotNo = akadaData["LotNo"],
                                BagWeight1 = bagWeight
                            };

                            _tenantDBContext.Add(bagWeightObject);
                            _tenantDBContext.SaveChanges();
                        }
                       
                    }
                    return doCalculations(akadaData);
                    //return res;
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new inventory");
                throw;
            }
            
        }

        public JObject doCalculations(JObject inventory)
        {
            var response = new JObject();
            
            try
            {
                Double PackingRate = 0;
                Double HamaliRate = 0;
                Double WeighManFeeRate = 0;
                Double DalaliRate = 0;
                Double CessRate = 0;

                Double IGST = 0;
                Double SGST = 0;
                Double CGST = 0;
                  
                var selectedLedgerId = Convert.ToDouble(inventory["LedgerId"]);
                var selectedCommodityId = Convert.ToDouble(inventory["CommodityId"]);
                var selectedCompanyId = Convert.ToDouble(inventory["CompanyId"]);

                var selectedTotalNoOfbags = Convert.ToDouble(inventory["NoOfBags"]);
                var selectedTotalAmount = Convert.ToDouble(inventory["Amount"]);

                var ledgerData = _tenantDBContext.Ledgers.Where(n => n.LedgerId == selectedLedgerId && n.CompanyId == selectedCompanyId).FirstOrDefault();
                if(ledgerData != null)
                {
                    PackingRate = Math.Round(Convert.ToDouble(ledgerData.PackingRate), 2);
                    HamaliRate = Math.Round(Convert.ToDouble(ledgerData.HamaliRate),2);
                    WeighManFeeRate = Math.Round(Convert.ToDouble(ledgerData.WeighManFeeRate),2);
                    DalaliRate = Math.Round(Convert.ToDouble(ledgerData.DalaliRate),2);
                    CessRate = Math.Round(Convert.ToDouble(ledgerData.CessRate),2);
                }

                var commodityData = _tenantDBContext.Commodities.Where(n=>n.CommodityId == selectedCommodityId).FirstOrDefault();
                if(commodityData != null)
                {
                    IGST = Math.Round(Convert.ToDouble(commodityData.IGST),2);
                    SGST = Math.Round(Convert.ToDouble(commodityData.SGST),2);
                    CGST = Math.Round(Convert.ToDouble(commodityData.CGST),2);
                }

                var Packing = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * PackingRate), 2);
                var Hamali = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * HamaliRate),2);
                var Weighman_Fee = Math.Round(Convert.ToDouble(selectedTotalNoOfbags * WeighManFeeRate),2);
                var Commission = Math.Round((Convert.ToDouble(selectedTotalAmount * DalaliRate / 100)) ,2);
                var Cess = Math.Round((Convert.ToDouble(selectedTotalAmount * CessRate) / 100), 2);
                var Taxable_Value = Math.Round(Convert.ToDouble(selectedTotalAmount + Packing + Hamali + Weighman_Fee + Commission + Cess),2);
                var SGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * SGST) / 100),2);
                var CGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * CGST) / 100),2);
                var OtherExpenses = 0;
                var Grand_Total = Taxable_Value + SGST_Tot + CGST_Tot + OtherExpenses;
                if(Grand_Total == null)
                {
                    Grand_Total = 0;
                }
                var roundOff = Math.Round(Math.Round(Convert.ToDouble(Grand_Total), 0) - Convert.ToDouble(Grand_Total), 2);

                Grand_Total = Math.Round(Convert.ToDouble(Grand_Total + roundOff),2);

                response.Add("packing", Packing);   
                response.Add("hamali", Hamali);
                response.Add("weightManFee", Weighman_Fee);
                response.Add("commission", Commission);
                response.Add("cess", Cess);
                response.Add("taxableValue", Taxable_Value);
                response.Add("sgst", SGST_Tot);
                response.Add("cgst", CGST_Tot);
                response.Add("grandTotal", Grand_Total);
                response.Add("roundOff", roundOff);
                 

                var selectedDate = Convert.ToDateTime(inventory["TranctDate"]);

                /*string connectionString = "Server=103.50.212.163;Database=K2223RGP;uid=sa;Password=root@123;TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT * FROM Inventory WHERE IsActive = @IsActive 
                    AND VochType = @VochType AND VochNo = @VochNo 
                    AND TranctDate = @TranctDate AND CompanyId = @CompanyId 
                    AND LedgerId = @LedgerId AND CommodityId = @CommodityId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IsActive", true);
                        command.Parameters.AddWithValue("@VochType", 2);
                        command.Parameters.AddWithValue("@VochNo", 0);
                        command.Parameters.AddWithValue("@TranctDate", selectedDate);
                        command.Parameters.AddWithValue("@CompanyId", selectedCompanyId);
                        command.Parameters.AddWithValue("@LedgerId", selectedLedgerId);
                        command.Parameters.AddWithValue("@CommodityId", selectedCommodityId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())  
                            {
                                Inventory toShowInventory = new Inventory();

                                toShowInventory.VochType = Convert.ToInt32(reader["VochType"]);
                                toShowInventory.VochNo = Convert.ToInt32(reader["VochNo"]);
                                toShowInventory.LedgerId = Convert.ToInt32(reader["LedgerId"]);
                                toShowInventory.CommodityId = Convert.ToInt32(reader["CommodityId"]);
                                toShowInventory.CommodityName = Convert.ToString(reader["CommodityName"]);
                                toShowInventory.PartyInvoiceNumber = Convert.ToString(reader["PartyInvoiceNumber"]);
                                toShowInventory.LotNo = Convert.ToInt32(reader["LotNo"]);
                                toShowInventory.NoOfBags = Convert.ToInt32(reader["NoOfBags"]);
                                toShowInventory.Rate = Convert.ToDecimal(reader["Rate"]);
                                toShowInventory.TotalWeight = Convert.ToDouble(reader["TotalWeight"]);
                                toShowInventory.Amount = Convert.ToDecimal(reader["Amount"]);
                                toShowInventory.Mark = Convert.ToString(reader["Mark"]);
                                toShowInventory.TranctDate = Convert.ToDateTime(reader["TranctDate"]);

                                response.Add("VochType", toShowInventory.VochType);
                                response.Add("VochNo", toShowInventory.VochNo);
                                response.Add("LedgerId", toShowInventory.LedgerId);
                                response.Add("commodityId", toShowInventory.CommodityId);
                                response.Add("commodityName", toShowInventory.CommodityName);
                                response.Add("partyInvoiceNumber", toShowInventory.PartyInvoiceNumber);
                                response.Add("lotNo", toShowInventory.LotNo);
                                response.Add("noOfBags", toShowInventory.NoOfBags);
                                response.Add("rate", toShowInventory.Rate);
                                response.Add("totalWeight", toShowInventory.TotalWeight);
                                response.Add("amount", toShowInventory.Amount);
                                response.Add("mark", toShowInventory.Mark);
                                response.Add("trancdate", toShowInventory.TranctDate);
                            }
                            reader.Close();
                        }
                    }
                }*/

                
                    var toShowInventory = _tenantDBContext.Inventory
                        .Where(i => i.IsActive == true &&
                                    i.VochType == 2 &&
                                    i.VochNo == 0 &&
                                    i.TranctDate == selectedDate &&
                                    i.CompanyId == selectedCompanyId &&
                                    i.LedgerId == selectedLedgerId &&
                                    i.CommodityId == selectedCommodityId)
                        .FirstOrDefault();

                    if (toShowInventory != null)
                    {
                        response.Add("VochType", toShowInventory.VochType);
                        response.Add("VochNo", toShowInventory.VochNo);
                        response.Add("LedgerId", toShowInventory.LedgerId);
                        response.Add("commodityId", toShowInventory.CommodityId);
                        response.Add("commodityName", toShowInventory.CommodityName);
                        response.Add("partyInvoiceNumber", toShowInventory.PartyInvoiceNumber);
                        response.Add("lotNo", toShowInventory.LotNo);
                        response.Add("noOfBags", toShowInventory.NoOfBags);
                        response.Add("rate", toShowInventory.Rate);
                        response.Add("totalWeight", toShowInventory.TotalWeight);
                        response.Add("amount", toShowInventory.Amount);
                        response.Add("mark", toShowInventory.Mark);
                        response.Add("trancdate", toShowInventory.TranctDate);
                    }
                 

                /*var toShowInventory = new Inventory();

                toShowInventory = _tenantDBContext.Inventory.Where(n => n.IsActive == true && n.VochType == 2 && n.VochNo == 0 && n.TranctDate == selectedDate &&
                                        n.CompanyId == selectedCompanyId && n.LedgerId == selectedLedgerId && n.CommodityId == selectedCommodityId).FirstOrDefault();

                if (toShowInventory != null)    
                {
                    response.Add("VochType", toShowInventory.VochType);
                    response.Add("VochNo", toShowInventory.VochNo);
                    response.Add("LedgerId", toShowInventory.LedgerId);
                    response.Add("commodityId", toShowInventory.CommodityId);
                    response.Add("commodityName", toShowInventory.CommodityName);
                    response.Add("partyInvoiceNumber", toShowInventory.PartyInvoiceNumber);
                    response.Add("lotNo", toShowInventory.LotNo);
                    response.Add("noOfBags", toShowInventory.NoOfBags);
                    response.Add("rate", toShowInventory.Rate);
                    response.Add("totalWeight", toShowInventory.TotalWeight);
                    response.Add("amount", toShowInventory.Amount);
                    response.Add("mark", toShowInventory.Mark);
                    response.Add("trancdate", toShowInventory.TranctDate);
                }*/

                var toShowLedger = _tenantDBContext.Ledgers.FirstOrDefault(n=>n.LedgerId == selectedLedgerId);
                
                if (toShowLedger != null)
                {
                    response.Add("PartyName", toShowLedger.LedgerName);
                    response.Add("gstin", toShowLedger.Gstin);
                }

            }
            catch (Exception ex) {
                throw ex;
            }

                return response;
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
