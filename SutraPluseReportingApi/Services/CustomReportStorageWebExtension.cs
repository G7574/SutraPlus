//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using DevExpress.XtraReports.UI;
//using SutraPlusReportApi.PredefinedReports;
//using SutraPlusReportApi.Data;
//using System.Collections.Immutable;
//using System.Web;
//using System;
//using DevExpress.Data.Helpers;
//using System.Collections;
//using System.ComponentModel;
//using System.Globalization;
//using System.Diagnostics;

//namespace SutraPlusReportApi.Services
//{
//    public class CustomReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
//    {
//        protected ReportDbContext DbContext { get; set; }
//        public CustomReportStorageWebExtension(ReportDbContext dbContext) {
//            this.DbContext = dbContext;
//        }

//        public override bool CanSetData(string url) {
//            // Determines whether a report with the specified URL can be saved.
//            // Add custom logic that returns **false** for reports that should be read-only.
//            // Return **true** if no valdation is required.
//            // This method is called only for valid URLs (if the **IsValidUrl** method returns **true**).

//            return true;
//        }

//        public override bool IsValidUrl(string url) {
//            // Determines whether the URL passed to the current report storage is valid.
//            // Implement your own logic to prohibit URLs that contain spaces or other specific characters.
//            // Return **true** if no validation is required.

//            return true;
//        }

//        public override byte[] GetData(string url) {
//            // Uses a specified URL to return report layout data stored within a report storage medium.
//            // This method is called if the **IsValidUrl** method returns **true**.
//            // You can use the **GetData** method to process report parameters sent from the client
//            // if the parameters are included in the report URL's query string..
//            //url = "ItemWise";
//            //if (url == "MonthView")
//            //{
//            //    var reportData = DbContext.Reports.FirstOrDefault(x => x.Name == url);
//            //    if (reportData != null)
//            //        return reportData.LayoutData;

//            //    if (ReportsFactory.Reports1.ContainsKey(url))
//            //    {
//            //        using var ms = new MemoryStream();
//            //        using XtraReport report = ReportsFactory.Reports1[url]();
//            //        report.SaveLayoutToXml(ms);
//            //        return ms.ToArray();
//            //    }
//            //}
//            //else if (url == "DateWise")
//            //{


//            //}
//            //else
//            //{
//                var reportData = DbContext.Reports.FirstOrDefault(x => x.Name == "ItemWise");
//                if (reportData != null)
//                    return reportData.LayoutData;

//                if (ReportsFactory.Reports.ContainsKey("ItemWise"))
//                {
//                    using var ms = new MemoryStream();
//                    using XtraReport report = ReportsFactory.Reports["ItemWise"]();
//                    report.SaveLayoutToXml(ms);
//                    return ms.ToArray();

//                    /* DateTime startDate = new DateTime(2022, 4, 11);
//                     DateTime endDate = new DateTime(2022, 11, 8);

//                     XtraReport report = ReportsFactory.Reports[url]();

//                     report.FilterString = $"Date >= #{startDate:dd/MM/yyyy}# AND Date <= #{endDate:dd/MM/yyyy}#";
//                     report.CreateDocument();*/

//                    /*using var ms = new MemoryStream();
//                    report.SaveLayoutToXml(ms);
//                    return ms.ToArray();*/
//               // }

//            }

//            throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", url));
//        }

//        public override Dictionary<string, string> GetUrls() {
//            // Returns a dictionary that contains the report names (URLs) and display names. 
//            // The Report Designer uses this method to populate the Open Report and Save Report dialogs.
//            var data = DbContext.Reports
//                .ToList()
//                .Select(x => x.Name)
//                .Union(ReportsFactory.Reports.Select(x => x.Key))
//                .ToDictionary<string, string>(x => x);
//            return DbContext.Reports
//                .ToList()
//                .Select(x => x.Name)
//                .Union(ReportsFactory.Reports.Select(x => x.Key))
//                .ToDictionary<string, string>(x => x);
//        }

//        public override void SetData(XtraReport report, string url) {
//            // Saves the specified report to the report storage with the specified name
//            // (saves existing reports only). 
//            using var stream = new MemoryStream(); report.SaveLayoutToXml(stream);
//            var reportData = DbContext.Reports.FirstOrDefault(x => x.Name == url);
//            if(reportData == null) {
//                DbContext.Reports.Add(new ReportItem { Name = url, LayoutData = stream.ToArray() });
//            } else {
//                reportData.LayoutData = stream.ToArray();
//            }
//            DbContext.SaveChanges();
//        }

//        public override string SetNewData(XtraReport report, string defaultUrl) {
//            // Allows you to validate and correct the specified name (URL).
//            // This method also allows you to return the resulting name (URL),
//            // and to save your report to a storage. The method is called only for new reports.
//            SetData(report, defaultUrl);
//            return defaultUrl;
//        }
//    }
//}
using DevExpress.XtraReports.Expressions;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Microsoft.AspNetCore.Hosting;
using SutraPlusReportApi.PredefinedReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PassParameterExample.Services
{
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        readonly string ReportDirectory;
        const string FileExtension = ".resx";
        public CustomReportStorageWebExtension(IWebHostEnvironment env)
        {
            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }
        private bool IsWithinReportsFolder(string url, string folder)
        {
            var rootDirectory = new DirectoryInfo(folder);
            var fileInfo = new FileInfo(Path.Combine(folder, url));
            return fileInfo.Directory.FullName.ToLower().StartsWith(rootDirectory.FullName.ToLower());
        }
        public override bool CanSetData(string url) { return true; }
        public override bool IsValidUrl(string url) { return Path.GetFileName(url) == url; }
        public override byte[] GetData(string url)
        {
            try
            {
              
                string[] parts = url.Split("&");
                string reportName = "XtraReport1";
                string parametersString = parts.Length > 1 ? parts[1] : String.Empty;

                using var ms = new MemoryStream();
                using XtraReport report = ReportsFactory.Reports["ItemWise"]();
                var parameters = HttpUtility.ParseQueryString(parametersString);
         

                if (report.Parameters["dateRangeStart"] == null)
                { 
                    var parameter = new Parameter();
                    parameter.Name = "dateRangeStart";
                    parameter.Value = DateTime.UtcNow;

                    report.Parameters.Add(parameter);
                }
                else
                {
                    // Update the existing parameter's value
                    report.Parameters["dateRangeStart"].Value = DateTime.UtcNow;
                }
                

                if (report.Parameters["dateRangeEnd"] == null)
                { 
                    var parameter = new Parameter();
                    parameter.Name = "dateRangeEnd";
                    parameter.Value = DateTime.UtcNow;

                    report.Parameters.Add(parameter);
                }
                else
                {
                    report.Parameters["dateRangeEnd"].Value = DateTime.UtcNow;
                }

             
                 report.SaveLayoutToXml(ms);
                return ms.ToArray();

               
            }
            catch (Exception ex)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                    "Could not get report data.", ex);
            }
            throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                string.Format("Could not find report '{0}'.", url));
        }

        public override Dictionary<string, string> GetUrls()
        {
            return Directory.GetFiles(ReportDirectory, "*" + FileExtension)
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .Union(ReportsFactory.Reports.Select(x => x.Key))
                                     .ToDictionary<string, string>(x => x);
        }
        public override void SetData(XtraReport report, string url)
        {
            if (!IsWithinReportsFolder(url, ReportDirectory))
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Invalid report name.");
            report.SaveLayoutToXml(Path.Combine(ReportDirectory, url + FileExtension));
        }
        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            SetData(report, defaultUrl);
            return defaultUrl;
        }
    }
}
