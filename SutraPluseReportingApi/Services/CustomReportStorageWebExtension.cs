using DevExpress.XtraCharts;
using DevExpress.XtraReports.Expressions;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraRichEdit.Import.Doc;
using Microsoft.AspNetCore.Hosting;
using SutraPlusReportApi.PredefinedReports;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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

                using var ms = new MemoryStream();
                using XtraReport report = ReportsFactory.Reports["ItemWise"]();
         
                if (report.Parameters["StartDate"] == null)
                {
                    var dateParameter = new Parameter()
                    {
                        Name = "StartDate",
                        Description = "From Date",
                        Value = DateTime.UtcNow,
                        Type = typeof(System.DateTime),
                    };
                    report.Parameters.Add(dateParameter);

                }
                else
                {
                    report.Parameters["StartDate"].Value = DateTime.UtcNow;
                }
                
                if (report.Parameters["EndDate"] == null)
                {

                    var dateParameter = new Parameter()
                    {
                        Name = "EndDate",
                        Description = "To Date",
                        Type = typeof(System.DateTime),
                        Value = DateTime.UtcNow,
                    };
                    report.Parameters.Add(dateParameter);

                }
                else
                {
                    report.Parameters["EndDate"].Value = DateTime.UtcNow;
                }
                //report.RequestParameters = false;

                report.CreateDocument();

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
