using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SutraPlusReportApi.PredefinedReports
{
    public static class ReportsFactory
    {
        public static Dictionary<string, Func<XtraReport>> Reports = new Dictionary<string, Func<XtraReport>>()
        {
            //["TestReport"] = () => new TestReport()
            ["TestReport"] = () => new XtraReport1()
        };
        public static Dictionary<string, Func<XtraReport>> Reports1 = new Dictionary<string, Func<XtraReport>>()
        {
            //["TestReport"] = () => new TestReport()
            ["MonthView"] = () => new MonthView()
        };
    }
}
