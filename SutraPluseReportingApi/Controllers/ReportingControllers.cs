using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;
using DevExpress.XtraReports.UI;
using System.IO;
using SutraPlusReportApi.PredefinedReports;
using PassParameterExample.Services;
using DevExpress.XtraReports;
using System.Threading.Tasks;

namespace DXWebApplication5.Controllers {
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        } 
    }

    [Route("DXXRD")]
    [ApiController]

    public class CustomReportDesignerController : ReportDesignerController {        
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService) : base(controllerService) {
        }
        [HttpPost("GetDesignerModel")]
        public IActionResult GetDesignerModel([FromForm] string reportUrl, [FromServices] IReportDesignerClientSideModelGenerator modelGenerator) {
            var model = modelGenerator.GetModel(reportUrl, null, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
            return DesignerModel(model);
        }
         
        [HttpGet("[action]")]
        public ActionResult Export(string format = "pdf")
        { 
            format = format.ToLower();
            string contentType = string.Format("application/{0}", format);
            using (MemoryStream ms = new MemoryStream())
            {
                switch (format)
                {
                    case "pdf":
                        contentType = "application/pdf"; 
                        CustomReportStorageWebExtension.report.ExportToPdf(ms);
                        break;
                    case "xls":
                        contentType = "application/vnd.ms-excel";
                        CustomReportStorageWebExtension.report.ExportToXls(ms);
                        break;
                    case "xlsx":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        CustomReportStorageWebExtension.report.ExportToXls(ms);
                        break;
                }
                   
                return File(ms.ToArray(), contentType);
            }
        }

        [HttpGet("[action]")]
        public ActionResult SetDatabaseName(string dataBaseName, int choice)
        { 
                switch (choice)
            {
                case 1:
                    CustomReportStorageWebExtension.databaseName = dataBaseName;
                    break;
                case 2:
                    CustomReportStorageWebExtension.DataSource = dataBaseName;
                    break;
                case 3:
                    CustomReportStorageWebExtension.UserID = dataBaseName;
                    break;
                case 4:
                    CustomReportStorageWebExtension.Password = dataBaseName;
                    break;
            }
            return Ok("Database name updated successfully");
        }

    }

    public class CustomQueryBuilderController : QueryBuilderController {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService) {
        }
    }
}
