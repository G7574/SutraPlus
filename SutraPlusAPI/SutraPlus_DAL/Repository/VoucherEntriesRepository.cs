using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.HadrModel;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus.Models;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SutraPlus_DAL.Repository
{
    public class VoucherEntriesRepository : BaseRepository
    {
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private MasterDBContext _masterDBContext;

        public VoucherEntriesRepository(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger) : base(tenantID, masterDBContext)
        {
            _logger = logger;
            _configuration = configuration;
            _masterDBContext = masterDBContext;
        }

        public JObject GetVoucherTypeList()
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Local Sale").Select(x => x.VoucherId).SingleOrDefault();
                response.Add("VoucherType", JArray.FromObject(result));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public string AddBankJournalEntries(JObject Data)
        {   
             var data = JsonConvert.DeserializeObject<dynamic>(Data["BankJournalEntries"].ToString());
             
             long CompanyID = Convert.ToInt64(JsonConvert.DeserializeObject<dynamic>(Data["CompanyID"].ToString()));
             long VoucherID = Convert.ToInt64(JsonConvert.DeserializeObject<dynamic>(Data["VoucherTypeID"].ToString()));


            var RunningVoucherNo = _tenantDBContext.Vouchers.Where(x => x.VoucherId == VoucherID && x.CompanyId == CompanyID).Select(x => x.VoucherNo).ToList().Max() +1;

            if(RunningVoucherNo == null)
            {
                RunningVoucherNo = 1;
            }


             foreach (var item in data)
             {  
                if (item["Sno"] == 1)
                {
                    Voucher voucherlst = new Voucher
                    {
                        CommodityId = 0,
                        TranctDate = item.TransDate,
                        VoucherId = VoucherID,
                        VoucherNo = RunningVoucherNo,
                        Narration = item.Narration,
                        LedgerId = item.LedgerID,
                        CompanyId = item.CompanyID,
                        Credit = Convert.ToDecimal(item.CreditAmount),
                        Debit = 0,
                        IsActive = true,
                        PartyInvoiceNumber = "",
                        LedgerNameForNarration = item.LedgerNameForNarration,
                        CreatedBy = 1
                    };

                    _tenantDBContext.AddRange(voucherlst);
                    _tenantDBContext.SaveChanges();
                }
                else
                {
                    Voucher voucherlst1 = new Voucher
                    {
                        CommodityId = 0,
                        TranctDate = item.TransDate,
                        VoucherId = VoucherID,
                        VoucherNo = RunningVoucherNo,
                        Narration = item.Narration,
                        LedgerId = item.LedgerID,
                        CompanyId = item.CompanyID,
                        Credit = 0,
                        Debit = Convert.ToDecimal(item.DebitAmount),
                        IsActive = true,
                        PartyInvoiceNumber = "",
                        LedgerNameForNarration = item.LedgerNameForNarration,
                        CreatedBy = 1
                    };

                    _tenantDBContext.AddRange(voucherlst1);
                    _tenantDBContext.SaveChanges();
                }

            }
            return "Added Successfully...!";
        }

        public string AddCashEntries(JObject Data)
        {
 
            long CompanyID = Convert.ToInt64(JsonConvert.DeserializeObject<dynamic>(Data["CompanyID"].ToString()));
            var TransDate = Convert.ToDateTime(Data["TransDate"]);
            long VoucherTypeID = Convert.ToInt64(Data["ReceiptPayment"]);
            var LedgerID = Convert.ToInt64(Data["SelectAccount"]);
            var VikriBillNo = Convert.ToString(Data["VikriBillNo"]);
            var Amount = Convert.ToDecimal(Data["Amount"]);
            var Narration = Convert.ToString(Data["Narration"]);



            var RunningVoucherNo = _tenantDBContext.Vouchers.Where(x => x.VoucherId == VoucherTypeID && x.CompanyId == CompanyID).Select(x => x.VoucherNo).ToList().Max() + 1;

            if (RunningVoucherNo == null)
            {
                RunningVoucherNo = 1;
            }


            if (VoucherTypeID == 19)
            {
                Voucher voucherlst = new Voucher
                {
                    CommodityId = 0,
                    TranctDate = TransDate,
                    VoucherId = VoucherTypeID,
                    VoucherNo = RunningVoucherNo,
                    Narration = Narration,
                    LedgerId = LedgerID,
                    CompanyId = CompanyID,
                    Credit = Convert.ToDecimal(Amount),
                    Debit = 0,
                    IsActive = true,
                    PartyInvoiceNumber = "",
                    LedgerNameForNarration = "Cash Received",
                    CreatedBy = 1
                };

                _tenantDBContext.AddRange(voucherlst);
                _tenantDBContext.SaveChanges();
            }
            else
            {
                Voucher voucherlst = new Voucher
                {
                    CommodityId = 0,
                    TranctDate = TransDate,
                    VoucherId = VoucherTypeID,
                    VoucherNo = RunningVoucherNo,
                    Narration = Narration,
                    LedgerId = LedgerID,
                    CompanyId = CompanyID,
                    Credit = 0,
                    Debit = Convert.ToDecimal(Amount),
                    IsActive = true,
                    PartyInvoiceNumber = "",
                    LedgerNameForNarration = "Cash Paid",
                    CreatedBy = 1
                };

                _tenantDBContext.AddRange(voucherlst);
                _tenantDBContext.SaveChanges();
            }

            return "Added Successfully...!";
        }

    }
}