using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace SutraPlusReportApi.PredefinedReports
{
    public static class ReportsFactory
    {
        public static Dictionary<string, Func<XtraReport>> Reports = new Dictionary<string, Func<XtraReport>>()
        {
            //["TestReport"] = () => new TestReport()
            ["ItemWise"] = () => new XtraReport1()
        }; 
        public static Dictionary<string, Func<XtraReport>> Reports1 = new Dictionary<string, Func<XtraReport>>()
        {
            //["TestReport"] = () => new TestReport()
            ["MonthView"] = () => new MonthView()
        };
        public static Dictionary<string, Func<XtraReport>> Reports2 = new Dictionary<string, Func<XtraReport>>()
        {
            //["TestReport"] = () => new TestReport()
            ["PartyCaseWise"] = () => new PartyCaseWise() 
        };
        public static Dictionary<string, Func<XtraReport>> Reports3 = new Dictionary<string, Func<XtraReport>>()
        {
            ["PartyWiseCommHamali"] = () => new PartyWiseCommHamali()
        };
        public static Dictionary<string, Func<XtraReport>> Reports4 = new Dictionary<string, Func<XtraReport>>()
        {
            ["ListAndRegisters"] = () => new ListAndRegisters()
        };
    }
}
