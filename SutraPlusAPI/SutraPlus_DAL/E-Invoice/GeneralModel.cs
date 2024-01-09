using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_DAL.E_Invoice
{
    public class GeneralModel
    {
        public string Status { get; set; }
        public string EwaybillStatus { get; set; }
        public List<ErrorResponse> ErrorResponse { get; set; }
        public object EInvoiceStatus { get; set; }
        public object AuthToken { get; set; }
        public string AckNo { get; set; }
        public string AckDate { get; set; }
        public string IRN { get; set; }
        public object SignedInvoice { get; set; }
        public string SignedQRCode { get; set; }
        public object QrCodeImage { get; set; }
        public object EwbNo { get; set; }
        public string EwbDt { get; set; }
        public string EwbValidTill { get; set; }
        public string Distance { get; set; }
        public string Response { get; set; }
    }

    public class ErrorResponse
    {
        public string ErrorInfo { get; set; }
        public object CoulumnName { get; set; }
        public object ColumnValue { get; set; }

    }
}
