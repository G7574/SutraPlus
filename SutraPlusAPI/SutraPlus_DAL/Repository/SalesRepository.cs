using Azure;
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
                    if (companyState.ToLower() == ledgerState.ToLower() && dealer_type == "Registered Dealer" && country_name == "India" && (InvoiceType == "GoodsInvoice" || InvoiceType == "GinningInvoice"))
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Local Sale").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("Local Sale"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 12 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                    }
                    else if (companyState.ToLower() != ledgerState.ToLower() && dealer_type == "Registered Dealer" && country_name == "India" && (InvoiceType == "GoodsInvoice" || InvoiceType == "GinningInvoice"))
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Interstate Sale").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("Interstate Sale"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 12 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                    }
                    else if (dealer_type != "Register Dealer" && country_name == "India" && (InvoiceType == "GoodsInvoice" || InvoiceType == "GinningInvoice"))
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "URD Sale").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("URD Sale"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 12 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                    }
                    else if (country_name != "India" && InvoiceType == "ExportInvoice")
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Export Sale").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("Export Sale"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 12 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                    }
                    else if (country_name == "India" && InvoiceType == "DeemedExport")
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Deemed Export").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("Deemed Export"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 13 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                    }
                    else if (country_name == "India" && InvoiceType == "PurchaseReturn")
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Purchase Return").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("Purchase Return"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 8 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
                    }
                    else if (country_name == "India" && InvoiceType == "ProfarmaInvoice")
                    {
                        var voucherId = _tenantDBContext.VoucherTypes.Where(v => v.VoucherName == "Profarma Invoice").Select(x => x.VoucherId).SingleOrDefault();
                        response.Add("VoucherType", new JValue("Profarma Invoice"));
                        response.Add("InvoiceNo", new JValue(Convert.ToUInt32(_tenantDBContext.Inventory.Where(x => x.VochType == 16 && x.CompanyId == CompanyId).Select(x => x.VochNo).ToList().Max()) + 1));
                        response.Add("VoucherId", new JValue(Convert.ToInt64(voucherId)));
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
                             where Vouchers.CompanyId == CompanyId && Vouchers.VoucherNo == vochno && Vouchers.VoucherId == VochTypeID && Vouchers.IsActive == true
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



        public InvoiceDetails GetInvoiceResponse(string InvoiceNumber, int CompnayId, string TYP = "Single")
        {
            string response;
            GeneralModel respobj;
            try
            {
                AuthenticationDetails auth = new AuthenticationDetails();


                InvoiceDetails invdetail = new InvoiceDetails();

                string connectionString = "Server=103.50.212.163;Database=SutraPlus;uid=sa;Password=root@123;TrustServerCertificate=True;";
                Company customer = new Company();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Companies WHERE CompanyId = @CompnayId";

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
                                            IsActive = reader["IsActive"] as bool?,
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

                                        if (TYP == "Single")
                                        {
                                            bill.IRNNO = respobj.IRN;
                                            bill.ACKNO = respobj.AckNo;
                                            bill.EwayBillNo = (string)respobj.EwbNo;
                                            bill.SignQRCODE = respobj.SignedQRCode;
                                            // Use an UPDATE query to save changes
                                            using (SqlCommand updateCommand = new SqlCommand("UPDATE BillSummaries SET " +
                   "LedgerName = @LedgerName, Place = @Place, VochType = @VochType, VoucherName = @VoucherName, " +
                   "DealerType = @DealerType, PAN = @PAN, GST = @GST, State = @State, InvoiceType = @InvoiceType, " +
                   "VochNo = @VochNo, EwayBillNo = @EwayBillNo, Ponumber = @Ponumber, Transporter = @Transporter, " +
                   "LorryNo = @LorryNo, LorryOwnerName = @LorryOwnerName, DriverName = @DriverName, Dlno = @Dlno, " +
                   "CheckPost = @CheckPost, FrieghtPerBag = @FrieghtPerBag, TotalFrieght = @TotalFrieght, " +
                   "Advance = @Advance, Balance = @Balance, AREDate = @AREDate, ARENo = @ARENo, IsLessOrPlus = @IsLessOrPlus, " +
                   "ExpenseName1 = @ExpenseName1, ExpenseName2 = @ExpenseName2, ExpenseName3 = @ExpenseName3, " +
                   "ExpenseAmount1 = @ExpenseAmount1, ExpenseAmount2 = @ExpenseAmount2, ExpenseAmount3 = @ExpenseAmount3, " +
                   "DeliveryName = @DeliveryName, DeliveryAddress1 = @DeliveryAddress1, DeliveryAddress2 = @DeliveryAddress2, " +
                   "DeliveryPlace = @DeliveryPlace, DeliveryState = @DeliveryState, " +
                   "DeliveryStateCode = @DeliveryStateCode, BillAmount = @BillAmount, INWords = @INWords, " +
                   "FrieghtAmount = @FrieghtAmount, RoundOff = @RoundOff, TotalBags = @TotalBags, " +
                   "TotalWeight = @TotalWeight, TotalAmount = @TotalAmount, PackingValue = @PackingValue, " +
                   "HamaliValue = @HamaliValue, WeighnamFeeValue = @WeighnamFeeValue, DalaliValue = @DalaliValue, " +
                   "CessValue = @CessValue, TaxableValue = @TaxableValue, SGSTValue = @SGSTValue, CSGSTValue = @CSGSTValue, " +
                   "IGSTValue = @IGSTValue, UserId = @UserId, SessionID = @SessionID, PaymentBy = @PaymentBy, " +
                   "AmountReceived = @AmountReceived, Change = @Change, CardDetails = @CardDetails, BillTime = @BillTime, " +
                   "TranctDate = @TranctDate, CustomerName = @CustomerName, CustomerPlace = @CustomerPlace, " +
                   "CustomerContactNo = @CustomerContactNo, PartyInvoiceNumber = @PartyInvoiceNumber WHERE Id = @Id", connection))
                                            {
                                                // Set parameters for the UPDATE query
                                                updateCommand.Parameters.AddWithValue("@Id", bill._Id);
                                                updateCommand.Parameters.AddWithValue("@LedgerName", bill.LedgerName);
                                                updateCommand.Parameters.AddWithValue("@Place", bill.Place);
                                                updateCommand.Parameters.AddWithValue("@VochType", bill.VochType);
                                                updateCommand.Parameters.AddWithValue("@VoucherName", bill.VoucherName);
                                                updateCommand.Parameters.AddWithValue("@DealerType", bill.DealerType);
                                                updateCommand.Parameters.AddWithValue("@PAN", bill.PAN);
                                                updateCommand.Parameters.AddWithValue("@GST", bill.GST);
                                                updateCommand.Parameters.AddWithValue("@State", bill.State);
                                                updateCommand.Parameters.AddWithValue("@InvoiceType", bill.InvoiceType);
                                                updateCommand.Parameters.AddWithValue("@VochNo", bill.VochNo);
                                                updateCommand.Parameters.AddWithValue("@EwayBillNo", bill.EwayBillNo);
                                                updateCommand.Parameters.AddWithValue("@Ponumber", bill.Ponumber);
                                                updateCommand.Parameters.AddWithValue("@Transporter", bill.Transporter);
                                                updateCommand.Parameters.AddWithValue("@LorryNo", bill.LorryNo);
                                                updateCommand.Parameters.AddWithValue("@LorryOwnerName", bill.LorryOwnerName);
                                                updateCommand.Parameters.AddWithValue("@DriverName", bill.DriverName);
                                                updateCommand.Parameters.AddWithValue("@Dlno", bill.Dlno);
                                                updateCommand.Parameters.AddWithValue("@CheckPost", bill.CheckPost);
                                                updateCommand.Parameters.AddWithValue("@FrieghtPerBag", bill.FrieghtPerBag);
                                                updateCommand.Parameters.AddWithValue("@TotalFrieght", bill.TotalFrieght);
                                                updateCommand.Parameters.AddWithValue("@Advance", bill.Advance);
                                                updateCommand.Parameters.AddWithValue("@Balance", bill.Balance);
                                                updateCommand.Parameters.AddWithValue("@AREDate", bill.AREDate);
                                                updateCommand.Parameters.AddWithValue("@ARENo", bill.ARENo);
                                                updateCommand.Parameters.AddWithValue("@IsLessOrPlus", bill.IsLessOrPlus);
                                                updateCommand.Parameters.AddWithValue("@ExpenseName1", bill.ExpenseName1);
                                                updateCommand.Parameters.AddWithValue("@ExpenseName2", bill.ExpenseName2);
                                                updateCommand.Parameters.AddWithValue("@ExpenseName3", bill.ExpenseName3);
                                                updateCommand.Parameters.AddWithValue("@ExpenseAmount1", bill.ExpenseAmount1);
                                                updateCommand.Parameters.AddWithValue("@ExpenseAmount2", bill.ExpenseAmount2);
                                                updateCommand.Parameters.AddWithValue("@ExpenseAmount3", bill.ExpenseAmount3);
                                                updateCommand.Parameters.AddWithValue("@DeliveryName", bill.DeliveryName);
                                                updateCommand.Parameters.AddWithValue("@DeliveryAddress1", bill.DeliveryAddress1);
                                                updateCommand.Parameters.AddWithValue("@DeliveryAddress2", bill.DeliveryAddress2);
                                                updateCommand.Parameters.AddWithValue("@DeliveryPlace", bill.DeliveryPlace);
                                                updateCommand.Parameters.AddWithValue("@DeliveryState", bill.DeliveryState);
                                                updateCommand.Parameters.AddWithValue("@DeliveryStateCode", bill.DeliveryStateCode);
                                                updateCommand.Parameters.AddWithValue("@BillAmount", bill.BillAmount);
                                                updateCommand.Parameters.AddWithValue("@INWords", bill.INWords);
                                                updateCommand.Parameters.AddWithValue("@FrieghtAmount", bill.FrieghtAmount);
                                                updateCommand.Parameters.AddWithValue("@RoundOff", bill.RoundOff);
                                                updateCommand.Parameters.AddWithValue("@TotalBags", bill.TotalBags);
                                                updateCommand.Parameters.AddWithValue("@TotalWeight", bill.TotalWeight);
                                                updateCommand.Parameters.AddWithValue("@TotalAmount", bill.TotalAmount);
                                                updateCommand.Parameters.AddWithValue("@PackingValue", bill.PackingValue);
                                                updateCommand.Parameters.AddWithValue("@HamaliValue", bill.HamaliValue);
                                                updateCommand.Parameters.AddWithValue("@WeighnamFeeValue", bill.WeighnamFeeValue);
                                                updateCommand.Parameters.AddWithValue("@DalaliValue", bill.DalaliValue);
                                                updateCommand.Parameters.AddWithValue("@CessValue", bill.CessValue);
                                                updateCommand.Parameters.AddWithValue("@TaxableValue", bill.TaxableValue);
                                                updateCommand.Parameters.AddWithValue("@SGSTValue", bill.SGSTValue);
                                                updateCommand.Parameters.AddWithValue("@CSGSTValue", bill.CSGSTValue);
                                                updateCommand.Parameters.AddWithValue("@IGSTValue", bill.IGSTValue);
                                                updateCommand.Parameters.AddWithValue("@UserId", bill.UserId);
                                                updateCommand.Parameters.AddWithValue("@SessionID", bill.SessionID);
                                                updateCommand.Parameters.AddWithValue("@PaymentBy", bill.PaymentBy);
                                                updateCommand.Parameters.AddWithValue("@AmountReceived", bill.AmountReceived);
                                                updateCommand.Parameters.AddWithValue("@Change", bill.Change);
                                                updateCommand.Parameters.AddWithValue("@CardDetails", bill.CardDetails);
                                                updateCommand.Parameters.AddWithValue("@BillTime", bill.BillTime);
                                                updateCommand.Parameters.AddWithValue("@TranctDate", bill.TranctDate);
                                                updateCommand.Parameters.AddWithValue("@CustomerName", bill.CustomerName);
                                                updateCommand.Parameters.AddWithValue("@CustomerPlace", bill.CustomerPlace);
                                                updateCommand.Parameters.AddWithValue("@CustomerContactNo", bill.CustomerContactNo);
                                                updateCommand.Parameters.AddWithValue("@PartyInvoiceNumber", bill.PartyInvoiceNumber);
                                                // Execute the UPDATE query
                                                updateCommand.ExecuteNonQuery();
                                            }
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
                                  where c.CommodityName.Contains(name) && c.IsActive == true
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
                                  where c.SGST == Convert.ToDecimal(GST_type) || c.CGST == Convert.ToDecimal(GST_type) || c.IGST == Convert.ToDecimal(GST_type) && c.IsActive == true
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
                              where c.IsActive == true
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


                var ledgerName = (_tenantDBContext.Ledgers.
                                   Where(l => l.LedgerId == ledgerid).Select(l => l.LedgerName)).FirstOrDefault();
                var ledgerplace = (_tenantDBContext.Ledgers.
                                   Where(l => l.LedgerId == ledgerid && l.LedgerName == ledgerName).Select(l => l.Place)).FirstOrDefault();


                var InvoiceNo = _tenantDBContext.Inventory.Where(x => x.VochType > 8 && x.VochType < 14 && x.VochNo == invoiceNo && x.CompanyId == companyid).ToList();
                if (InvoiceNo.Count > 0)
                {
                    var entityBls = _tenantDBContext.BillSummaries.FirstOrDefault(item => item.VochNo == Convert.ToInt64(invoiceNo) && item.VochType == VochType);
                    if (entityBls != null)
                    {
                        entityBls.IsActive = false;
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
                        entityVoch.IsActive = false;
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
                        entityVoch.IsActive = false;
                        _tenantDBContext.SaveChanges();
                        _tenantDBContext.Update(entityVoch);
                    }









                }


                var InvoiceString = (_tenantDBContext.Companies.
                                Where(c => c.CompanyId == companyid).Select(c => c.InvoiceString)).FirstOrDefault();

                BillSummary billsummary = new BillSummary
                {
                    CompanyId = data["CompanyId"], //selling party
                    LedgerId = data["LedgerId"],//party id
                    LedgerName = ledgerName + "-" + ledgerplace,
                    TranctDate = data["OriginalInvDate"],
                    // CommodityID = itemdata["CommodityId"],
                    DisplayinvNo = InvoiceString + '/' + invoiceNo,
                    PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
                    VochType = data["VochType"],
                    VoucherName = data["VoucherName"],
                    DealerType = data["DealerType"],
                    PAN = data["PAN"],
                    GST = data["GST"],
                    InvoiceType = invType,
                    VochNo = data["InoviceNo"],
                    State = data["State"],
                    TotalBags = data["NoOfBags"],
                    TotalWeight = data["TotalWeight"],
                    //other charges block
                    ExpenseName1 = data["ExpenseName1"],
                    ExpenseName2 = data["ExpenseName2"],
                    ExpenseName3 = data["ExpenseName3"],
                    ExpenseAmount1 = data["ExpenseAmount1"],
                    ExpenseAmount2 = data["ExpenseAmount2"],
                    ExpenseAmount3 = data["ExpenseAmount3"],
                    //paymentDetails
                    TaxableValue = data["TaxableValue"],
                    Discount = data["Discount"],
                    SGSTValue = data["Sgstvalue"],
                    CSGSTValue = data["Cgstvalue"],
                    IGSTValue = data["Igstvalue"],
                    IsSEZ = data["IsSEZ"],
                    RoundOff = data["RoundOff"],
                    TotalAmount = data["TotalAmount"],
                    BillAmount = data["BillAmount"],
                    INWords = AmountToWord(Convert.ToDouble(TotalAmount)),// call amt here (write method to which pass amt & get return amt in words like Ruppes One Hundred & Fifty Five Only)                                                                     
                                                                          //DispatcherAddress1 = data["Address"],
                    FromPlace = data["FromPlace"],
                    ToPlace = data["ToPlace"],
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
                    IsActive = true,

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
                        TotalWeight = item["TotalWeight"],//3
                        Rate = item["Rate"],//4
                        Amount = item["Amount"],//5
                        Mark = item["Remark"],//6
                        PartyInvoiceNumber = InvoiceString + '/' + Convert.ToString(invoiceNo),
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
                        IsActive = true
                    };
                    inventorylist.Add(inventory);
                }
                _tenantDBContext.Add(billsummary);
                _tenantDBContext.SaveChanges();
                _tenantDBContext.AddRange(inventorylist);
                _tenantDBContext.SaveChanges();


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
                                IsActive = true
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
                            LedgerNameForNarration = data["LedgerName"],
                            CreatedBy = 1
                        };
                        voucherlist.Add(voucherlst);

                        string CommodityAccountName2 = "Output CGST";

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
                            Credit = Convert.ToDecimal(data["Cgstvalue"]),
                            Debit = 0,
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                Ledger ledger = new Ledger
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
                                _tenantDBContext.SaveChanges();
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
                                IsActive = true,
                                PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true,
                                PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                Ledger ledger = new Ledger
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
                                _tenantDBContext.SaveChanges();
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
                                IsActive = true,
                                PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            Credit = Convert.ToDecimal(data["RoundOff"]) > 0 ? Convert.ToDecimal(data["RoundOff"]) : 0,
                            Debit = (Convert.ToDecimal(data["RoundOff"]) < 0 ? Convert.ToDecimal(data["RoundOff"]) : 0) * -1,
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            Ledger ledger = new Ledger
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
                            _tenantDBContext.SaveChanges();
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
                            LedgerNameForNarration = data["LedgerName"],
                            CreatedBy = 1
                        };
                        voucherlist.Add(VochTDSExpenses);


                        string TransPorterNameFroTDS = LorryData["Transporter"];
                        var ledgerValTransPorterForTDS = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == TransPorterNameFroTDS && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                        if (ledgerValTransPorterForTDS == 0 || ledgerValTransPorterForTDS == null)
                        {
                            Ledger ledger = new Ledger
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
                            _tenantDBContext.SaveChanges();
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            Credit = 0,
                            Debit = Convert.ToDecimal(data["BillAmount"]),
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            Ledger ledger = new Ledger
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
                            Credit = 0,
                            Debit = Convert.ToDecimal(item.Amount),
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
                            LedgerNameForNarration = LorryData["DeliveryName"],
                            CreatedBy = 1
                        };
                        voucherlist.Add(voucherlst);
                    }


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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true
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
                            Credit = 0,
                            Debit = Convert.ToDecimal(data["ExpenseAmount1"]),
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true
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
                            Credit = 0,
                            Debit = Convert.ToDecimal(data["ExpenseAmount2"]),
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true
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
                            Credit = 0,
                            Debit = Convert.ToDecimal(data["ExpenseAmount3"]),
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                Ledger ledger = new Ledger
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
                                _tenantDBContext.SaveChanges();
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
                                IsActive = true,
                                PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                IsActive = true,
                                PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                                Ledger ledger = new Ledger
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
                                _tenantDBContext.SaveChanges();
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
                                IsActive = true,
                                PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            Ledger ledger = new Ledger
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
                            _tenantDBContext.SaveChanges();
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
                            LedgerNameForNarration = data["LedgerName"],
                            CreatedBy = 1
                        };
                        voucherlist.Add(VochTDSExpenses);


                        string TransPorterNameFroTDS = LorryData["Transporter"];
                        var ledgerValTransPorterForTDS = (_tenantDBContext.Ledgers.
                                          Where(l => l.LedgerName == TransPorterNameFroTDS && l.CompanyId == companyid).Select(l => l.LedgerId)).FirstOrDefault();

                        if (ledgerValTransPorterForTDS == 0 || ledgerValTransPorterForTDS == null)
                        {
                            Ledger ledger = new Ledger
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
                            _tenantDBContext.SaveChanges();
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
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
                            IsActive = true,
                            PartyInvoiceNumber = InvoiceString + '/' + invoiceNo,
                            LedgerNameForNarration = ItemNameForVoucherInsert + " Account",
                            CreatedBy = 1
                        };
                        voucherlist.Add(voucherlstBillAmount);
                    }



                    _tenantDBContext.AddRange(voucherlist);
                    _tenantDBContext.SaveChanges();





                }




                _logger.LogDebug("salesRepo : InvoiceDetails Added");
                return "Added Successfully...!|||||" + Convert.ToString(invoiceNo) + "|||||" + Convert.ToString(VochType);




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
                string DisplayInvNo = data["DisplayInvNo"];

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
                                    && bls.CompanyId == companyid_Invoice && bls.VochNo == vochNo_Invoice && bls.VochType == 9
                                    select new
                                    {
                                        LedgerID = bls.LedgerId,
                                        LedgerName = led.LedgerName,
                                        LegalName = led.LegalName,
                                        Address1 = led.Address1,
                                        Address2 = led.Address2,
                                        VoucherName = VochType.VoucherName,
                                        VoucherTypeID = bls.VochType,
                                        DealerType = led.DealerType,
                                        PAN = led.Pan,
                                        PIN = led.Pin,
                                        GST = led.Gstin,
                                        State = led.State,
                                        CellNo = led.CellNo,
                                        VochNo = bls.VochNo,
                                        TrancDate = Convert.ToString(Convert.ToDateTime(bls.TranctDate).ToString("yyyy-MM-dd")),
                                        TDS = bls.tdsperc,
                                        frieghtPlus = bls.frieghtPlus,
                                        LorryOwnerName = bls.LorryOwnerName,
                                        ExpenseName1 = bls.ExpenseName1,
                                        ExpenseName2 = bls.ExpenseName2,
                                        ExpenseName3 = bls.ExpenseName3,
                                        ExpenseAmount1 = bls.ExpenseAmount1,
                                        ExpenseAmount2 = bls.ExpenseAmount2,
                                        ExpenseAmount3 = bls.ExpenseAmount3,
                                        TaxableValue = bls.TaxableValue,
                                        Discount = bls.Discount,
                                        SGSTValue = bls.SGSTValue,
                                        CSGSTValue = bls.CSGSTValue,
                                        IGSTValue = bls.IGSTValue,

                                        CGSTRate = inv.CGSTRate,
                                        IGSTRate = inv.IGSTRate,
                                        SGSTRate = inv.SGSTRate,

                                        IsSEZ = bls.IsSEZ,
                                        RoundOff = bls.RoundOff,
                                        TotalAmount = bls.TotalAmount,
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
                                  where t1.CompanyId == companyid_Invoice && t1.VochType == vochtype_Invoice && t1.VochNo == vochNo_Invoice
                                  select new
                                  {
                                      id = t1 != null ? t1.Id : 0,
                                      CompanyId = t1 != null ? t1.CompanyId : 0,
                                      VochType = t1 != null ? t1.VochType : 0,
                                      InvoiceType = t1 != null ? t1.InvoiceType : "",
                                      VochNo = t1 != null ? t1.VochNo : 0,
                                      LedgerId = t1 != null ? t1.LedgerId : 0,
                                      CommodityId = t1 != null ? t1.CommodityId : 0,
                                      CommodityName = t1 != null ? t1.CommodityName : "",
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
                                      IsActive = t1 != null ? t1.IsActive : false
                                  }).ToList();




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
                         (e.IsActive.GetValueOrDefault() && (invoiceType == null || (e.InvoiceType != null && e.InvoiceType.Contains(invoiceType))))
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
                                     && bls.CompanyId == companyId && bls.VochType >= 9 && bls.VochType <= 13 && bls.IsActive == true && bls.IsServiceInvoice == null
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
                                     && bls.CompanyId == companyId && bls.VochType >= 2 && bls.VochType <= 4 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 6 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 5 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 8 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType >= 2 && bls.VochType <= 4 && bls.IsActive == true && bls.TotalWeight == 0
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
                                     && bls.CompanyId == companyId && bls.VochType >= 9 && bls.VochType <= 13 && bls.IsActive == true && bls.IsServiceInvoice == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 12 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 14 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 15 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 16 && bls.IsActive == true
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
                                     && bls.CompanyId == companyId && bls.VochType == 13 && bls.IsActive == true
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
                             Inv.IsActive == true &&
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

        public pagination<LedgerInfo> GetPaymentList(JObject Data)
        {
            try
            {

                var searchText = Convert.ToString(Data?["SearchText"]);
                var balance = Convert.ToDecimal(Data?["Balance"]);
                var Date = Convert.ToString(Data?["Date"]);

                if (Convert.ToString(Data?["Balance"]) == null || Convert.ToString(Data?["Balance"]).Equals(""))
                {
                    balance = -1;
                }

                //var searchText = Convert.ToString(Data?["SearchText"]);
                var data = JsonConvert.DeserializeObject<dynamic>(Data?["ReportData"]?.ToString());

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "ReportType is required and cannot be null.");
                }

                int companyId = data?["CompanyId"];
                DateTime? startDate = data?["StartDate"];
                DateTime? endDate = data?["EndDate"];
                string reportType = data?["ReportType"];

                //var query = from led in _tenantDBContext.Ledgers
                //            join tr in _tenantDBContext.TmpPaymentList on new { CompanyId = (int?)led.CompanyId, LedgerId = (int?)led.LedgerId }
                //            equals new { CompanyId = tr.CompanyId, LedgerId = tr.LedgerID }
                //            orderby led.LedgerName
                //            select new ReportDTO
                //            {
                //                LedgerName = led.LedgerName,
                //                Place = led.Place,
                //                YadiBalance = tr.YadiBalance,
                //                AccountBalance = tr.AccountBalance,
                //                AsOnDate = tr.AsOnDate
                //            };


                /*var query = from ledger in _tenantDBContext.Ledgers
                            join voucher in _tenantDBContext.Vouchers on ledger.LedgerId equals voucher.LedgerId
                            where voucher.CompanyId == companyId && ledger.AccountingGroupId == 21
                            select new
                            {
                                ledger.LedgerName,
                                ledger.Place,
                                voucher.Credit,
                                voucher.TranctDate
                            };*/

                var query = from ledger in _tenantDBContext.Ledgers
                            join voucher in _tenantDBContext.Vouchers on ledger.LedgerId equals voucher.LedgerId
                            where voucher.CompanyId == companyId && ledger.AccountingGroupId == 21
                            select new
                            {
                                ledger.LedgerName,
                                ledger.Place,
                                voucher.Credit,
                                voucher.TranctDate
                            };


                //var ledgerInfoList1 = query.ToList()
                //        .GroupBy(x => new { x.LedgerName, x.Place })
                //        .Select(grouped => new LedgerInfo
                //        {
                //            LedgerName = grouped.Key.LedgerName,
                //            Place = grouped.Key.Place,
                //            AsOnDateBalance = grouped.Sum(x => x.TranctDate == null ? x.Credit : 0) - grouped.Sum(x => x.Credit) ?? 0,
                //            TotalBalance = grouped.Sum(x => x.Credit) - grouped.Sum(x => x.Credit) ?? 0,
                //            TranctDate = new DateTime()
                //        })
                //        .Where(info => info.AsOnDateBalance > 0)
                //        .ToList();


                     var ledgerInfoList = query
                    .GroupBy(result => new { result.LedgerName, result.Place, result.TranctDate })
                    .Select(group => new LedgerInfo
                    {
                        LedgerName = group.Key.LedgerName,
                        Place = group.Key.Place,
                        AsOnDateBalance = group.Sum(x => x.Credit),
                        TotalBalance = group.Sum(x => x.Credit),
                        TranctDate = group.Key.TranctDate ?? new DateTime()
                    })
                    .ToList();

              
                if (!string.IsNullOrEmpty(searchText) || balance != null || !string.IsNullOrEmpty(Date))
                {

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        ledgerInfoList = ledgerInfoList.Where(n => n.Place.Contains(searchText) || n.LedgerName.Contains(searchText)).ToList();
                    }

                    if (balance != null && balance > -1)
                    {
                        ledgerInfoList = ledgerInfoList.Where(n => n.TotalBalance > balance).ToList();
                    }

                    if (!string.IsNullOrEmpty(Date))
                    {
                        ledgerInfoList = ledgerInfoList.Where(n => n.TranctDate <= DateTime.Parse(Date)).ToList();
                    }

                }

                var result1 = ledgerInfoList.ToList();

                var page = new pagination<LedgerInfo>();
                page.TotalCount = query.Count();
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

        public class LedgerInfo
        {
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
                        entity.IsActive = true;
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
                            Itementity.IsActive = true;
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
                        var activeentity = _tenantDBContext.BillSummaries.SingleOrDefault(item => item._Id == id && item.IsActive == true);
                        var deactivateentity = _tenantDBContext.BillSummaries.SingleOrDefault(item => item._Id == id && item.IsActive == false);
                        if (activeentity != null)
                        {
                            activeentity.IsActive = false;
                            _tenantDBContext.SaveChanges();
                            _tenantDBContext.Update(activeentity);
                            _logger.LogDebug("SalesReo : sales Deactivated");
                        }
                        else if (deactivateentity != null)
                        {
                            deactivateentity.IsActive = true;
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
