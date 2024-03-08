using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using NLog.Fluent;
using SutraPlus_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication2.Implementations
{
    public class Tool
    {
        private static Tool tool;
      private string AUTH_URL = "https://www.ewaybills.com/MVEINVAuthenticate/EINVAuthentication";
       // private string AUTH_URL = "https://powergstservice.microvistatech.com/api/MVEINVAuthenticate/EINVAuthentication";
        private string INV_GEN_URL = "https://www.ewaybills.com/MVEINVAuthenticate/EINVGeneration";
        //private string INV_GEN_URL = "https://powergstservice.microvistatech.com/api/MVEINVAuthenticate/EINVGeneration";
        private static Authentication Authentication;

        public Tool()
        {
            Authentication = new Authentication();
        }

        public static Tool CreateTool()
        {
            if (tool == null)
            {
                tool = new Tool();
                Authentication = new Authentication();

            }
            return tool;
        }

        public AuthentictionResponse AuthentictionH()
        {
            try
            {
                AuthentictionResponse auth = new AuthentictionResponse();

                var request = WebRequest.Create(AUTH_URL);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    Authentication = new Authentication();
                    string json = JsonConvert.SerializeObject(Authentication);
                    streamWriter.Write(json);
                }

                var httpResponse = request.GetResponse();
                using (var streamreader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamreader.ReadToEnd();
                    auth = JsonConvert.DeserializeObject<AuthentictionResponse>(result);
                }
                return auth;
            }
            catch (Exception ex)
            {
                //log.WriteError(ex.Message);
                throw;
            }
        }

        public string GenerateInvoiceH(IRNRequestAuthHeader headerObjct, string inventoryNumber, int days, string resString, int CompnayId)
        {
            try
            {
                Root Root = GetInventoryDetails(inventoryNumber, days, ref resString, CompnayId);
                //if (Root.Response == "")
                //{
                if (Root == null)
                {
                    return null;
                }
                var request = WebRequest.Create(INV_GEN_URL);
                request.ContentType = "application/json";
                request.Headers.Add("MVApiKey", headerObjct.MVApiKey);
                request.Headers.Add("MVSecretKey", headerObjct.MVSecretKey);
                request.Headers.Add("Gstin", headerObjct.gstin);
                request.Headers.Add("eInvoiceUserName", headerObjct.eInvoiceUserName);
                request.Headers.Add("eInvoicePassword", headerObjct.eInvoicePassword);
                request.Headers.Add("authToken", headerObjct.authToken);
                request.Headers.Add("MonthYear", headerObjct.MonthYear);
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    Authentication = new Authentication();

                    string json = JsonConvert.SerializeObject(Root);
                    //try
                    //{
                    //    WriteJsonFile(json, "Request_", inventoryNumber);
                    //}
                    //catch (System.Exception)
                    //{

                    //    throw;
                    //}
                    streamWriter.Write(json);
                }
                var httpResponse = request.GetResponse();
                var result = "";

                using (var streamreader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamreader.ReadToEnd();
                    //try
                    //{
                    //    WriteJsonFile(result, "Response_", inventoryNumber);
                    //}
                    //catch (Exception)
                    //{

                    //    throw;
                    //}
                }
                return result;
                //}
                //else
                //{
                //    string json = JsonConvert.SerializeObject(Root);
                //    return json;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void WriteJsonFile(string JsonString, string prefix, string invoice)
        {
            try
            {
                if (!Directory.Exists("IRN"))
                {
                    Directory.CreateDirectory("IRN");
                }
                var fpath = Path.Combine(Environment.CurrentDirectory, "IRN", prefix + invoice.Replace(@"/", "") + ".json");
                File.WriteAllText(fpath, JsonString);
            }
            catch (Exception ex)
            {
                //log.WriteError(ex.Message);

                if (!Directory.Exists("Exception"))
                {
                    Directory.CreateDirectory("Exception");
                }
                var fpath = Path.Combine(Environment.CurrentDirectory, "Exception", prefix + "_Exception_" + DateTime.Now.ToString().Replace(@"\", "").Replace("-", "_").Replace(":", "_") + ".json");
                File.WriteAllText(fpath, JsonString);
                throw;
            }
        }

        #region WriteJsonFile Two parmeter
        //public void WriteJsonFile(string JsonString, string prefix)
        //{
        //    try
        //    {
        //        if (!Directory.Exists("IRN"))
        //            Directory.CreateDirectory("IRN");
        //        InvoiceReponse invoice = JsonConvert.DeserializeObject(JsonString);
        //        var fpath = Path.Combine(Environment.CurrentDirectory, "IRN", prefix + invoice.IRN.Replace(@"\", "") + ".json");
        //        File.WriteAllText(fpath, JsonString);
        //    }
        //    // File.WriteAllText(@"D:\"+invoice.IRN+".json", JsonString);
        //    catch
        //    {
        //        if (!Directory.Exists("Exception"))
        //            Directory.CreateDirectory("Exception");
        //        var fpath = Path.Combine(Environment.CurrentDirectory, "Exception", prefix + "_Exception_" + DateTime.Now.ToString().Replace(@"\", "") + ".json");
        //        File.WriteAllText(fpath, JsonString);
        //    }
        //}
        #endregion

        private Root GetInventoryDetails(string invoiceNo, int days, ref string resString, int CompanyId)
        {
            var root = new Root();
            try
            {
                string partyInvoice = string.Empty;
                Ledger buyer = new Ledger();
                Company supplier = new Company();
                BillSummary billSummary = new BillSummary();
                List<Inventory> inventories = new List<Inventory>();
                using (SqlConnection connection = new SqlConnection("Server=103.50.212.163;Database=K2223RGP;uid=sa;Password=root@123;TrustServerCertificate=True;"))
                {
                    connection.Open();

                    // Use raw SQL queries to fetch data

                    using (SqlCommand command = new SqlCommand("SELECT * FROM Inventory WHERE PartyInvoiceNumber = @InvoiceNo AND CompanyId = @CompanyId AND VochType = 9", connection))
                    {
                        command.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                        command.Parameters.AddWithValue("@CompanyId", CompanyId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (partyInvoice != String.Empty)
                                    partyInvoice = reader["PartyInvoiceNumber"].ToString();

                                Inventory inventory = new Inventory
                                {
                                    //_Id = reader["Id"] as int?,
                                    CompanyId = reader["CompanyId"] as long?,
                                    VochType = reader["VochType"] as long?,
                                    InvoiceType = reader["InvoiceType"] as string,
                                    VochNo = reader["VochNo"] as long?,
                                    LedgerId = reader["LedgerId"] as long?,
                                    CommodityId = reader["CommodityId"] as long?,
                                    CommodityName = reader["CommodityName"] as string,
                                    TranctDate = reader["TranctDate"] as DateTime?,
                                    PoNumber = reader["PoNumber"] as string,
                                    EwaybillNo = reader["EwaybillNo"] as string,
                                    LotNo = reader["LotNo"] as long?,
                                    WeightPerBag = reader["WeightPerBag"] as double?,
                                    NoOfBags = reader["NoOfBags"] as double?,
                                    NoOfDocra = reader["NoOfDocra"] as double?,
                                    NoOfBodhs = reader["NoOfBodhs"] as double?,
                                    TotalWeight = reader["TotalWeight"] as double?,
                                    Rate = reader["Rate"] as decimal?,
                                    Amount = reader["Amount"] as decimal?,
                                    Mark = reader["Mark"] as string,
                                    PartyInvoiceNumber = reader["PartyInvoiceNumber"] as string,
                                    Discount = reader["Discount"] as decimal?,
                                    NetAmount = reader["NetAmount"] as decimal?,
                                    Cess = reader["Cess"] as decimal?,
                                    SGST = reader["SGST"] as decimal?,
                                    CGST = reader["CGST"] as decimal?,
                                    IGST = reader["IGST"] as decimal?,
                                    CreatedDate = reader["CreatedDate"] as DateTime?,
                                    Createdby = reader["Createdby"] as int?,
                                    UpdatedDate = reader["UpdatedDate"] as DateTime?,
                                    UpdatedBy = reader["UpdatedBy"] as int?,
                                    SchemeDiscount = reader["SchemeDiscount"] as decimal?,
                                    FreeQty = reader["FreeQty"] as double?,
                                    IGSTRate = reader["IGSTRate"] as decimal?,
                                    SGSTRate = reader["SGSTRate"] as decimal?,
                                    CGSTRate = reader["CGSTRate"] as decimal?,
                                    CessRate = reader["CessRate"] as decimal?,
                                    UnloadHamali = reader["UnloadHamali"] as decimal?,
                                    VikriFrieght = reader["VikriFrieght"] as decimal?,
                                    VikriCashAdvance = reader["VikriCashAdvance"] as decimal?,
                                    EmptyGunnybags = reader["EmptyGunnybags"] as decimal?,
                                    BuyerCode = reader["BuyerCode"] as string,
                                    ToPrint = reader["ToPrint"] as int?,
                                    Taxable = reader["Taxable"] as decimal?,
                                    DisplayNo = reader["DisplayNo"] as string,
                                    DisplayinvNo = reader["DisplayinvNo"] as string,
                                    IsTender = reader["IsTender"] as int?,
                                    IsTotalWeight = reader["IsTotalWeight"] as int?,
                                    SellingRate = reader["SellingRate"] as decimal?,
                                    WeightInString = reader["WeightInString"] as string,
                                    InputCode = reader["InputCode"] as string,
                                    InputLot = reader["InputLot"] as string,
                                    OutputCode = reader["OutputCode"] as string,
                                    OutputLot = reader["OutputLot"] as string,
                                    MonthNo = reader["MonthNo"] as int?,
                                    IsTaxableValueupdt = reader["IsTaxableValueupdt"] as int?,
                                    OriginalInvNo = reader["OriginalInvNo"] as string,
                                    AprReturned = reader["AprReturned"] as int?,
                                    ISUSED = reader["ISUSED"] as int?,
                                    OriginalInvDate = reader["OriginalInvDate"] as DateTime?,
                                    Frieght = reader["Frieght"] as decimal?
                                };

                                // Access the other properties as needed...

                                // Example: partyInvoice
                                partyInvoice = inventory.PartyInvoiceNumber;

                                inventories.Add(inventory);
                            }

                        }

                    }
                    if (string.IsNullOrEmpty(partyInvoice))
                    {
                        root.Response += " Associated Inventory Not Found";
                        resString = " Associated Inventory Not Found";
                        return root;
                    }
                    connection.Close();
                }
                using (SqlConnection connection = new SqlConnection("Server=103.50.212.163;Database=K2223RGP;uid=sa;Password=root@123;TrustServerCertificate=True;"))
                {
                    connection.Open();
                    using (SqlCommand command2 = new SqlCommand("SELECT * FROM BillSummary WHERE DisplayinvNo = @InvoiceNo AND CompanyID = @CompanyId AND VochType = 9", connection))
                    {
                        command2.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                        command2.Parameters.AddWithValue("@CompanyId", CompanyId);

                        using (SqlDataReader reader1 = command2.ExecuteReader())
                        {
                            while (reader1.Read())
                            {

                              billSummary = new BillSummary
                                {
                                    //_Id = reader["_id"] as int?;
                                    CompanyId = reader1["CompanyId"] as long?,
                                LedgerId = reader1["LedgerId"] as long?,
                                LedgerName = reader1["LedgerName"] as string,
                                Place = reader1["Place"] as string,
                                VochType = reader1["VochType"] as long?,
                                VoucherName = reader1["VoucherName"] as string,
                                DealerType = reader1["DealerType"] as string,
                                PAN = reader1["PAN"] as string,
                                GST = reader1["GST"] as string,
                                State = reader1["State"] as string,
                                InvoiceType = reader1["InvoiceType"] as string,
                                VochNo = reader1["VochNo"] as long?,
                                EwayBillNo = reader1["EwayBillNo"] as string,
                                Ponumber = reader1["Ponumber"] as string,
                                Transporter = reader1["Transporter"] as string,
                                LorryNo = reader1["LorryNo"] as string,
                                LorryOwnerName = reader1["LorryOwnerName"] as string,
                                DriverName = reader1["DriverName"] as string,
                                Dlno = reader1["Dlno"] as string,
                                CheckPost = reader1["CheckPost"] as string,
                                FrieghtPerBag = reader1["FrieghtPerBag"] as decimal?,
                                TotalFrieght = reader1["TotalFrieght"] as decimal?,
                                Advance = reader1["Advance"] as decimal?,
                                Balance = reader1["Balance"] as decimal?,
                                AREDate = reader1["AREDate"] as string,
                                ARENo = reader1["ARENo"] as string,
                                IsLessOrPlus = reader1["IsLessOrPlus"] as bool?,
                                ExpenseName1 = reader1["ExpenseName1"] as string,
                                ExpenseName2 = reader1["ExpenseName2"] as string,
                                ExpenseName3 = reader1["ExpenseName3"] as string,
                                ExpenseAmount1 = reader1["ExpenseAmount1"] as decimal?,
                                ExpenseAmount2 = reader1["ExpenseAmount2"] as decimal?,
                                ExpenseAmount3 = reader1["ExpenseAmount3"] as decimal?,
                                DeliveryName = reader1["DeliveryName"] as string,
                                DeliveryAddress1 = reader1["DeliveryAddress1"] as string,
                                DeliveryAddress2 = reader1["DeliveryAddress2"] as string,
                                DeliveryPlace = reader1["DeliveryPlace"] as string,
                                DeliveryState = reader1["DeliveryState"] as string,
                                DeliveryStateCode = reader1["DeliveryStateCode"] as string,
                                BillAmount = reader1["BillAmount"] as decimal?,
                                INWords = reader1["INWords"] as string,
                                FrieghtAmount = reader1["FrieghtAmount"] as decimal?,
                                RoundOff = reader1["RoundOff"] as decimal?,
                                TotalBags = reader1["TotalBags"] as long?,
                                TotalWeight = reader1["TotalWeight"] as double?,
                                TotalAmount = reader1["TotalAmount"] as decimal?,
                                PackingValue = reader1["PackingValue"] as decimal?,
                                HamaliValue = reader1["HamaliValue"] as decimal?,
                                WeighnamFeeValue = reader1["WeighnamFeeValue"] as decimal?,
                                DalaliValue = reader1["DalaliValue"] as decimal?,
                                CessValue = reader1["CessValue"] as decimal?,
                                TaxableValue = reader1["TaxableValue"] as decimal?,
                                SGSTValue = reader1["SGSTValue"] as decimal?,
                                CSGSTValue = reader1["CSGSTValue"] as decimal?,
                                IGSTValue = reader1["IGSTValue"] as decimal?,
                                UserId = reader1["UserId"] as int?,
                                SessionID = reader1["SessionID"] as int?,
                                PaymentBy = reader1["PaymentBy"] as string,
                                AmountReceived = reader1["AmountReceived"] as decimal?,
                                Change = reader1["Change"] as decimal?,
                                CardDetails = reader1["CardDetails"] as string,
                                BillTime = reader1["BillTime"] as DateTime?,
                                TranctDate = reader1["TranctDate"] as DateTime?,
                                CustomerName = reader1["CustomerName"] as string,
                                CustomerPlace = reader1["CustomerPlace"] as string,
                                CustomerContactNo = reader1["CustomerContactNo"] as string,
                                PartyInvoiceNumber = reader1["PartyInvoiceNumber"] as string,
                                OtherCreated = reader1["OtherCreated"] as int?,
                                Recent = reader1["Recent"] as int?,
                                PaymentReceived = reader1["PaymentReceived"] as int?,
                                TagName = reader1["TagName"] as string,
                                TagDate = reader1["TagDate"] as DateTime?,
                                ToPrint = reader1["ToPrint"] as int?,
                                frieghtPlus = reader1["frieghtPlus"] as int?,
                                StateCode1 = reader1["StateCode1"] as string,
                                StateCode2 = reader1["StateCode2"] as string,
                                ExpenseTax = reader1["ExpenseTax"] as decimal?,
                                FrieghtinBill = reader1["FrieghtinBill"] as decimal?,
                                LastOpenend = reader1["LastOpenend"] as DateTime?,
                                VikriCommission = reader1["VikriCommission"] as decimal?,
                                VikriULH = reader1["VikriULH"] as decimal?,
                                VikriCashAdvance = reader1["VikriCashAdvance"] as decimal?,
                                VikriFrieght = reader1["VikriFrieght"] as decimal?,
                                VikriEmpty = reader1["VikriEmpty"] as decimal?,
                                VikriNet = reader1["VikriNet"] as decimal?,
                                CashToFarmer = reader1["CashToFarmer"] as decimal?,
                                VikriOther1 = reader1["VikriOther1"] as decimal?,
                                VikriOther2 = reader1["VikriOther2"] as decimal?,
                                VikriOther1Name = reader1["VikriOther1Name"] as string,
                                VikriOther2Name = reader1["VikriOther2Name"] as string,
                                Note1 = reader1["Note1"] as string,
                                FromPlace = reader1["FromPlace"] as string,
                                ToPlace = reader1["ToPlace"] as string,
                                DisplayNo = reader1["DisplayNo"] as string,
                                DisplayinvNo = reader1["DisplayinvNo"] as string,
                                FrieghtLabel = reader1["FrieghtLabel"] as string,
                                FormName = reader1["FormName"] as string,
                                DCNote = reader1["DCNote"] as string,
                                WeightInString = reader1["WeightInString"] as string,
                                SGSTLabel = reader1["SGSTLabel"] as string,
                                CGSTLabel = reader1["CGSTLabel"] as string,
                                IGSTLabel = reader1["IGSTLabel"] as string,
                                TCSLabel = reader1["TCSLabel"] as string,
                                tdsperc = reader1["tdsperc"] as decimal?,
                                TCSValue = reader1["TCSValue"] as decimal?,
                                TCSPerc = reader1["TCSPerc"] as decimal?,
                                DelPinCode = reader1["DelPinCode"] as string,
                                CommodityID = reader1["CommodityID"] as int?,
                                QrCode = reader1["QrCode"] as byte[],
                                IsGSTUpload = reader1["IsGSTUpload"] as int?,
                                DispatcherName = reader1["DispatcherName"] as string,
                                DispatcherAddress1 = reader1["DispatcherAddress1"] as string,
                                DispatcherAddress2 = reader1["DispatcherAddress2"] as string,
                                DispatcherPlace = reader1["DispatcherPlace"] as string,
                                DispatcherPIN = reader1["DispatcherPIN"] as string,
                                DispatcherStatecode = reader1["DispatcherStatecode"] as string,
                                CountryCode = reader1["CountryCode"] as string,
                                ShipBillNo = reader1["ShipBillNo"] as string,
                                ForCur = reader1["ForCur"] as string,
                                PortName = reader1["PortName"] as string,
                                RefClaim = reader1["RefClaim"] as string,
                                ShipBillDate = reader1["ShipBillDate"] as DateTime?,
                                ExpDuty = reader1["ExpDuty"] as string,
                                monthNo = reader1["monthNo"] as int?,
                                Distance = reader1["Distance"] as int?,
                                GSTR1SectionName = reader1["GSTR1SectionName"] as string,
                                GSTR1InvoiceType = reader1["GSTR1InvoiceType"] as string,
                                EInvoiceNo = reader1["EInvoiceNo"] as string,
                                IsSEZ = reader1["IsSEZ"] as int?,
                                id = reader1["id"] as int?,
                                OriginalInvNo = reader1["OriginalInvNo"] as string,
                                OriginalInvDate = reader1["OriginalInvDate"] as DateTime?,
                                AprClosed = reader1["AprClosed"] as int?,
                                Discount = reader1["Discount"] as decimal?,
                                TwelveValue = reader1["TwelveValue"] as decimal?,
                                FiveValue = reader1["FiveValue"] as decimal?,
                                EgthValue = reader1["EgthValue"] as decimal?,
                                ACKNO = reader1["ACKNO"] as string,
                                IRNNO = reader1["IRNNO"] as string,
                                SignQRCODE = reader1["SignQRCODE"] as string,
                                IsActive = reader1["IsActive"] as bool?,
                                IsServiceInvoice = reader1["IsServiceInvoice"] as bool?,
                            };
                                 
                            }
                        }
                    }

                    connection.Close();

                    if (billSummary == null)
                    {
                        root.Response += " Associated BillSummery Not Found";
                        resString = " Associated BillSummery Not Found";
                        return root;
                    }

                    long? companyId = billSummary.CompanyId;
                    if (companyId != null)
                    {
                        connection.Open();

                        using (SqlCommand command3 = new SqlCommand($"SELECT * FROM Company WHERE CompanyId = {companyId}", connection))
                        using (SqlDataReader reader = command3.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                supplier = new Company();
                                //supplier._Id = reader["Id"] as int;
                                //supplier.CompanyId = reader["CompanyId"] as int;
                                supplier.CompanyName = reader["CompanyName"] as string;
                                supplier.AddressLine1 = reader["AddressLine1"] as string;
                                supplier.AddressLine2 = reader["AddressLine2"] as string;
                                supplier.AddressLine3 = reader["AddressLine3"] as string;
                                supplier.Place = reader["Place"] as string ?? string.Empty;
                                supplier.Gstin = reader["Gstin"] as string;
                                supplier.ContactDetails = reader["ContactDetails"] as string;
                                supplier.Shree = reader["Shree"] as string;
                                supplier.KannadaName = reader["KannadaName"] as string;
                                supplier.Pan = reader["Pan"] as string;
                                supplier.FirmCode = reader["FirmCode"] as string;
                                supplier.Apmccode = reader["Apmccode"] as string;
                                supplier.Iec = reader["Iec"] as string;
                                supplier.Fln = reader["Fln"] as string;
                                supplier.Bin = reader["Bin"] as string;
                                supplier.Bank1 = reader["Bank1"] as string;
                                supplier.Ifsc1 = reader["Ifsc1"] as string;
                                supplier.AccountNo1 = reader["AccountNo1"] as string;
                                supplier.Bank2 = reader["Bank2"] as string;
                                supplier.Ifsc2 = reader["Ifsc2"] as string;
                                supplier.AccountNo2 = reader["AccountNo2"] as string;
                                supplier.Bank3 = reader["Bank3"] as string;
                                supplier.Ifsc3 = reader["Ifsc3"] as string;
                                supplier.Account3 = reader["Account3"] as string;
                                supplier.Title = reader["Title"] as string;
                                supplier.District = reader["District"] as string;
                                supplier.State = reader["State"] as string;
                                supplier.Email = reader["Email"] as string;
                                supplier.CellPhone = reader["CellPhone"] as string;
                                supplier.Tan = reader["Tan"] as string;
                                supplier.Cst = reader["Cst"] as string;
                                supplier.Tin = reader["Tin"] as string;
                                supplier.StandardBilling = reader["StandardBilling"] as bool?;
                                supplier.AutoDeductTds = reader["AutoDeductTds"] as bool?;
                                supplier.CashEntrySystem = reader["CashEntrySystem"] as bool?;
                                supplier.RateInclusiveTax = reader["RateInclusiveTax"] as bool?;
                                supplier.FarmerBill = reader["FarmerBill"] as bool?;
                                supplier.TraderBill = reader["TraderBill"] as bool?;
                                supplier.PrintVochour = reader["PrintVochour"] as bool?;
                                supplier.Sender = reader["Sender"] as string;
                                supplier.SecondLineForReport = reader["SecondLineForReport"] as string;
                                supplier.ThirdLineForReport = reader["ThirdLineForReport"] as string;
                                supplier.ReportTile1 = reader["ReportTile1"] as string;
                                supplier.ReportTile2 = reader["ReportTile2"] as string;
                                supplier.CreatedDate = reader["CreatedDate"] as DateTime?;
                                supplier.UpdatedDate = reader["UpdatedDate"] as DateTime?;
                                supplier.CreatedBy = reader["CreatedBy"] as int?;
                                supplier.Logo = reader["Logo"] as byte[];
                                supplier.LastOpenend = reader["LastOpenend"] as DateTime?;
                                supplier.BillNo = reader["BillNo"] as int?;
                                supplier.KannadaAddress = reader["KannadaAddress"] as string;
                                supplier.KannadaPlace = reader["KannadaPlace"] as string;
                                supplier.Jurisdiction = reader["Jurisdiction"] as string;
                                supplier.Fssai = reader["Fssai"] as string;
                                supplier.OldFid = reader["OldFid"] as int?;
                                supplier.JurisLine = reader["JurisLine"] as string;
                                supplier.NameColor = reader["NameColor"] as string;
                                supplier.Printweights = reader["Printweights"] as int?;
                                supplier.Cp = reader["Cp"] as int?;
                                supplier.Gid = reader["Gid"] as string;
                                supplier.Gpw = reader["Gpw"] as string;
                                supplier.SelfEmailId = reader["SelfEmailId"] as string;
                                supplier.SelfWhatsUpNo = reader["SelfWhatsUpNo"] as string;
                                supplier.LicenceKey = reader["LicenceKey"] as string;
                                supplier.WebId = reader["WebId"] as int?;
                                supplier.Lutno = reader["Lutno"] as string;
                                supplier.Tcsreq = reader["Tcsreq"] as int?;
                                supplier.TcsreqinReceipt = reader["TcsreqinReceipt"] as int?;
                                supplier.VrtinForm = reader["VrtinForm"] as int?;
                                supplier.BillCode = reader["BillCode"] as string;
                                supplier.PrintNumber = reader["PrintNumber"] as string;
                                supplier.DirectPrint = reader["DirectPrint"] as int?;
                                supplier.NoOfCopies = reader["NoOfCopies"] as int?;
                                supplier.DeleteWeight = reader["DeleteWeight"] as int?;
                                supplier.EinvoiceKey = reader["EinvoiceKey"] as string;
                                supplier.EinvoiceSkey = reader["EinvoiceSkey"] as string;
                                supplier.EinvoiceUserName = reader["EinvoiceUserName"] as string;
                                supplier.EinvoicePassword = reader["EinvoicePassword"] as string;
                                supplier.DontTaxFrieght = reader["DontTaxFrieght"] as int?;
                                supplier.EinvoiceReq = reader["EinvoiceReq"] as int?;
                                supplier.Pin = reader["Pin"] as string;
                                supplier.LegalName = reader["LegalName"] as string;
                                supplier.PortaluserName = reader["PortaluserName"] as string;
                                supplier.PortalPw = reader["PortalPw"] as string;
                                supplier.PortalEmail = reader["PortalEmail"] as string;
                                supplier.NeftAcno = reader["NeftAcno"] as string;
                                supplier.VochNo = reader["VochNo"] as int?;
                                supplier.VochType = reader["VochType"] as int?;
                                supplier.LedgerId = reader["LedgerId"] as int?;
                                supplier.InvType = reader["InvType"] as string;
                                supplier.Iscrystal = reader["Iscrystal"] as int?;
                                supplier.ReportName = reader["ReportName"] as string;
                                supplier.Dbname = reader["Dbname"] as string;
                                supplier.IsStandard = reader["IsStandard"] as int?;
                                supplier.AskTcs = reader["AskTcs"] as int?;
                                supplier.AskPdf = reader["AskPdf"] as int?;
                                supplier.IsShopSent = reader["IsShopSent"] as int?;
                                supplier.HexCode = reader["HexCode"] as string;
                                supplier.CustomerId = reader["CustomerId"] as string;
                                supplier.IsVps = reader["IsVps"] as int?;
                                supplier.Setcmdac = reader["Setcmdac"] as int?;
                                supplier.AddColumns1 = reader["AddColumns1"] as int?;
                                supplier.AskEinvoice = reader["AskEinvoice"] as int?;
                                supplier.IsDuplicateDeleted = reader["IsDuplicateDeleted"] as int?;
                                supplier.ExpiryDate = reader["ExpiryDate"] as DateTime?;
                                supplier.IsVikriReq = reader["IsVikriReq"] as int?;
                                supplier.AutoTds = reader["AutoTds"] as int?;
                                supplier.Tcstotaxable = reader["Tcstotaxable"] as int?;
                                supplier.AskOnlineBackUpe = reader["AskOnlineBackUpe"] as int?;
                                supplier.PhyPath = reader["PhyPath"] as string;
                                supplier.DeleteEx = reader["DeleteEx"] as int?;
                                supplier.ResetPacking = reader["ResetPacking"] as int?;
                                //supplier.IsActive = reader["IsActive"] as bool;
                                supplier.InvoiceString = reader["InvoiceString"] as string;

                            }
                        }
                            if (supplier != null && supplier.Gstin.Length != 15)
                            {
                                root.Response += "Please Check Company GSTIN";
                                resString = "Please Check Company GSTIN";
                                return root;
                            }

                             connection.Close();

                            long? ledgerId = billSummary.LedgerId;
                            if (ledgerId != null)
                            {

                                connection.Open();
                                 
                                using (SqlCommand ledgerCommand = new SqlCommand($"SELECT * FROM Ledger WHERE LedgerId = {ledgerId}", connection))
                                using (SqlDataReader ledgerReader = ledgerCommand.ExecuteReader())
                                {
                                    if (ledgerReader.Read())
                                    {
                                        buyer = new Ledger();
                                        //buyer._Id = reader["Id"] as int?;
                                        buyer.CompanyId = ledgerReader["CompanyId"] as long?;
                                        buyer.LedgerType = ledgerReader["LedgerType"] as string;
                                        buyer.LedgerId = ledgerReader["LedgerId"] as long?;
                                        buyer.LedgerName = ledgerReader["LedgerName"] as string;
                                        buyer.Address1 = ledgerReader["Address1"] as string;
                                        buyer.Address2 = ledgerReader["Address2"] as string;
                                        buyer.Place = ledgerReader["Place"] as string;
                                        buyer.State = ledgerReader["State"] as string;
                                        buyer.Country = ledgerReader["Country"] as string;
                                        buyer.Gstin = ledgerReader["Gstin"] as string;
                                        buyer.DealerType = ledgerReader["DealerType"] as string;
                                        buyer.KannadaName = ledgerReader["KannadaName"] as string;
                                        buyer.KannadaPlace = ledgerReader["KannadaPlace"] as string;
                                        buyer.LedgerCode = ledgerReader["LedgerCode"] as string;
                                        buyer.ContactDetails = ledgerReader["ContactDetails"] as string;
                                        buyer.Pan = ledgerReader["Pan"] as string;
                                        buyer.BankName = ledgerReader["BankName"] as string;
                                        buyer.Ifsc = ledgerReader["Ifsc"] as string;
                                        buyer.AccountNo = ledgerReader["AccountNo"] as string;
                                        buyer.AccountingGroupId = ledgerReader["AccountingGroupId"] as int?;
                                        buyer.NameAndPlace = ledgerReader["NameAndPlace"] as string;
                                        buyer.PackingRate = ledgerReader["PackingRate"] as decimal?;
                                        buyer.HamaliRate = ledgerReader["HamaliRate"] as decimal?;
                                        buyer.WeighManFeeRate = ledgerReader["WeighManFeeRate"] as decimal?;
                                        buyer.DalaliRate = ledgerReader["DalaliRate"] as decimal?;
                                        buyer.CessRate = ledgerReader["CessRate"] as decimal?;
                                        buyer.DeprPerc = ledgerReader["DeprPerc"] as decimal?;
                                        buyer.CaplPerc = ledgerReader["CaplPerc"] as decimal?;
                                        buyer.ExpiryDate = ledgerReader["ExpiryDate"] as DateTime?;
                                        buyer.RenewDate = ledgerReader["RenewDate"] as DateTime?;
                                        buyer.CreatedDate = ledgerReader["CreatedDate"] as DateTime?;
                                        buyer.UpdatedDate = ledgerReader["UpdatedDate"] as DateTime?;
                                        buyer.CreatedBy = ledgerReader["CreatedBy"] as int?;
                                        buyer.CellNo = ledgerReader["CellNo"] as string;
                                        buyer.OtherCreated = ledgerReader["OtherCreated"] as int?;
                                        buyer.LocalName = ledgerReader["LocalName"] as string;
                                        buyer.LocalAddress = ledgerReader["LocalAddress"] as string;
                                        buyer.UnloadHamaliRate = ledgerReader["UnloadHamaliRate"] as decimal?;
                                        buyer.OldHdid = ledgerReader["OldHdid"] as long?;
                                        buyer.Gstn = ledgerReader["Gstn"] as string;
                                        buyer.Dlr_Type = ledgerReader["Dlr_Type"] as long?;
                                        buyer.Tp = ledgerReader["Tp"] as int?;
                                        buyer.Ist = ledgerReader["Ist"] as int?;
                                        buyer.Bname = ledgerReader["Bname"] as string;
                                        buyer.EmailId = ledgerReader["EmailId"] as string;
                                        buyer.PrintAcpay = ledgerReader["PrintAcpay"] as int?;
                                        buyer.ExclPay = ledgerReader["ExclPay"] as int?;
                                        buyer.OldLedgerId = ledgerReader["OldLedgerId"] as long?;
                                        buyer.AgentCode = ledgerReader["AgentCode"] as string;
                                        buyer.Pin = ledgerReader["Pin"] as string;
                                        buyer.StateCode = ledgerReader["StateCode"] as string;
                                        buyer.LegalName = ledgerReader["LegalName"] as string;
                                        buyer.NeftAcno = ledgerReader["NeftAcno"] as string;
                                        buyer.ChequeNo = ledgerReader["ChequeNo"] as string;
                                        buyer.AskIndtcs = ledgerReader["AskIndtcs"] as int?;
                                        buyer.CommodityAccount = ledgerReader["CommodityAccount"] as int?;
                                        buyer.ApplyTds = ledgerReader["ApplyTds"] as int?;
                                        buyer.Fssai = ledgerReader["Fssai"] as string;
                                        buyer.Dperc = ledgerReader["Dperc"] as int?;
                                        buyer.RentTdsperc = ledgerReader["RentTdsperc"] as decimal?;
                                        buyer.IsSelected = ledgerReader["IsSelected"] as int?;
                                        buyer.ToPrint = ledgerReader["ToPrint"] as int?;
                                        buyer.TotalCommission = ledgerReader["TotalCommission"] as decimal?;
                                        buyer.Tdsdeducted = ledgerReader["Tdsdeducted"] as decimal?;
                                        buyer.IsExported = ledgerReader["IsExported"] as int?;
                                        buyer.DeductFrieghtTds = ledgerReader["DeductFrieghtTds"] as int?;
                                        buyer.TotalForTds2 = ledgerReader["TotalForTds2"] as decimal?;
                                        buyer.Tds2deducted = ledgerReader["Tds2deducted"] as decimal?;
                                        buyer.ManualBookPageNo = ledgerReader["ManualBookPageNo"] as int?;
                                        buyer.QtoBeDeducted = ledgerReader["QtoBeDeducted"] as decimal?;
                                        buyer.Qtdsdeducted = ledgerReader["Qtdsdeducted"] as decimal?;
                                        buyer.TotalTv = ledgerReader["TotalTv"] as decimal?;
                                        buyer.TotalTurnoverforTcs = ledgerReader["TotalTurnoverforTcs"] as decimal?;
                                        buyer.Tcsdeducted = ledgerReader["Tcsdeducted"] as decimal?;
                                        buyer.TcstoBeDeducted = ledgerReader["TcstoBeDeducted"] as decimal?;
                                        buyer.BalanceTcs = ledgerReader["BalanceTcs"] as decimal?;
                                        buyer.BalanceQtds = ledgerReader["BalanceQtds"] as decimal?;
                                        buyer.RentToBeDeducted = ledgerReader["RentToBeDeducted"] as decimal?;
                                        buyer.RentTdsdeducted = ledgerReader["RentTdsdeducted"] as decimal?;
                                        buyer.BalanceRentTds = ledgerReader["BalanceRentTds"] as decimal?;
                                        buyer.ClosingBalanceCr = ledgerReader["ClosingBalanceCr"] as decimal?;
                                        buyer.TotalTransaction = ledgerReader["TotalTransaction"] as decimal?;
                                        buyer.ClosingBalanceDr = ledgerReader["ClosingBalanceDr"] as decimal?;
                                        buyer.TotalContactTv = ledgerReader["TotalContactTv"] as decimal?;
                                        buyer.OpeningBalance = ledgerReader["OpeningBalance"] as decimal?;
                                        buyer.CrDr = ledgerReader["CrDr"] as string;
                                        buyer.IsActive = ledgerReader["IsActive"] as bool?;
                                        buyer.LedType = ledgerReader["LedType"] as string;
                                    }

                                }
                            }
                            else
                            {
                                root.Response += " Associated LedgerId in BillSummery Not Found";
                                resString = " Associated LedgerId in BillSummery Not Found";
                                return root;
                            }

                            connection.Close();

                        
                    }
                    else
                    {
                        root.Response += " Associated CompanyId in BillSummery Not Found";
                        resString = " Associated CompanyId in BillSummery Not Found";
                        return root;
                    }

                    var precdocDtl = new PrecDocDtl();
                    precdocDtl.InvNo = billSummary.DisplayinvNo;

                    // Format TranctDate as dd-MM-yyyy
                    if (billSummary.TranctDate.HasValue)
                    {
                        precdocDtl.InvDt = billSummary.TranctDate.Value.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        // Handle the case when TranctDate is null
                        precdocDtl.InvDt = "N/A"; // Example: Set to "N/A" if the date is null
                    }

                    precdocDtl.OthRefNo = billSummary.Ponumber;

                    var docperdDetail = new DocPerdDtls();
                    // Format TranctDate as dd-MM-yyyy
                    if (billSummary.TranctDate.HasValue)
                    {
                        docperdDetail.InvEndDt = billSummary.TranctDate.Value.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        // Handle the case when TranctDate is null
                        docperdDetail.InvEndDt = "N/A"; // Example: Set to "N/A" if the date is null
                    }
                    if (billSummary.TranctDate.HasValue)
                    {
                        docperdDetail.InvStDt = billSummary.TranctDate.Value.AddDays(days).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        // Handle the case when TranctDate is null
                        docperdDetail.InvStDt = "N/A"; // Example: Set to "N/A" if the date is null
                    }

                    // Continue initializing the remaining objects...

                    var sval = new ValDtls();
                    sval.CesVal = 0;
                    sval.CgstVal = Math.Round(Convert.ToDecimal(billSummary.CSGSTValue), 2);
                    sval.Discount = 0;
                    sval.IgstVal = Math.Round(Convert.ToDecimal(billSummary.IGSTValue), 2);
                    sval.SgstVal = Math.Round(Convert.ToDecimal(billSummary.SGSTValue), 2);
                    sval.StCesVal = 0;
                    sval.TotInvVal = Math.Round(Convert.ToDecimal(billSummary.BillAmount), 2);
                    sval.TotInvValFc = Math.Round(Convert.ToDecimal(billSummary.TaxableValue), 2);

                    // Check if DontTaxFrieght is null and handle accordingly
                    bool dontTaxFreight = supplier != null && supplier.DontTaxFrieght == 0;



                    if (dontTaxFreight)
                        sval.OthChrg = Math.Round(Convert.ToDecimal(billSummary.ExpenseAmount1 + billSummary.ExpenseAmount2 + billSummary.ExpenseAmount3 + billSummary.TCSValue), 2);
                    else
                        sval.OthChrg = Math.Round(Convert.ToDecimal(billSummary.ExpenseAmount1 + billSummary.ExpenseAmount2 + billSummary.ExpenseAmount3 + billSummary.TCSValue + billSummary.Advance), 2);

                    sval.AssVal = Math.Round(billSummary.TotalAmount.Value, 2);
                    sval.RndOffAmt = Math.Round(billSummary.RoundOff.Value, 2);

                    var transDtls = new TranDtls();
                    if (billSummary.VochType != 12)
                    {
                        transDtls.TaxSch = "GST";
                        if (billSummary.IsSEZ == 0)
                            transDtls.SupTyp = "B2B";
                        else if (billSummary.VochType == 25)
                            transDtls.SupTyp = "B2B";
                        else
                            transDtls.SupTyp = "SEZWOP";

                        transDtls.RegRev = "N";
                        transDtls.EcmGstin = null;
                        transDtls.IgstOnIntra = "N";
                    }
                    else
                    {
                        transDtls.TaxSch = "GST";
                        transDtls.SupTyp = "EXPWOP";
                        transDtls.RegRev = "N";
                        transDtls.EcmGstin = null;
                        transDtls.IgstOnIntra = "N";
                    }

                    // Initialize transDtls properties...

                    var docDtls = new DocDtls();
                    docDtls.Typ = GetDocumentType(billSummary.VochType); // Implement a function to get document type based on VochType
                    docDtls.No = billSummary.DisplayinvNo;
                    docDtls.Dt = String.Format("{0:dd/MM/yyyy}", billSummary.TranctDate).Replace("-", "/");

                    // Continue initializing the remaining objects...

                    var paydetails = new PayDtls();

                    // Initialize paydetails properties...

                    var exportdtls = new ExpDtls();
                    if (billSummary.VochType == 12)
                    {
                        exportdtls.CntCode = null;
                        exportdtls.ShipBNo = billSummary.ShipBillNo;
                        exportdtls.ForCur = "INR";
                        exportdtls.Port = billSummary.PortName;
                        exportdtls.RefClm = "N";
                        //exportdtls.ShipBDt = Format(billsummery.ShipBillDate, "dd-MM-yyyy").Replace("-", "/");
                        //exportdtls.ShipBDt = string.Format(billsummery.ShipBillDate.ToString(), "dd-MM-yyyy").Replace("-", "/").Split(' ')[0];

                        exportdtls.ShipBDt = String.Format("{0:dd/MM/yyyy}", billSummary.TranctDate).Replace("-", "/"); ;
                        exportdtls.ExpDuty = "0";

                    }
                    else
                    {
                    }

 
                     var supp = new SellerDtls();

                    supp.LglNm = supplier.CompanyName;
                    supp.Addr1 = supplier.AddressLine1;
                    supp.Addr2 = supplier.AddressLine2;
                    supp.Gstin = supplier.Gstin;
                    // supp.Pin = System.Convert.ToInt32("390001");
                    supp.Pin = System.Convert.ToInt32(supplier.Pin);
                    supp.Em = supplier.Email;
                    supp.Ph = supplier.CellPhone;
                    supp.Loc = supplier.Place;
                    // supp.Stcd = "24";
                    supp.Stcd = supplier.Gstin?.Substring(0, 2);
                    supp.TrdNm = supplier.CompanyName;
                     
                    var dispdet = new DispDtls();
                    /* '' */
                    if (billSummary.VochType == 9 | billSummary.VochType == 27 | billSummary.VochType == 13 | billSummary.VochType == 10 | billSummary.VochType == 12 | billSummary.VochType == 13 | billSummary.VochType == 14 | billSummary.VochType == 15)
                    {
                        dispdet.Nm = billSummary.DispatcherName;
                        dispdet.Addr1 = billSummary.DispatcherAddress1;
                        dispdet.Addr2 = billSummary.DispatcherAddress2;
                        dispdet.Loc = billSummary.DispatcherPlace;
                        dispdet.Pin = billSummary.DispatcherPIN;
                        dispdet.Stcd = billSummary.DispatcherStatecode;
                    }
                    else if (billSummary.VochType == 6 | billSummary.VochType == 8)
                    {

                        dispdet.Nm = buyer.LedgerName;
                        dispdet.Addr1 = buyer.Address1;
                        dispdet.Addr2 = buyer.Address2;
                        dispdet.Loc = buyer.Place;
                        dispdet.Pin = buyer.Pin;
                        dispdet.Stcd = billSummary.StateCode2;
                    }

 
                    var EwayBillDetails = new EwbDtls();
                    /*EwayBillDetails.TransId = null;
                    EwayBillDetails.TransName = billSummary.Transporter;
                    EwayBillDetails.Distance = System.Convert.ToInt32(billSummary.Distance);
                    EwayBillDetails.TransDocNo = null;
                    EwayBillDetails.VehNo = billSummary.LorryNo;
                    EwayBillDetails.VehType = "1";
                    EwayBillDetails.TransMode = "1";*/

                    // Initialize EwayBillDetails properties...

                    var shipdet = new ShipDtls();
                    if (billSummary.VochType == 6 | billSummary.VochType == 8)
                    {

 

                        shipdet.LglNm = supplier.CompanyName;
                        shipdet.Addr1 = supplier.AddressLine1;
                        shipdet.Addr2 = supplier.AddressLine2;
                        shipdet.Gstin = supplier.Gstin;
                        shipdet.Loc = supplier.Place;
                        shipdet.Pin = System.Convert.ToInt32(supplier.Pin);
                        shipdet.Stcd = "29";
                        shipdet.TrdNm = supplier.CompanyName;
                    }

                    else
                    {
                        shipdet.LglNm = billSummary.DeliveryName;
                        shipdet.Addr1 = billSummary.DeliveryAddress1;
                        shipdet.Addr2 = billSummary.DeliveryAddress2;
                        shipdet.Gstin = buyer.Gstin;
                        shipdet.Loc = billSummary.DeliveryPlace;
                        if (billSummary.VochType == 14 | billSummary.VochType == 15 | billSummary.VochType == 6 | billSummary.VochType == 8)
                        {
                            shipdet.Pin = System.Convert.ToInt32(buyer.Pin);
                        }
                        else
                        {
                            shipdet.Pin = System.Convert.ToInt32((billSummary.DelPinCode == "" ? "0" : billSummary.DelPinCode));
                        }
                        shipdet.Stcd = billSummary.DeliveryStateCode;
                        shipdet.TrdNm = billSummary.DeliveryName;
                    }


                    var buyerdet = new BuyerDtls();
                    if (billSummary.VochType != 12)
                    {
                        if (buyer.LedgerName != null)
                        {
                            buyerdet.LglNm = buyer.LedgerName;
                        }
                        else
                        {
                            buyerdet.LglNm = "..";
                        }

                        buyerdet.Gstin = buyer.Gstin;
                        buyerdet.Addr1 = buyer.Address1;
                        buyerdet.Addr2 = buyer.Address2;
                        buyerdet.Em = buyer.EmailId;
                        buyerdet.Ph = buyer.CellNo;
                        buyerdet.Loc = buyer.Place;
                        buyerdet.Pin = System.Convert.ToInt32(buyer.Pin);
                        if (buyer.Gstin != "")
                        {
                            buyerdet.Pos = buyer.Gstin?.Substring(0, 2);
                            buyerdet.Stcd = buyer.Gstin?.Substring(0, 2);
                        }

                        buyerdet.TrdNm = buyer.LedgerName;


                    }
                    else
                    {
                        buyerdet.LglNm = buyer.LedgerName;
                        buyerdet.Gstin = "URP";
                        buyerdet.Addr1 = "**********";
                        buyerdet.Addr2 = "**********";
                        buyerdet.Em = "xyz@gmail.com";

                        buyerdet.Ph = "99999999";
                        buyerdet.Loc = "**********";
                        buyerdet.Pin = 999999;
                        buyerdet.Pos = "96";
                        buyerdet.Stcd = "96";
                        buyerdet.TrdNm = buyer.LedgerName;
                    }

                    // Initialize shipdet properties...

                    // Continue initializing the remaining objects...

                    root.PayDtls = paydetails;
                    root.PayDtls = paydetails;
                    root.BuyerDtls = buyerdet;
                    root.SellerDtls = supp;
                    // root.EwbDtls = EwayBillDetails;
                    root.AddlDocDtls = null;
                    root.DispDtls = dispdet;
                    root.DocDtls = docDtls;
                    root.ExpDtls = exportdtls;
                    root.RefDtls = null;
                    root.ShipDtls = shipdet;
                    root.TranDtls = transDtls;
                    root.ValDtls = sval;
                    root.Version = "1.01";

                    root.ItemList = new List<ItemList>();
                    int sr = 1;

                    // Loop through inventories and initialize ItemList
                    foreach (var item in inventories)
                    {
                        using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Commodity WHERE CommodityId = @CommodityId", connection))
                        {
                            command.Parameters.AddWithValue("@CommodityId", item.CommodityId);

                            connection.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var commodity = new Commodity
                                    {
                                        // Map properties from reader to Commodity object
                                        CommodityId = reader["CommodityId"] as long?,
                                        CommodityName = reader["CommodityName"] as string,
                                        IGST = reader["IGST"] as long?,
                                        HSN = reader["HSN"] as string,
                                        IsService = reader["IsService"] as bool?,
                                        CGST = reader["CGST"] as long?,
                                        SGST = reader["SGST"] as long?,

                                        // Include other properties as needed
                                    };

                                    var itemdet = new ItemList
                                    {
                                        SlNo = sr.ToString(),
                                        PrdDesc = commodity != null ? commodity.CommodityName : "",
                                        AssAmt = Math.Round(item.Amount.Value, 2),
                                        Barcde = "*-*",
                                        BchDtls = null,
                                        CesAmt = 0,
                                        CesNonAdvlAmt = 0,
                                        CesRt = 0,
                                        CgstAmt = Math.Round(Convert.ToDecimal(item.CGST), 2),
                                        Discount = 0,
                                        FreeQty = 0,
                                        GstRt = commodity != null ? Math.Round(Convert.ToDecimal(commodity.IGST), 2) : 0,
                                        HsnCd = commodity != null ? commodity.HSN : "",
                                        IgstAmt = item.IGST != null ? Math.Round(Convert.ToDecimal(item.IGST), 2) : 0,
                                        IsServc = commodity?.IsService != true ? "N" : "Y",
                                        OrdLineRef = "*-*",
                                        OrgCntry = "91",
                                        OthChrg = 0,
                                        PrdSlNo = "N",
                                        PreTaxVal = 1,
                                        Qty = commodity?.IsService != true ? Math.Round(Convert.ToDecimal(item.TotalWeight), 2) : 0,
                                        UnitPrice = commodity?.IsService != true ? Math.Round(Convert.ToDecimal((item.Rate / 100)), 2) : 0,
                                        Unit = commodity?.IsService != true ? "Kgs" : "OTH",
                                        SgstAmt = Math.Round(Convert.ToDecimal(item.SGST), 2),
                                        //SlNo = sr.ToString(),
                                        StateCesAmt = 0,
                                        StateCesNonAdvlAmt = 0,
                                        StateCesRt = 0,
                                        TotAmt = Math.Round(Convert.ToDecimal(item.Amount), 2),
                                        TotItemVal = Math.Round((Convert.ToDecimal(item.Amount) + Convert.ToDecimal(item.CGST) + Convert.ToDecimal(item.SGST) + Convert.ToDecimal(item.IGST)), 2),
                                        AttribDtls = null
                                    };

                                    sr++;
                                    root.ItemList.Add(itemdet);
                                }
                            }

                            connection.Close();

                        }
                    }
                }

                resString = "OK";

                return root;
            }
            catch (Exception ex)
            {
                resString = $" On Getting Data (GetInventoryDetails) from database: {ex.Message}";
                root.Response += $" On Getting Data (GetInventoryDetails) from database: {ex.Message}";
                return root;
            }
        }

        private static bool IsValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        private string GetDocumentType(long? vochType)
        {
            switch (vochType)
            {
                case 9:
                case 10:
                case 12:
                case 13:
                case 27:
                    return "INV";
                case 8:
                case 6:
                    return "CRN";
                case 15:
                    return "DBN";
                case 14:
                    return "DBN";
                default:
                    return "UNKNOWN";
            }
        }


    }
}