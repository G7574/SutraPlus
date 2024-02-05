using DevExpress.CodeParser;
using DevExpress.XtraReports.Parameters;

namespace SutraPlusReportApi.PredefinedReports
{
    public partial class PartyCaseWise
    {
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //Required by the Designer
        private System.ComponentModel.IContainer components;

        //NOTE: The following procedure is required by the Designer
        //It can be modified using the Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters MsSqlConnectionParameters1 = new DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters();
            DevExpress.DataAccess.Sql.TableQuery TableQuery1 = new DevExpress.DataAccess.Sql.TableQuery();
            DevExpress.DataAccess.Sql.RelationInfo RelationInfo1 = new DevExpress.DataAccess.Sql.RelationInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo RelationColumnInfo1 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.RelationInfo RelationInfo2 = new DevExpress.DataAccess.Sql.RelationInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo RelationColumnInfo2 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.RelationInfo RelationInfo3 = new DevExpress.DataAccess.Sql.RelationInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo RelationColumnInfo3 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.RelationInfo RelationInfo4 = new DevExpress.DataAccess.Sql.RelationInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo RelationColumnInfo4 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo RelationColumnInfo5 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo1 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo1 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo2 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo3 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo4 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo5 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo6 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo7 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo8 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo9 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo10 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo11 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo2 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo12 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo13 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo14 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo3 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo15 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo16 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo17 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo18 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo19 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo20 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo21 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo22 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo23 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo24 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo25 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo26 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo27 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo28 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo29 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo30 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo31 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo32 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo4 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo33 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo34 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo35= new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo36 = new DevExpress.DataAccess.Sql.ColumnInfo();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PartyCaseWise));
            DevExpress.XtraReports.UI.XRSummary XrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary8 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.SqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.XrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblHeader = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.Others = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.XrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.XrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();


            var dateRangeParam = new Parameter();
            dateRangeParam.Name = "dateRange";
            dateRangeParam.Description = "Date Range:";
            dateRangeParam.Type = typeof(System.DateTime);
            var dateRangeSettings = new RangeParametersSettings();

            //this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate)";

            //this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate, ?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate) and CompanyId=?companyidrecord";


            this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate) and (VochType Between(?vochtype1,?vochtype2) or (VochType = ?vochtype1) or (VochType = ?vochtype2) ) and CompanyId=?companyidrecord";

            //Detail
            //
            this.Detail.HeightF = 0.0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            //TopMargin
            //
            this.TopMargin.HeightF = 0.0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            //BottomMargin
            //
            this.BottomMargin.HeightF = 13.54167F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            //SqlDataSource1
            //
            this.SqlDataSource1.ConnectionName = ".\\SQLEXPRESS_K2122_Connection 2";
            MsSqlConnectionParameters1.AuthorizationType = DevExpress.DataAccess.ConnectionParameters.MsSqlAuthorizationType.SqlServer;
            //MsSqlConnectionParameters1.DatabaseName = "K2223RGP";
            MsSqlConnectionParameters1.DatabaseName = "K2223RGP";
            MsSqlConnectionParameters1.UserName = "sa";
            MsSqlConnectionParameters1.Password = "root@123";
            MsSqlConnectionParameters1.ServerName = "103.50.212.163";
            this.SqlDataSource1.ConnectionParameters = MsSqlConnectionParameters1;
            this.SqlDataSource1.Name = "SqlDataSource1";

            TableQuery1.Name = "Company";
            RelationColumnInfo1.NestedKeyColumn = "CompanyId";
            RelationColumnInfo1.ParentKeyColumn = "CompanyId";
            RelationInfo1.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo1 });
            RelationInfo1.NestedTable = "Ledger";
            RelationInfo1.ParentTable = "Company";
            RelationColumnInfo2.NestedKeyColumn = "VoucherId";
            RelationColumnInfo2.ParentKeyColumn = "VochType";
            RelationInfo2.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo2 });
            RelationInfo2.NestedTable = "VoucherTypes";
            RelationInfo2.ParentTable = "BillSummary";
            RelationColumnInfo3.NestedKeyColumn = "CompanyID";
            RelationColumnInfo3.ParentKeyColumn = "CompanyId";
            RelationInfo3.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo3 });
            RelationInfo3.NestedTable = "BillSummary";
            RelationInfo3.ParentTable = "Company";
            RelationColumnInfo4.NestedKeyColumn = "CompanyID";
            RelationColumnInfo4.ParentKeyColumn = "CompanyId";
            RelationColumnInfo5.NestedKeyColumn = "LedgerId";
            RelationColumnInfo5.ParentKeyColumn = "LedgerId";
            RelationInfo4.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo4, RelationColumnInfo5 });
            RelationInfo4.NestedTable = "BillSummary";
            RelationInfo4.ParentTable = "Ledger";
            TableQuery1.Relations.AddRange(new DevExpress.DataAccess.Sql.RelationInfo[] { RelationInfo1, RelationInfo2, RelationInfo3, RelationInfo4 });
            TableInfo1.Name = "Company";
            ColumnInfo1.Name = "CompanyName";
            ColumnInfo2.Name = "AddressLine1";
            ColumnInfo3.Name = "AddressLine2";
            ColumnInfo4.Name = "AddressLine3";
            ColumnInfo5.Name = "Place";
            ColumnInfo6.Name = "GSTIN";
            ColumnInfo7.Name = "ReportTile1";
            ColumnInfo8.Name = "ReportTile2";
            ColumnInfo9.Name = "ThirdLineForReport";
            ColumnInfo10.Name = "SecondLineForReport";
            ColumnInfo11.Name = "PAN";
            
            TableInfo1.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo1, ColumnInfo2, ColumnInfo3, ColumnInfo4, ColumnInfo5, ColumnInfo6, ColumnInfo7, ColumnInfo8, ColumnInfo9, ColumnInfo10, ColumnInfo11,ColumnInfo13 });
            TableInfo2.Name = "Ledger";
            ColumnInfo12.Name = "LedgerName";
            ColumnInfo13.Alias = "Ledger_GSTIN";
            ColumnInfo13.Name = "GSTIN";
            ColumnInfo14.Alias = "Ledger_PAN";
            ColumnInfo14.Name = "PAN";
            TableInfo2.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo12, ColumnInfo13, ColumnInfo14 });
            TableInfo3.Name = "BillSummary";
            ColumnInfo15.Name = "PartyInvoiceNumber";
            ColumnInfo16.Name = "TranctDate";
            ColumnInfo17.Name = "TotalWeight";
            ColumnInfo18.Name = "TaxableValue";
            ColumnInfo19.Name = "SGSTValue";
            ColumnInfo20.Name = "CSGSTValue";
            ColumnInfo21.Name = "IGSTValue";
            ColumnInfo22.Name = "ExpenseAmount1";
            ColumnInfo23.Name = "ExpenseAmount2";
            ColumnInfo24.Name = "ExpenseAmount3";
            ColumnInfo25.Name = "BillAmount";
            ColumnInfo26.Name = "RoundOff";
            ColumnInfo27.Name = "ToPrint";
            ColumnInfo28.Name = "TotalBags";
            ColumnInfo29.Name = "VochNo";
            ColumnInfo30.Name = "VochType";
            ColumnInfo31.Name = "CessValue";
            ColumnInfo32.Name = "DalaliValue";
            ColumnInfo35.Name = "CompanyID";

            TableInfo3.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo15, ColumnInfo16, ColumnInfo17, ColumnInfo18, ColumnInfo19, ColumnInfo20, ColumnInfo21, ColumnInfo22, ColumnInfo23, ColumnInfo24, ColumnInfo25, ColumnInfo26, ColumnInfo27, ColumnInfo28, ColumnInfo29, ColumnInfo30, ColumnInfo31, ColumnInfo32, ColumnInfo35 });
            TableInfo4.Name = "VoucherTypes";
            ColumnInfo33.Name = "VoucherId";
            ColumnInfo34.Name = "VoucherName";
            
            TableInfo4.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo33, ColumnInfo34 });
            TableQuery1.Tables.AddRange(new DevExpress.DataAccess.Sql.TableInfo[] { TableInfo1, TableInfo2, TableInfo3, TableInfo4 });
            this.SqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] { TableQuery1 });
            this.SqlDataSource1.ResultSchemaSerializable = resources.GetString("SqlDataSource1.ResultSchemaSerializable");



            //
            //PageHeader
            //
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable1, this.lblHeader, this.XrLabel4, this.XrLabel3, this.XrLabel2, this.XrLabel1, this.XrPageInfo1 });
            this.PageHeader.HeightF = 136.4583F;
            this.PageHeader.Name = "PageHeader";
            //
            //XrTable1
            //
            this.XrTable1.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable1.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 120.5F);
            this.XrTable1.Name = "XrTable1";
            this.XrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow1 });
            this.XrTable1.SizeF = new System.Drawing.SizeF(735.4581F, 15.0F);
            this.XrTable1.StylePriority.UseBorders = false;
            this.XrTable1.StylePriority.UseFont = false;
            this.XrTable1.StylePriority.UseTextAlignment = false;
            this.XrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            //XrTableRow1
            //
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell1, this.XrTableCell3, this.XrTableCell11, this.XrTableCell2, this.XrTableCell5 });
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 1.0D;
            //
            //XrTableCell1
            //
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.StylePriority.UseTextAlignment = false;
            XrSummary1.FormatString = "{0:##,##,###.00}";
            this.XrTableCell1.Summary = XrSummary1;
            this.XrTableCell1.Text = "Party Name";
            this.XrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell1.Weight = 0.6019937030153254D;
            //
            //XrTableCell3
            //
            this.XrTableCell3.Name = "XrTableCell3";
            this.XrTableCell3.StylePriority.UseTextAlignment = false;
            this.XrTableCell3.Text = "GSTIN ";
            this.XrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell3.Weight = 0.55638973222585786D;
            //
            //XrTableCell11
            //
            this.XrTableCell11.Name = "XrTableCell11";
            this.XrTableCell11.StylePriority.UseTextAlignment = false;
            this.XrTableCell11.Text = "PAN";
            this.XrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell11.Weight = 0.50066031983783876D;
            //
            //XrTableCell2
            //
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.StylePriority.UseTextAlignment = false;
            XrSummary2.FormatString = "{0:##,##,###.00}";
            this.XrTableCell2.Summary = XrSummary2;
            this.XrTableCell2.Text = "Commission";
            this.XrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell2.Weight = 0.35473436354161403D;
            //
            //XrTableCell5
            //
            this.XrTableCell5.Name = "XrTableCell5";
            this.XrTableCell5.StylePriority.UseTextAlignment = false;
            XrSummary3.FormatString = "{0:##,##,###.00}";
            this.XrTableCell5.Summary = XrSummary3;
            this.XrTableCell5.Text = "Cess";
            this.XrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell5.Weight = 0.34279383229668237D;
            //
            //lblHeader
            //
            this.lblHeader.LocationFloat = new DevExpress.Utils.PointFloat(96.4582F, 102.625F);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.lblHeader.SizeF = new System.Drawing.SizeF(554.6251F, 15.0F);
            this.lblHeader.StylePriority.UseTextAlignment = false;
            this.lblHeader.Text = "Party Case Wise Report";
            this.lblHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel4
            //
            this.XrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.GSTIN") });
            this.XrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(9.999959F, 87.62499F);
            this.XrLabel4.Name = "XrLabel4";
            this.XrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel4.SizeF = new System.Drawing.SizeF(739.0001F, 15.0F);
            this.XrLabel4.StylePriority.UseTextAlignment = false;
            this.XrLabel4.Text = "XrLabel4";
            this.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel3
            //
            this.XrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Place") });
            this.XrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(8.957966F, 72.62498F);
            this.XrLabel3.Name = "XrLabel3";
            this.XrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel3.SizeF = new System.Drawing.SizeF(739.0001F, 15.0F);
            this.XrLabel3.StylePriority.UseTextAlignment = false;
            this.XrLabel3.Text = "XrLabel3";
            this.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel2
            //
            this.XrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.AddressLine1") });
            this.XrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(8.957966F, 57.62499F);
            this.XrLabel2.Name = "XrLabel2";
            this.XrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel2.SizeF = new System.Drawing.SizeF(739.0001F, 15.0F);
            this.XrLabel2.StylePriority.UseTextAlignment = false;
            this.XrLabel2.Text = "XrLabel2";
            this.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel1
            //
            this.XrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CompanyName") });
            this.XrLabel1.Font = new System.Drawing.Font("Times New Roman", 11.0F, System.Drawing.FontStyle.Bold);
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999959F, 34.62499F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(739.0001F, 23.0F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "XrLabel1";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrPageInfo1
            //
            this.XrPageInfo1.Format = "Page {0} of {1}";
            this.XrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(651.0833F, 102.625F);
            this.XrPageInfo1.Name = "XrPageInfo1";
            this.XrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrPageInfo1.SizeF = new System.Drawing.SizeF(96.87476F, 14.37502F);
            this.XrPageInfo1.StylePriority.UseTextAlignment = false;
            this.XrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            //Others
            //
            this.Others.DataMember = "Company";
            this.Others.Expression = "[BillAmount]-[CSGSTValue]-[SGSTValue]-[TaxableValue]-[IGSTValue]";
            this.Others.Name = "Others";
            //
            //ReportFooter
            //
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable3 });
            this.ReportFooter.HeightF = 18.75001F;
            this.ReportFooter.Name = "ReportFooter";
            //
            //XrTable3
            //
            this.XrTable3.Borders = (DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTable3.LocationFloat = new DevExpress.Utils.PointFloat(530.2678F, 0.0F);
            this.XrTable3.Name = "XrTable3";
            this.XrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow3 });
            this.XrTable3.SizeF = new System.Drawing.SizeF(218.7323F, 15.0F);
            this.XrTable3.StylePriority.UseBorders = false;
            this.XrTable3.StylePriority.UseFont = false;
            //
            //XrTableRow3
            //
            this.XrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell4, this.XrTableCell9 });
            this.XrTableRow3.Name = "XrTableRow3";
            this.XrTableRow3.Weight = 1.0D;
            //
            //XrTableCell4
            //
            this.XrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.DalaliValue") });
            this.XrTableCell4.Name = "XrTableCell4";
            this.XrTableCell4.StylePriority.UseTextAlignment = false;
            XrSummary4.FormatString = "{0:##,##,###.00}";
            XrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell4.Summary = XrSummary4;
            this.XrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell4.Weight = 0.7316277734339458D;
            //
            //XrTableCell9
            //
            this.XrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CessValue") });
            this.XrTableCell9.Name = "XrTableCell9";
            this.XrTableCell9.StylePriority.UseTextAlignment = false;
            XrSummary5.FormatString = "{0:##,##,###.00}";
            XrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell9.Summary = XrSummary5;
            this.XrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell9.Weight = 0.71388731741065226D;
            //
            //GroupHeader1
            //
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable2 });
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("LedgerName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader1.HeightF = 25.41666F;
            this.GroupHeader1.Name = "GroupHeader1";
            //
            //XrTable2
            //
            this.XrTable2.Borders = (DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable2.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.XrTable2.LocationFloat = new DevExpress.Utils.PointFloat(12.50002F, 0.0F);
            this.XrTable2.Name = "XrTable2";
            this.XrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow2 });
            this.XrTable2.SizeF = new System.Drawing.SizeF(736.5001F, 25.41666F);
            this.XrTable2.StylePriority.UseBorders = false;
            this.XrTable2.StylePriority.UseFont = false;
            //
            //XrTableRow2
            //
            this.XrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell6, this.XrTableCell10, this.XrTableCell12, this.XrTableCell7, this.XrTableCell8 });
            this.XrTableRow2.Name = "XrTableRow2";
            this.XrTableRow2.Weight = 1.0D;
            //
            //XrTableCell6
            //
            this.XrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.LedgerName") });
            this.XrTableCell6.Name = "XrTableCell6";
            this.XrTableCell6.StylePriority.UseTextAlignment = false;
            XrSummary6.FormatString = "{0:##,##,###.00}";
            this.XrTableCell6.Summary = XrSummary6;
            this.XrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell6.Weight = 0.60199375190789173D;
            //
            //XrTableCell10
            //
            this.XrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Ledger_GSTIN") });
            this.XrTableCell10.Name = "XrTableCell10";
            this.XrTableCell10.StylePriority.UseTextAlignment = false;
            this.XrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell10.Weight = 0.55638931870860209D;
            //
            //XrTableCell12
            //
            this.XrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Ledger_PAN") });
            this.XrTableCell12.Name = "XrTableCell12";
            this.XrTableCell12.StylePriority.UseTextAlignment = false;
            this.XrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell12.Weight = 0.50066059627247239D;
            //
            //XrTableCell7
            //
            this.XrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.DalaliValue") });
            this.XrTableCell7.Name = "XrTableCell7";
            this.XrTableCell7.StylePriority.UseTextAlignment = false;
            XrSummary7.FormatString = "{0:##,##,###.00}";
            XrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell7.Summary = XrSummary7;
            this.XrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell7.Weight = 0.35473435024534367D;
            //
            //XrTableCell8
            //
            this.XrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CessValue") });
            this.XrTableCell8.Name = "XrTableCell8";
            this.XrTableCell8.StylePriority.UseTextAlignment = false;
            XrSummary8.FormatString = "{0:##,##,###.00}";
            XrSummary8.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell8.Summary = XrSummary8;
            this.XrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell8.Weight = 0.34613267406007608D;
            //
            //GroupFooter1
            //
            this.GroupFooter1.HeightF = 0.0F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.RepeatEveryPage = true;
            //
            //PageFooter
            //
            this.PageFooter.HeightF = 18.75F;
            this.PageFooter.Name = "PageFooter";
            //
            //rptPartywiseCessReport
            //
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { this.Detail, this.TopMargin, this.BottomMargin, this.PageHeader, this.ReportFooter, this.GroupHeader1, this.GroupFooter1, this.PageFooter });
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] { this.Others });
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] { this.SqlDataSource1 });
            this.DataMember = "Company";
            this.DataSource = this.SqlDataSource1;
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.Margins = new System.Drawing.Printing.Margins(48, 20, 0, 14);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = (DevExpress.Drawing.Printing.DXPaperKind)System.Drawing.Printing.PaperKind.A4;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable3).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this).EndInit();

        }
        internal DevExpress.XtraReports.UI.DetailBand Detail;
        internal DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        internal DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        internal DevExpress.DataAccess.Sql.SqlDataSource SqlDataSource1;
        internal DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        internal DevExpress.XtraReports.UI.CalculatedField Others;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel4;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel3;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel2;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel1;
        internal DevExpress.XtraReports.UI.XRLabel lblHeader;
        internal DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        internal DevExpress.XtraReports.UI.XRPageInfo XrPageInfo1;
        internal DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        internal DevExpress.XtraReports.UI.XRTable XrTable1;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell5;
        internal DevExpress.XtraReports.UI.XRTable XrTable3;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow3;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell4;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell9;
        internal DevExpress.XtraReports.UI.XRTable XrTable2;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow2;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell6;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell7;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell8;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell3;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell10;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell11;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell12;
    }

}