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
        //private string AUTH_URL = "https://powergstservice.microvistatech.com/api/MVEINVAuthenticate/EINVAuthentication";
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
                Ledger buyer = null;
                Company supplier = null;
                BillSummary billSummary = null;
                List<Inventory> inventories = new List<Inventory>();
                using (SqlConnection connection = new SqlConnection("Server=103.50.212.163;Database=SutraPlus;uid=sa;Password=root@123;TrustServerCertificate=True;"))
                {
                    connection.Open();

                    // Use raw SQL queries to fetch data
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Inventories WHERE VochNo = @InvoiceNo AND CompanyId = @CompanyId AND VochType = 9", connection))
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

                    using (SqlCommand command2 = new SqlCommand("SELECT * FROM BillSummaries WHERE VochNo = @InvoiceNo AND CompanyID = @CompanyId AND VochType = 9", connection))
                    {
                        command2.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                        command2.Parameters.AddWithValue("@CompanyId", CompanyId);

                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                billSummary._Id = reader["Id"] as int?;
                                billSummary.CompanyId = reader["CompanyId"] as long?;
                                billSummary.LedgerId = reader["LedgerId"] as long?;
                                billSummary.LedgerName = reader["LedgerName"] as string;
                                billSummary.Place = reader["Place"] as string;
                                billSummary.VochType = reader["VochType"] as long?;
                                billSummary.VoucherName = reader["VoucherName"] as string;
                                billSummary.DealerType = reader["DealerType"] as string;
                                billSummary.PAN = reader["PAN"] as string;
                                billSummary.GST = reader["GST"] as string;
                                billSummary.State = reader["State"] as string;
                                billSummary.InvoiceType = reader["InvoiceType"] as string;
                                billSummary.VochNo = reader["VochNo"] as long?;
                                billSummary.EwayBillNo = reader["EwayBillNo"] as string;
                                billSummary.Ponumber = reader["Ponumber"] as string;
                                billSummary.Transporter = reader["Transporter"] as string;
                                billSummary.LorryNo = reader["LorryNo"] as string;
                                billSummary.LorryOwnerName = reader["LorryOwnerName"] as string;
                                billSummary.DriverName = reader["DriverName"] as string;
                                billSummary.Dlno = reader["Dlno"] as string;
                                billSummary.CheckPost = reader["CheckPost"] as string;
                                billSummary.FrieghtPerBag = reader["FrieghtPerBag"] as decimal?;
                                billSummary.TotalFrieght = reader["TotalFrieght"] as decimal?;
                                billSummary.Advance = reader["Advance"] as decimal?;
                                billSummary.Balance = reader["Balance"] as decimal?;
                                billSummary.AREDate = reader["AREDate"] as string;
                                billSummary.ARENo = reader["ARENo"] as string;
                                billSummary.IsLessOrPlus = reader["IsLessOrPlus"] as bool?;
                                billSummary.ExpenseName1 = reader["ExpenseName1"] as string;
                                billSummary.ExpenseName2 = reader["ExpenseName2"] as string;
                                billSummary.ExpenseName3 = reader["ExpenseName3"] as string;
                                billSummary.ExpenseAmount1 = reader["ExpenseAmount1"] as decimal?;
                                billSummary.ExpenseAmount2 = reader["ExpenseAmount2"] as decimal?;
                                billSummary.ExpenseAmount3 = reader["ExpenseAmount3"] as decimal?;
                                billSummary.DeliveryName = reader["DeliveryName"] as string;
                                billSummary.DeliveryAddress1 = reader["DeliveryAddress1"] as string;
                                billSummary.DeliveryAddress2 = reader["DeliveryAddress2"] as string;
                                billSummary.DeliveryPlace = reader["DeliveryPlace"] as string;
                                billSummary.DeliveryState = reader["DeliveryState"] as string;
                                billSummary.DeliveryStateCode = reader["DeliveryStateCode"] as string;
                                billSummary.BillAmount = reader["BillAmount"] as decimal?;
                                billSummary.INWords = reader["INWords"] as string;
                                billSummary.FrieghtAmount = reader["FrieghtAmount"] as decimal?;
                                billSummary.RoundOff = reader["RoundOff"] as decimal?;
                                billSummary.TotalBags = reader["TotalBags"] as long?;
                                billSummary.TotalWeight = reader["TotalWeight"] as double?;
                                billSummary.TotalAmount = reader["TotalAmount"] as decimal?;
                                billSummary.PackingValue = reader["PackingValue"] as decimal?;
                                billSummary.HamaliValue = reader["HamaliValue"] as decimal?;
                                billSummary.WeighnamFeeValue = reader["WeighnamFeeValue"] as decimal?;
                                billSummary.DalaliValue = reader["DalaliValue"] as decimal?;
                                billSummary.CessValue = reader["CessValue"] as decimal?;
                                billSummary.TaxableValue = reader["TaxableValue"] as decimal?;
                                billSummary.SGSTValue = reader["SGSTValue"] as decimal?;
                                billSummary.CSGSTValue = reader["CSGSTValue"] as decimal?;
                                billSummary.IGSTValue = reader["IGSTValue"] as decimal?;
                                billSummary.UserId = reader["UserId"] as int?;
                                billSummary.SessionID = reader["SessionID"] as int?;
                                billSummary.PaymentBy = reader["PaymentBy"] as string;
                                billSummary.AmountReceived = reader["AmountReceived"] as decimal?;
                                billSummary.Change = reader["Change"] as decimal?;
                                billSummary.CardDetails = reader["CardDetails"] as string;
                                billSummary.BillTime = reader["BillTime"] as DateTime?;
                                billSummary.TranctDate = reader["TranctDate"] as DateTime?;
                                billSummary.CustomerName = reader["CustomerName"] as string;
                                billSummary.CustomerPlace = reader["CustomerPlace"] as string;
                                billSummary.CustomerContactNo = reader["CustomerContactNo"] as string;
                                billSummary.PartyInvoiceNumber = reader["PartyInvoiceNumber"] as string;
                                billSummary.OtherCreated = reader["OtherCreated"] as int?;
                                billSummary.Recent = reader["Recent"] as int?;
                                billSummary.PaymentReceived = reader["PaymentReceived"] as int?;
                                billSummary.TagName = reader["TagName"] as string;
                                billSummary.TagDate = reader["TagDate"] as DateTime?;
                                billSummary.ToPrint = reader["ToPrint"] as int?;
                                billSummary.frieghtPlus = reader["frieghtPlus"] as int?;
                                billSummary.StateCode1 = reader["StateCode1"] as string;
                                billSummary.StateCode2 = reader["StateCode2"] as string;
                                billSummary.ExpenseTax = reader["ExpenseTax"] as decimal?;
                                billSummary.FrieghtinBill = reader["FrieghtinBill"] as decimal?;
                                billSummary.LastOpenend = reader["LastOpenend"] as DateTime?;
                                billSummary.VikriCommission = reader["VikriCommission"] as decimal?;
                                billSummary.VikriULH = reader["VikriULH"] as decimal?;
                                billSummary.VikriCashAdvance = reader["VikriCashAdvance"] as decimal?;
                                billSummary.VikriFrieght = reader["VikriFrieght"] as decimal?;
                                billSummary.VikriEmpty = reader["VikriEmpty"] as decimal?;
                                billSummary.VikriNet = reader["VikriNet"] as decimal?;
                                billSummary.CashToFarmer = reader["CashToFarmer"] as decimal?;
                                billSummary.VikriOther1 = reader["VikriOther1"] as decimal?;
                                billSummary.VikriOther2 = reader["VikriOther2"] as decimal?;
                                billSummary.VikriOther1Name = reader["VikriOther1Name"] as string;
                                billSummary.VikriOther2Name = reader["VikriOther2Name"] as string;
                                billSummary.Note1 = reader["Note1"] as string;
                                billSummary.FromPlace = reader["FromPlace"] as string;
                                billSummary.ToPlace = reader["ToPlace"] as string;
                                billSummary.DisplayNo = reader["DisplayNo"] as string;
                                billSummary.DisplayinvNo = reader["DisplayinvNo"] as string;
                                billSummary.FrieghtLabel = reader["FrieghtLabel"] as string;
                                billSummary.FormName = reader["FormName"] as string;
                                billSummary.DCNote = reader["DCNote"] as string;
                                billSummary.WeightInString = reader["WeightInString"] as string;
                                billSummary.SGSTLabel = reader["SGSTLabel"] as string;
                                billSummary.CGSTLabel = reader["CGSTLabel"] as string;
                                billSummary.IGSTLabel = reader["IGSTLabel"] as string;
                                billSummary.TCSLabel = reader["TCSLabel"] as string;
                                billSummary.tdsperc = reader["tdsperc"] as decimal?;
                                billSummary.TCSValue = reader["TCSValue"] as decimal?;
                                billSummary.TCSPerc = reader["TCSPerc"] as decimal?;
                                billSummary.DelPinCode = reader["DelPinCode"] as string;
                                billSummary.CommodityID = reader["CommodityID"] as int?;
                                billSummary.QrCode = reader["QrCode"] as byte[];
                                billSummary.IsGSTUpload = reader["IsGSTUpload"] as int?;
                                billSummary.DispatcherName = reader["DispatcherName"] as string;
                                billSummary.DispatcherAddress1 = reader["DispatcherAddress1"] as string;
                                billSummary.DispatcherAddress2 = reader["DispatcherAddress2"] as string;
                                billSummary.DispatcherPlace = reader["DispatcherPlace"] as string;
                                billSummary.DispatcherPIN = reader["DispatcherPIN"] as string;
                                billSummary.DispatcherStatecode = reader["DispatcherStatecode"] as string;
                                billSummary.CountryCode = reader["CountryCode"] as string;
                                billSummary.ShipBillNo = reader["ShipBillNo"] as string;
                                billSummary.ForCur = reader["ForCur"] as string;
                                billSummary.PortName = reader["PortName"] as string;
                                billSummary.RefClaim = reader["RefClaim"] as string;
                                billSummary.ShipBillDate = reader["ShipBillDate"] as DateTime?;
                                billSummary.ExpDuty = reader["ExpDuty"] as string;
                                billSummary.monthNo = reader["monthNo"] as int?;
                                billSummary.Distance = reader["Distance"] as int?;
                                billSummary.GSTR1SectionName = reader["GSTR1SectionName"] as string;
                                billSummary.GSTR1InvoiceType = reader["GSTR1InvoiceType"] as string;
                                billSummary.EInvoiceNo = reader["EInvoiceNo"] as string;
                                billSummary.IsSEZ = reader["IsSEZ"] as int?;
                                billSummary.id = reader["id"] as int?;
                                billSummary.OriginalInvNo = reader["OriginalInvNo"] as string;
                                billSummary.OriginalInvDate = reader["OriginalInvDate"] as DateTime?;
                                billSummary.AprClosed = reader["AprClosed"] as int?;
                                billSummary.Discount = reader["Discount"] as decimal?;
                                billSummary.TwelveValue = reader["TwelveValue"] as decimal?;
                                billSummary.FiveValue = reader["FiveValue"] as decimal?;
                                billSummary.EgthValue = reader["EgthValue"] as decimal?;
                                billSummary.ACKNO = reader["ACKNO"] as string;
                                billSummary.IRNNO = reader["IRNNO"] as string;
                                billSummary.SignQRCODE = reader["SignQRCODE"] as string;
                                billSummary.IsActive = reader["IsActive"] as bool?;
                                billSummary.IsServiceInvoice = reader["IsServiceInvoice"] as bool?;
                            }
                        }
                    }



                    if (billSummary == null)
                    {
                        root.Response += " Associated BillSummery Not Found";
                        resString = " Associated BillSummery Not Found";
                        return root;
                    }

                    long? companyId = billSummary.CompanyId;
                    if (companyId != null)
                    {
                        using (SqlCommand command3 = new SqlCommand($"SELECT * FROM Companies WHERE CompanyId = {companyId}", connection))
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

                            if (supplier != null && supplier.Gstin.Length != 15)
                            {
                                root.Response += "Please Check Company GSTIN";
                                resString = "Please Check Company GSTIN";
                                return root;
                            }

                            long? ledgerId = billSummary.LedgerId;
                            if (ledgerId != null)
                            {
                                using (SqlCommand ledgerCommand = new SqlCommand($"SELECT * FROM Ledgers WHERE LedgerId = {ledgerId}", connection))
                                using (SqlDataReader ledgerReader = ledgerCommand.ExecuteReader())
                                {
                                    if (ledgerReader.Read())
                                    {
                                        buyer = new Ledger();
                                        //buyer._Id = reader["Id"] as int?;
                                        buyer.CompanyId = reader["CompanyId"] as long?;
                                        buyer.LedgerType = reader["LedgerType"] as string;
                                        buyer.LedgerId = reader["LedgerId"] as long?;
                                        buyer.LedgerName = reader["LedgerName"] as string;
                                        buyer.Address1 = reader["Address1"] as string;
                                        buyer.Address2 = reader["Address2"] as string;
                                        buyer.Place = reader["Place"] as string;
                                        buyer.State = reader["State"] as string;
                                        buyer.Country = reader["Country"] as string;
                                        buyer.Gstin = reader["Gstin"] as string;
                                        buyer.DealerType = reader["DealerType"] as string;
                                        buyer.KannadaName = reader["KannadaName"] as string;
                                        buyer.KannadaPlace = reader["KannadaPlace"] as string;
                                        buyer.LedgerCode = reader["LedgerCode"] as string;
                                        buyer.ContactDetails = reader["ContactDetails"] as string;
                                        buyer.Pan = reader["Pan"] as string;
                                        buyer.BankName = reader["BankName"] as string;
                                        buyer.Ifsc = reader["Ifsc"] as string;
                                        buyer.AccountNo = reader["AccountNo"] as string;
                                        buyer.AccountingGroupId = reader["AccountingGroupId"] as int?;
                                        buyer.NameAndPlace = reader["NameAndPlace"] as string;
                                        buyer.PackingRate = reader["PackingRate"] as decimal?;
                                        buyer.HamaliRate = reader["HamaliRate"] as decimal?;
                                        buyer.WeighManFeeRate = reader["WeighManFeeRate"] as decimal?;
                                        buyer.DalaliRate = reader["DalaliRate"] as decimal?;
                                        buyer.CessRate = reader["CessRate"] as decimal?;
                                        buyer.DeprPerc = reader["DeprPerc"] as decimal?;
                                        buyer.CaplPerc = reader["CaplPerc"] as decimal?;
                                        buyer.ExpiryDate = reader["ExpiryDate"] as DateTime?;
                                        buyer.RenewDate = reader["RenewDate"] as DateTime?;
                                        buyer.CreatedDate = reader["CreatedDate"] as DateTime?;
                                        buyer.UpdatedDate = reader["UpdatedDate"] as DateTime?;
                                        buyer.CreatedBy = reader["CreatedBy"] as int?;
                                        buyer.CellNo = reader["CellNo"] as string;
                                        buyer.OtherCreated = reader["OtherCreated"] as int?;
                                        buyer.LocalName = reader["LocalName"] as string;
                                        buyer.LocalAddress = reader["LocalAddress"] as string;
                                        buyer.UnloadHamaliRate = reader["UnloadHamaliRate"] as decimal?;
                                        buyer.OldHdid = reader["OldHdid"] as long?;
                                        buyer.Gstn = reader["Gstn"] as string;
                                        buyer.Dlr_Type = reader["Dlr_Type"] as long?;
                                        buyer.Tp = reader["Tp"] as int?;
                                        buyer.Ist = reader["Ist"] as int?;
                                        buyer.Bname = reader["Bname"] as string;
                                        buyer.EmailId = reader["EmailId"] as string;
                                        buyer.PrintAcpay = reader["PrintAcpay"] as int?;
                                        buyer.ExclPay = reader["ExclPay"] as int?;
                                        buyer.OldLedgerId = reader["OldLedgerId"] as long?;
                                        buyer.AgentCode = reader["AgentCode"] as string;
                                        buyer.Pin = reader["Pin"] as string;
                                        buyer.StateCode = reader["StateCode"] as string;
                                        buyer.LegalName = reader["LegalName"] as string;
                                        buyer.NeftAcno = reader["NeftAcno"] as string;
                                        buyer.ChequeNo = reader["ChequeNo"] as string;
                                        buyer.AskIndtcs = reader["AskIndtcs"] as int?;
                                        buyer.CommodityAccount = reader["CommodityAccount"] as int?;
                                        buyer.ApplyTds = reader["ApplyTds"] as int?;
                                        buyer.Fssai = reader["Fssai"] as string;
                                        buyer.Dperc = reader["Dperc"] as int?;
                                        buyer.RentTdsperc = reader["RentTdsperc"] as decimal?;
                                        buyer.IsSelected = reader["IsSelected"] as int?;
                                        buyer.ToPrint = reader["ToPrint"] as int?;
                                        buyer.TotalCommission = reader["TotalCommission"] as decimal?;
                                        buyer.Tdsdeducted = reader["Tdsdeducted"] as decimal?;
                                        buyer.IsExported = reader["IsExported"] as int?;
                                        buyer.DeductFrieghtTds = reader["DeductFrieghtTds"] as int?;
                                        buyer.TotalForTds2 = reader["TotalForTds2"] as decimal?;
                                        buyer.Tds2deducted = reader["Tds2deducted"] as decimal?;
                                        buyer.ManualBookPageNo = reader["ManualBookPageNo"] as int?;
                                        buyer.QtoBeDeducted = reader["QtoBeDeducted"] as decimal?;
                                        buyer.Qtdsdeducted = reader["Qtdsdeducted"] as decimal?;
                                        buyer.TotalTv = reader["TotalTv"] as decimal?;
                                        buyer.TotalTurnoverforTcs = reader["TotalTurnoverforTcs"] as decimal?;
                                        buyer.Tcsdeducted = reader["Tcsdeducted"] as decimal?;
                                        buyer.TcstoBeDeducted = reader["TcstoBeDeducted"] as decimal?;
                                        buyer.BalanceTcs = reader["BalanceTcs"] as decimal?;
                                        buyer.BalanceQtds = reader["BalanceQtds"] as decimal?;
                                        buyer.RentToBeDeducted = reader["RentToBeDeducted"] as decimal?;
                                        buyer.RentTdsdeducted = reader["RentTdsdeducted"] as decimal?;
                                        buyer.BalanceRentTds = reader["BalanceRentTds"] as decimal?;
                                        buyer.ClosingBalanceCr = reader["ClosingBalanceCr"] as decimal?;
                                        buyer.TotalTransaction = reader["TotalTransaction"] as decimal?;
                                        buyer.ClosingBalanceDr = reader["ClosingBalanceDr"] as decimal?;
                                        buyer.TotalContactTv = reader["TotalContactTv"] as decimal?;
                                        buyer.OpeningBalance = reader["OpeningBalance"] as decimal?;
                                        buyer.CrDr = reader["CrDr"] as string;
                                        buyer.IsActive = reader["IsActive"] as bool?;
                                        buyer.LedType = reader["LedType"] as string;
                                    }

                                }
                            }
                            else
                            {
                                root.Response += " Associated LedgerId in BillSummery Not Found";
                                resString = " Associated LedgerId in BillSummery Not Found";
                                return root;
                            }
                        }
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

                    // Initialize transDtls properties...

                    var docDtls = new DocDtls();
                    docDtls.Typ = GetDocumentType(billSummary.VochType); // Implement a function to get document type based on VochType
                    docDtls.No = billSummary.DisplayinvNo;
                    docDtls.Dt = billSummary.TranctDate.Value.ToString("dd-MM-yyyy");

                    // Continue initializing the remaining objects...

                    var paydetails = new PayDtls();

                    // Initialize paydetails properties...

                    var exportdtls = new ExpDtls();

                    // Initialize exportdtls properties...

                    var supp = new SellerDtls();

                    // Initialize supp properties...

                    var dispdet = new DispDtls();

                    // Initialize dispdet properties...

                    var EwayBillDetails = new EwbDtls();

                    // Initialize EwayBillDetails properties...

                    var shipdet = new ShipDtls();

                    // Initialize shipdet properties...

                    // Continue initializing the remaining objects...

                    root.PayDtls = paydetails;
                    //root.BuyerDtls = buyerdet;
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
                        using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Commodities WHERE CommodityId = @CommodityId", connection))
                        {
                            command.Parameters.AddWithValue("@CommodityId", item.CommodityId);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    var commodity = new Commodity
                                    {
                                        // Map properties from reader to Commodity object
                                        CommodityId = reader["CommodityId"] as long?,
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