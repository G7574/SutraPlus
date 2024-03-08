using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus.Models;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Models.DTO;
using SutraPlus_DAL.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SutraPlus_DAL.Repository.SalesRepository;

namespace SutraPlus_BAL.Service
{
    public class SalesService
    {
        public SalesRepository _salesRepository = null;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public IConfiguration _configuration { get; }
        private EmailSender _emailSender = null;
        public SalesService(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _masterDBContext = masterDBContext;
            _logger = logger;
            _configuration = configuration;
            _emailSender = new EmailSender(_configuration);
            _salesRepository = new SalesRepository(tenantID, masterDBContext, _configuration, _logger);
        }
        public JObject Get(JObject Data)
        {
            try
            {
                var response = new JObject();
                var data = JsonConvert.DeserializeObject<dynamic>(Data["SalesDetails"].ToString());
                int CompanyId = data["CompanyId"];
                int LedgerId = data["LedgerId"];
                string DealerType = data["DealerType"];
                string InvoiceType = data["InvoiceType"];
                response = _salesRepository.Get(CompanyId, LedgerId, DealerType, InvoiceType);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        
        public JObject GetInvtype(JObject Data)
        {
            try
            {
                var response = new JObject();
                var data = JsonConvert.DeserializeObject<dynamic>(Data["SalesDetails"].ToString());
                int CompanyId = data["CompanyId"]; 
                int LedgerId = data["LedgerId"]; 
                int VoucherNumber = data["VoucherNumber"]; 
                int VoucherType = data["VoucherType"]; 
                response = _salesRepository.GetInvtype(CompanyId, LedgerId, VoucherNumber, VoucherType);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetEinvoiceKey(JObject Data)
        {
            try
            {
                var response = new JObject();
                var data = JsonConvert.DeserializeObject<dynamic>(Data["SalesDetails"].ToString());
                int CompanyId = data["CompanyId"]; 
                response = _salesRepository.GetEinvoiceKey(CompanyId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }


        public JObject CrDrDetails(JObject Data)
        {
            try
            {
                var response = new JObject();
                var data = JsonConvert.DeserializeObject<dynamic>(Data["InvioceData"].ToString());
                int CompanyId = Convert.ToInt32(data["CompanyId"]);
                int VochTypeID = Convert.ToInt32(data["VochTypeID"]);
                int VoucherNo = Convert.ToInt32(data["vochno"]);

                response = _salesRepository.CrDrDetails(CompanyId, VoucherNo, VochTypeID);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetAllCommodities()
        {
            try
            {
                var response = new JObject();
                response = _salesRepository.GetAllCommodities();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        public string AddProductionEntry(JObject Data)
        {
            try
            {
                return _salesRepository.AddProductionEntry(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetAllProductionEntries(JObject Data)
        {
            try
            {
                //var data = JsonConvert.DeserializeObject<dynamic>(Data["InvioceData"].ToString());
                int CompanyId = Convert.ToInt32(Data["CompanyId"]);

                var response = new JObject();
                response = _salesRepository.GetAllProductionEntries(CompanyId);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }


        public JObject GetItem(JObject Data)
        {
            try
            {
                var response = new JObject();
                var data = JsonConvert.DeserializeObject<dynamic>(Data["SelesItem"].ToString());
                string name = data["Name"];
                string GST_type = data["GSTType"];
                response = _salesRepository.GetItem(name, GST_type);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        public string SaveDC(JObject Data)
        {
            try
            {
                return _salesRepository.SaveDC(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public int? GetBNo(JObject Data)
        {
            try
            {
                return _salesRepository.GetBNo(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public bool SaveExcelData(List<ExcelDataWrapper> Data)
        {
            try
            {
                return _salesRepository.SaveExcelData(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public string ProductionEntry(JObject Data)
        {
            try
            {
                return _salesRepository.AddInvoice(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        
        public string AddInvoice(JObject Data)
        {
            try
            {
                return _salesRepository.AddInvoice(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public void UpdateInvoice(JObject Data)
        {
            try
            {
                _salesRepository.UpdateInvoice(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetDispatcherDetails(JObject Data)
        {
            try
            {
                return _salesRepository.GetDispatcherDetails(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        
        public JObject GetsingleList(JObject Data)
        {
            try
            {
                return _salesRepository.GetsingleList(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public pagination<BillSummary> GetList(JObject Data)
        {
            try
            {
                return _salesRepository.GetList(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public Boolean Update(JObject Data)
        {
            try
            {
                return _salesRepository.Update(Data);
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
                _salesRepository.Delete(Data);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public List<JObject> GetReport(JObject Data)
            {    
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());
                string reportType = data?["ReportType"];
                if (reportType == "PurchaseRegister")
                {
                    List<ReportDTO> reportDTOs = _salesRepository.GetPurchaseRegister(Data);
                    List<JObject> result = reportDTOs.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result;
                }
                else if (reportType == "MonthwisePurchase")
                {
                    List<ReportDTO> reportDTOs = _salesRepository.GetMonthwisePurchase(Data);
                    List<JObject> result = reportDTOs.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result;
                }
                else if (reportType == "TrialBalance")
                {
                    List<ReportDTO> reportDTOs = _salesRepository.GetTrialBalance(Data);
                    List<JObject> result = reportDTOs.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result;
                }
                else if (reportType == "PaymentList")
                {
                    pagination<LedgerInfo> reportDTOs = _salesRepository.GetPaymentList(Data);
                    List<JObject> result = reportDTOs.Records.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result; 
                }
                else if (reportType == "TDSReport")
                {
                    pagination<TDSReportEntry> reportDTOs = _salesRepository.GetTDSReport(Data);
                    List<JObject> result = reportDTOs.Records.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result;
                }
                else if (reportType == "StockLedger")
                {
                    List<TmpStockLedger> reportDTOs = _salesRepository.GetStockLedger(Data);
                    List<JObject> result = reportDTOs.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result;
                }
                else if (reportType == "PartywiseTDS")
                {
                    List<ReportDTO> reportDTOs = _salesRepository.GetPartywiseTDS(Data);
                    List<JObject> result = reportDTOs.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result;
                }
                else if (reportType == "TransactionSummary")
                {
                    List<ReportDTO> reportDTOs = _salesRepository.GetTransactinSummary(Data);
                    List<JObject> result = reportDTOs.Select(reportDTO => JObject.FromObject(reportDTO)).ToList();
                    return result;
                }

                else
                    return new List<JObject>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }


        public List<Voucher> SavePayemnts(JObject Data)
        {
            try
            {
                 return _salesRepository.SavePayemnts(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public InvoiceDetails GetInvoiceResponse(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["InvoiceDetails"]?.ToString());
                string InvoiceNumber = data?["InvoiceNumber"];
                string companyId = data?["CompanyId"];
                int parsedCompanyId;

                if (int.TryParse(companyId, out parsedCompanyId))
                {
                    // Now 'parsedCompanyId' holds the integer value of 'companyId'
                    // You can use 'parsedCompanyId' in your code
                    return _salesRepository.GetInvoiceResponse(InvoiceNumber, parsedCompanyId);
                }
                else
                {
                    // Handle the case where 'companyId' is not a valid integer
                    // Maybe show an error message or set a default value
                    // For now, let's set a default value of 0
                    return _salesRepository.GetInvoiceResponse(InvoiceNumber, 0);
                }

                //return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject GetBanks(int companyId)
        {
            try
            {    
                return _salesRepository.GetBanks(companyId);
                 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetAccountGroups(int temp)
        {
            try
            {    
                return _salesRepository.GetAccountGroups(temp);
                 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject GetLorryDetailAutoComplete(JObject Data)
        {
            try
            {
                return _salesRepository.GetLorryDetailAutoComplete(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetAddPartyAutoComplete(JObject Data)
        {
            try
            {
                return _salesRepository.GetAddPartyAutoComplete(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}
