using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.Models.DTO
{
    public class BillSummaryDTO
    {
        public long CompanyId { get; set; }
        public long LedgerId { get; set; }
        public long VochType { get; set; }
        public long VochNo { get; set; }
        public string DisplayinvNo { get; set; }
        public DateTime TranctDate { get; set; }
        public string Ponumber { get; set; }
        public string StateCode2 { get; set; }
        public decimal CessValue { get; set; }
        public decimal CSGSTValue { get; set; }
        public decimal IGSTValue { get; set; }
        public decimal SGSTValue { get; set; }
        public decimal BillAmount { get; set; }
        public decimal TaxableValue { get; set; }
        public decimal ExpenseAmount1 { get; set; }
        public decimal ExpenseAmount2 { get; set; }
        public decimal ExpenseAmount3 { get; set; }
        public decimal TCSValue { get; set; }
        public decimal Advance { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal RoundOff { get; set; }
        public int IsSEZ { get; set; }
        public string ShipBillNo { get; set; }
        public string PortName { get; set; }
        public DateTime ShipBillDate { get; set; }
        public string DispatcherName { get; set; }
        public string DispatcherAddress1 { get; set; }
        public string DispatcherAddress2 { get; set; }
        public string DispatcherPlace { get; set; }
        public string DispatcherPIN { get; set; }
        public string DispatcherStatecode { get; set; }
        public string StateCode1 { get; set; }
        public string Transporter { get; set; }
        public int Distance { get; set; }
        public string LorryNo { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryPlace { get; set; }
        public string DelPinCode { get; set; }
        public double TotalWeight { get; set; }
        public string PartyInvoiceNumber { get; set; }
        public string DeliveryStateCode { get; set; }
        public bool IsServiceInvoice  { get; set; }
    }
}
