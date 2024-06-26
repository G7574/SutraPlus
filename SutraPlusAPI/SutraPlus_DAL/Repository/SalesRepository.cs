﻿using Azure;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using Microsoft.Data.SqlClient;
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
using NLog.Fluent;
using QRCoder;
using SutraPlus.Models;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.E_Invoice;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApplication2.Implementations;
using static iTextSharp.text.pdf.AcroFields;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static QRCoder.PayloadGenerator.SwissQrCode;
using QRCodeGenerator = QRCoder.QRCodeGenerator;


namespace SutraPlus_DAL.Repository
{
    public class SalesRepository : BaseRepository
    {
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private MasterDBContext _masterDBContext;

        public SalesRepository(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger) : base(tenantID, masterDBContext)
        {
            _logger = logger;
            _configuration = configuration;
            _masterDBContext = masterDBContext;
        }
        public long GetMaxInvoice()
        {
            //var result = _tenantDBContext.BillSummaries.Max(i => i._Id);
            //return _tenantDBContext.BillSummaries.Select(x => x.VochNo).ToList().Max()+1 ;
            return 0;
        }
        /// <summary>
        /// get Invoice NO for three parameter
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="LedgerId"></param>
        /// <param name="DealerType"></param>
        /// <returns></returns>
        

        public JObject GetEinvoiceKey(int companyId)
        {
            var response = new JObject();
            try
            {
                var query = _tenantDBContext.Companies.Where(n => n.CompanyId == companyId).FirstOrDefault();

                if(query == null)
                {
                    response.Add("EinvoiceKey",false);
                } else
                {
                    if (query.EinvoiceKey == "" || query.EinvoiceKey == null) {
                        response.Add("EinvoiceKey", false);
                    } else {
                        response.Add("EinvoiceKey", true);
                    }
                    
                }

                return response;

            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public JObject GetInvtype(int companyId,int LedgerId,int VoucherNumber, int VoucherType)
        {
            var response = new JObject();
            int EinvReq = 0;

            try
            {
                var query = _tenantDBContext.Companies.Where(n=>n.CompanyId == companyId).FirstOrDefault();
                 

                if (query == null)
                {
                    EinvReq = 0;

                }
                else
                {
                    if (query.EinvoiceKey == "" || query.EinvoiceKey == null)
                    {
                        EinvReq = 0;

                    }
                    else
                    {
                        EinvReq = 1;

                    }

                }

                var ch = _tenantDBContext.BillSummaries.Where(n => n.LedgerId == LedgerId && n.VochNo == VoucherNumber && n.VochType == VoucherType).FirstOrDefault();
               
                response.Add("einvreq", EinvReq);
                if(ch != null)
                {
                    response.Add("frieghtPlus", ch.frieghtPlus);
                }
 
                return response;
            }
            catch(Exception e)
            {
                throw e;
            }
             
        }
        public JObject GetInvtypeForCalculation(int companyId,int LedgerId)
        {
            var response = new JObject();
            int EinvReq = 0;

            try
            {
                var query = _tenantDBContext.Companies.Where(n=>n.CompanyId == companyId).FirstOrDefault();
                 

                if (query == null)
                {
                    EinvReq = 0;

                }
                else
                {
                    if (query.EinvoiceKey == "" || query.EinvoiceKey == null)
                    {
                        EinvReq = 0;

                    }
                    else
                    {
                        EinvReq = 1;

                    }

                }

                var ch = _tenantDBContext.BillSummaries.Where(n => n.LedgerId == LedgerId).FirstOrDefault();
               
                response.Add("einvreq", EinvReq);
                if(ch != null)
                {
                    if(ch.frieghtPlus != null)
                    {
                        response.Add("frieghtPlus", ch.frieghtPlus);
                    } else
                    {
                        response.Add("frieghtPlus", 0);
                    }
                    
                }
 
                return response;
            }
            catch(Exception e)
            {
                throw e;
            }
             
        }

        public JObject Get(int CompanyId, int LedgerId, string DealerType, string InvoiceType)
        {
            var response = new JObject();
            try
            {
                var companyState = _tenantDBContext.Companies.Where(c => c.CompanyId == CompanyId).Select(c => c.State).SingleOrDefault();
                var ledgerState = _tenantDBContext.Ledgers.Where(l => l.LedgerId == LedgerId && l.CompanyId == CompanyId && l.DealerType == DealerType).Select(l => l.State).SingleOrDefault();
                var dealer_type = _tenantDBContext.Ledgers.Where(l => l.LedgerId == LedgerId && l.CompanyId == CompanyId).Select(l => l.DealerType).SingleOrDefault();
                var country_name = _tenantDBContext.Ledgers.Where(l => l.LedgerId == LedgerId && l.CompanyId == CompanyId).Select(l => l.Country).SingleOrDefault();

                if (InvoiceType == "GoodsInvoice" || InvoiceType == "GinningInvoice" || InvoiceType == "ProfarmaInvoice" || InvoiceType == "ExportInvoice")
                {
                    if (ledgerState != null)
                    {
                        if (companyState.ToLower() == ledgerState.ToLower() && dealer_type == "Registered Dealer" && country_name == "India" && (InvoiceType == "GoodsInvoice" || InvoiceType == "GinningInvoice"))
                        {
                            var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Local Sale").Select(x => x.VoucherId).SingleOrDefault();
                            response.Add("VoucherType", new JValue("Local Sale"));
                            response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 12 && x.CompanyId == CompanyId && x.IsActive == 1).Select(x => x.VochNo).ToList().Max()) + 1));
                            response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                        }
                        else if (companyState.ToLower() != ledgerState.ToLower() && dealer_type == "Registered Dealer" && country_name == "India" && (InvoiceType == "GoodsInvoice" || InvoiceType == "GinningInvoice"))
                        {
                            var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Interstate Sale").Select(x => x.VoucherId).SingleOrDefault();
                            response.Add("VoucherType", new JValue("Interstate Sale"));
                            response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 12 && x.CompanyId == CompanyId && x.IsActive == 1).Select(x => x.VochNo).ToList().Max()) + 1));
                            response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                        }
                        else if (dealer_type != "Register Dealer" && country_name == "India" && (InvoiceType == "GoodsInvoice" || InvoiceType == "GinningInvoice"))
                        {
                            var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "URD Sale").Select(x => x.VoucherId).SingleOrDefault();
                            response.Add("VoucherType", new JValue("URD Sale"));
                            response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 12 && x.CompanyId == CompanyId && x.IsActive == 1).Select(x => x.VochNo).ToList().Max()) + 1));
                            response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                        }
                        else if (country_name != "India" && InvoiceType == "ExportInvoice")
                        {
                            var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Export Sale").Select(x => x.VoucherId).SingleOrDefault();
                            response.Add("VoucherType", new JValue("Export Sale"));
                            response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 12 && x.CompanyId == CompanyId && x.IsActive == 1).Select(x => x.VochNo).ToList().Max()) + 1));
                            response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                        }
                        else if (country_name == "India" && InvoiceType == "DeemedExport")
                        {
                            var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Deemed Export").Select(x => x.VoucherId).SingleOrDefault();
                            response.Add("VoucherType", new JValue("Deemed Export"));
                            response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 13 && x.CompanyId == CompanyId && x.IsActive == 1).Select(x => x.VochNo).ToList().Max()) + 1));
                            response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                        }
                        else if (country_name == "India" && InvoiceType == "PurchaseReturn")
                        {
                            var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Purchase Return").Select(x => x.VoucherId).SingleOrDefault();
                            response.Add("VoucherType", new JValue("Purchase Return"));
                            response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 8 && x.CompanyId == CompanyId && x.IsActive == 1).Select(x => x.VochNo).ToList().Max()) + 1));
                            response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                        }
                        else if (country_name == "India" && InvoiceType == "ProfarmaInvoice")
                        {
                            var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Profarma Invoice").Select(x => x.VoucherId).SingleOrDefault();
                            response.Add("VoucherType", new JValue("Profarma Invoice"));
                            response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 16 && x.CompanyId == CompanyId && x.IsActive == 1).Select(x => x.VochNo).ToList().Max()) + 1));
                            response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                        }
                    }
                }

                if (InvoiceType == "DebitNote" || InvoiceType == "PurchaseReturn")
                {
                    if (country_name == "India")
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Debit Note").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("Debit Note"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 15 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                    }
                }

                var ch = _tenantDBContext.BillSummaries.Where(n => n.LedgerId == LedgerId).FirstOrDefault();
                if(ch != null)
                {

                    response.Add("Transporter", new JValue(ch.Transporter));
                    response.Add("LorryNo", new JValue(ch.LorryNo));
                    response.Add("Owner", new JValue(ch.LorryOwnerName));
                    response.Add("Driver", new JValue(ch.DriverName));

                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }



        public JObject CrDrDetails(int CompanyId, int vochno, int VochTypeID)
        {
            var response = new JObject();
            try
            {
                var query = (from Vouchers in _tenantDBContext.Vouchers
                             join cpn in _tenantDBContext.Companies on Vouchers.CompanyId equals cpn.CompanyId
                             join VochType in _tenantDBContext.VoucherTypes on Vouchers.VoucherId equals VochType.VoucherId
                             join led in _tenantDBContext.Ledgers on Vouchers.LedgerId equals led.LedgerId
                             where Vouchers.CompanyId == CompanyId && Vouchers.VoucherNo == vochno && Vouchers.VoucherId == VochTypeID && Vouchers.IsActive == 1
                             select new
                             {
                                 Date = Vouchers.TranctDate,
                                 LedgerName = led.LedgerName,
                                 Debit = Vouchers.Debit,
                                 Credit = Vouchers.Credit
                             });

                response.Add("CRDRDetails", JArray.FromObject(query.ToList()));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject GetBanks(int CompanyId)
        { 
            JObject result = new JObject();
            List<string> bankNames = new List<string>();

           
                var distinctLedgerNames = _tenantDBContext.Ledgers
                    .Where(l => l.AccountingGroupId < 4 && l.CompanyId == CompanyId)
                    .Select(l => l.LedgerName)
                    .Distinct()
                    .ToList();

                // Convert the list of bank names to a JArray
                JArray bankNamesArray = new JArray(distinctLedgerNames);

                // Add the bank names array to the result JObject
                result["bankNames"] = bankNamesArray;
            
            return result;
        }

        public JObject GetAccountGroups(int temp)
        {
            var response = new JObject();
            try
            {
                var result = _tenantDBContext.AccounitngGroups
                    .Select(l => new { l.AccontingGroupId, l.GroupName })
                    .Distinct()
                    .ToList();

                if (result != null)
                { 
                    var jsonArray = new JArray();
                    foreach (var item in result)
                    {
                        var jsonObj = new JObject();
                        jsonObj.Add("AccountId", item.AccontingGroupId);
                        jsonObj.Add("GroupName", item.GroupName);
                        jsonArray.Add(jsonObj);
                    }
                    response.Add("GetAccountGroups", jsonArray);
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
         
        public List<Voucher> SavePayemnts(JObject Data) {
             
            dynamic paymentData = JsonConvert.DeserializeObject<dynamic>(Data?["paymentData"]?.ToString());
            string bankName = paymentData?["bankName"];
            string RTGS = paymentData?["RTGS"];
            DateTime chequeDate = paymentData?["Date"];
            string select = paymentData?["select"];
            string bankCode = paymentData?["bankCode"];
            string companyId = paymentData?["CompanyId"];
            var invoiceList = paymentData?["invoiceList"];
             
            var maxVoucherNo = _tenantDBContext.Vouchers
            .Where(v => v.VoucherId == 23 && v.CompanyId == int.Parse(companyId))
            .Select(v => v.VoucherNo)
            .Max();

            var ledgerVal2 = (_tenantDBContext.Ledgers.
              Where(l => l.LedgerName == bankName && l.CompanyId == int.Parse(companyId)).Select(l => l.LedgerId)).FirstOrDefault();

            List<Voucher> voucherlist = new List<Voucher>();

            foreach (var invoice in invoiceList) {

                string name = invoice["LedgerName"];

                var PartyLedgerID = (_tenantDBContext.Ledgers.
                     Where(l => l.LedgerName == name && l.CompanyId == int.Parse(companyId)).Select(l => l.LedgerId)).FirstOrDefault();

                string s1 = invoice["payAmount"];
                string s2 = invoice["chequeNo"];

                long payAmount = long.Parse(s1);
                long chequeNo = long.Parse(s2);

                Voucher voucherlst1 = new Voucher
                {
                    CommodityId = 0,
                    TranctDate = chequeDate,
                    VoucherId = 23,
                    VoucherNo = maxVoucherNo,
                    Narration = "Payment Against Purchase - " + chequeNo,
                    LedgerId = ledgerVal2,
                    CompanyId = int.Parse(companyId),
                    Credit = payAmount,
                    Debit = 0,
                    IsActive = 1,
                    PartyInvoiceNumber = chequeNo.ToString(),
                    LedgerNameForNarration = "Cheque Payment",
                    CreatedBy = 1
                };


                Voucher voucherlst2 = new Voucher
                {
                    CommodityId = 0,
                    TranctDate = chequeDate,
                    VoucherId = 23,
                    VoucherNo = maxVoucherNo,
                    Narration = "Payment Agiannst Purchase-" + chequeNo,
                    LedgerId = PartyLedgerID,
                    CompanyId = int.Parse(companyId),
                    Credit = 0,
                    Debit = payAmount,
                    IsActive = 1,
                    PartyInvoiceNumber = chequeNo.ToString(),
                    LedgerNameForNarration = bankName,
                    CreatedBy = 1
                };

                voucherlist.Add(voucherlst1);
                voucherlist.Add(voucherlst2);

            }

            _tenantDBContext.Vouchers.AddRange(voucherlist);
            _tenantDBContext.SaveChanges();

            return voucherlist;
        }


        public InvoiceDetails GetInvoiceResponse(string InvoiceNumber, int CompnayId, string TYP = "Single")
        {
            string response;
            GeneralModel respobj;
            try
            {
                AuthenticationDetails auth = new AuthenticationDetails();


                InvoiceDetails invdetail = new InvoiceDetails();

                //string connectionString = "Server=103.50.212.163;Database=K2223RGP;uid=sa;Password=root@123;TrustServerCertificate=True;";
                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                string connectionString = builder.ToString();
                Company customer = new Company();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Company WHERE CompanyId = @CompnayId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CompnayId", CompnayId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Fetch data from the reader
                                customer.EinvoiceKey = reader["EinvoiceKey"].ToString();
                                customer.EinvoiceSkey = reader["EinvoiceSkey"].ToString();
                                customer.Gstin = reader["Gstin"].ToString();
                                customer.EinvoiceUserName = reader["EinvoiceUserName"].ToString();
                                customer.EinvoicePassword = reader["EinvoicePassword"].ToString();
                            }
                            else
                            {
                                Console.WriteLine("Customer not found.");
                            }
                        }
                    }


                    if (customer == null)
                    {
                        // ViewData["ErrorMessage"] = "Company Not Found";
                        //MessageBox.Show("Company Not Found");
                        return null;
                    }
                    else if (customer.Gstin == null)
                    {
                        //MessageBox.Show("Please CheckSupplyer  GSTIN");
                        // ViewData["ErrorMessage"] = "Please CheckSupplyer  GSTIN";
                        return null;
                    }

                    AuthValues.MVApiKey = customer.EinvoiceKey;
                    AuthValues.MVSecretKey = customer.EinvoiceSkey;
                    AuthValues.gstin = customer.Gstin;
                    AuthValues.eInvoiceUserName = customer.EinvoiceUserName;
                    AuthValues.eInvoicePassword = customer.EinvoicePassword;

                    Tool tool = Tool.CreateTool();
                    string authres = string.Empty;
                    string invres = string.Empty;
                    AuthentictionResponse AuthResp = tool.AuthentictionH();
                    // Start Authentication
                    if (AuthResp == null)
                    {
                        auth.AuthenticationToken = "Exception Found";
                        auth.AuthenticationResponseData = authres;
                    }
                    else
                    {
                        auth.AuthenticationToken = AuthResp.AuthToken;
                        auth.AuthenticationResponseData = authres;
                        IRNRequestAuthHeader irnheader = new IRNRequestAuthHeader();
                        irnheader.authToken = AuthResp.AuthToken;
                        irnheader.MonthYear = "04-2021";

                        response = tool.GenerateInvoiceH(irnheader, InvoiceNumber.Trim(), 3, invres, CompnayId);
                        if (response == null)
                            auth.InvoiceResponse = invres;
                        else
                            auth.InvoiceResponse = response;

                        respobj = JsonConvert.DeserializeObject<GeneralModel>(response);



                        //if (respobj != null)
                        //{
                        //    //responsedet.QRCode = respobj.SignedQRCode;
                        //    //responsedet.IRN = respobj.IRN;
                        //}

                        string strerr = string.Empty;
                        if (respobj.ErrorResponse != null && respobj.ErrorResponse.Count > 0)
                        {
                            foreach (var item in respobj.ErrorResponse)
                            {
                                strerr = strerr + " " + item.ErrorInfo.FirstOrDefault();
                            };
                        }

                        if (respobj.Status == "1")
                        {
                            //ViewData["ResponseMessage"] = "E Invoice Generated Successfully ";
                            //invdetail.Status = "E Invoice Generated Successfully ";
                            invdetail.Status = respobj.Status;
                            //ViewData["AckNo"] = respobj.AckNo;
                            //ViewData["IRNNo"] = respobj.IRN;
                            //ViewData["Status"] = respobj.Status;
                            invdetail.IRN = respobj.IRN;
                            invdetail.AckNo = respobj.AckNo;
                        }
                        else if (respobj.Status == null)
                        {
                            respobj.Status = "0";
                            //ViewData["Status"] = "0";
                            //ViewData["ResponseMessage"] = respobj.ErrorResponse.FirstOrDefault().ErrorInfo;
                        }
                        else
                        {
                            //ViewData["ResponseMessage"] = respobj.ErrorResponse.FirstOrDefault().ErrorInfo;
                            //ViewData["Status"] = respobj.Status;
                            invdetail.Status = respobj.Status;
                            invdetail.AckNo = respobj.ErrorResponse.FirstOrDefault().ErrorInfo;
                        }


                        if (respobj.SignedQRCode != null)
                        {
                            using (SqlCommand command = new SqlCommand("SELECT * FROM BillSummaries WHERE DisplayinvNo = @InvoiceNumber", connection))
                            {
                                command.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        // Access the columns using reader["ColumnName"] and process the results.
                                        // Example:
                                        var bill = new BillSummary
                                        {
                                            _Id = reader["Id"] as int?,
                                            CompanyId = reader["CompanyId"] as long?,
                                            LedgerId = reader["LedgerId"] as long?,
                                            LedgerName = reader["LedgerName"] as string,
                                            Place = reader["Place"] as string,
                                            VochType = reader["VochType"] as long?,
                                            VoucherName = reader["VoucherName"] as string,
                                            DealerType = reader["DealerType"] as string,
                                            PAN = reader["PAN"] as string,
                                            GST = reader["GST"] as string,
                                            State = reader["State"] as string,
                                            InvoiceType = reader["InvoiceType"] as string,
                                            VochNo = reader["VochNo"] as long?,
                                            EwayBillNo = reader["EwayBillNo"] as string,
                                            Ponumber = reader["Ponumber"] as string,
                                            Transporter = reader["Transporter"] as string,
                                            LorryNo = reader["LorryNo"] as string,
                                            LorryOwnerName = reader["LorryOwnerName"] as string,
                                            DriverName = reader["DriverName"] as string,
                                            Dlno = reader["Dlno"] as string,
                                            CheckPost = reader["CheckPost"] as string,
                                            FrieghtPerBag = reader["FrieghtPerBag"] as decimal?,
                                            TotalFrieght = reader["TotalFrieght"] as decimal?,
                                            Advance = reader["Advance"] as decimal?,
                                            Balance = reader["Balance"] as decimal?,
                                            AREDate = reader["AREDate"] as string,
                                            ARENo = reader["ARENo"] as string,
                                            IsLessOrPlus = reader["IsLessOrPlus"] as bool?,
                                            ExpenseName1 = reader["ExpenseName1"] as string,
                                            ExpenseName2 = reader["ExpenseName2"] as string,
                                            ExpenseName3 = reader["ExpenseName3"] as string,
                                            ExpenseAmount1 = reader["ExpenseAmount1"] as decimal?,
                                            ExpenseAmount2 = reader["ExpenseAmount2"] as decimal?,
                                            ExpenseAmount3 = reader["ExpenseAmount3"] as decimal?,
                                            DeliveryName = reader["DeliveryName"] as string,
                                            DeliveryAddress1 = reader["DeliveryAddress1"] as string,
                                            DeliveryAddress2 = reader["DeliveryAddress2"] as string,
                                            DeliveryPlace = reader["DeliveryPlace"] as string,
                                            DeliveryState = reader["DeliveryState"] as string,
                                            DeliveryStateCode = reader["DeliveryStateCode"] as string,
                                            BillAmount = reader["BillAmount"] as decimal?,
                                            INWords = reader["INWords"] as string,
                                            FrieghtAmount = reader["FrieghtAmount"] as decimal?,
                                            RoundOff = reader["RoundOff"] as decimal?,
                                            TotalBags = reader["TotalBags"] as long?,
                                            TotalWeight = reader["TotalWeight"] as double?,
                                            TotalAmount = reader["TotalAmount"] as decimal?,
                                            PackingValue = reader["PackingValue"] as decimal?,
                                            HamaliValue = reader["HamaliValue"] as decimal?,
                                            WeighnamFeeValue = reader["WeighnamFeeValue"] as decimal?,
                                            DalaliValue = reader["DalaliValue"] as decimal?,
                                            CessValue = reader["CessValue"] as decimal?,
                                            TaxableValue = reader["TaxableValue"] as decimal?,
                                            SGSTValue = reader["SGSTValue"] as decimal?,
                                            CSGSTValue = reader["CSGSTValue"] as decimal?,
                                            IGSTValue = reader["IGSTValue"] as decimal?,
                                            UserId = reader["UserId"] as int?,
                                            SessionID = reader["SessionID"] as int?,
                                            PaymentBy = reader["PaymentBy"] as string,
                                            AmountReceived = reader["AmountReceived"] as decimal?,
                                            Change = reader["Change"] as decimal?,
                                            CardDetails = reader["CardDetails"] as string,
                                            BillTime = reader["BillTime"] as DateTime?,
                                            TranctDate = reader["TranctDate"] as DateTime?,
                                            CustomerName = reader["CustomerName"] as string,
                                            CustomerPlace = reader["CustomerPlace"] as string,
                                            CustomerContactNo = reader["CustomerContactNo"] as string,
                                            PartyInvoiceNumber = reader["PartyInvoiceNumber"] as string,
                                            OtherCreated = reader["OtherCreated"] as int?,
                                            Recent = reader["Recent"] as int?,
                                            PaymentReceived = reader["PaymentReceived"] as int?,
                                            TagName = reader["TagName"] as string,
                                            TagDate = reader["TagDate"] as DateTime?,
                                            ToPrint = reader["ToPrint"] as int?,
                                            frieghtPlus = reader["frieghtPlus"] as int?,
                                            StateCode1 = reader["StateCode1"] as string,
                                            StateCode2 = reader["StateCode2"] as string,
                                            ExpenseTax = reader["ExpenseTax"] as decimal?,
                                            FrieghtinBill = reader["FrieghtinBill"] as decimal?,
                                            LastOpenend = reader["LastOpenend"] as DateTime?,
                                            VikriCommission = reader["VikriCommission"] as decimal?,
                                            VikriULH = reader["VikriULH"] as decimal?,
                                            VikriCashAdvance = reader["VikriCashAdvance"] as decimal?,
                                            VikriFrieght = reader["VikriFrieght"] as decimal?,
                                            VikriEmpty = reader["VikriEmpty"] as decimal?,
                                            VikriNet = reader["VikriNet"] as decimal?,
                                            CashToFarmer = reader["CashToFarmer"] as decimal?,
                                            VikriOther1 = reader["VikriOther1"] as decimal?,
                                            VikriOther2 = reader["VikriOther2"] as decimal?,
                                            VikriOther1Name = reader["VikriOther1Name"] as string,
                                            VikriOther2Name = reader["VikriOther2Name"] as string,
                                            Note1 = reader["Note1"] as string,
                                            FromPlace = reader["FromPlace"] as string,
                                            ToPlace = reader["ToPlace"] as string,
                                            DisplayNo = reader["DisplayNo"] as string,
                                            DisplayinvNo = reader["DisplayinvNo"] as string,
                                            FrieghtLabel = reader["FrieghtLabel"] as string,
                                            FormName = reader["FormName"] as string,
                                            DCNote = reader["DCNote"] as string,
                                            WeightInString = reader["WeightInString"] as string,
                                            SGSTLabel = reader["SGSTLabel"] as string,
                                            CGSTLabel = reader["CGSTLabel"] as string,
                                            IGSTLabel = reader["IGSTLabel"] as string,
                                            TCSLabel = reader["TCSLabel"] as string,
                                            tdsperc = reader["tdsperc"] as decimal?,
                                            TCSValue = reader["TCSValue"] as decimal?,
                                            TCSPerc = reader["TCSPerc"] as decimal?,
                                            DelPinCode = reader["DelPinCode"] as string,
                                            CommodityID = reader["CommodityID"] as int?,
                                            QrCode = reader["QrCode"] as byte[],
                                            IsGSTUpload = reader["IsGSTUpload"] as int?,
                                            DispatcherName = reader["DispatcherName"] as string,
                                            DispatcherAddress1 = reader["DispatcherAddress1"] as string,
                                            DispatcherAddress2 = reader["DispatcherAddress2"] as string,
                                            DispatcherPlace = reader["DispatcherPlace"] as string,
                                            DispatcherPIN = reader["DispatcherPIN"] as string,
                                            DispatcherStatecode = reader["DispatcherStatecode"] as string,
                                            CountryCode = reader["CountryCode"] as string,
                                            ShipBillNo = reader["ShipBillNo"] as string,
                                            ForCur = reader["ForCur"] as string,
                                            PortName = reader["PortName"] as string,
                                            RefClaim = reader["RefClaim"] as string,
                                            ShipBillDate = reader["ShipBillDate"] as DateTime?,
                                            ExpDuty = reader["ExpDuty"] as string,
                                            monthNo = reader["monthNo"] as int?,
                                            Distance = reader["Distance"] as int?,
                                            GSTR1SectionName = reader["GSTR1SectionName"] as string,
                                            GSTR1InvoiceType = reader["GSTR1InvoiceType"] as string,
                                            EInvoiceNo = reader["EInvoiceNo"] as string,
                                            IsSEZ = reader["IsSEZ"] as int?,
                                            id = reader["id"] as int?,
                                            OriginalInvNo = reader["OriginalInvNo"] as string,
                                            OriginalInvDate = reader["OriginalInvDate"] as DateTime?,
                                            AprClosed = reader["AprClosed"] as int?,
                                            Discount = reader["Discount"] as decimal?,
                                            TwelveValue = reader["TwelveValue"] as decimal?,
                                            FiveValue = reader["FiveValue"] as decimal?,
                                            EgthValue = reader["EgthValue"] as decimal?,
                                            ACKNO = reader["ACKNO"] as string,
                                            IRNNO = reader["IRNNO"] as string,
                                            SignQRCODE = reader["SignQRCODE"] as string,
                                            IsActive = reader["IsActive"] as int?,
                                            IsServiceInvoice = reader["IsServiceInvoice"] as bool?
                                        };

                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            QRCodeGenerator QR = new QRCodeGenerator();
                                            // Generate QR code with the necessary data (replace 'response' with actual data)
                                            QRCodeData QRData = QR.CreateQrCode("response", QRCodeGenerator.ECCLevel.Q);
                                            //QRCode qRCode = new QRCode(QRData);

                                            //using (Bitmap bitmap = qRCode.GetGraphic(20))
                                            //{
                                            //    // Save QR code as base64 string
                                            //    bitmap.Save(ms, ImageFormat.Png);
                                            //    bill.SignQRCODE = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                                            //}
                                        }

                                        
                                            bill.IRNNO = respobj.IRN;
                                            bill.ACKNO = respobj.AckNo;
                                            bill.EwayBillNo = (string)respobj.EwbNo;
                                            bill.SignQRCODE = respobj.SignedQRCode;
                                            // Use an UPDATE query to save changes
                                            using (SqlCommand updateCommand = new SqlCommand("UPDATE BillSummaries SET " +
                   "IRNNO = @IRNNO, ACKNO = @ACKNO, EwayBillNo = @EwayBillNo, SignQRCODE = @SignQRCODE WHERE Id = @Id", connection))
                                            {
                                                // Set parameters for the UPDATE query
                                                updateCommand.Parameters.AddWithValue("@IRNNO", bill.IRNNO);
                                                updateCommand.Parameters.AddWithValue("@ACKNO", bill.ACKNO);
                                                updateCommand.Parameters.AddWithValue("@EwayBillNo", bill.EwayBillNo);
                                                updateCommand.Parameters.AddWithValue("@SignQRCODE", bill.SignQRCODE);
 
                                                // Execute the UPDATE query
                                                updateCommand.ExecuteNonQuery();
                                            }
                                       
                                    }
                                }
                            }

                            return invdetail;
                        }
                    }
                }

                return invdetail;
            }
            catch (Exception ex)
            {
                //log.WriteError(ex.Message);
                throw;
            }
        }



        /// <summary>
        /// GetItem by Commodity table
        /// </summary>
        /// <param name="name"></param>
        /// <param name="GST_type"></param>
        /// <returns></returns>
        public JObject GetItem(string name, string GST_type)
        {
            try
            {
                var response = new JObject();
                if (string.IsNullOrEmpty(GST_type))
                {
                    var result = (from c in _tenantDBContext.Commodities
                                  where c.CommodityName.Contains(name) && c.IsActive == 1
                                  select new
                                  {
                                      c.CommodityId,
                                      c.CommodityName,
                                      c.IGST,
                                      c.SGST,
                                      c.CGST
                                  }).ToList();
                    response.Add("Commodities", JArray.FromObject(result));

                }
                else
                {
                    var result = (from c in _tenantDBContext.Commodities
                                  where c.SGST == Convert.ToDecimal(GST_type) || c.CGST == Convert.ToDecimal(GST_type) || c.IGST == Convert.ToDecimal(GST_type) && c.IsActive == 1
                                  select new
                                  {
                                      c.CommodityId,
                                      c.CommodityName,
                                      c.IGST,
                                      c.SGST,
                                      c.CGST,
                                      c.Mou
                                  }).ToList();
                    response.Add("Commodities", JArray.FromObject(result));
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex; throw;
            }
        }
        public JObject GetAllCommodities()
        {
            try
            {
                var response = new JObject();
                var result = (from c in _tenantDBContext.Commodities
                              where c.IsActive == 1
                              select new
                              {
                                  c.CommodityId,
                                  c.CommodityName,
                                  c.IGST,
                                  c.SGST,
                                  c.CGST
                              }).ToList();
                response.Add("Commodities", JArray.FromObject(result));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex; throw;
            }
        }


        public string AddProductionEntry(JObject Data)
        {
            try
            {
                // Extract data from the JObject
                var companyId = Data["Companyid"]?.ToString();
                var tranctDate = Data["TranctDate"]?.ToString(); // Assuming TranctDate is a string; adjust if it's another type
                var sentItemName = Data["SentItemName"]?.ToString();
                var qtySent = Data["QtySent"]?.ToString(); // Assuming QtySent is a string; adjust if it's another type
                var receivedItemName = Data["ReceivedItemName"]?.ToString();
                var qtyRcd = Data["QtyRcd"]?.ToString(); // Assuming QtyRcd is a string; adjust if it's another type
                var goodsValue = Data["GoodsValue"]?.ToString(); // Assuming GoodsValue is a string; adjust if it's another type
                var refno = Data["Refno"]?.ToString(); // Assuming Refno is a string; adjust if it's another type

                // Validate or convert data as needed

                // Create a new ProductionMaster instance
                var newProductionEntry = new ProductionMaster
                {
                    Companyid = int.Parse(companyId),
                    TranctDate = DateTime.Parse(tranctDate),
                    SentItemName = sentItemName,
                    QtySent = double.Parse(qtySent), // Adjust if QtySent is not a double
                    ReceivedItemName = receivedItemName,
                    QtyRcd = double.Parse(qtyRcd), // Adjust if QtyRcd is not a double
                    GoodsValue = decimal.Parse(goodsValue), // Adjust if GoodsValue is not a decimal
                    Refno = decimal.Parse(refno) // Adjust if Refno is not a decimal
                };

                // Add the new production entry to the database
                _tenantDBContext.ProductionMasters.Add(newProductionEntry);
                _tenantDBContext.SaveChanges();

                return "Production entry added successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public JObject GetAllProductionEntries(int CompnayId)
        {
            try
            {
                var response = new JObject();

                var result = (from p in _tenantDBContext.ProductionMasters
                              where p.Companyid == CompnayId // Assuming outLetId is a variable with the company ID
                              select new
                              {
                                  p.TranctDate,
                                  p.SentItemName,
                                  p.QtySent,
                                  p.ReceivedItemName,
                                  p.QtyRcd,
                                  p.GoodsValue,
                                  p.Refno
                              }).ToList();

                response.Add("ProductionEntries", JArray.FromObject(result));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw; // No need for 'throw ex;' as it's already caught
            }
        }

        /// <summary>
        /// NumberToWords(), this method is convert Digit into words number and method   
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        /// 
        public string AmountToWord(double number)
        {
            if (number == 0) return "Zero";

            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };

            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };

            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };

            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            num[0] = (int)number % 1000; // units
            num[1] = (int)number / 1000;
            num[2] = (int)number / 100000;
            num[1] = (int)num[1] - 100 * num[2]; // thousands
            num[3] = (int)number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;

                u = (int)num[i] % 10; // ones
                t = (int)num[i] / 10;
                h = (int)num[i] / 100; // hundreds
                t = t - 10 * h; // tens

                if (h > 0) sb.Append(words0[h] + "Hundred ");

                if (u > 0 || t > 0)
                {
                    if (h == 0)
                        sb.Append("");
                    else
                        if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd() + " Rupees only";
        }


        public void UpdateInvoice(JObject Data)
        {
            try
            {

                dynamic data = JsonConvert.DeserializeObject<dynamic>(Data["data"].ToString());
                long companyId = JsonConvert.DeserializeObject<dynamic>(Data["CompanyId"].ToString());

                List<Voucher> voucherlist = new List<Voucher>();

                foreach (var item in data["InvioceData"])
                {  
                     
                    long ledgerId = item["ledger_id"];
                    string legder_name = item["legder_name"];
                    DateTime tranctDate = item["tranctdate"];
                    string BillNumber = item["billNumber"];
                    int noOfBags = item["total_no_of_bags"];
                    long totalWeight = item["total_weight"];
                    long commodityId = item["commodityId"];
                    long taxableValue = item["total_taxable_amount"];
                    long total_amount = item["total_amount"];


                    Double PackingRate = 0;
                    Double HamaliRate = 0;
                    Double WeighManFeeRate = 0;
                    Double DalaliRate = 0;
                    Double CessRate = 0;

                    Double IGST = 0;
                    Double SGST = 0;
                    Double CGST = 0;
 
                    var ledgerData = _tenantDBContext.Ledgers.Where(n => n.LedgerId == ledgerId && n.CompanyId == companyId).FirstOrDefault();
                    if (ledgerData != null)
                    {
                        PackingRate = Math.Round(Convert.ToDouble(ledgerData.PackingRate), 2);
                        HamaliRate = Math.Round(Convert.ToDouble(ledgerData.HamaliRate), 2);
                        WeighManFeeRate = Math.Round(Convert.ToDouble(ledgerData.WeighManFeeRate), 2);
                        DalaliRate = Math.Round(Convert.ToDouble(ledgerData.DalaliRate), 2);
                        CessRate = Math.Round(Convert.ToDouble(ledgerData.CessRate), 2);
                    }

                    var commodityData = _tenantDBContext.Commodities.Where(n => n.CommodityId == commodityId).FirstOrDefault();
                    if (commodityData != null)
                    {
                        IGST = Math.Round(Convert.ToDouble(commodityData.IGST), 2);
                        SGST = Math.Round(Convert.ToDouble(commodityData.SGST), 2);
                        CGST = Math.Round(Convert.ToDouble(commodityData.CGST), 2);
                    }

                    var Packing = Math.Round(Convert.ToDouble(noOfBags * PackingRate), 2);
                    var Hamali = Math.Round(Convert.ToDouble(noOfBags * HamaliRate), 2);
                    var Weighman_Fee = Math.Round(Convert.ToDouble(noOfBags * WeighManFeeRate), 2);
                    var Commission = Math.Round((Convert.ToDouble(total_amount * DalaliRate / 100)), 2);
                    var Cess = Math.Round((Convert.ToDouble(total_amount * CessRate) / 100), 2);
                    var Taxable_Value = Math.Round(Convert.ToDouble(total_amount + Packing + Hamali + Weighman_Fee + Commission + Cess), 2);

                    var SGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * SGST) / 100), 2);
                    var CGST_Tot = Math.Round(Convert.ToDouble((Taxable_Value * CGST) / 100), 2);

                    decimal billAmount = item["total_amount"];
                    decimal Bastani = item["total_inclusive_amount"];

                    var Grand_Total = Taxable_Value + SGST_Tot + CGST_Tot;
                    if (Grand_Total == null)
                    {
                        Grand_Total = 0;
                    }
                    var roundOff = Math.Round(Math.Round(Convert.ToDouble(Grand_Total), 0) - Convert.ToDouble(Grand_Total), 2);

                    Grand_Total = Math.Round(Convert.ToDouble(Grand_Total + roundOff), 2);


                    var maxVochNo = _tenantDBContext.Inventory
                        .Where(inv => inv.CompanyId == companyId && inv.VochType == 2)
                        .Max(inv => inv.VochNo);

                    
                    var inventoryRecordsToUpdate = _tenantDBContext.Inventory
                        .Where(inv => inv.CompanyId == companyId && inv.VochNo == 0 && inv.IsTender == 1 && inv.LedgerId == ledgerId && inv.TranctDate == tranctDate)
                        .ToList();

                    foreach (var record in inventoryRecordsToUpdate)
                    {
                        record.VochNo = maxVochNo + 1;
                        record.PartyInvoiceNumber = BillNumber.ToString();  
                    }

                    BillSummary newBillSummary = new BillSummary
                    {
                        CompanyId = companyId,
                        LedgerId = ledgerId,
                        TranctDate = tranctDate,
                        DisplayinvNo = BillNumber,
                        PartyInvoiceNumber = BillNumber,
                        VochType = 2,
                        VochNo = maxVochNo,
                        TotalBags = noOfBags,
                        TotalWeight = totalWeight,
                        TaxableValue = taxableValue,
                        Discount = 0,
                        SGSTValue = Convert.ToDecimal(SGST_Tot),
                        CSGSTValue = Convert.ToDecimal(CGST_Tot),
                        IGSTValue = 0,
                        RoundOff = Convert.ToDecimal(roundOff),
                        TotalAmount = billAmount,
                        BillAmount = billAmount,
                        
                    };

                    _tenantDBContext.BillSummaries.Add(newBillSummary);
                    _tenantDBContext.SaveChanges();

                    /**/

                    var CommodityAccount = _tenantDBContext.Commodities.FirstOrDefault(A => A.CommodityId == commodityId);

                    string CommodityAccountName = CommodityAccount?.CommodityName + " " + "Account";
                    
                    var ledgerVal = (_tenantDBContext.Ledgers.
                                  Where(l => l.LedgerName == CommodityAccountName && l.CompanyId == companyId).Select(l => l.LedgerId)).FirstOrDefault();
                    
                    if (ledgerVal == 0 || ledgerVal == null)
                    {

                        //Ledger ledger = new Ledger
                        //{ 
                        //    CompanyId = companyId,
                        //    LedgerName = CommodityAccountName,
                        //    AccountingGroupId = 24,
                        //    CreatedBy = 1,
                        //    CreatedDate = DateTime.Now,
                        //    IsActive = true
                        //};

                        //ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                        //ledgerVal = ledger.LedgerId;

                        //_tenantDBContext.Add(ledger); 
                        //_tenantDBContext.SaveChanges();

                        Ledger ledger = new Ledger
                        {
                            CompanyId = companyId,
                            LedgerName = CommodityAccountName,
                            AccountingGroupId = 24,
                            CreatedBy = 1,
                            CreatedDate = DateTime.Now,
                            IsActive = 1
                        };

                        ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                       //ledger.LedgerId = 0;
                        ledgerVal = ledger.LedgerId;
                        _tenantDBContext.Add(ledger);
                        _tenantDBContext.SaveChanges();
                    }

                    Voucher voucherlst = new Voucher
                    {
                        CommodityId = 0,
                        TranctDate = tranctDate,
                        VoucherId = 2,
                        VoucherNo = maxVochNo,
                        Narration = "Invoicer-" + BillNumber,
                        LedgerId = ledgerVal,
                        CompanyId = companyId,
                        Credit = 0,
                        Debit = taxableValue,
                        IsActive = 1,
                        PartyInvoiceNumber = BillNumber,
                        LedgerNameForNarration = legder_name,
                        CreatedBy = 1
                    };
                    voucherlist.Add(voucherlst);

                    string CommodityAccountName1 = "Input SGST";
                    var ledgerVal1 = (_tenantDBContext.Ledgers.
                                  Where(l => l.LedgerName == CommodityAccountName1 && l.CompanyId == companyId).Select(l => l.LedgerId)).FirstOrDefault();
                     
                    Voucher voucherlst1 = new Voucher
                    {
                        CommodityId = 0,
                        TranctDate = tranctDate,
                        VoucherId = 2,
                        VoucherNo = maxVochNo,
                        Narration = "Invoice Number - " + BillNumber,
                        LedgerId = ledgerVal1,
                        CompanyId = companyId,
                        Credit = 0,
                        Debit = Convert.ToDecimal(SGST_Tot),
                        IsActive = 1,
                        PartyInvoiceNumber = BillNumber,
                        LedgerNameForNarration = legder_name,
                        CreatedBy = 1
                    };
                    voucherlist.Add(voucherlst1);
                     
                    string CommodityAccountName2 = "Input CGST";

                    var ledgerVal2 = (_tenantDBContext.Ledgers.
                                  Where(l => l.LedgerName == CommodityAccountName2 && l.CompanyId == companyId).Select(l => l.LedgerId)).FirstOrDefault();
                       
                    Voucher voucherlst11 = new Voucher
                    {
                        CommodityId = 0,
                        TranctDate = tranctDate,
                        VoucherId = 2,
                        VoucherNo = maxVochNo,
                        Narration = "Invoice Number - " + BillNumber,
                        LedgerId = ledgerVal2,
                        CompanyId = companyId,
                        Credit = 0,
                        Debit = Convert.ToDecimal(CGST_Tot),
                        IsActive = 1,
                        PartyInvoiceNumber = BillNumber,
                        LedgerNameForNarration = legder_name,
                        CreatedBy = 1
                    };
                    voucherlist.Add(voucherlst11);

                    var RoundOff = roundOff;
                    if (RoundOff == null)
                        roundOff = 0;

                    if (Convert.ToDecimal(roundOff) != 0)
                    {
                        string RoundoffAccountName = "Round Off Account";
                        var RoundoffLedgerID = (_tenantDBContext.Ledgers.
                                      Where(l => l.LedgerName == RoundoffAccountName && l.CompanyId == companyId).Select(l => l.LedgerId)).FirstOrDefault();

                        Voucher voucherlstExpenses3 = new Voucher
                        {
                            CommodityId = 0,
                            TranctDate = tranctDate,
                            VoucherId = 2,
                            VoucherNo = maxVochNo,
                            Narration = "Invoice Number - " + BillNumber,
                            LedgerId = RoundoffLedgerID,
                            CompanyId = companyId,
                            Debit = Convert.ToDecimal(roundOff) > 0 ? Convert.ToDecimal(roundOff) : 0,
                            Credit = (Convert.ToDecimal(roundOff) < 0 ? Convert.ToDecimal(roundOff) : 0) * -1,
                            IsActive = 1,
                            PartyInvoiceNumber = BillNumber,
                            LedgerNameForNarration = legder_name,
                            CreatedBy = 1
                        };
                        voucherlist.Add(voucherlstExpenses3);
                    }


                    Voucher voucherlstExpenses4 = new Voucher
                    {
                        CommodityId = 0,
                        TranctDate = tranctDate,
                        VoucherId = 2,
                        VoucherNo = maxVochNo,
                        Narration = "Invoice Number - " + BillNumber,
                        LedgerId = ledgerId,
                        CompanyId = companyId,
                        Credit = Convert.ToDecimal(Grand_Total),
                        Debit = 0,
                        IsActive = 1,
                        PartyInvoiceNumber = BillNumber,
                        LedgerNameForNarration = CommodityAccountName,
                        CreatedBy = 1
                    };
                    voucherlist.Add(voucherlstExpenses4);

                    _tenantDBContext.AddRange(voucherlist);
                    _tenantDBContext.SaveChanges();
                }

              
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

  
        /// <summary>
        /// Add sales  for the Invoicedat ,LorryData And Item data 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public string AddInvoice(JObject Data)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["InvioceData"].ToString());
                var LorryData = JsonConvert.DeserializeObject<dynamic>(Data["LorryDetails"].ToString());
                var itemdata = JsonConvert.DeserializeObject<dynamic>(Data["ItemData"].ToString());
                var invType = data["InvoiceType"];
                string TotalAmount = data["TotalAmount"];
                int companyid = data["CompanyId"];
                int invoiceNo = data["InoviceNo"];
                int ledgerid = data["LedgerId"];
                int VochType = data["VochType"];
                string CurrentFinanceYear = data["CurrentFinanceYear"];


                var ledgerName = (_tenantDBContext.Ledgers.
                                   Where(l => l.LedgerId == ledgerid).Select(l => l.LedgerName)).FirstOrDefault();
                var ledgerplace = (_tenantDBContext.Ledgers.
                                   Where(l => l.LedgerId == ledgerid && l.LedgerName == ledgerName).Select(l => l.Place)).FirstOrDefault();

                var InvoiceNo = new List<Inventory>();

                if (VochType == 9 || VochType == 10 || VochType == 11)
                {
                    InvoiceNo = _tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 12 && x.VochNo == invoiceNo && x.CompanyId == companyid).ToList();

                } else if(VochType == 12)
                {
                    InvoiceNo = _tenantDBContext.Inventory.Where(x => x.VochType == 12 && x.VochNo == invoiceNo && x.CompanyId == companyid).ToList();
                } else if(VochType == 15)
                {
                    InvoiceNo = _tenantDBContext.Inventory.Where(x => x.VochType == 15 && x.VochNo == invoiceNo && x.CompanyId == companyid).ToList();
                }  else if(VochType == 8)
                {
                    InvoiceNo = _tenantDBContext.Inventory.Where(x => x.VochType == 8 && x.VochNo == invoiceNo && x.CompanyId == companyid).ToList();
                }  else if(VochType == 13)
                {
                    InvoiceNo = _tenantDBContext.Inventory.Where(x => x.VochType == 13 && x.VochNo == invoiceNo && x.CompanyId == companyid).ToList();
                }   else if(VochType == 16)
                {
                    InvoiceNo = _tenantDBContext.Inventory.Where(x => x.VochType == 16 && x.VochNo == invoiceNo && x.CompanyId == companyid).ToList();
                } 

                if (InvoiceNo.Count > 0)
                {
                    var entityBls = _tenantDBContext.BillSummaries.FirstOrDefault(item => item.VochNo == Convert.ToInt64(invoiceNo) && item.VochType == VochType);
                    if (entityBls != null)
                    {
                        entityBls.IsActive = 0;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entityBls);
                    }



                    var Invresult = (from c in _tenantDBContext.Inventory
                                     where c.VochNo == Convert.ToInt64(invoiceNo) && c.VochType == VochType
                                     select new
                                     {
                                         c.Id
                                     }).ToList();


                    foreach (var invitem in Invresult)
                    {
                        var entityVoch = _tenantDBContext.Inventory.FirstOrDefault(item => item.VochNo == Convert.ToInt64(invoiceNo) && item.VochType == VochType && item.Id == Convert.ToInt64(invitem.Id));
                        entityVoch.IsActive = 0;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entityVoch);
                    }




                    var Vochresult = (from c in _tenantDBContext.Vouchers
                                      where c.VoucherNo == Convert.ToInt64(invoiceNo) && c.VoucherId == VochType
                                      select new
                                      {
                                          c.Id
                                      }).ToList();


                    foreach (var Vochitem in Vochresult)
                    {
                        var entityVoch = _tenantDBContext.Vouchers.FirstOrDefault(item => item.VoucherNo == Convert.ToInt64(invoiceNo) && item.VoucherId == VochType && item.Id == Convert.ToInt64(Vochitem.Id));
                        entityVoch.IsActive = 0;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entityVoch);
                    }


                     



                }


                var InvoiceString = (_tenantDBContext.Companies.
                                Where(c => c.CompanyId == companyid).Select(c => c.InvoiceString)).FirstOrDefault();

                var ShipBillDatevalue = LorryData["ShipBillDate"];
                if(ShipBillDatevalue == " ")
                {
                    LorryData["ShipBillDate"] = null;
                }

                string stateCode2value = data["GST"];
                stateCode2value = stateCode2value.Substring(0, 2).ToString();

                var RartyInvoiceNumber = "";

                var companyData = _tenantDBContext.Companies.Where(c => c.CompanyId == companyid).FirstOrDefault();

                if (VochType == 9 || VochType == 10 || VochType == 11)
                { 
                    if(invoiceNo.ToString().Length > 3)
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/" + invoiceNo;

                    } else
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/" + invoiceNo.ToString().PadLeft(3, '0');

                    }

                } else if(VochType == 6 || VochType == 8)
                {

                    if (invoiceNo.ToString().Length > 3)
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/" + invoiceNo;

                    }
                    else
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/CN" + invoiceNo.ToString().PadLeft(3, '0');

                    }

                } else if(VochType == 12)
                {

                    if (invoiceNo.ToString().Length > 3)
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/" + invoiceNo;

                    }
                    else
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/EXP" + invoiceNo.ToString().PadLeft(3, '0');

                    }

                } else if(VochType == 13)
                {

                    if (invoiceNo.ToString().Length > 3)
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/" + invoiceNo;

                    }
                    else
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/DMD" + invoiceNo.ToString().PadLeft(3, '0');

                    }

                } else if(VochType == 14 || VochType == 15)
                {

                    if (invoiceNo.ToString().Length > 3)
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/" + invoiceNo;

                    }
                    else
                    {
                        RartyInvoiceNumber = companyData.FirmCode + "/" + CurrentFinanceYear.Substring(2, 2) + CurrentFinanceYear.Substring(7, 2) + "/DN" + invoiceNo.ToString().PadLeft(3, '0');

                    }

                }

                var FrieghtPlus = 0;

                try
                {
                    if (LorryData["FrieghtPlus_Less"] == "Party Lorry Frieght" && LorryData["AdvanceFrieght"] > 0)
                    {
                        FrieghtPlus = 1;
                    }
                    else
                    {
                        FrieghtPlus = 0;
                    }
                } catch (Exception e)
                {
                    LorryData["FrieghtPlus_Less"] = "";
                }

                

                BillSummary billsummary = new BillSummary
                {
                    CompanyId = data["CompanyId"], //selling party
                    LedgerId = data["LedgerId"],//party id
                    LedgerName = ledgerName + "-" + ledgerplace,
                    frieghtPlus = FrieghtPlus,
                    TranctDate = data["OriginalInvDate"],
                    // CommodityID = itemdata["CommodityId"],
                    DisplayinvNo = RartyInvoiceNumber,
                    PartyInvoiceNumber = RartyInvoiceNumber,
                    VochType = data["VochType"],
                    VoucherName = data["VoucherName"]??"",
                    DealerType = data["DealerType"],
                    PAN = data["PAN"],
                    GST = data["GST"],
                    InvoiceType = invType,
                    VochNo = data["InoviceNo"],
                    State = data["State"]??"",
                    TotalBags = data["NoOfBags"],
                    TotalWeight = data["TotalWeight"],
                    //other charges block
                    ExpenseName1 = data["ExpenseName1"] ?? "",
                    ExpenseName2 = data["ExpenseName2"]??"",
                    ExpenseName3 = data["ExpenseName3"]??"",
                    ExpenseAmount1 = data["ExpenseAmount1"] ?? "",
                    ExpenseAmount2 = data["ExpenseAmount2"]??"",
                    ExpenseAmount3 = data["ExpenseAmount3"]??"",
                    //paymentDetails
                    TaxableValue = data["TaxableValue"],
                    Discount = data["Discount"],
                    SGSTValue = data["Sgstvalue"],
                    CSGSTValue = data["Cgstvalue"],
                    IGSTValue = data["Igstvalue"],
                    IsSEZ = data["IsSEZ"],
                    RoundOff = data["RoundOff"] ?? "",
                    TotalAmount = data["TotalAmount"],
                    BillAmount = data["BillAmount"],
                    StateCode2 = stateCode2value,
                    INWords = AmountToWord(Convert.ToDouble(TotalAmount)),// call amt here (write method to which pass amt & get return amt in words like Ruppes One Hundred & Fifty Five Only)                                                                     
                                                                          //DispatcherAddress1 = data["Address"],
                    FromPlace = data["FromPlace"] ?? "",
                    ToPlace = data["ToPlace"] ?? "",
                    //Lorray details
                    Ponumber = LorryData["PoNumber"],
                    EwayBillNo = LorryData["EwaybillNo"],
                    Transporter = LorryData["Transporter"],
                    LorryNo = LorryData["LorryNo"],
                    LorryOwnerName = LorryData["Owner"],
                    DriverName = LorryData["Driver"],
                    Dlno = LorryData["Dlno"],
                    CheckPost = LorryData["CheckPost"],
                    FrieghtPerBag = LorryData["FrieghtPerBag"],
                    TotalFrieght = LorryData["TotalFrieght"],
                    Advance = LorryData["AdvanceFrieght"],
                    Balance = LorryData["BalanceFrieght"],
                    IsLessOrPlus = (LorryData["FrieghtPlus_Less"] == "Party Lorry Frieght" ? true : false),//FrieghtPlus/Less field
                    tdsperc = LorryData["TDS"],
                    //  Dilivary Address Details                
                    DeliveryName = LorryData["DeliveryName"],
                    DeliveryAddress1 = LorryData["DeliveryAddress1"],
                    DeliveryAddress2 = LorryData["DeliveryAddress2"],
                    DeliveryPlace = LorryData["DeliveryPlace"],
                    DelPinCode = LorryData["DeliveryPin"],
                    DeliveryState = LorryData["DeliveryState"],
                    DeliveryStateCode = LorryData["DeliveryStateCode"],
                    Distance = LorryData["Distance"],
                    DCNote = LorryData["Dcnote"],
                    IsActive = 1,

                    // dispatcher
                    DispatcherName = LorryData["DispatcherName"],
                    DispatcherAddress1 = LorryData["DispatcherAddress1"],
                    DispatcherAddress2 = LorryData["DispatcherAddress2"],
                    DispatcherPlace = LorryData["DispatcherPlace"],
                    DispatcherPIN = LorryData["DispatcherPIN"],
                    DispatcherStatecode = LorryData["DispatcherStatecode"],
                    CountryCode = LorryData["CountryCode"],
                    ShipBillNo = LorryData["ShipBillNo"],
                    ForCur = LorryData["ForCur"],
                    PortName = LorryData["PortName"],
                    RefClaim = LorryData["RefClaim"],
                    ShipBillDate = LorryData["ShipBillDate"],
                    ExpDuty = LorryData["ExpDuty"],
                    FrieghtinBill = data["advancePaintAmount"],
                    IsServiceInvoice = data["IsServiceInvoice"],

                };
                //Item Details
                //add List of Itemdata
                List<Inventory> inventorylist = new List<Inventory>();

                var ItemNameForVoucherInsert = "";

                foreach (var item in itemdata)
                {
 
                    if (item["Sno"] == 1)
                    {
                        ItemNameForVoucherInsert = item["CommodityName"];
                    }

                    var NoOfDocradata = Convert.ToString(item["NoOfDocra"].Value);
                    if (NoOfDocradata == "")
                        item["NoOfDocra"] = 0;

                    var weight = 0;

                    try
                    {
                        if (item["TotalWeight"] != "")
                        {
                            weight = Convert.ToDouble(item["TotalWeight"]);
                        }
                    } catch(Exception e)
                    { 
                           weight = item["TotalWeight"];
                    }
                     
                    
                    string commodityid = item["CommodityId"];
                    var commodityname = _tenantDBContext.Commodities
                    .Where(c => c.CommodityId == Int32.Parse(commodityid)).Select(c => c.CommodityName).FirstOrDefault();
                    Inventory inventory = new Inventory()
                    {
                        CompanyId = data["CompanyId"],
                        VochNo = data["InoviceNo"],
                        VochType = data["VochType"],
                        InvoiceType = invType,
                        LedgerId = data["LedgerId"],
                        CommodityId = item["CommodityId"],
                        CommodityName = commodityname,
                        TranctDate = data["OriginalInvDate"], //from data
                        PoNumber = LorryData["PoNumber"], //from lorry
                        EwaybillNo = LorryData["EwaybillNo"], //from lorry
                        WeightPerBag = item["WeightPerBag"],//1
                        NoOfBags = item["NoOfBags"],//2
                        TotalWeight = weight,//3
                        Rate = item["Rate"],//4
                        Amount = item["Amount"],//5
                        Mark = item["Remark"],//6
                        PartyInvoiceNumber = RartyInvoiceNumber,
                        Discount = data["Discount"],
                        NetAmount = item["Amount"],
                        SGST = item["SgstAmount"],
                        CGST = item["CgstAmount"],
                        IGST = item["IgstAmount"],
                        NoOfDocra = item["NoOfDocra"], //for item qty in table no field so this field is used
                        CreatedDate = DateTime.Now,
                        FreeQty = 0,
                        IGSTRate = item["IgstRate"],
                        SGSTRate = item["SgstRate"],
                        CGSTRate = item["CgstRate"],
                        Taxable = item["Taxable"],
                        
                        IsActive = 1
                    };
                    inventorylist.Add(inventory);
                }
                _tenantDBContext.Add(billsummary);
                //_tenantDBContext.SaveChanges();
                _tenantDBContext.AddRange(inventorylist);
                _tenantDBContext.SaveChanges();

                if(VochType != 16)
                {
                    if (VochType > 8)
                    {
                        var Comm = (from t1 in _tenantDBContext.Inventory
                                    join t2 in _tenantDBContext.Commodities on t1.CommodityId equals t2.CommodityId
                                    where t1.CompanyId == companyid && t1.VochType == VochType && t1.VochNo == invoiceNo
                                    group t1 by new { t1.CommodityId, t2.CommodityName } into g
                                    select new
                                    {
                                        CommodityName = g.Key.CommodityName,
                                        Amount = g.Sum(t1 => t1.Amount)
                                    }).ToList();
                        List<Voucher> voucherlist = new List<Voucher>();

                        string FirstRowCommodityAccountName = "";

                        foreach (var item in Comm)
                        {
                            var CommodityAccount = item.CommodityName;
                            string CommodityAccountName = CommodityAccount + " " + "Account";
                            var sumofAmt = Comm.Select(c => c.Amount).First();
                            var ledgerVal = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                            if (ledgerVal == 0 || ledgerVal == null)
                            {
                                Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = CommodityAccountName,
                                    AccountingGroupId = 24,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = 1
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                ledgerVal = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();
                            }








                            Voucher voucherlst = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(item.Amount),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = LorryData["DeliveryName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst);
                        }


                        if (Convert.ToDecimal(data["Igstvalue"]) > 0)
                        {
                            string CommodityAccountName1 = "Output IGST";
                            var ledgerVal1 = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            Voucher voucherlst = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal1,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["Igstvalue"]),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = LorryData["DeliveryName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst);


                        }
                        else
                        {
                            string CommodityAccountName1 = "Output SGST";
                            var ledgerVal1 = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            var sgstvalue = data["Sgstvalue"];
                            if (sgstvalue == null)
                                data["Sgstvalue"] = 0;
                            Voucher voucherlst = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal1,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["Sgstvalue"]),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst);

                            string CommodityAccountName2 = "Output CGST";

                            var ledgerVal2 = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName2 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            var Cgstvalue = data["Cgstvalue"];
                            if (Cgstvalue == null)
                                data["Cgstvalue"] = 0;

                            Voucher voucherlst1 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal2,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["Cgstvalue"]),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst1);
                        }




                        if (Convert.ToDecimal(data["ExpenseAmount1"]) > 0)
                        {
                            string ExpenseName1 = data["ExpenseName1"];
                            var Expense1LedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == ExpenseName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                            if (Expense1LedgerID == 0 || Expense1LedgerID == null)
                            {
                                Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = ExpenseName1,
                                    AccountingGroupId = 24,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = 1
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                Expense1LedgerID = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();
                            }




                            Voucher voucherlstExpenses1 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = Expense1LedgerID,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["ExpenseAmount1"]),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses1);
                        }

                        if (Convert.ToDecimal(data["ExpenseAmount2"]) > 0)
                        {
                            string ExpenseName2 = data["ExpenseName2"];
                            var Expense2LedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == ExpenseName2 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                            if (Expense2LedgerID == 0 || Expense2LedgerID == null)
                            {
                                Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = ExpenseName2,
                                    AccountingGroupId = 24,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = 1
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                Expense2LedgerID = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();
                            }


                            Voucher voucherlstExpenses2 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = Expense2LedgerID,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["ExpenseAmount2"]),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses2);
                        }

                        if (Convert.ToDecimal(data["ExpenseAmount3"]) > 0)
                        {
                            string ExpenseName3 = data["ExpenseName3"];
                            var Expense3LedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == ExpenseName3 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            if (Expense3LedgerID == 0 || Expense3LedgerID == null)
                            {
                                Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = ExpenseName3,
                                    AccountingGroupId = 24,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = 1
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                Expense3LedgerID = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();
                            }




                            Voucher voucherlstExpenses3 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = Expense3LedgerID,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["ExpenseAmount3"]),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses3);
                        }



                        if (LorryData["FrieghtPlus_Less"] == "Party Lorry Frieght")
                        {
                            if (Convert.ToDecimal(LorryData["AdvanceFrieght"]) > 0)
                            {
                                string TransPorterName1 = "";

                                if (LorryData["Transporter"] == null || LorryData["Transporter"] == "")
                                {
                                    TransPorterName1 = "Lorry Frieght Advance Account";
                                }
                                else
                                {
                                    TransPorterName1 = LorryData["Transporter"];
                                }

                                var ledgerValTransPorterName1 = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TransPorterName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                                if (ledgerValTransPorterName1 == 0 || ledgerValTransPorterName1 == null)
                                {
                                    /*Ledger ledger = new Ledger
                                    {
                                        CompanyId = companyid,
                                        LedgerName = LorryData["Transporter"],
                                        AccountingGroupId = 21,
                                        CreatedBy = 1,
                                        CreatedDate = DateTime.Now,
                                        IsActive = true
                                    };

                                    ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                    ledgerValTransPorterName1 = ledger.LedgerId;
                                    _tenantDBContext.Add(ledger);
                                    _tenantDBContext.SaveChanges();*/

                                    var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                    string connectionString = builder.ToString();
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        SqlCommand insertCommand = connection.CreateCommand();
                                        insertCommand.CommandType = CommandType.Text;
                                        insertCommand.CommandText = @"INSERT INTO Ledger (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) 
                                   VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive);";

                                        insertCommand.Parameters.AddWithValue("@CompanyId", companyid);
                                        insertCommand.Parameters.AddWithValue("@LedgerName", Convert.ToString(LorryData["Transporter"]));
                                        insertCommand.Parameters.AddWithValue("@AccountingGroupId", 21);
                                        insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                        insertCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                        insertCommand.Parameters.AddWithValue("@IsActive", 1);

                                        insertCommand.ExecuteNonQuery();

                                        SqlCommand selectMaxIdCommand = connection.CreateCommand();
                                        selectMaxIdCommand.CommandType = CommandType.Text;
                                        selectMaxIdCommand.CommandText = "SELECT MAX(LedgerId) FROM Ledger;";

                                        int ledgerId = Convert.ToInt32(selectMaxIdCommand.ExecuteScalar());

                                        ledgerValTransPorterName1 = ledgerId;

                                        // Close the connection
                                        connection.Close();
                                    }

                                }

                                Voucher voucherlstAdvanceFrieght1 = new Voucher
                                {
                                    CommodityId = 0,
                                    TranctDate = data["OriginalInvDate"],
                                    VoucherId = VochType,
                                    VoucherNo = invoiceNo,
                                    Narration = Convert.ToString(invoiceNo),
                                    LedgerId = ledgerValTransPorterName1,
                                    CompanyId = companyid,
                                    Credit = Convert.ToDecimal(LorryData["AdvanceFrieght"]),
                                    Debit = 0,
                                    IsActive = 1,
                                    PartyInvoiceNumber = RartyInvoiceNumber,
                                    LedgerNameForNarration = data["LedgerName"],
                                    CreatedBy = 1
                                };
                                voucherlist.Add(voucherlstAdvanceFrieght1);
                            }
                        }

                        if (LorryData["FrieghtPlus_Less"] == "Own Lorry Frieght")
                        {
                            if (LorryData["TotalFrieght"] > 0)
                            {
                                string TotalFrieghtName = "";

                                TotalFrieghtName = "Lorry Frieght Account";

                                var ledgerValTransPorterName1 = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TotalFrieghtName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                                Voucher voucherlstAdvanceFrieght1 = new Voucher
                                {
                                    CommodityId = 0,
                                    TranctDate = data["OriginalInvDate"],
                                    VoucherId = VochType,
                                    VoucherNo = invoiceNo,
                                    Narration = Convert.ToString(invoiceNo),
                                    LedgerId = ledgerValTransPorterName1,
                                    CompanyId = companyid,
                                    Credit = 0,
                                    Debit = Convert.ToDecimal(LorryData["TotalFrieght"]),
                                    IsActive = 1,
                                    PartyInvoiceNumber = RartyInvoiceNumber,
                                    LedgerNameForNarration = data["LedgerName"],
                                    CreatedBy = 1
                                };
                                voucherlist.Add(voucherlstAdvanceFrieght1);
                            }

                            if (Convert.ToDecimal(LorryData["AdvanceFrieght"]) > 0)
                            {
                                string TransPorterName1 = "";

                                if (LorryData["Transporter"] == null || LorryData["Transporter"] == "")
                                {
                                    TransPorterName1 = "Lorry Frieght Advance Account";
                                }
                                else
                                {
                                    TransPorterName1 = LorryData["Transporter"];
                                }

                                var ledgerValTransPorterName1 = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TransPorterName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                                if (ledgerValTransPorterName1 == 0 || ledgerValTransPorterName1 == null)
                                {
                                    /*Ledger ledger = new Ledger
                                    {
                                        CompanyId = companyid,
                                        LedgerName = TransPorterName1,
                                        AccountingGroupId = 21,
                                        CreatedBy = 1,
                                        CreatedDate = DateTime.Now,
                                        IsActive = true
                                    };

                                    ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                    ledgerValTransPorterName1 = ledger.LedgerId;
                                    _tenantDBContext.Add(ledger);
                                    _tenantDBContext.SaveChanges();*/

                                    var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                    string connectionString = builder.ToString();

                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        string insertQuery = @"INSERT INTO Ledgers (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive)
                            VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive);
                            SELECT SCOPE_IDENTITY();";
                                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                                        {
                                            insertCommand.Parameters.AddWithValue("@CompanyId", companyid);
                                            insertCommand.Parameters.AddWithValue("@LedgerName", TransPorterName1);
                                            insertCommand.Parameters.AddWithValue("@AccountingGroupId", 21);
                                            insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                            insertCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                            insertCommand.Parameters.AddWithValue("@IsActive", 1);

                                            int newLedgerId = Convert.ToInt32(insertCommand.ExecuteScalar());

                                            ledgerValTransPorterName1 = newLedgerId;
                                        }
                                    }

                                }


                                Voucher voucherlstAdvanceFrieght1 = new Voucher
                                {
                                    CommodityId = 0,
                                    TranctDate = data["OriginalInvDate"],
                                    VoucherId = VochType,
                                    VoucherNo = invoiceNo,
                                    Narration = Convert.ToString(invoiceNo),
                                    LedgerId = ledgerValTransPorterName1,
                                    CompanyId = companyid,
                                    Credit = Convert.ToDecimal(LorryData["AdvanceFrieght"]),
                                    Debit = 0,
                                    IsActive = 1,
                                    PartyInvoiceNumber = RartyInvoiceNumber,
                                    LedgerNameForNarration = data["LedgerName"],
                                    CreatedBy = 1
                                };
                                voucherlist.Add(voucherlstAdvanceFrieght1);
                            }
                        }

                        var RoundOff = data["RoundOff"];
                        if (RoundOff == null)
                            data["RoundOff"] = 0;

                        if (Convert.ToDecimal(data["RoundOff"]) != 0)
                        {
                            string RoundoffAccountName = "Round Off Account";
                            var RoundoffLedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == RoundoffAccountName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            Voucher voucherlstExpenses3 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = RoundoffLedgerID,
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["RoundOff"]) > 0 ? Convert.ToDecimal(data["RoundOff"]) : 0,
                                Debit = (Convert.ToDecimal(data["RoundOff"]) < 0 ? Convert.ToDecimal(data["RoundOff"]) : 0) * -1,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses3);
                        }


                        if (LorryData["FrieghtPlus_Less"] == "Own Lorry Frieght" && LorryData["Transporter"] != "" && LorryData["TotalFrieght"] > 0 && LorryData["TDS"] > 0)
                        {
                            var TDSAmount = (LorryData["TotalFrieght"] * LorryData["TDS"]) / 100;

                            string TDSAccountName = "TDS On Rent";
                            var TDSLedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == TDSAccountName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            if (TDSLedgerID == 0 || TDSLedgerID == null)
                            {
                                /* Ledger ledger = new Ledger
                                 {
                                     CompanyId = companyid,
                                     LedgerName = TDSAccountName,
                                     AccountingGroupId = 27,
                                     CreatedBy = 1,
                                     CreatedDate = DateTime.Now,
                                     IsActive = true
                                 };

                                 ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                 TDSLedgerID = ledger.LedgerId;
                                 _tenantDBContext.Add(ledger);
                                 _tenantDBContext.SaveChanges();*/

                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();

                                    SqlCommand command = connection.CreateCommand();
                                    command.CommandText = "INSERT INTO Ledger (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive); SELECT SCOPE_IDENTITY();";
                                    command.Parameters.AddWithValue("@CompanyId", companyid);
                                    command.Parameters.AddWithValue("@LedgerName", TDSAccountName);
                                    command.Parameters.AddWithValue("@AccountingGroupId", 27);
                                    command.Parameters.AddWithValue("@CreatedBy", 1);
                                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                    command.Parameters.AddWithValue("@IsActive", 1);

                                    int ledgerId = Convert.ToInt32(command.ExecuteScalar());

                                    TDSLedgerID = ledgerId;
                                }

                            }



                            Voucher VochTDSExpenses = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = TDSLedgerID,
                                CompanyId = companyid,
                                Credit = TDSAmount,
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(VochTDSExpenses);


                            string TransPorterNameFroTDS = LorryData["Transporter"];
                            var ledgerValTransPorterForTDS = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TransPorterNameFroTDS && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            if (ledgerValTransPorterForTDS == 0 || ledgerValTransPorterForTDS == null)
                            {
                                /*Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = LorryData["Transporter"],
                                    AccountingGroupId = 21,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                ledgerValTransPorterForTDS = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();*/


                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();

                                    string insertQuery = @"INSERT INTO Ledgers (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) 
                           VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive);
                           SELECT SCOPE_IDENTITY();";

                                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyid);
                                    insertCommand.Parameters.AddWithValue("@LedgerName", Convert.ToString(LorryData["Transporter"]));
                                    insertCommand.Parameters.AddWithValue("@AccountingGroupId", 21);
                                    insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                    insertCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                    insertCommand.Parameters.AddWithValue("@IsActive", 1);

                                    int ledgerId = Convert.ToInt32(insertCommand.ExecuteScalar());

                                    connection.Close();
                                }

                            }

                            Voucher voucherlstAdvanceFrieght1 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerValTransPorterForTDS,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = TDSAmount,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstAdvanceFrieght1);

                        }




                        var BillAmount = data["BillAmount"];
                        if (BillAmount == null)
                            data["BillAmount"] = 0;

                        if (Convert.ToDecimal(data["BillAmount"]) != 0)
                        {

                            Voucher voucherlstBillAmount = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = data["LedgerId"],
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(data["BillAmount"]),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = ItemNameForVoucherInsert + " Account",
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstBillAmount);
                        }
                        _tenantDBContext.AddRange(voucherlist);
                        _tenantDBContext.SaveChanges();
                    }
                    else
                    {
                        var Comm = (from t1 in _tenantDBContext.Inventory
                                    join t2 in _tenantDBContext.Commodities on t1.CommodityId equals t2.CommodityId
                                    where t1.CompanyId == companyid && t1.VochType == VochType && t1.VochNo == invoiceNo
                                    group t1 by new { t1.CommodityId, t2.CommodityName } into g
                                    select new
                                    {
                                        CommodityName = g.Key.CommodityName,
                                        Amount = g.Sum(t1 => t1.Amount)
                                    }).ToList();
                        List<Voucher> voucherlist = new List<Voucher>();

                        string FirstRowCommodityAccountName = "";

                        foreach (var item in Comm)
                        {
                            var CommodityAccount = Comm.Select(c => c.CommodityName).First();
                            string CommodityAccountName = CommodityAccount + " " + "Account";
                            var sumofAmt = Comm.Select(c => c.Amount).First();
                            var ledgerVal = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                            if (ledgerVal == 0 || ledgerVal == null)
                            {
                                /*Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = CommodityAccountName,
                                    AccountingGroupId = 24,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                ledgerVal = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();*/

                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();

                                    SqlCommand insertCommand = new SqlCommand(
                                        "INSERT INTO Ledgers (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) " +
                                        "VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive);" +
                                        "SELECT CAST(scope_identity() AS int)", connection);

                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyid);
                                    insertCommand.Parameters.AddWithValue("@LedgerName", CommodityAccountName);
                                    insertCommand.Parameters.AddWithValue("@AccountingGroupId", 24);
                                    insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                    insertCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                    insertCommand.Parameters.AddWithValue("@IsActive", 1);

                                    int insertedLedgerId = (int)insertCommand.ExecuteScalar();

                                    connection.Close();
                                }

                            }








                            Voucher voucherlst = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(item.Amount),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = LorryData["DeliveryName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst);
                        }

                        var Igstvalue = data["Igstvalue"];
                        if (Igstvalue == null)
                            data["Igstvalue"] = 0;

                        if (Convert.ToDecimal(data["Igstvalue"]) > 0)
                        {
                            string CommodityAccountName1 = "Input IGST";
                            var ledgerVal1 = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            Voucher voucherlst = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal1,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(data["Igstvalue"]),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = LorryData["DeliveryName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst);


                        }
                        else
                        {
                            string CommodityAccountName1 = "Input SGST";
                            var ledgerVal1 = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            Voucher voucherlst = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal1,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(data["Sgstvalue"]),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst);

                            string CommodityAccountName2 = "Input CGST";

                            var ledgerVal2 = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == CommodityAccountName2 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            Voucher voucherlst1 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerVal2,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(data["Cgstvalue"]),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlst1);
                        }




                        if (Convert.ToDecimal(data["ExpenseAmount1"]) > 0)
                        {
                            string ExpenseName1 = data["ExpenseName1"];
                            var Expense1LedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == ExpenseName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                            if (Expense1LedgerID == 0 || Expense1LedgerID == null)
                            {
                                /*Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = ExpenseName1,
                                    AccountingGroupId = 24,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                Expense1LedgerID = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();*/

                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();
                                    string insertQuery = "INSERT INTO Ledger (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) " +
                                                         "VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive)";

                                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyid);
                                    insertCommand.Parameters.AddWithValue("@LedgerName", ExpenseName1);
                                    insertCommand.Parameters.AddWithValue("@AccountingGroupId", 24);
                                    insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                    insertCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                    insertCommand.Parameters.AddWithValue("@IsActive", 1);

                                    insertCommand.ExecuteNonQuery();

                                    string selectMaxQuery = "SELECT MAX(LedgerId) FROM Ledger";
                                    SqlCommand selectMaxCommand = new SqlCommand(selectMaxQuery, connection);
                                    int ledgerId = Convert.ToInt32(selectMaxCommand.ExecuteScalar());

                                    Expense1LedgerID = ledgerId;
                                }

                            }




                            Voucher voucherlstExpenses1 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = Expense1LedgerID,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(data["ExpenseAmount1"]),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses1);
                        }

                        if (Convert.ToDecimal(data["ExpenseAmount2"]) > 0)
                        {
                            string ExpenseName2 = data["ExpenseName2"];
                            var Expense2LedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == ExpenseName2 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                            if (Expense2LedgerID == 0 || Expense2LedgerID == null)
                            {
                                /* Ledger ledger = new Ledger
                                 {
                                     CompanyId = companyid,
                                     LedgerName = ExpenseName2,
                                     AccountingGroupId = 24,
                                     CreatedBy = 1,
                                     CreatedDate = DateTime.Now,
                                     IsActive = true
                                 };

                                 ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                 Expense2LedgerID = ledger.LedgerId;
                                 _tenantDBContext.Add(ledger);
                                 _tenantDBContext.SaveChanges();*/

                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    // Open the connection
                                    connection.Open();

                                    // Create a SqlCommand object for inserting data
                                    using (SqlCommand command = connection.CreateCommand())
                                    {
                                        command.CommandType = CommandType.Text;

                                        // Insert command text
                                        command.CommandText = "INSERT INTO Ledger (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) " +
                                                              "VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive)";

                                        // Add parameters
                                        command.Parameters.AddWithValue("@CompanyId", companyid);
                                        command.Parameters.AddWithValue("@LedgerName", ExpenseName2);
                                        command.Parameters.AddWithValue("@AccountingGroupId", 24);
                                        command.Parameters.AddWithValue("@CreatedBy", 1);
                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                        command.Parameters.AddWithValue("@IsActive", 1);

                                        // Execute the insert command
                                        command.ExecuteNonQuery();
                                    }

                                    // Retrieve the maximum LedgerId
                                    int ledgerId;
                                    using (SqlCommand command = connection.CreateCommand())
                                    {
                                        command.CommandText = "SELECT MAX(LedgerId) FROM Ledger";

                                        // Execute the select command
                                        ledgerId = Convert.ToInt32(command.ExecuteScalar());
                                    }

                                    // Set Expense2LedgerID
                                    Expense2LedgerID = ledgerId;

                                    Console.WriteLine("Expense2LedgerID: " + Expense2LedgerID);
                                }

                            }


                            Voucher voucherlstExpenses2 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = Expense2LedgerID,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(data["ExpenseAmount2"]),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses2);
                        }

                        if (Convert.ToDecimal(data["ExpenseAmount3"]) > 0)
                        {
                            string ExpenseName3 = data["ExpenseName3"];
                            var Expense3LedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == ExpenseName3 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            if (Expense3LedgerID == 0 || Expense3LedgerID == null)
                            {
                                /*Ledger ledger = new Ledger
                                {
                                    CompanyId = companyid,
                                    LedgerName = ExpenseName3,
                                    AccountingGroupId = 24,
                                    CreatedBy = 1,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true
                                };

                                ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                Expense3LedgerID = ledger.LedgerId;
                                _tenantDBContext.Add(ledger);
                                _tenantDBContext.SaveChanges();*/
                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    // Open the connection
                                    connection.Open();

                                    // Create a command to insert the new ledger
                                    SqlCommand insertCommand = new SqlCommand("INSERT INTO Ledger (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) " +
                                                                              "VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive); " +
                                                                              "SELECT SCOPE_IDENTITY();", connection);
                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyid);
                                    insertCommand.Parameters.AddWithValue("@LedgerName", ExpenseName3);
                                    insertCommand.Parameters.AddWithValue("@AccountingGroupId", 24);
                                    insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                    insertCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                    insertCommand.Parameters.AddWithValue("@IsActive", 1);

                                    // Execute the insert command and get the new LedgerId
                                    int ledgerId = Convert.ToInt32(insertCommand.ExecuteScalar());

                                    // Set the Expense3LedgerID to the new LedgerId
                                    int expense3LedgerId = ledgerId;

                                    // Close the connection
                                    connection.Close();
                                }

                            }




                            Voucher voucherlstExpenses3 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = Expense3LedgerID,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = Convert.ToDecimal(data["ExpenseAmount3"]),
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses3);
                        }



                        if (LorryData["FrieghtPlus_Less"] == "Party Lorry Frieght")
                        {
                            if (Convert.ToDecimal(LorryData["AdvanceFrieght"]) > 0)
                            {
                                string TransPorterName1 = "";

                                if (LorryData["Transporter"] == null || LorryData["Transporter"] == "")
                                {
                                    TransPorterName1 = "Lorry Frieght Advance Account";
                                }
                                else
                                {
                                    TransPorterName1 = LorryData["Transporter"];
                                }

                                var ledgerValTransPorterName1 = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TransPorterName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                                if (ledgerValTransPorterName1 == 0 || ledgerValTransPorterName1 == null)
                                {
                                    /* Ledger ledger = new Ledger
                                     {
                                         CompanyId = companyid,
                                         LedgerName = LorryData["Transporter"],
                                         AccountingGroupId = 21,
                                         CreatedBy = 1,
                                         CreatedDate = DateTime.Now,
                                         IsActive = true
                                     };

                                     ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                     ledgerValTransPorterName1 = ledger.LedgerId;
                                     _tenantDBContext.Add(ledger);
                                     _tenantDBContext.SaveChanges();*/
                                    var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                    string connectionString = builder.ToString();
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        string insertQuery = @"INSERT INTO Ledgers (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive)
                                                       VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive);
                                                       SELECT SCOPE_IDENTITY();";
                                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                        {
                                            command.Parameters.AddWithValue("@CompanyId", companyid);
                                            command.Parameters.AddWithValue("@LedgerName", Convert.ToString(LorryData["Transporter"]));
                                            command.Parameters.AddWithValue("@AccountingGroupId", 21);
                                            command.Parameters.AddWithValue("@CreatedBy", 1);
                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@IsActive", 1);

                                            int ledgerId = Convert.ToInt32(command.ExecuteScalar());
                                            ledgerValTransPorterName1 = ledgerId;
                                        }
                                    }

                                }

                                Voucher voucherlstAdvanceFrieght1 = new Voucher
                                {
                                    CommodityId = 0,
                                    TranctDate = data["OriginalInvDate"],
                                    VoucherId = VochType,
                                    VoucherNo = invoiceNo,
                                    Narration = Convert.ToString(invoiceNo),
                                    LedgerId = ledgerValTransPorterName1,
                                    CompanyId = companyid,
                                    Credit = 0,
                                    Debit = Convert.ToDecimal(LorryData["AdvanceFrieght"]),
                                    IsActive = 1,
                                    PartyInvoiceNumber = RartyInvoiceNumber,
                                    LedgerNameForNarration = data["LedgerName"],
                                    CreatedBy = 1
                                };
                                voucherlist.Add(voucherlstAdvanceFrieght1);
                            }
                        }

                        if (LorryData["FrieghtPlus_Less"] == "Own Lorry Frieght")
                        {
                            if (LorryData["TotalFrieght"] > 0)
                            {
                                string TotalFrieghtName = "";

                                TotalFrieghtName = "Lorry Frieght Account";

                                var ledgerValTransPorterName1 = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TotalFrieghtName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                                Voucher voucherlstAdvanceFrieght1 = new Voucher
                                {
                                    CommodityId = 0,
                                    TranctDate = data["OriginalInvDate"],
                                    VoucherId = VochType,
                                    VoucherNo = invoiceNo,
                                    Narration = Convert.ToString(invoiceNo),
                                    LedgerId = ledgerValTransPorterName1,
                                    CompanyId = companyid,
                                    Credit = Convert.ToDecimal(LorryData["TotalFrieght"]),
                                    Debit = 0,
                                    IsActive = 1,
                                    PartyInvoiceNumber = RartyInvoiceNumber,
                                    LedgerNameForNarration = data["LedgerName"],
                                    CreatedBy = 1
                                };
                                voucherlist.Add(voucherlstAdvanceFrieght1);
                            }

                            if (Convert.ToDecimal(LorryData["AdvanceFrieght"]) > 0)
                            {
                                string TransPorterName1 = "";

                                if (LorryData["Transporter"] == null || LorryData["Transporter"] == "")
                                {
                                    TransPorterName1 = "Lorry Frieght Advance Account";
                                }
                                else
                                {
                                    TransPorterName1 = LorryData["Transporter"];
                                }

                                var ledgerValTransPorterName1 = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TransPorterName1 && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();


                                if (ledgerValTransPorterName1 == 0 || ledgerValTransPorterName1 == null)
                                {
                                    /*Ledger ledger = new Ledger
                                    {
                                        CompanyId = companyid,
                                        LedgerName = TransPorterName1,
                                        AccountingGroupId = 21,
                                        CreatedBy = 1,
                                        CreatedDate = DateTime.Now,
                                        IsActive = true
                                    };

                                    ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                    ledgerValTransPorterName1 = ledger.LedgerId;
                                    _tenantDBContext.Add(ledger);
                                    _tenantDBContext.SaveChanges();*/

                                    var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                    string connectionString = builder.ToString();
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        // Open the connection
                                        connection.Open();

                                        // Create a SqlCommand to insert the new Ledger record
                                        string insertQuery = @"INSERT INTO Ledger (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive)
                                                       VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive);
                                                       SELECT SCOPE_IDENTITY();";

                                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                        {
                                            // Set parameters for the insert query
                                            command.Parameters.AddWithValue("@CompanyId", companyid);
                                            command.Parameters.AddWithValue("@LedgerName", TransPorterName1);
                                            command.Parameters.AddWithValue("@AccountingGroupId", 21);
                                            command.Parameters.AddWithValue("@CreatedBy", 1);
                                            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                            command.Parameters.AddWithValue("@IsActive", 1);

                                            // Execute the insert query and retrieve the newly generated LedgerId
                                            int newLedgerId = Convert.ToInt32(command.ExecuteScalar());

                                            // Assign the newly generated LedgerId to the ledger object
                                            ledgerValTransPorterName1 = newLedgerId;
                                        }
                                    }

                                }


                                Voucher voucherlstAdvanceFrieght1 = new Voucher
                                {
                                    CommodityId = 0,
                                    TranctDate = data["OriginalInvDate"],
                                    VoucherId = VochType,
                                    VoucherNo = invoiceNo,
                                    Narration = Convert.ToString(invoiceNo),
                                    LedgerId = ledgerValTransPorterName1,
                                    CompanyId = companyid,
                                    Debit = Convert.ToDecimal(LorryData["AdvanceFrieght"]),
                                    Credit = 0,
                                    IsActive = 1,
                                    PartyInvoiceNumber = RartyInvoiceNumber,
                                    LedgerNameForNarration = data["LedgerName"],
                                    CreatedBy = 1
                                };
                                voucherlist.Add(voucherlstAdvanceFrieght1);
                            }
                        }


                        if (Convert.ToDecimal(data["RoundOff"]) != 0)
                        {
                            string RoundoffAccountName = "Round Off Account";
                            var RoundoffLedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == RoundoffAccountName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            Voucher voucherlstExpenses3 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = RoundoffLedgerID,
                                CompanyId = companyid,
                                Debit = Convert.ToDecimal(data["RoundOff"]) > 0 ? Convert.ToDecimal(data["RoundOff"]) : 0,
                                Credit = (Convert.ToDecimal(data["RoundOff"]) < 0 ? Convert.ToDecimal(data["RoundOff"]) : 0) * -1,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstExpenses3);
                        }


                        if (LorryData["FrieghtPlus_Less"] == "Own Lorry Frieght" && LorryData["Transporter"] != "" && LorryData["TotalFrieght"] > 0 && LorryData["TDS"] > 0)
                        {
                            var TDSAmount = (LorryData["TotalFrieght"] * LorryData["TDS"]) / 100;

                            string TDSAccountName = "TDS On Rent";
                            var TDSLedgerID = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == TDSAccountName && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            if (TDSLedgerID == 0 || TDSLedgerID == null)
                            {
                                /*  Ledger ledger = new Ledger
                                  {
                                      CompanyId = companyid,
                                      LedgerName = TDSAccountName,
                                      AccountingGroupId = 27,
                                      CreatedBy = 1,
                                      CreatedDate = DateTime.Now,
                                      IsActive = true
                                  };

                                  ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                  TDSLedgerID = ledger.LedgerId;
                                  _tenantDBContext.Add(ledger);
                                  _tenantDBContext.SaveChanges();*/

                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    // Open the connection
                                    connection.Open();

                                    // Create a SqlCommand for inserting the ledger
                                    string insertQuery = @"INSERT INTO Ledgers (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) 
                           VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive)";

                                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                                    // Set the parameters for the insert command
                                    insertCommand.Parameters.AddWithValue("@CompanyId", companyid);
                                    insertCommand.Parameters.AddWithValue("@LedgerName", TDSAccountName);
                                    insertCommand.Parameters.AddWithValue("@AccountingGroupId", 27);
                                    insertCommand.Parameters.AddWithValue("@CreatedBy", 1);
                                    insertCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                    insertCommand.Parameters.AddWithValue("@IsActive", 1);

                                    // Execute the insert command
                                    insertCommand.ExecuteNonQuery();

                                    // Create a SqlCommand for getting the max LedgerId
                                    string maxQuery = "SELECT MAX(LedgerId) FROM Ledgers";

                                    SqlCommand maxCommand = new SqlCommand(maxQuery, connection);

                                    // Get the max LedgerId
                                    int maxLedgerId = Convert.ToInt32(maxCommand.ExecuteScalar());

                                    // Set the LedgerId and TDSLedgerID
                                    int newLedgerId = maxLedgerId + 1;
                                    TDSLedgerID = newLedgerId;

                                    // Close the connection
                                    connection.Close();
                                }



                            }



                            Voucher VochTDSExpenses = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = TDSLedgerID,
                                CompanyId = companyid,
                                Credit = 0,
                                Debit = TDSAmount,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(VochTDSExpenses);


                            string TransPorterNameFroTDS = LorryData["Transporter"];
                            var ledgerValTransPorterForTDS = (_tenantDBContext.Ledgers.
                                              Where(l => l.LedgerName == TransPorterNameFroTDS && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                            if (ledgerValTransPorterForTDS == 0 || ledgerValTransPorterForTDS == null)
                            {
                                /* Ledger ledger = new Ledger
                                 {
                                     CompanyId = companyid,
                                     LedgerName = LorryData["Transporter"],
                                     AccountingGroupId = 21,
                                     CreatedBy = 1,
                                     CreatedDate = DateTime.Now,
                                     IsActive = true
                                 };

                                 ledger.LedgerId = _tenantDBContext.Ledgers.Select(x => x.LedgerId).ToList().Max() + 1;
                                 ledgerValTransPorterForTDS = ledger.LedgerId;
                                 _tenantDBContext.Add(ledger);
                                 _tenantDBContext.SaveChanges();*/
                                var builder = new SqlConnectionStringBuilder(TenantDBContext.staticConnectionString);
                                string connectionString = builder.ToString();
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    // Open the connection
                                    connection.Open();

                                    // Create a command for inserting ledger
                                    string insertQuery = @"INSERT INTO Ledgers (CompanyId, LedgerName, AccountingGroupId, CreatedBy, CreatedDate, IsActive) 
                           VALUES (@CompanyId, @LedgerName, @AccountingGroupId, @CreatedBy, @CreatedDate, @IsActive);
                           SELECT SCOPE_IDENTITY();"; // This will return the inserted ledger's ID

                                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                    {
                                        // Set parameters
                                        command.Parameters.AddWithValue("@CompanyId", companyid);
                                        command.Parameters.AddWithValue("@LedgerName", Convert.ToString(LorryData["Transporter"]));
                                        command.Parameters.AddWithValue("@AccountingGroupId", 21);
                                        command.Parameters.AddWithValue("@CreatedBy", 1);
                                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                        command.Parameters.AddWithValue("@IsActive", 1);

                                        // Execute the command and get the inserted ledger's ID
                                        int newLedgerId = Convert.ToInt32(command.ExecuteScalar());

                                        // Assign the new ledger ID
                                        int ledgerId = newLedgerId;

                                        // Use ledgerId as needed
                                        ledgerValTransPorterForTDS = ledgerId;
                                    }

                                    // Close the connection
                                    connection.Close();
                                }

                            }

                            Voucher voucherlstAdvanceFrieght1 = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = ledgerValTransPorterForTDS,
                                CompanyId = companyid,
                                Credit = TDSAmount,
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = data["LedgerName"],
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstAdvanceFrieght1);

                        }






                        if (Convert.ToDecimal(data["BillAmount"]) != 0)
                        {

                            Voucher voucherlstBillAmount = new Voucher
                            {
                                CommodityId = 0,
                                TranctDate = data["OriginalInvDate"],
                                VoucherId = VochType,
                                VoucherNo = invoiceNo,
                                Narration = Convert.ToString(invoiceNo),
                                LedgerId = data["LedgerId"],
                                CompanyId = companyid,
                                Credit = Convert.ToDecimal(data["BillAmount"]),
                                Debit = 0,
                                IsActive = 1,
                                PartyInvoiceNumber = RartyInvoiceNumber,
                                LedgerNameForNarration = ItemNameForVoucherInsert + " Account",
                                CreatedBy = 1
                            };
                            voucherlist.Add(voucherlstBillAmount);
                        }



                        _tenantDBContext.AddRange(voucherlist);
                        _tenantDBContext.SaveChanges();





                    }
                } 
                  
                _logger.LogDebug("salesRepo : InvoiceDetails Added");
                return "Added Successfully...!|||||" + Convert.ToString(invoiceNo) + "|||||" + Convert.ToString(VochType) + "|||||" + Convert.ToString(RartyInvoiceNumber);




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
                var data = JsonConvert.DeserializeObject<dynamic>(Data["SalesInvoice"].ToString());
                int companyid = data["CompanyId"];
                var response = _tenantDBContext.Companies
                    .Where(m => m.CompanyId == companyid && m.EinvoiceReq == 1)
                    .Select(m => new
                    {
                        CompanyName = m.CompanyName,
                        AddressLine1 = m.AddressLine1,
                        AddressLine2 = m.AddressLine2,
                        Place = m.Place,
                        PIN = m.Pin
                    })
                    .FirstOrDefault();
                // Initialize the JObject with default values
                var jsonResponse = new JObject(
                    new JProperty("CompanyName", ""),
                    new JProperty("AddressLine1", ""),
                    new JProperty("AddressLine2", ""),
                    new JProperty("Place", ""),
                    new JProperty("PIN", "")
                );

                // Update the JObject if a valid response is found
                if (response != null)
                {
                    jsonResponse["CompanyName"] = response.CompanyName;
                    jsonResponse["AddressLine1"] = response.AddressLine1;
                    jsonResponse["AddressLine2"] = response.AddressLine2;
                    jsonResponse["Place"] = response.Place;
                    jsonResponse["PIN"] = response.PIN;
                }

                return jsonResponse;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public JObject GetsingleList(JObject Data)
        {
            var response = new JObject();
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["SalesInvoice"].ToString());
                int companyid_Invoice = data["CompanyId"];
                int ledgerid_Invoice = data["LedgerId"];
                int vochNo_Invoice = data["InvoiceNO"];
                int vochtype_Invoice = data["VouchType"];
                //string DisplayInvNo = data["DisplayInvNo"];

                //string invoice_Type = data["InvoiceType"];
                //var LedgerName = _tenantDBContext.Ledgers
                //    .Where(l=>l.LedgerId== ledgerid_Invoice).Select(l=>l.LedgerName).FirstOrDefault();  


                var SingleBill_P = (from bls in _tenantDBContext.BillSummaries.DefaultIfEmpty()
                                    join cpn in _tenantDBContext.Companies.DefaultIfEmpty() on bls.CompanyId equals cpn.CompanyId
                                    join inv in _tenantDBContext.Inventory.DefaultIfEmpty() on cpn.CompanyId equals inv.CompanyId
                                    join VochType in _tenantDBContext.VoucherTypes.DefaultIfEmpty() on inv.VochType equals VochType.VoucherId
                                    join led in _tenantDBContext.Ledgers.DefaultIfEmpty() on bls.LedgerId equals led.LedgerId
                                    join comdty in _tenantDBContext.Commodities.DefaultIfEmpty() on inv.CommodityId equals comdty.CommodityId
                                    where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                    && bls.CompanyId == companyid_Invoice && bls.VochNo == vochNo_Invoice && bls.VochType == vochtype_Invoice && bls.IsActive == 1
                                    select new
                                    {
                                        tcsValue = bls.TCSValue,
                                        LedgerID = bls.LedgerId,
                                        LedgerName = led.LedgerName,
                                        LegalName = led.LegalName,
                                        lineItemQty = inv.TotalWeight,
                                        place = led.Place,
                                        frieghtinBill = bls.FrieghtinBill,
                                        billAmount = bls.BillAmount,
                                        TotalWeight = bls.TotalWeight,
                                        sgstLabel = "Add SGST",
                                        cgstLabel = "Add CGST",
                                        igstLabel = "Add IGST",
                                        frieghtLabel = "Add Frieght",
                                        totalWeightInString = bls.WeightInString,
                                        totalBags = bls.TotalBags,
                                        Address1 = led.Address1,
                                        Address2 = led.Address2,
                                        weightInString = inv.WeightInString,
                                        ledgerCELLNO = led.CellNo,
                                        ledgerfssai = led.Fssai,
                                        ledgerPAN = led.Pan,
                                        VoucherName = VochType.VoucherName,
                                        VoucherTypeID = bls.VochType,
                                        DealerType = led.DealerType,
                                        companyName = cpn.CompanyName,
                                        companyGSTIN = cpn.Gstin,
                                        companyDistrict = cpn.District,
                                        companyPlace = cpn.Place,
                                        bank1 = cpn.Bank1,
                                        jurisLine = cpn.JurisLine,
                                        ifsC1 = cpn.Ifsc1,
                                        accountNo1 = cpn.AccountNo1,
                                        bank2 = cpn.Bank2,
                                        ifsC2 = cpn.Ifsc2,
                                        accountNo2 = cpn.AccountNo2,
                                        bank3 = cpn.Bank3,
                                        ifsC3 = cpn.Ifsc3,
                                        account3 = cpn.Account3,
                                        companyPAN = cpn.Pan,
                                        secondLineForReport = cpn.SecondLineForReport,
                                        thirdLineForReport = cpn.ThirdLineForReport,
                                        companyState = cpn.State,
                                        PAN = led.Pan,
                                        PIN = led.Pin,
                                        GST = led.Gstin,
                                        State = led.State,
                                        CellNo = led.CellNo,
                                        VochNo = bls.VochNo,
                                        TrancDate = Convert.ToString(Convert.ToDateTime(bls.TranctDate).ToString("yyyy-MM-dd")),
                                        TDS = bls.tdsperc,
                                        frieghtPlus = bls.frieghtPlus,
                                        irnNo = bls.IRNNO,
                                        ackno = bls.ACKNO,
                                        displayinvNo = bls.DisplayinvNo,
                                        LorryOwnerName = bls.LorryOwnerName,
                                        ExpenseName1 = bls.ExpenseName1,
                                        ExpenseName2 = bls.ExpenseName2,
                                        ExpenseName3 = bls.ExpenseName3,
                                        ExpenseAmount1 = bls.ExpenseAmount1,
                                        inWords = bls.INWords,
                                        stateCode2 = bls.StateCode2,
                                        ExpenseAmount2 = bls.ExpenseAmount2,
                                        ExpenseAmount3 = bls.ExpenseAmount3,
                                        TaxableValue = bls.TaxableValue,
                                        Discount = bls.Discount,
                                        SGSTValue = bls.SGSTValue,
                                        CSGSTValue = bls.CSGSTValue,
                                        IGSTValue = bls.IGSTValue,
                                        paymentterms = "",
                                        deliveryterms = "",
                                        CGSTRate = inv.CGSTRate,
                                        IGSTRate = inv.IGSTRate,
                                        SGSTRate = inv.SGSTRate,
                                        hsn = comdty.HSN,
                                        mou = comdty.Mou,
                                        IsSEZ = bls.IsSEZ,
                                        RoundOff = bls.RoundOff,
                                        TotalAmount = bls.TotalAmount,
                                        note1 = bls.Note1,
                                        FromPlace = bls.FromPlace,
                                        ToPlace = bls.ToPlace,
                                        Ponumber = bls.Ponumber,
                                        EwayBillNo = bls.EwayBillNo,
                                        Transporter = bls.Transporter,
                                        LorryNo = bls.LorryNo,
                                        DriverName = bls.DriverName,
                                        Dlno = bls.Dlno,
                                        CheckPost = bls.CheckPost,
                                        FrieghtPerBag = bls.FrieghtPerBag,
                                        TotalFrieght = bls.TotalFrieght,
                                        Advance = bls.Advance,
                                        Balance = bls.Balance,
                                        IsLessOrPlus = bls.IsLessOrPlus,
                                        tdsperc = bls.tdsperc,
                                        DeliveryName = bls.DeliveryName,
                                        DeliveryAddress1 = bls.DeliveryAddress1,
                                        DeliveryAddress2 = bls.DeliveryAddress2,
                                        DeliveryPlace = bls.DeliveryPlace,
                                        DelPinCode = bls.DelPinCode,
                                        DeliveryState = bls.DeliveryState,
                                        DeliveryStateCode = bls.DeliveryStateCode,
                                        DeliveryDistance = bls.Distance,
                                        DeliveryNote = bls.Note1
                                    }).FirstOrDefault();


                var Itemresult = (from t1 in _tenantDBContext.Inventory
                                  join t2 in _tenantDBContext.Commodities on t1.CommodityId equals t2.CommodityId
                                  where t1.CompanyId == companyid_Invoice && t1.VochType == vochtype_Invoice && t1.VochNo == vochNo_Invoice && t1.IsActive == 1
                                  select new
                                  {
                                      id = t1 != null ? t1.Id : 0,
                                      CompanyId = t1 != null ? t1.CompanyId : 0,
                                      lineItemQty = t1.TotalWeight??0,
                                      hsn = t2.HSN ?? "",
                                      mou = t2.Mou ?? "",
                                      weightInString = t1.WeightInString ?? "",
                                      VochType = t1 != null ? t1.VochType : 0,
                                      InvoiceType = t1 != null ? t1.InvoiceType : "",
                                      VochNo = t1 != null ? t1.VochNo : 0,
                                      LedgerId = t1 != null ? t1.LedgerId : 0,
                                      CommodityId = t1 != null ? t2.CommodityId : 0,
                                      CommodityName = t1 != null ? t2.CommodityName : "",
                                      TranctDate = t1 != null ? t1.TranctDate : null,
                                      WeightPerBag = t1 != null ? t1.WeightPerBag : 0,
                                      NoOfBags = t1 != null ? t1.NoOfBags : 0,
                                      TotalWeight = t1 != null ? t1.TotalWeight : 0,
                                      Rate = t1 != null ? t1.Rate : 0,
                                      Amount = t1 != null ? t1.Amount : 0,
                                      Mark = t1 != null ? t1.Mark : "",
                                      NetAmount = t1 != null ? t1.NetAmount : 0,
                                      SGST = t1 != null ? t1.SGST : 0,
                                      CGST = t1 != null ? t1.CGST : 0,
                                      IGST = t1 != null ? t1.IGST : 0,
                                      CreatedDate = t1 != null ? t1.CreatedDate : null,
                                      FreeQty = t1 != null ? t1.FreeQty : 0,
                                      IGSTRate = t1 != null ? t1.IGSTRate : 0,
                                      CGSTRate = t1 != null ? t1.CGSTRate : 0,
                                      SGSTRate = t1 != null ? t1.SGSTRate : 0,
                                      Taxable = t1 != null ? t1.Taxable : 0,
                                      IsActive = t1 != null ? t1.IsActive : 0
                                  }).ToList();




                // Create an instance of CombinedData and populate it with values from SingleBill_P and Itemresult
                var combinedDataaa = new
                {
                    Itemresult = Itemresult,
                    LedgerID = SingleBill_P.LedgerID,
                    lineItemQty = SingleBill_P.lineItemQty,
                    bank1 = SingleBill_P.bank1 ?? "",
                    tcsValue = SingleBill_P.tcsValue ?? 0,
                    jurisLine = SingleBill_P.jurisLine ?? "",
                    LedgerName = SingleBill_P.LedgerName ?? "",
                    frieghtinBill = SingleBill_P.frieghtinBill ?? 0,
                    cgstLabel = SingleBill_P.cgstLabel ?? "",
                    sgstLabel = SingleBill_P.sgstLabel ?? "",
                    igstLabel = SingleBill_P.igstLabel ?? "",
                    frieghtLabel = SingleBill_P.frieghtLabel ?? "",
                    ifsC1 = SingleBill_P.ifsC1 ?? "",
                    billAmount = SingleBill_P.billAmount,
                    accountNo1 = SingleBill_P.accountNo1 ?? "",
                    bank2 = SingleBill_P.bank2 ?? "",
                    ifsC2 = SingleBill_P.ifsC2 ?? "",
                    accountNo2 = SingleBill_P.accountNo2 ?? "",
                    bank3 = SingleBill_P.bank3 ?? "",
                    ifsC3 = SingleBill_P.ifsC3 ?? "",
                    stateCode2 = SingleBill_P.stateCode2 ?? "0",
                    account3 = SingleBill_P.account3 ?? "",

                    note1 = SingleBill_P.note1 ?? "",
                    ledgerPAN = SingleBill_P.ledgerPAN ?? "",
                    inWords = SingleBill_P.inWords ?? "",
                    totalBags = SingleBill_P.totalBags ?? 0,
                    companyPAN = SingleBill_P.companyPAN ?? "",
                    totalWeightInString = SingleBill_P.totalWeightInString ?? "",
                    weightInString = SingleBill_P.weightInString ?? "",
                    TotalWeight = SingleBill_P.TotalWeight ?? 0,
                    mou = SingleBill_P.mou ?? "",
                    hsn = SingleBill_P.hsn ?? "",
                    LegalName = SingleBill_P.LegalName ?? "",
                    Address1 = SingleBill_P.Address1 ?? "",
                    Address2 = SingleBill_P.Address2 ?? "",
                    VoucherName = SingleBill_P.VoucherName ?? "",
                    VoucherTypeID = SingleBill_P.VoucherTypeID ?? 0,
                    DealerType = SingleBill_P.DealerType ?? "",
                    PAN = SingleBill_P.PAN ?? "",
                    companyName = SingleBill_P.companyName ?? "",
                    companyGSTIN = SingleBill_P.companyGSTIN ?? "",
                    place = SingleBill_P.place ?? "",
                    PIN = SingleBill_P.PIN ?? "",
                    companyDistrict = SingleBill_P.companyDistrict ?? "",
                    companyPlace = SingleBill_P.companyPlace ?? "",
                    companyState = SingleBill_P.companyState ?? "",
                    GST = SingleBill_P.GST ?? "",
                    ledgerfssai = SingleBill_P.ledgerfssai ?? "",
                    ledgerCELLNO = SingleBill_P.ledgerCELLNO ?? "",
                    State = SingleBill_P.State ?? "",
                    CellNo = SingleBill_P.CellNo ?? "",
                    VochNo = SingleBill_P.VochNo ?? 0,
                    irnNo = SingleBill_P.irnNo ?? "",
                    TrancDate = SingleBill_P.TrancDate ?? "",
                    TDS = SingleBill_P.TDS ?? 0,
                    frieghtPlus = SingleBill_P.frieghtPlus ?? 0,
                    LorryOwnerName = SingleBill_P.LorryOwnerName ?? "",
                    thirdLineForReport = SingleBill_P.thirdLineForReport ?? "",
                    ackno = SingleBill_P.ackno ?? "",
                    ExpenseName1 = SingleBill_P.ExpenseName1 ?? "",
                    ExpenseName2 = SingleBill_P.ExpenseName2 ?? "",
                    ExpenseName3 = SingleBill_P.ExpenseName3 ?? "",
                    ExpenseAmount1 = SingleBill_P.ExpenseAmount1 ?? 0,
                    ExpenseAmount2 = SingleBill_P.ExpenseAmount2 ?? 0,
                    ExpenseAmount3 = SingleBill_P.ExpenseAmount3 ?? 0,
                    TaxableValue = SingleBill_P.TaxableValue ?? 0,
                    Discount = SingleBill_P.Discount ?? 0,
                    SGSTValue = SingleBill_P.SGSTValue ?? 0,
                    CSGSTValue = SingleBill_P.CSGSTValue ?? 0,
                    IGSTValue = SingleBill_P.IGSTValue ?? 0,
                    CGSTRate = SingleBill_P.CGSTRate ?? 0,
                    IGSTRate = SingleBill_P.IGSTRate ?? 0,
                    SGSTRate = SingleBill_P.SGSTRate ?? 0,
                    IsSEZ = SingleBill_P.IsSEZ ?? 0,
                    RoundOff = SingleBill_P.RoundOff ?? 0,
                    displayinvNo = SingleBill_P.displayinvNo ?? "",
                    secondLineForReport = SingleBill_P.secondLineForReport ?? "",
                    TotalAmount = SingleBill_P.TotalAmount ?? 0,
                    FromPlace = SingleBill_P.FromPlace ?? "",
                    ToPlace = SingleBill_P.ToPlace ?? "",
                    Ponumber = SingleBill_P.Ponumber ?? "",
                    EwayBillNo = SingleBill_P.EwayBillNo ?? "",
                    Transporter = SingleBill_P.Transporter ?? "",
                    LorryNo = SingleBill_P.LorryNo ?? "",
                    DriverName = SingleBill_P.DriverName ?? "",
                    Dlno = SingleBill_P.Dlno ?? "",
                    paymentterms = SingleBill_P.paymentterms ?? "",
                    deliveryterms = SingleBill_P.deliveryterms ?? "",
                    CheckPost = SingleBill_P.CheckPost ?? "",
                    FrieghtPerBag = SingleBill_P.FrieghtPerBag ?? 0,
                    TotalFrieght = SingleBill_P.TotalFrieght ?? 0,
                    Advance = SingleBill_P.Advance ?? 0,
                    Balance = SingleBill_P.Balance ?? 0,
                    IsLessOrPlus = SingleBill_P.IsLessOrPlus ?? false,
                    tdsperc = SingleBill_P.tdsperc ?? 0,
                    DeliveryName = SingleBill_P.DeliveryName ?? "",
                    DeliveryAddress1 = SingleBill_P.DeliveryAddress1 ?? "",
                    DeliveryAddress2 = SingleBill_P.DeliveryAddress2 ?? "",
                    DeliveryPlace = SingleBill_P.DeliveryPlace ?? "",
                    DelPinCode = SingleBill_P.DelPinCode ?? "",
                    DeliveryState = SingleBill_P.DeliveryState ?? "",
                    DeliveryStateCode = SingleBill_P.DeliveryStateCode ?? "",
                    DeliveryDistance = SingleBill_P.DeliveryDistance ?? 0,
                    DeliveryNote = SingleBill_P.DeliveryNote ?? "",

                    Id = Itemresult.FirstOrDefault()?.id ?? 0,
                    CompanyId = Itemresult.FirstOrDefault()?.CompanyId ?? 0,
                    VochType = Itemresult.FirstOrDefault()?.VochType ?? 0,
                    InvoiceType = Itemresult.FirstOrDefault()?.InvoiceType ?? "",
                    //VochNo = Itemresult.FirstOrDefault().VochNo ,
                    LedgerId = Itemresult.FirstOrDefault()?.LedgerId ?? 0,
                    CommodityId = Itemresult.FirstOrDefault()?.CommodityId ?? 0,
                    CommodityName = Itemresult.FirstOrDefault()?.CommodityName ?? "",
                    TranctDate = Itemresult.FirstOrDefault()?.TranctDate ?? DateTime.MinValue,
                    WeightPerBag = Itemresult.FirstOrDefault()?.WeightPerBag ?? 0,
                    NoOfBags = Itemresult.FirstOrDefault()?.NoOfBags ?? 0,
                    /*TotalWeight = Itemresult.FirstOrDefault()?.TotalWeight ?? 0,*/
                    Rate = Itemresult.FirstOrDefault()?.Rate ?? 0,
                    Amount = Itemresult.FirstOrDefault()?.Amount ?? 0,
                    Mark = Itemresult.FirstOrDefault()?.Mark ?? "",
                    NetAmount = Itemresult.FirstOrDefault()?.NetAmount ?? 0,
                    SGST = Itemresult.FirstOrDefault()?.SGST ?? 0,
                    CGST = Itemresult.FirstOrDefault()?.CGST ?? 0,
                    IGST = Itemresult.FirstOrDefault()?.IGST ?? 0,
                    CreatedDate = Itemresult.FirstOrDefault()?.CreatedDate ?? DateTime.MinValue,
                    FreeQty = Itemresult.FirstOrDefault()?.FreeQty ?? 0,
                    //IGSTRate = Itemresult.FirstOrDefault()?.IGSTRate ?? 0,
                    //CGSTRate = Itemresult.FirstOrDefault()?.CGSTRate ?? 0,
                    //SGSTRate = Itemresult.FirstOrDefault()?.SGSTRate ?? 0,
                    Taxable = Itemresult.FirstOrDefault()?.Taxable ?? 0,
                    IsActive = Itemresult.FirstOrDefault()?.IsActive ?? 0
                };

  
                var listdata = new List<object>();
                listdata.Add(combinedDataaa);
                // Serialize the combined object
                var serializedData = JsonConvert.SerializeObject(listdata);

                // Add the serialized data to the response
                response.Add("combinedDataaa", serializedData);





                if (SingleBill_P != null && Itemresult != null)
                {
                    response.Add("InvoiceData", JsonConvert.SerializeObject(SingleBill_P));
                }
                else
                {
                    // Handle the case where SingleBill_P is null
                }

                if (Itemresult != null)
                {
                    response.Add("ItemData", JsonConvert.SerializeObject(Itemresult));
                }
                else
                {
                    // Handle the case where Itemresult is null
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
        /// Get by add list by search by LesgerId  filed
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        //public pagination<BillSummary> GetList(JObject Data)
        //{
        //    try
        //    {
        //        var searchText = Convert.ToString(Data["SearchText"]);
        //        var page = JsonConvert.DeserializeObject<pagination<BillSummary>>(Convert.ToString(Data["Page"]));
        //        var data = JsonConvert.DeserializeObject<dynamic>(Data["InvoiceData"].ToString());
        //        int companyId = data["CompanyId"];
        //        string invoiceType = data["InvoiceType"];
        //        IEnumerable<BillSummary> objList1 = null;
        //        var list = new List<BillSummary>();

        //        DAS objDas = new DAS();
        //        DataTable DT = new DataTable();


        //        if (searchText != null && searchText != string.Empty)
        //        {
        //            // string ledgername = searchText;
        //            objList1 = _tenantDBContext.BillSummaries.Where(e => e.LedgerName.Contains(searchText) && e.IsActive == true && e.InvoiceType.Contains(invoiceType)).OrderBy(i => i.OriginalInvDate).ToList();
        //            page.TotalCount = objList1.Count();
        //        }
        //        else
        //        {

        //            DT = objDas.GetDataTable("select distinct led.LedgerName,led.Place,bls.TranctDate,bls.DisplayinvNo,bls.BillAmount,bls.VochNo from BillSummary as bls inner join Company as cpn on bls.CompanyId = cpn.CompanyId inner join Inventory as inv on cpn.CompanyId = inv.CompanyId and bls.VochType = inv.VochType and bls.VochNo = inv.VochNo inner join VoucherTypes as voucher on inv.VochType = voucher.VoucherId  and bls.VochType = voucher.VoucherID inner join Ledger as led on bls.LedgerId = led.LedgerId and inv.LedgerId=led.LedgerId inner join Commodity on Commodity.CommodityId  = inv.CommodityId where bls.CompanyId = 1 and bls.VochType>=9 and bls.VochType<=13 and bls.IsActive = 1  group by led.LedgerName,led.Place,bls.TranctDate,bls.DisplayinvNo,bls.BillAmount,bls.VochNo   order by bls.VochNo desc");


        //            var entryPoint = (from bls in _tenantDBContext.BillSummaries
        //                              join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
        //                              join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
        //                              join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
        //                              join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
        //                              join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
        //                              where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
        //                              && bls.CompanyId == companyId && bls.VochType >= 9 && bls.VochType <= 13 && bls.IsActive == true
        //                              select new
        //                              {
        //                                  LedgerId = led.LedgerId,
        //                                  LedgerName = led.LedgerName,
        //                                  Place = led.Place,
        //                                  TranctDate = bls.TranctDate,
        //                                  DisplayinvNo = bls.DisplayinvNo,
        //                                  BillAmount = bls.BillAmount,
        //                                  VochN = bls.VochNo
        //                              });

        //            foreach (var obj in entryPoint.ToList())
        //            {
        //                var salesInvoice = new BillSummary();

        //                salesInvoice.LedgerId = obj.LedgerId;
        //                salesInvoice.LedgerName = obj.LedgerName;
        //                salesInvoice.Place = obj.Place;
        //                salesInvoice.TranctDate = obj.TranctDate;
        //                salesInvoice.DisplayinvNo = obj.DisplayinvNo;
        //                salesInvoice.BillAmount = obj.BillAmount;
        //                salesInvoice.VochNo = obj.VochN;
        //                list.Add(salesInvoice);
        //            }
        //            page.TotalCount = list.Count;
        //        }


        //        page.Records = list.OrderByDescending(s => s.VochNo).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();
        //        if (!string.IsNullOrEmpty(searchText))
        //        {
        //            page.Records = list.OrderByDescending(s => s.VochNo).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();
        //        }
        //        else
        //        {
        //            page.Records = list.OrderByDescending(s => s.VochNo).Skip((page.PageNumber - 1) * page.PageSize).Take(page.PageSize).ToList();
        //        }
        //        return page;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.StackTrace);
        //        throw ex;
        //    }
        //}

        public pagination<BillSummary> GetList(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var balance = Convert.ToString(Data?["Balance"]);
                var page = JsonConvert.DeserializeObject<pagination<BillSummary>>(Convert.ToString(Data?["Page"]));
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["InvoiceData"]?.ToString());

                if (data == null)
                {
                    // For now, I'll throw a new ArgumentNullException.
                    throw new ArgumentNullException(nameof(data), "InvoiceData is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                string invoiceType = data?["InvoiceType"];
                var list = new List<BillSummary>();

                IQueryable<BillSummary> query = _tenantDBContext.BillSummaries;


                if (!string.IsNullOrEmpty(searchText))
                {
                    query = query
                     .Where(e =>
                         (searchText != null && (e.LedgerName != null && e.LedgerName.Contains(searchText))) &&
                         (e.IsActive.GetValueOrDefault() == 1 && (invoiceType == null || (e.InvoiceType != null && e.InvoiceType.Contains(invoiceType))))
                     );
                }

                else
                {
                    switch (invoiceType)
                    {

                        case "GoodsInvoice":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && (bls.VochType >= 9 && bls.VochType <= 13) && bls.IsActive == 1 && bls.IsServiceInvoice == null || bls.IsServiceInvoice == false
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "BuiltyPurchase":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType >= 2 && bls.VochType <= 4 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "SalesReturn":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 6 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;
                        case "DeemedPurchase":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 5 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "CreditNote":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 8 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;


                        case "OtherGSTBills":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType >= 2 && bls.VochType <= 4 && bls.IsActive == 1 && bls.TotalWeight == 0
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "GinningInvoice":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType >= 9 && bls.VochType <= 13 && bls.IsActive == 1 && bls.IsServiceInvoice == true
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;



                        case "ExportInvoice":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 12 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "PurchaseReturn":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 14 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "DebitNote":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 15 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "ProfarmaInvoice":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 16 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;

                        case "DeemedExport":
                            query = (from bls in _tenantDBContext.BillSummaries
                                     join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                                     join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                                     join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                                     join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                                     join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                                     where bls.VochType == inv.VochType && bls.VochNo == inv.VochNo && bls.VochType == VochType.VoucherId && inv.LedgerId == led.LedgerId
                                     && bls.CompanyId == companyId && bls.VochType == 13 && bls.IsActive == 1
                                     select new BillSummary
                                     {
                                         LedgerId = led.LedgerId,
                                         LedgerName = led.LedgerName,
                                         Place = led.Place,
                                         TranctDate = bls.TranctDate,
                                         DisplayinvNo = bls.DisplayinvNo,
                                         BillAmount = bls.BillAmount,
                                         VochNo = bls.VochNo,
                                         VochType = bls.VochType
                                     });
                            break;


                        default:
                            // Default case or handle unrecognized InvoiceType
                            break;
                    }

                    //query = from bls in query 
                    //        join cpn in _tenantDBContext.Companies on bls.CompanyId equals cpn.CompanyId
                    //        join inv in _tenantDBContext.Inventory on cpn.CompanyId equals inv.CompanyId
                    //        join VochType in _tenantDBContext.VoucherTypes on inv.VochType equals VochType.VoucherId
                    //        join led in _tenantDBContext.Ledgers on bls.LedgerId equals led.LedgerId
                    //        join comdty in _tenantDBContext.Commodities on inv.CommodityId equals comdty.CommodityId
                    //        where (bls.VochType == inv.VochType || (bls.VochType == null && inv.VochType == null)) &&
                    //              (bls.VochNo == inv.VochNo) &&
                    //              (bls.VochType == VochType.VoucherId || (bls.VochType == null && VochType.VoucherId == null)) &&
                    //              (inv.LedgerId == led.LedgerId || (inv.LedgerId == null && led.LedgerId == null)) &&
                    //              (bls.CompanyId == companyId) &&
                    //              (bls.VochType >= 9 && bls.VochType <= 13) &&
                    //              (bls.IsActive == true || bls.IsActive == null)

                    //        select new BillSummary
                    //        {
                    //            LedgerId = led.LedgerId,
                    //            LedgerName = led.LedgerName,
                    //            VochType = inv.VochType,
                    //            VochNo=inv.VochNo,
                    //            Place = led.Place,
                    //            TranctDate = bls.TranctDate,
                    //            DisplayinvNo = bls.DisplayinvNo,
                    //            BillAmount = bls.BillAmount
                    //        };

                }
                if (query.Any())
                {
                    query = query.Distinct();
                }

                page.TotalCount = query.Count();
                page.Records = query.OrderByDescending(s => s.VochNo)
                                   .Skip((page.PageNumber - 1) * page.PageSize)
                                   .Take(page.PageSize)
                                   .ToList();

                return page;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public List<ReportDTO> GetPurchaseRegister(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];

                var list = new List<ReportDTO>();
                list = (
                       from Inv in _tenantDBContext.Inventory
                       join cpn in _tenantDBContext.Companies on Inv.CompanyId equals cpn.CompanyId
                       join VochType in _tenantDBContext.VoucherTypes on Inv.VochType equals VochType.VoucherId
                       join led in _tenantDBContext.Ledgers on Inv.LedgerId equals led.LedgerId
                       join comdty in _tenantDBContext.Commodities on Inv.CommodityId equals comdty.CommodityId
                       where Inv.VochType > 1 &&
                             Inv.VochType < 5 &&
                             Inv.IsActive == 1 &&
                             Inv.CompanyId == companyId &&
                             Inv.TranctDate >= startDate && Inv.TranctDate <= endDate
                       orderby Inv.TranctDate, led.LedgerName
                       group new { Inv, cpn, VochType, led, comdty } by new { Inv.CommodityId, Inv.TranctDate, led.LedgerName, led.Gstin, comdty.CommodityName, Inv.VochType, Inv.VochNo, Inv.PartyInvoiceNumber, VochType.VoucherName } into g
                       select new ReportDTO
                       {
                           //CompanyId = g.First().cpn.CompanyId,
                           //CompanyName = g.First().cpn.CompanyName,
                           //AddressLine1 = g.First().cpn.AddressLine1,
                           //AddressLine2 = g.First().cpn.AddressLine2,
                           //AddressLine3 = g.First().cpn.AddressLine3,
                           //Place = g.First().cpn.Place,
                           GSTIN = g.First().cpn.Gstin,
                           //VochType = g.Key.VochType,
                           //VochNo = g.Key.VochNo,
                           TranctDate = g.Key.TranctDate,
                           NoOfBags = g.Sum(x => x.Inv.NoOfBags),
                           TotalWeight = g.Sum(x => x.Inv.TotalWeight),
                           Amount = g.Sum(x => x.Inv.NetAmount),
                           PartyInvoiceNumber = g.Key.PartyInvoiceNumber,
                           SGST = g.Sum(x => x.Inv.SGST),
                           CGST = g.Sum(x => x.Inv.CGST),
                           IGST = g.Sum(x => x.Inv.IGST),
                           //Taxable = g.Sum(x => x.Inv.SGST), // Ensure you are using the correct field for Taxable
                           //CommodityId = g.Key.CommodityId,
                           //CommodityName = g.Key.CommodityName,
                           //HSN = g.First().comdty.HSN,
                           LedgerName = g.Key.LedgerName,
                           //Ledger_Place = g.First().led.Place,
                           //Ledger_GSTIN = g.First().led.Gstin,
                           //VoucherId = g.First().VochType.VoucherId,
                           //VoucherName = g.First().VochType.VoucherName,
                       })
                       .ToList();

                var totalRow = new ReportDTO
                {
                    LedgerName = "Total",
                    NoOfBags = list.Sum(x => x.NoOfBags),
                    TotalWeight = list.Sum(x => x.TotalWeight),
                    Amount = list.Sum(x => x.Amount),
                    SGST = list.Sum(x => x.SGST),
                    CGST = list.Sum(x => x.CGST),
                    IGST = list.Sum(x => x.IGST),
                    //Taxable = list.Sum(x => x.Taxable),
                    //VochType = list.Sum(x => x.VochType),
                    //CommodityId = list.Sum(x => x.CommodityId)
                };
                list.Add(totalRow);
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        public List<ReportDTO> GetMonthwisePurchase(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];
                var result = (from bls in _tenantDBContext.BillSummaries
                              join led in _tenantDBContext.Ledgers on new { bls.CompanyId, bls.LedgerId } equals new { led.CompanyId, led.LedgerId }
                              group bls by bls.monthNo into g
                              orderby g.Key
                              select new ReportDTO
                              {
                                  MonthNo = g.Key.HasValue ? g.Key.Value + "" : "Unknown",
                                  BasicValue = g.Sum(x => x.TotalAmount),
                                  Taxable = g.Sum(x => x.TaxableValue),
                                  SGST = g.Sum(x => x.SGSTValue),
                                  CGST = g.Sum(x => x.CSGSTValue),
                                  IGST = g.Sum(x => x.IGSTValue),
                                  Others = g.Sum(x => x.RoundOff + x.ExpenseAmount1 + x.ExpenseAmount2 + x.ExpenseAmount3 + x.Advance),
                                  BillAmount = g.Sum(x => x.BillAmount)
                              }).ToList();

                // Calculate the total for each column
                var totalRow = new ReportDTO
                {
                    MonthNo = "Total",
                    BasicValue = result.Sum(x => x.BasicValue),
                    Taxable = result.Sum(x => x.Taxable),
                    SGST = result.Sum(x => x.SGST),
                    CGST = result.Sum(x => x.CGST),
                    IGST = result.Sum(x => x.IGST),
                    Others = result.Sum(x => x.Others),
                    BillAmount = result.Sum(x => x.BillAmount)
                };

                // Add the total row to the end of the list
                result.Add(totalRow);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public List<ReportDTO> GetTrialBalance(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];
                //var trialBalanceData = (
                //     from led in _tenantDBContext.Ledgers
                //     join ag in _tenantDBContext.AccounitngGroups on led.AccountingGroupId equals ag.AccontingGroupId
                //     join tr in _tenantDBContext.TmpTrialBalances
                //     on new { CompanyId = (int)led.CompanyId.Value, LedgerId = (int)led.LedgerId.Value }
                //     equals new { tr.CompanyId, tr.LedgerId }
                //     select new { led, ag, tr }
                // ).ToList();

                var trialBalanceQuery = (
                        from led in _tenantDBContext.Ledgers
                        join ag in _tenantDBContext.AccounitngGroups on led.AccountingGroupId equals ag.AccontingGroupId
                        join tr in _tenantDBContext.TmpTrialBalances
                            on new { CompanyId = (int)led.CompanyId, LedgerId = (int)led.LedgerId }
                            equals new { CompanyId = tr.CompanyId, LedgerId = tr.LedgerId }
                        group new { led, ag, tr } by new { ag.GroupName, led.AccountingGroupId, led.LedgerName, led.Place } into grouped
                        orderby grouped.Key.LedgerName
                        select new ReportDTO
                        {
                            GroupName = grouped.Key.GroupName,
                            AccountingGroupId = grouped.Key.AccountingGroupId,
                            LedgerName = grouped.Key.LedgerName,
                            Place = grouped.Key.Place,
                            //OpeningBalance = grouped.Sum(x => x.OpeningBalance),
                            Credit = grouped.Sum(x => x.tr.Credit),
                            Debit = grouped.Sum(x => x.tr.Debit)
                        })
                        .ToList();

                var totalRow = new ReportDTO
                {
                    GroupName = "Total",
                    //OpeningBalance = trialBalanceQuery.Sum(x => x.OpeningBalance),
                    Credit = trialBalanceQuery.Sum(x => x.Credit),
                    Debit = trialBalanceQuery.Sum(x => x.Debit)
                };

                trialBalanceQuery.Add(totalRow);
                return trialBalanceQuery;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public class TDSReportEntry
        {
            public long LedgerId { get; set; }
            public string LedgerName { get; set; }
            public string Place { get; set; }
            public decimal TotalCommission { get; set; }
            public decimal TDSBalance { get; set; }
        }

        public pagination<TDSReportEntry> GetTDSReport(JObject Data)
        {
            try
            { 
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];

                var ledgerQuery = (from led in _tenantDBContext.Ledgers
                                   where led.CompanyId == companyId && led.AccountingGroupId == 21
                                   orderby led.LedgerName
                                   select new
                                   {
                                       led.LedgerId,
                                       led.LedgerName,
                                       led.Place,
                                       TotalCommission = 0m,
                                       TDSBalance = 0m
                                   }).ToList();
                 
                string dateString = "2023-02-25";
                DateTime dateTime = DateTime.Today;
                //DateTime dateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                _tenantDBContext.Database.SetCommandTimeout(220);
                var ledgerInfoList = _tenantDBContext.Ledgers
                    .Where(led => led.CompanyId == companyId && led.AccountingGroupId == 21)
                    .OrderBy(led => led.LedgerName)
                    .Select(led => new
                    {
                        led.LedgerId,
                        led.LedgerName,
                        led.Place,
                        AsOnDateCredit = _tenantDBContext.Vouchers
                            .Where(v => v.LedgerId == led.LedgerId && v.CompanyId == companyId && v.TranctDate <= dateTime)
                            .Sum(v => (decimal?)v.Credit) ?? 0,
                        TotalDebit = _tenantDBContext.Vouchers
                            .Where(v => v.LedgerId == led.LedgerId && v.CompanyId == companyId && v.Tdstype == 1)
                            .Sum(v => (decimal?)v.Debit) ?? 0
                    })
                    .Where(ledger => (ledger.AsOnDateCredit - ledger.TotalDebit) > 0)
                    .Select(ledger => new TDSReportEntry
                    {
                        LedgerId = ledger.LedgerId ?? 0,
                        LedgerName = ledger.LedgerName,
                        Place = ledger.Place,
                        TotalCommission = _tenantDBContext.BillSummaries
                            .Where(bill => bill.LedgerId == ledger.LedgerId && bill.CompanyId == companyId && bill.TranctDate <= dateTime)
                            .Sum(bill => (decimal?)bill.DalaliValue) ?? 0,
                        TDSBalance = Math.Round((((_tenantDBContext.BillSummaries
                            .Where(bill => bill.LedgerId == ledger.LedgerId && bill.CompanyId == companyId && bill.TranctDate <= dateTime)
                            .Sum(bill => (decimal?)bill.DalaliValue) ?? 0) * 5) / 100) -
                            (_tenantDBContext.Vouchers
                            .Where(voucher => voucher.LedgerId == ledger.LedgerId && voucher.CompanyId == companyId && voucher.Tdstype == 1)
                            .Sum(voucher => (decimal?)voucher.Debit) ?? 0), 2)
                    }) 
                    .Where(n => n.TDSBalance > 0 && n.TotalCommission > 0)
                    .ToList();

                var page = new pagination<TDSReportEntry>
                {
                    TotalCount = ledgerInfoList.Count(),
                    Records = ledgerInfoList
                };

                return page;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        public pagination<LedgerInfo> GetPaymentList(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var balance = Convert.ToDecimal(Data?["Balance"]);
                var Date = Convert.ToString(Data?["Date"]);

                var pageNumber = Convert.ToInt32(Data?["Page"]?["PageNumber"]);
                var pageSize = Convert.ToInt32(Data?["Page"]?["PageSize"]);

                if (Convert.ToString(Data?["Balance"]) == null || Convert.ToString(Data?["Balance"]).Equals(""))
                {
                    balance = -1;
                }

                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];



                var ledgerQuery = from led in _tenantDBContext.Ledgers
                                  where led.CompanyId == companyId && led.AccountingGroupId == 21
                                  orderby led.LedgerName
                                  select new
                                  {
                                      led.LedgerId,
                                      led.LedgerName,
                                      led.Place,
                                      TotalBalance = 0m,
                                      AsOnDateBalance = 0m,
                                      Sno = 0
                                  };


                string dateString = Date;
                DateTime dateTime = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                _tenantDBContext.Database.SetCommandTimeout(220);

                int skipCount = (pageNumber - 1) * pageSize;

                var ledgerInfoList = _tenantDBContext.Ledgers
                                    .Where(led => led.CompanyId == companyId && led.AccountingGroupId == 21)
                                    .OrderBy(led => led.LedgerName)
                                    .Select(led => new
                                    {
                                        led.LedgerId,
                                        led.LedgerName,
                                        led.Place,
                                        AsOnDateCredit = _tenantDBContext.Vouchers
                                            .Where(v => v.LedgerId == led.LedgerId && v.CompanyId == companyId && v.TranctDate <= dateTime)
                                            .Sum(v => (decimal?)v.Credit) ?? 0,
                                        TotalDebit = _tenantDBContext.Vouchers
                                            .Where(v => v.LedgerId == led.LedgerId && v.CompanyId == companyId)
                                            .Sum(v => (decimal?)v.Debit) ?? 0
                                    })
                                    .Where(ledger => (ledger.AsOnDateCredit - ledger.TotalDebit) > 1000)
                                    .Select(ledger => new LedgerInfo
                                    {
                                        LedgerId = Convert.ToInt32(ledger.LedgerId),
                                        LedgerName = ledger.LedgerName,
                                        Place = ledger.Place,
                                        TotalBalance = _tenantDBContext.Vouchers
                                            .Where(v => v.LedgerId == ledger.LedgerId && v.CompanyId == companyId)
                                            .Sum(v => (decimal?)v.Credit - v.Debit) ?? 0,
                                        AsOnDateBalance = ledger.AsOnDateCredit - ledger.TotalDebit
                                    })
                                    .Skip(skipCount)
                                    .Take(pageSize)
                                    .ToList();

                if (!string.IsNullOrEmpty(searchText) || balance != null || !string.IsNullOrEmpty(Date))
                {

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        ledgerInfoList = ledgerInfoList.Where(n => n.Place.Contains(searchText) || n.LedgerName.Contains(searchText)).ToList();
                    }

                    if (balance != null && balance > -1)
                    {
                        ledgerInfoList = ledgerInfoList.Where(n => n.AsOnDateBalance > balance).ToList();
                    }

                    if (!string.IsNullOrEmpty(Date))
                    {
                        ledgerInfoList = ledgerInfoList.Where(n => n.TranctDate <= DateTime.Parse(Date)).ToList();
                    }

                }

                var result1 = ledgerInfoList.ToList();

                var page = new pagination<LedgerInfo>();
                page.TotalCount = ledgerQuery.Count();
                page.Records = ledgerInfoList.ToList();

                return page;

                //return result1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private decimal CalculateAsOnDateBalance(int companyId,int ledgerId, DateTime? tranctDate)
        {
            var asOnDateCredit = _tenantDBContext.Vouchers
                .Where(v => v.LedgerId == ledgerId && v.CompanyId == companyId && v.TranctDate <= tranctDate)
                .Sum(v => (decimal?)v.Credit) ?? 0;

            var totalDebit = _tenantDBContext.Vouchers
                .Where(v => v.LedgerId == ledgerId && v.CompanyId == companyId)
                .Sum(v => (decimal?)v.Debit) ?? 0;

            return asOnDateCredit - totalDebit;
        }

        private decimal CalculateTotalBalance(int companyId, int ledgerId)
        {
            var totalCredit = _tenantDBContext.Vouchers
                .Where(v => v.LedgerId == ledgerId && v.CompanyId == companyId)
                .Sum(v => (decimal?)v.Credit) ?? 0;

            var totalDebit = _tenantDBContext.Vouchers
                .Where(v => v.LedgerId == ledgerId && v.CompanyId == companyId)
                .Sum(v => (decimal?)v.Debit) ?? 0;

            return totalCredit - totalDebit;
        }

        public class LedgerInfo
        {
            public int LedgerId { get; set; } 
            public string LedgerName { get; set; }
            public string Place { get; set; }
            public decimal? AsOnDateBalance { get; set; }
            public decimal? TotalBalance { get; set; }
            public DateTime TranctDate { get; set; }
        }

        public List<TmpStockLedger> GetStockLedger(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];
                var query = from stockLedger in _tenantDBContext.TmpStockLedgers
                            where stockLedger.CompanyId == companyId
                            select stockLedger;
                var result = query.ToList();

                // Calculate the total for each column
                var totalRow = new TmpStockLedger
                {
                    Obstock = result.Sum(x => x.Obstock),
                    Obvalue = result.Sum(x => x.Obvalue),
                    PurchaseQty = result.Sum(x => x.PurchaseQty),
                    PurchaseValue = result.Sum(x => x.PurchaseValue),
                    SalesReturnQty = result.Sum(x => x.SalesReturnQty),
                    SalesReturnValue = result.Sum(x => x.SalesReturnValue),
                    FromProductionQty = result.Sum(x => x.FromProductionQty),
                    FromProductionValue = result.Sum(x => x.FromProductionValue),
                    OnwSalesQty = result.Sum(x => x.OnwSalesQty),
                    OnwSalesValue = result.Sum(x => x.OnwSalesValue),
                    ToProduction = result.Sum(x => x.ToProduction),
                    ClosingStock = result.Sum(x => x.ClosingStock),
                    Average = result.Sum(x => x.Average),
                    ClosingValue = result.Sum(x => x.ClosingValue)
                };
                result.Add(totalRow);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public List<ReportDTO> GetPartywiseTDS(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];

                var query = from company in _tenantDBContext.Companies
                            join ledger in _tenantDBContext.Ledgers on company.CompanyId equals ledger.CompanyId
                            select new ReportDTO
                            {
                                CompanyName = company.CompanyName,
                                AddressLine1 = company.AddressLine1,
                                Place = company.Place,
                                GSTIN = company.Gstin,
                                PAN = company.Pan,
                                LedgerName = ledger.LedgerName,
                                Ledger_GSTIN = ledger.Gstin,
                                Ledger_PAN = ledger.Pan,
                                TotalCommission = ledger.TotalCommission,
                                TDSDeducted = ledger.Tdsdeducted,
                                CommissionTDS = ledger.TotalCommission
                            };


                var totalRow = new ReportDTO
                {
                    CompanyName = "Total",
                    TotalCommission = query.Sum(x => x.TotalCommission),
                    TDSDeducted = query.Sum(x => x.TDSDeducted),
                    CommissionTDS = query.Sum(x => x.CommissionTDS)
                };

                var result = query.ToList();
                result.Add(totalRow);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public List<ReportDTO> GetTransactinSummary(JObject Data)
        {
            try
            {
                var searchText = Convert.ToString(Data?["SearchText"]);
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];

                var query = from led in _tenantDBContext.Ledgers
                            join ag in _tenantDBContext.AccounitngGroups on led.AccountingGroupId equals ag.AccontingGroupId
                            join tr in _tenantDBContext.TmpTrialBalances on led.LedgerId equals tr.LedgerId
                            select new ReportDTO
                            {
                                GroupName = ag.GroupName,
                                AccountingGroupId = led.AccountingGroupId,
                                LedgerName = led.LedgerName,
                                AddressLine1 = led.Address1,
                                Place = led.Place,
                                Gstn = led.Gstn,
                                PAN = led.Pan,
                                Credit = tr.Credit,
                                Debit = tr.Debit
                            };
                var result = query.ToList();



                var totalRow = new ReportDTO
                {
                    LedgerName = "Total",
                    Debit = query.Sum(x => x.Debit),
                    Credit = query.Sum(x => x.Credit)
                };

                result.Add(totalRow);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public bool Update(JObject Data) //TODO Panding for UI logic as per required chenges
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(Data["InvoiceData"].ToString());
                var LorryData = JsonConvert.DeserializeObject<dynamic>(Data["LorryDetails"].ToString());
                var itemdata = JsonConvert.DeserializeObject<dynamic>(Data["ItemData"].ToString());
                string Inword = data["TotalAmount"];
                if (data != null)
                {

                    int id = data["_Id"];
                    var entity = _tenantDBContext.BillSummaries.FirstOrDefault(item => item._Id == id);
                    if (entity != null)
                    {
                        entity.CompanyId = data["CompanyId"];
                        entity.LedgerId = data["LedgerId"];
                        entity.OriginalInvDate = data["OriginalInvDate"];
                        entity.VochType = data["VochType"];
                        entity.DealerType = data["DealerType"];
                        entity.PAN = data["PAN"];
                        entity.GST = data["GST"];
                        entity.InvoiceType = data["InvoiceType"];
                        entity.VochNo = data["InoviceNo"];
                        entity.State = data["State"];
                        //other charges block
                        entity.ExpenseName1 = data["ExpenseName1"];
                        entity.ExpenseName2 = data["ExpenseName2"];
                        entity.ExpenseName3 = data["ExpenseName3"];
                        entity.ExpenseAmount1 = data["ExpenseAmount1"];
                        entity.ExpenseAmount2 = data["ExpenseAmount2"];
                        entity.ExpenseAmount3 = data["ExpenseAmount3"];
                        //paymentDetails
                        entity.TaxableValue = data["TaxableValue"];
                        entity.Discount = data["Discount"];
                        entity.SGSTValue = data["Sgstvalue"];
                        entity.CSGSTValue = data["Csgstvalue"];
                        entity.IGSTValue = data["Igstvalue"];
                        entity.IsSEZ = data["IsSEZ"];
                        entity.RoundOff = data["RoundOff"];
                        entity.TotalAmount = Int32.Parse(Inword);
                        entity.INWords = AmountToWord(Convert.ToDouble(Inword));
                        //entity.INWords = TODO : amount in words
                        //address block 
                        entity.DispatcherAddress1 = data["Address"];
                        entity.FromPlace = data["FromPlace"];
                        entity.ToPlace = data["ToPlace"];
                        //Lorray details
                        entity.Ponumber = LorryData["Ponumber"];
                        entity.EwayBillNo = LorryData["EwayBillNo"];
                        entity.Transporter = LorryData["Transporter"];
                        entity.LorryNo = LorryData["LorryNo"];
                        entity.LorryOwnerName = LorryData["Owner"];
                        entity.DriverName = LorryData["Driver"];
                        entity.Dlno = LorryData["Dlno"];
                        entity.CheckPost = LorryData["CheckPost"];
                        entity.FrieghtPerBag = LorryData["FrieghtPerBag"];
                        entity.TotalFrieght = LorryData["TotalFrieght"];
                        entity.Advance = LorryData["AdvanceFrieght"];
                        entity.Balance = LorryData["BalanceFrieght"];
                        entity.IsLessOrPlus = LorryData["FrieghtPlus_Less"];//FrieghtPlus/Less field
                        entity.tdsperc = LorryData["TDS"];
                        //  Dilivary Address Details                
                        entity.DeliveryName = LorryData["DeliveryName"];
                        entity.DeliveryAddress1 = LorryData["DeliveryAddress1"];
                        entity.DeliveryAddress2 = LorryData["DeliveryAddress2"];
                        entity.DeliveryPlace = LorryData["DeliveryPlace"];
                        entity.DelPinCode = LorryData["DelPinCode"];
                        entity.DeliveryState = LorryData["DeliveryState"];
                        entity.DeliveryStateCode = LorryData["DeliveryStateCode"];
                        entity.Distance = LorryData["Distance"];
                        entity.DCNote = LorryData["Dcnote"];
                        entity.IsActive = 1;
                    }
                    ///List<Inventory> inventorylist = new List<Inventory>();
                    _tenantDBContext.Update(entity);
                    _tenantDBContext.SaveChanges();

                    foreach (var item in itemdata)
                    {
                        //var Itementity = _tenantDBContext.Inventory.FirstOrDefault(Inv => Inv.CompanyId == entity.CompanyId && Inv.VochType == entity.VochType && Inv.InvoiceType == entity.InvoiceType && Inv.VochNo == entity.VochNo && Inv.LedgerId == entity.LedgerId && Inv.CommodityId == entity.CommodityID);
                        //var Itementity = _tenantDBContext.Inventory.FirstOrDefault(Inv => Inv.CompanyId == entity.CompanyId && Inv.VochType == entity.VochType && Inv.InvoiceType == entity.InvoiceType && Inv.VochNo == entity.VochNo && Inv.LedgerId == entity.LedgerId && Inv._Id == item._Id);
                        int _Id = Convert.ToInt32(item._Id);
                        var Itementity = _tenantDBContext.Inventory.FirstOrDefault(Inv => Inv._Id == _Id);
                        if (Itementity != null)
                        {

                            Itementity.CompanyId = data["CompanyId"];
                            Itementity.VochNo = data["InoviceNo"];
                            Itementity.VochType = data["VochType"];
                            Itementity.InvoiceType = data["InvoiceType"];
                            Itementity.LedgerId = data["LedgerId"];
                            Itementity.CommodityId = item["CommodityId"];
                            Itementity.TranctDate = data["OriginalInvDate"];
                            Itementity.PoNumber = LorryData["Ponumber"];
                            Itementity.EwaybillNo = LorryData["EwayBillNo"];
                            //LotNo = data["LotNo"];
                            Itementity.WeightPerBag = item["WeightPerBag"];
                            Itementity.NoOfBags = item["NoOfBags"];
                            Itementity.TotalWeight = item["TotalWeight"];
                            Itementity.Rate = item["Rate"];
                            Itementity.Amount = item["Amount"];
                            Itementity.Mark = item["Remark"];
                            Itementity.Discount = LorryData["Discount"];
                            Itementity.NetAmount = item["GrandTotal"];
                            Itementity.SGST = item["SgstAmount"];
                            Itementity.CGST = item["CsgstAmount"];
                            Itementity.IGST = item["IgstAmount"];
                            Itementity.NoOfDocra = item["NoOfDocra"];
                            Itementity.UpdatedDate = DateTime.Now;
                            Itementity.UpdatedBy = item["UpdatedBy"];
                            Itementity.IGSTRate = data["Igstvalue"];
                            Itementity.SGSTRate = data["Sgstvalue"];
                            Itementity.CGSTRate = data["Csgstvalue"];
                            Itementity.Taxable = data["TaxableValue"];
                            Itementity.IsActive = 1;
                            _tenantDBContext.UpdateRange(Itementity);
                            _tenantDBContext.SaveChanges();
                        };
                    }
                    _logger.LogDebug("salesRepo : BillSummry/Inventory Updated");
                    return true;
                }
                else
                {
                    _logger.LogDebug("salesRepo : BillSummry/Inventory Updated Failed");
                    return false;
                }
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

                if (data != null)
                {
                    using (var context = new MasterDBContext())
                    {
                        var activeentity = _tenantDBContext.BillSummaries.SingleOrDefault(item => item._Id == id && item.IsActive == 1);
                        var deactivateentity = _tenantDBContext.BillSummaries.SingleOrDefault(item => item._Id == id && item.IsActive == 0);
                        if (activeentity != null)
                        {
                            activeentity.IsActive = 0;
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(activeentity);
                            _logger.LogDebug("SalesReo : sales Deactivated");
                        }
                        else if (deactivateentity != null)
                        {
                            deactivateentity.IsActive = 1;
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(deactivateentity);
                            _logger.LogDebug("SalesRepo : sales Activated");
                        }
                    }
                }
                return true;
                _logger.LogDebug("Delete sales done : ");
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
                var response = new JObject();
                try
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(Data.ToString());
                    int companyId = int.Parse(data.CompanyId.ToString());
                    string searchText = data.SearchText.ToString().ToLower();
                    string type = data.Type.ToString();

                    // Assuming _context is your DbContext
                    var billSummaries = _tenantDBContext.BillSummaries
                        .Where(b => b.CompanyId == companyId);

                    switch (type)
                    {
                        case "Transporter":
                            billSummaries = billSummaries.Where(b => !string.IsNullOrEmpty(b.Transporter) && b.Transporter.ToLower().Contains(searchText));
                            break;
                        case "LorryNo":
                            billSummaries = billSummaries.Where(b => !string.IsNullOrEmpty(b.LorryNo) && b.LorryNo.ToLower().Contains(searchText));
                            break;
                        case "Owner":
                            billSummaries = billSummaries.Where(b => !string.IsNullOrEmpty(b.LorryOwnerName) && b.LorryOwnerName.ToLower().Contains(searchText));
                            break;
                        case "Driver":
                            billSummaries = billSummaries.Where(b => !string.IsNullOrEmpty(b.DriverName) && b.DriverName.ToLower().Contains(searchText));
                            break;
                        default:
                            throw new Exception("Invalid type");
                    }

                    var results = billSummaries.Select(b => new
                    {
                        Transporter = b.Transporter,
                        LorryNo = b.LorryNo,
                        Owner = b.LorryOwnerName,
                        Driver = b.DriverName
                    }).ToList();

                    response.Add("BillSummaries", JArray.FromObject(results));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);
                    throw ex;
                }

                return response;
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
                var response = new JObject();
                try
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(Data.ToString());
                    int companyId = int.Parse(data.CompanyId.ToString());
                    string searchText = data.SearchText.ToString().ToLower();
                    string type = data.Type.ToString();

                    // Assuming _context is your DbContext
                    var partyDetails = _tenantDBContext.Ledgers
                        .Where(b => b.CompanyId == companyId);

                    switch (type)
                    {
                        case "Address1":
                            partyDetails = partyDetails.Where(b => !string.IsNullOrEmpty(b.Address1) && b.Address1.ToLower().Contains(searchText));
                            break;
                        case "Address2":
                            partyDetails = partyDetails.Where(b => !string.IsNullOrEmpty(b.Address2) && b.Address2.ToLower().Contains(searchText));
                            break;
                        case "Place":
                            partyDetails = partyDetails.Where(b => !string.IsNullOrEmpty(b.Place) && b.Place.ToLower().Contains(searchText));
                            break;
                        case "BankName":
                            partyDetails = partyDetails.Where(b => !string.IsNullOrEmpty(b.BankName) && b.BankName.ToLower().Contains(searchText));
                            break;
                        default:
                            throw new Exception("Invalid type");
                    }

                    var results = partyDetails.Select(b => new
                    {
                        Address1 = b.Address1,
                        Address2 = b.Address2,
                        Place = b.Place,
                        BankName = b.BankName
                    }).ToList();

                    response.Add("AddPartyAutoComplete", JArray.FromObject(results));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.StackTrace);
                    throw ex;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }


        public string SaveDC(JObject data)
        {

            try
            {
                if (data.ContainsKey("InvoiceNumber") && data["InvoiceNumber"].ToString() != "")
                {
                    var lineItem = new DclineItem
                    {
                        Bno = data.Value<long>("BNo"),
                        CompanyId = data.Value<long>("CompanyId"),
                        InvoiceNumber = data.Value<string?>("InvoiceNumber"),
                        Date = data.Value<DateTime>("Date"),
                        MethodOfPacking = data.Value<string?>("MethodOfPacking"),
                        Description = data.Value<string?>("Description"),
                        PrivateMark = data.Value<string?>("PrivateMark"),
                        ActualWeightKg = data.Value<decimal>("ActualWeightKg"),
                        ChangedWeightKg = data.Value<decimal>("ChangedWeightKg"),
                        Total = data.Value<decimal>("Total"),
                        Freight = data.Value<decimal>("Freight")

                    };

                    _tenantDBContext.DclineItems.Add(lineItem);
                    _tenantDBContext.SaveChanges();

                }

                var lineItemsToDelete = _tenantDBContext.DclineItems
                    .Where(item => item.Bno == data.Value<long>("BNo") && item.CompanyId == data.Value<long>("CompanyId"))
                    .ToList();
                foreach (var lineItem in lineItemsToDelete)
                {
                    _tenantDBContext.DclineItems.Remove(lineItem);
                }
                _tenantDBContext.SaveChanges();

                var summariesToDelete = _tenantDBContext.Dcsummaries
                    .Where(item => item.Bno == data.Value<long>("BNo") && item.CompanyId == data.Value<long>("CompanyId"))
                    .ToList();
                foreach (var sum in summariesToDelete)
                {
                    _tenantDBContext.Dcsummaries.Remove(sum);
                }
                _tenantDBContext.SaveChanges();

                var lineItems = _tenantDBContext.DclineItems
                    .Where(item => item.Bno == data.Value<long>("BNo") && item.CompanyId == data.Value<long>("CompanyId"))
                    .ToList();

                var summary = new Dcsummary
                {
                    Bno = data.Value<int?>("BNo"),
                    CompanyId = data.Value<long>("CompanyId"),
                    Date = data.Value<DateTime>("Date"),
                    LorryNumber = data.Value<string>("LorryNumber"),
                    SenderDetails = data.Value<string>("SenderDetails"),
                    ReceiverDetails = data.Value<string>("ReceiverDetails"),
                    DeliveryDetails = data.Value<string>("DeliveryDetails"),
                    DeliveryAddress = data.Value<string>("DeliveryAddress"),
                    ConsignorsName = data.Value<string>("ConsignorsName"),
                    ConsignorsAddress = data.Value<long>("ConsignorsAddress"),
                    ConsigneesName = data.Value<string>("ConsigneesName"),
                    ConsigneesAddress = data.Value<string>("ConsigneesAddress"),
                    FromLocation = data.Value<string>("FromLocation"),
                    TotalFrieght = data.Value<decimal>("TotalFrieght"),
                    Advance = data.Value<decimal>("Advance"),
                    Balance = data.Value<decimal>("Balance"),
                    ToLocation = data.Value<string>("ToLocation")
                };

                _tenantDBContext.Dcsummaries.Add(summary);
                _tenantDBContext.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        public int? GetBNo(JObject data)
        {
            try
            {
                if (data.ContainsKey("CompanyId") && data["CompanyId"].ToString() != "")
                {
                    var CompanyId = data.Value<long>("CompanyId");
                    var res = _tenantDBContext.Dcsummaries.Where(m => m.CompanyId == CompanyId).Max(m => m.Bno);
                    if (res == null)
                        return 1;
                    else
                        return res + 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public bool SaveExcelData(List<ExcelDataWrapper> dataList)
        {
            try
            {
                string connectionString = "Data Source=103.50.212.163;Initial Catalog=K2122;User ID=sa;Password=root@123;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (var data in dataList)
                    {
                        if (data.Ledger != null && data.Ledger.CompanyId > 0)
                        {
                            // Save Ledger data
                            using (SqlCommand ledgerCommand = new SqlCommand(
                                @"INSERT INTO Ledgers (CompanyId, LedgerId, LedgerName, GSTIN, Address1, Address2, EmailId, CellNo, Place, Pin)
                        VALUES (@CompanyId, @LedgerId, @LedgerName, @GSTIN, @Address1, @Address2, @EmailId, @CellNo, @Place, @Pin)", connection))
                            {
                                ledgerCommand.Parameters.AddWithValue("@CompanyId", data.Ledger.CompanyId);
                                ledgerCommand.Parameters.AddWithValue("@LedgerId", data.Ledger.LedgerId);
                                // Add other parameters for Ledger properties
                                ledgerCommand.ExecuteNonQuery();
                            }

                            // Save BillSummary data
                            using (SqlCommand billSummaryCommand = new SqlCommand(
                                @"INSERT INTO BillSummaries (CompanyId, LedgerId, VochType, VochNo, DisplayinvNo, TranctDate, Ponumber, StateCode2, CessValue, CSGSTValue, IGSTValue, SGSTValue, BillAmount, TaxableValue, ExpenseAmount1, ExpenseAmount2, ExpenseAmount3, TCSValue, Advance, TotalAmount, RoundOff, IsSEZ, ShipBillNo, PortName, ShipBillDate, DispatcherName, DispatcherAddress1, DispatcherAddress2, DispatcherPlace, DispatcherPIN, DispatcherStatecode, StateCode1, Transporter, Distance, LorryNo, DeliveryName, DeliveryAddress1, DeliveryAddress2, DeliveryPlace, DelPinCode, TotalWeight, PartyInvoiceNumber, DeliveryStateCode)
                        VALUES (@CompanyId, @LedgerId, @VochType, @VochNo, @DisplayinvNo, @TranctDate, @Ponumber, @StateCode2, @CessValue, @CSGSTValue, @IGSTValue, @SGSTValue, @BillAmount, @TaxableValue, @ExpenseAmount1, @ExpenseAmount2, @ExpenseAmount3, @TCSValue, @Advance, @TotalAmount, @RoundOff, @IsSEZ, @ShipBillNo, @PortName, @ShipBillDate, @DispatcherName, @DispatcherAddress1, @DispatcherAddress2, @DispatcherPlace, @DispatcherPIN, @DispatcherStatecode, @StateCode1, @Transporter, @Distance, @LorryNo, @DeliveryName, @DeliveryAddress1, @DeliveryAddress2, @DeliveryPlace, @DelPinCode, @TotalWeight, @PartyInvoiceNumber, @DeliveryStateCode)", connection))
                            {
                                billSummaryCommand.Parameters.AddWithValue("@CompanyId", data.BillSummary.CompanyId);
                                billSummaryCommand.Parameters.AddWithValue("@LedgerId", data.BillSummary.LedgerId);
                                // Add other parameters for BillSummary properties
                                billSummaryCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }

    }
}
