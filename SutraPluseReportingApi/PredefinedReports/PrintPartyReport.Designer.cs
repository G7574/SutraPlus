
using DevExpress.XtraReports.Parameters;
using PassParameterExample.Services;

namespace SutraPlusReportApi.PredefinedReports
{
    public partial class PrintPartyReport : DevExpress.XtraReports.UI.XtraReport
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
            DevExpress.DataAccess.Sql.TableInfo TableInfo1 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo1 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo2 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo3 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo4 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo2 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo5 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo6 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo7 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo8 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo9 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo10 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo11 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo12 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo13 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo14 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo15 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo51 = new DevExpress.DataAccess.Sql.ColumnInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPartyReport));
            DevExpress.XtraReports.UI.XRSummary XrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.SqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.PageHeaderBand1 = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.XrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeaderBand1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.PageFooterBand1 = new DevExpress.XtraReports.UI.PageFooterBand();
            this.ReportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.FieldCaption = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DataField = new DevExpress.XtraReports.UI.XRControlStyle();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();

            var dateRangeParam = new Parameter();
            dateRangeParam.Name = "dateRange";
            dateRangeParam.Description = "Date Range:";
            dateRangeParam.Type = typeof(System.DateTime);
            var dateRangeSettings = new RangeParametersSettings();

            if (CustomReportStorageWebExtension.doWeHaveLedgerId)
            {
                this.Report.FilterString = "CompanyId=?companyidrecord and AccountingGroupId=?accountinggroupId";
            }
            else
            {
                this.Report.FilterString = "CompanyId=?companyidrecord";
            }

            //this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate) and (VochType Between(?vochtype1,?vochtype2) or (VochType = ?vochtype1) or (VochType = ?vochtype2) ) and CompanyId=?companyidrecord";
            //
            //Detail
            //
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable1 });
            this.Detail.HeightF = 20.62502F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("LedgerName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.Detail.StyleName = "DataField";
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
            this.BottomMargin.HeightF = 100.0F;
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
            TableQuery1.Relations.AddRange(new DevExpress.DataAccess.Sql.RelationInfo[] { RelationInfo1 });
            TableInfo1.Name = "Company";
            ColumnInfo1.Name = "CompanyName";
            ColumnInfo2.Name = "AddressLine1";
            ColumnInfo3.Name = "Place";
            ColumnInfo4.Name = "Title";
            TableInfo1.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo1, ColumnInfo2, ColumnInfo3, ColumnInfo4 });
            TableInfo2.Name = "Ledger";
            ColumnInfo5.Name = "LedgerName";
            ColumnInfo6.Name = "Address1";
            ColumnInfo7.Alias = "Ledger_Place";
            ColumnInfo7.Name = "Place";
            ColumnInfo8.Name = "State";
            ColumnInfo9.Name = "GSTIN";
            ColumnInfo10.Name = "CellNo";
            ColumnInfo11.Name = "ToPrint";
            ColumnInfo12.Name = "address2";
            ColumnInfo13.Name = "PIN";
            ColumnInfo14.Name = "PAN";
            ColumnInfo15.Name = "CompanyId";
            ColumnInfo51.Name = "AccountingGroupId";

            TableInfo2.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo5, ColumnInfo6, ColumnInfo7, ColumnInfo8, ColumnInfo9, ColumnInfo10, ColumnInfo11, ColumnInfo12, ColumnInfo13, ColumnInfo14 , ColumnInfo15, ColumnInfo51 });
            TableQuery1.Tables.AddRange(new DevExpress.DataAccess.Sql.TableInfo[] { TableInfo1, TableInfo2 });
            this.SqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] { TableQuery1 });
            this.SqlDataSource1.ResultSchemaSerializable = resources.GetString("SqlDataSource1.ResultSchemaSerializable");
            //
            //PageHeaderBand1
            //
            this.PageHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrLabel3, this.XrLabel2, this.XrLabel1, this.XrTable2 });
            this.PageHeaderBand1.HeightF = 111.4583F;
            this.PageHeaderBand1.Name = "PageHeaderBand1";
            //
            //XrLabel3
            //
            this.XrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Place") });
            this.XrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(26.04167F, 67.83331F);
            this.XrLabel3.Name = "XrLabel3";
            this.XrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel3.SizeF = new System.Drawing.SizeF(982.4999F, 23.0F);
            this.XrLabel3.StylePriority.UseTextAlignment = false;
            this.XrLabel3.Text = "XrLabel3";
            this.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel2
            //
            this.XrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.AddressLine1") });
            this.XrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(26.04167F, 44.83331F);
            this.XrLabel2.Name = "XrLabel2";
            this.XrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel2.SizeF = new System.Drawing.SizeF(982.4999F, 23.0F);
            this.XrLabel2.StylePriority.UseTextAlignment = false;
            this.XrLabel2.Text = "XrLabel2";
            this.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel1
            //
            this.XrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CompanyName") });
            this.XrLabel1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(26.04167F, 21.83331F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(982.4999F, 23.0F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "XrLabel1";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTable2
            //
            this.XrTable2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.XrTable2.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0.0F, 90.83331F);
            this.XrTable2.Name = "XrTable2";
            this.XrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow2 });
            this.XrTable2.SizeF = new System.Drawing.SizeF(1069.0F, 20.62502F);
            this.XrTable2.StylePriority.UseBorders = false;
            this.XrTable2.StylePriority.UseFont = false;
            this.XrTable2.StylePriority.UseTextAlignment = false;
            this.XrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTableRow2
            //
            this.XrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell4, this.XrTableCell5, this.XrTableCell1, this.XrTableCell6, this.XrTableCell10, this.XrTableCell11, this.XrTableCell12, this.XrTableCell14, this.XrTableCell16 });
            this.XrTableRow2.Name = "XrTableRow2";
            this.XrTableRow2.Weight = 1.0D;
            //
            //XrTableCell4
            //
            this.XrTableCell4.BackColor = System.Drawing.Color.White;
            this.XrTableCell4.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell4.CanGrow = false;
            this.XrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell4.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell4.Name = "XrTableCell4";
            this.XrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell4.StylePriority.UseBackColor = false;
            this.XrTableCell4.StylePriority.UseBorders = false;
            this.XrTableCell4.StylePriority.UseFont = false;
            this.XrTableCell4.StylePriority.UseForeColor = false;
            this.XrTableCell4.StylePriority.UsePadding = false;
            this.XrTableCell4.Text = "S. No.";
            this.XrTableCell4.Weight = 0.16288433130891808D;
            //
            //XrTableCell5
            //
            this.XrTableCell5.BackColor = System.Drawing.Color.White;
            this.XrTableCell5.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell5.CanGrow = false;
            this.XrTableCell5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell5.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell5.Name = "XrTableCell5";
            this.XrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell5.StylePriority.UseBackColor = false;
            this.XrTableCell5.StylePriority.UseBorders = false;
            this.XrTableCell5.StylePriority.UseFont = false;
            this.XrTableCell5.StylePriority.UseForeColor = false;
            this.XrTableCell5.StylePriority.UsePadding = false;
            this.XrTableCell5.StylePriority.UseTextAlignment = false;
            this.XrTableCell5.Text = "Name";
            this.XrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell5.Weight = 0.898652598973066D;
            //
            //XrTableCell12
            //
            this.XrTableCell12.BackColor = System.Drawing.Color.White;
            this.XrTableCell12.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell12.CanGrow = false;
            this.XrTableCell12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell12.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell12.Name = "XrTableCell12";
            this.XrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrTableCell12.StylePriority.UseBackColor = false;
            this.XrTableCell12.StylePriority.UseBorders = false;
            this.XrTableCell12.StylePriority.UseFont = false;
            this.XrTableCell12.StylePriority.UseForeColor = false;
            this.XrTableCell12.StylePriority.UsePadding = false;
            this.XrTableCell12.StylePriority.UseTextAlignment = false;
            this.XrTableCell12.Text = "State";
            this.XrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell12.Weight = 0.424088786761892D;
            //
            //XrTableCell14
            //
            this.XrTableCell14.BackColor = System.Drawing.Color.White;
            this.XrTableCell14.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell14.CanGrow = false;
            this.XrTableCell14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell14.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell14.Name = "XrTableCell14";
            this.XrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrTableCell14.StylePriority.UseBackColor = false;
            this.XrTableCell14.StylePriority.UseBorders = false;
            this.XrTableCell14.StylePriority.UseFont = false;
            this.XrTableCell14.StylePriority.UseForeColor = false;
            this.XrTableCell14.StylePriority.UsePadding = false;
            this.XrTableCell14.StylePriority.UseTextAlignment = false;
            this.XrTableCell14.Text = "GSTIN";
            this.XrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell14.Weight = 0.76541352904446969D;
            //
            //XrTableCell16
            //
            this.XrTableCell16.BackColor = System.Drawing.Color.White;
            this.XrTableCell16.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell16.CanGrow = false;
            this.XrTableCell16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell16.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell16.Name = "XrTableCell16";
            this.XrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrTableCell16.StylePriority.UseBackColor = false;
            this.XrTableCell16.StylePriority.UseBorders = false;
            this.XrTableCell16.StylePriority.UseFont = false;
            this.XrTableCell16.StylePriority.UseForeColor = false;
            this.XrTableCell16.StylePriority.UsePadding = false;
            this.XrTableCell16.StylePriority.UseTextAlignment = false;
            this.XrTableCell16.Text = "PAN";
            this.XrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell16.Weight = 0.58711990302957262D;
            //
            //GroupHeaderBand1
            //
            this.GroupHeaderBand1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("Place", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeaderBand1.HeightF = 0.0F;
            this.GroupHeaderBand1.Name = "GroupHeaderBand1";
            this.GroupHeaderBand1.StyleName = "DataField";
            //
            //PageFooterBand1
            //
            this.PageFooterBand1.HeightF = 29.0F;
            this.PageFooterBand1.Name = "PageFooterBand1";
            //
            //ReportHeaderBand1
            //
            this.ReportHeaderBand1.HeightF = 0.0F;
            this.ReportHeaderBand1.Name = "ReportHeaderBand1";
            //
            //Title
            //
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1.0F;
            this.Title.Font = new System.Drawing.Font("Times New Roman", 20.0F, System.Drawing.FontStyle.Bold);
            this.Title.ForeColor = System.Drawing.Color.Maroon;
            this.Title.Name = "Title";
            //
            //FieldCaption
            //
            this.FieldCaption.BackColor = System.Drawing.Color.Transparent;
            this.FieldCaption.BorderColor = System.Drawing.Color.Black;
            this.FieldCaption.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.FieldCaption.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.FieldCaption.BorderWidth = 1.0F;
            this.FieldCaption.Font = new System.Drawing.Font("Arial", 10.0F, System.Drawing.FontStyle.Bold);
            this.FieldCaption.ForeColor = System.Drawing.Color.Maroon;
            this.FieldCaption.Name = "FieldCaption";
            //
            //PageInfo
            //
            this.PageInfo.BackColor = System.Drawing.Color.Transparent;
            this.PageInfo.BorderColor = System.Drawing.Color.Black;
            this.PageInfo.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.PageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PageInfo.BorderWidth = 1.0F;
            this.PageInfo.Font = new System.Drawing.Font("Times New Roman", 10.0F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.Black;
            this.PageInfo.Name = "PageInfo";
            //
            //DataField
            //
            this.DataField.BackColor = System.Drawing.Color.Transparent;
            this.DataField.BorderColor = System.Drawing.Color.Black;
            this.DataField.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.DataField.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DataField.BorderWidth = 1.0F;
            this.DataField.Font = new System.Drawing.Font("Times New Roman", 10.0F);
            this.DataField.ForeColor = System.Drawing.Color.Black;
            this.DataField.Name = "DataField";
            //
            //XrTableCell1
            //
            this.XrTableCell1.BackColor = System.Drawing.Color.White;
            this.XrTableCell1.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell1.CanGrow = false;
            this.XrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell1.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell1.StylePriority.UseBackColor = false;
            this.XrTableCell1.StylePriority.UseBorders = false;
            this.XrTableCell1.StylePriority.UseFont = false;
            this.XrTableCell1.StylePriority.UseForeColor = false;
            this.XrTableCell1.StylePriority.UsePadding = false;
            this.XrTableCell1.StylePriority.UseTextAlignment = false;
            this.XrTableCell1.Text = "Address-1";
            this.XrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell1.Weight = 0.61878677571009888D;
            //
            //XrTableCell6
            //
            this.XrTableCell6.BackColor = System.Drawing.Color.White;
            this.XrTableCell6.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell6.CanGrow = false;
            this.XrTableCell6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell6.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell6.Name = "XrTableCell6";
            this.XrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell6.StylePriority.UseBackColor = false;
            this.XrTableCell6.StylePriority.UseBorders = false;
            this.XrTableCell6.StylePriority.UseFont = false;
            this.XrTableCell6.StylePriority.UseForeColor = false;
            this.XrTableCell6.StylePriority.UsePadding = false;
            this.XrTableCell6.StylePriority.UseTextAlignment = false;
            this.XrTableCell6.Text = "Address-2";
            this.XrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell6.Weight = 0.59007045732436136D;
            //
            //XrTableCell10
            //
            this.XrTableCell10.BackColor = System.Drawing.Color.White;
            this.XrTableCell10.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell10.CanGrow = false;
            this.XrTableCell10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell10.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell10.Name = "XrTableCell10";
            this.XrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell10.StylePriority.UseBackColor = false;
            this.XrTableCell10.StylePriority.UseBorders = false;
            this.XrTableCell10.StylePriority.UseFont = false;
            this.XrTableCell10.StylePriority.UseForeColor = false;
            this.XrTableCell10.StylePriority.UsePadding = false;
            this.XrTableCell10.StylePriority.UseTextAlignment = false;
            this.XrTableCell10.Text = "Place";
            this.XrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell10.Weight = 0.538666241647546D;
            //
            //XrTableCell11
            //
            this.XrTableCell11.BackColor = System.Drawing.Color.White;
            this.XrTableCell11.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell11.CanGrow = false;
            this.XrTableCell11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell11.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell11.Name = "XrTableCell11";
            this.XrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell11.StylePriority.UseBackColor = false;
            this.XrTableCell11.StylePriority.UseBorders = false;
            this.XrTableCell11.StylePriority.UseFont = false;
            this.XrTableCell11.StylePriority.UseForeColor = false;
            this.XrTableCell11.StylePriority.UsePadding = false;
            this.XrTableCell11.StylePriority.UseTextAlignment = false;
            this.XrTableCell11.Text = "PIN";
            this.XrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell11.Weight = 0.36483417060974432D;
            //
            //XrTable1
            //
            this.XrTable1.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.XrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0.0F, 0.0F);
            this.XrTable1.Name = "XrTable1";
            this.XrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow1 });
            this.XrTable1.SizeF = new System.Drawing.SizeF(1069.0F, 20.62502F);
            this.XrTable1.StylePriority.UseBorders = false;
            this.XrTable1.StylePriority.UseFont = false;
            this.XrTable1.StylePriority.UseTextAlignment = false;
            this.XrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTableRow1
            //
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell2, this.XrTableCell3, this.XrTableCell7, this.XrTableCell8, this.XrTableCell9, this.XrTableCell13, this.XrTableCell15, this.XrTableCell17, this.XrTableCell18 });
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 1.0D;
            //
            //XrTableCell2
            //
            this.XrTableCell2.BackColor = System.Drawing.Color.White;
            this.XrTableCell2.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell2.CanGrow = false;
            this.XrTableCell2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell2.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell2.StylePriority.UseBackColor = false;
            this.XrTableCell2.StylePriority.UseBorders = false;
            this.XrTableCell2.StylePriority.UseFont = false;
            this.XrTableCell2.StylePriority.UseForeColor = false;
            this.XrTableCell2.StylePriority.UsePadding = false;
            XrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            XrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell2.Summary = XrSummary1;
            this.XrTableCell2.Text = "S. No.";
            this.XrTableCell2.Weight = 0.16288433130891808D;
            //
            //XrTableCell3
            //
            this.XrTableCell3.BackColor = System.Drawing.Color.White;
            this.XrTableCell3.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell3.CanGrow = false;
            this.XrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.LedgerName") });
            this.XrTableCell3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell3.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell3.Name = "XrTableCell3";
            this.XrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell3.StylePriority.UseBackColor = false;
            this.XrTableCell3.StylePriority.UseBorders = false;
            this.XrTableCell3.StylePriority.UseFont = false;
            this.XrTableCell3.StylePriority.UseForeColor = false;
            this.XrTableCell3.StylePriority.UsePadding = false;
            this.XrTableCell3.StylePriority.UseTextAlignment = false;
            this.XrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell3.Weight = 0.898652598973066D;
            //
            //XrTableCell7
            //
            this.XrTableCell7.BackColor = System.Drawing.Color.White;
            this.XrTableCell7.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Address1") });
            this.XrTableCell7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell7.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell7.Name = "XrTableCell7";
            this.XrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell7.StylePriority.UseBackColor = false;
            this.XrTableCell7.StylePriority.UseBorders = false;
            this.XrTableCell7.StylePriority.UseFont = false;
            this.XrTableCell7.StylePriority.UseForeColor = false;
            this.XrTableCell7.StylePriority.UsePadding = false;
            this.XrTableCell7.StylePriority.UseTextAlignment = false;
            this.XrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell7.Weight = 0.61878677571009888D;
            //
            //XrTableCell8
            //
            this.XrTableCell8.BackColor = System.Drawing.Color.White;
            this.XrTableCell8.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.address2") });
            this.XrTableCell8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell8.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell8.Name = "XrTableCell8";
            this.XrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell8.StylePriority.UseBackColor = false;
            this.XrTableCell8.StylePriority.UseBorders = false;
            this.XrTableCell8.StylePriority.UseFont = false;
            this.XrTableCell8.StylePriority.UseForeColor = false;
            this.XrTableCell8.StylePriority.UsePadding = false;
            this.XrTableCell8.StylePriority.UseTextAlignment = false;
            this.XrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell8.Weight = 0.59007045732436136D;
            //
            //XrTableCell9
            //
            this.XrTableCell9.BackColor = System.Drawing.Color.White;
            this.XrTableCell9.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell9.CanGrow = false;
            this.XrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Ledger_Place") });
            this.XrTableCell9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell9.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell9.Name = "XrTableCell9";
            this.XrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell9.StylePriority.UseBackColor = false;
            this.XrTableCell9.StylePriority.UseBorders = false;
            this.XrTableCell9.StylePriority.UseFont = false;
            this.XrTableCell9.StylePriority.UseForeColor = false;
            this.XrTableCell9.StylePriority.UsePadding = false;
            this.XrTableCell9.StylePriority.UseTextAlignment = false;
            this.XrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell9.Weight = 0.538666241647546D;
            //
            //XrTableCell13
            //
            this.XrTableCell13.BackColor = System.Drawing.Color.White;
            this.XrTableCell13.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell13.CanGrow = false;
            this.XrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.PIN") });
            this.XrTableCell13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell13.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell13.Name = "XrTableCell13";
            this.XrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100.0F);
            this.XrTableCell13.StylePriority.UseBackColor = false;
            this.XrTableCell13.StylePriority.UseBorders = false;
            this.XrTableCell13.StylePriority.UseFont = false;
            this.XrTableCell13.StylePriority.UseForeColor = false;
            this.XrTableCell13.StylePriority.UsePadding = false;
            this.XrTableCell13.StylePriority.UseTextAlignment = false;
            this.XrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell13.Weight = 0.36483417060974432D;
            //
            //XrTableCell15
            //
            this.XrTableCell15.BackColor = System.Drawing.Color.White;
            this.XrTableCell15.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell15.CanGrow = false;
            this.XrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.State") });
            this.XrTableCell15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell15.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell15.Name = "XrTableCell15";
            this.XrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrTableCell15.StylePriority.UseBackColor = false;
            this.XrTableCell15.StylePriority.UseBorders = false;
            this.XrTableCell15.StylePriority.UseFont = false;
            this.XrTableCell15.StylePriority.UseForeColor = false;
            this.XrTableCell15.StylePriority.UsePadding = false;
            this.XrTableCell15.StylePriority.UseTextAlignment = false;
            this.XrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell15.Weight = 0.424088786761892D;
            //
            //XrTableCell17
            //
            this.XrTableCell17.BackColor = System.Drawing.Color.White;
            this.XrTableCell17.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell17.CanGrow = false;
            this.XrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.GSTIN") });
            this.XrTableCell17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell17.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell17.Name = "XrTableCell17";
            this.XrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrTableCell17.StylePriority.UseBackColor = false;
            this.XrTableCell17.StylePriority.UseBorders = false;
            this.XrTableCell17.StylePriority.UseFont = false;
            this.XrTableCell17.StylePriority.UseForeColor = false;
            this.XrTableCell17.StylePriority.UsePadding = false;
            this.XrTableCell17.StylePriority.UseTextAlignment = false;
            this.XrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell17.Weight = 0.76541359970761291D;
            //
            //XrTableCell18
            //
            this.XrTableCell18.BackColor = System.Drawing.Color.White;
            this.XrTableCell18.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTableCell18.CanGrow = false;
            this.XrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.PAN") });
            this.XrTableCell18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTableCell18.ForeColor = System.Drawing.Color.Black;
            this.XrTableCell18.Name = "XrTableCell18";
            this.XrTableCell18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrTableCell18.StylePriority.UseBackColor = false;
            this.XrTableCell18.StylePriority.UseBorders = false;
            this.XrTableCell18.StylePriority.UseFont = false;
            this.XrTableCell18.StylePriority.UseForeColor = false;
            this.XrTableCell18.StylePriority.UsePadding = false;
            this.XrTableCell18.StylePriority.UseTextAlignment = false;
            this.XrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell18.Weight = 0.58711983236642939D;
            //
            //rptPrintParty
            //
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { this.Detail, this.TopMargin, this.BottomMargin, this.PageHeaderBand1, this.GroupHeaderBand1, this.PageFooterBand1, this.ReportHeaderBand1 });
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] { this.SqlDataSource1 });
            this.DataMember = "Company";
            this.DataSource = this.SqlDataSource1;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(16, 5, 0, 100);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] { this.Title, this.FieldCaption, this.PageInfo, this.DataField });
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this).EndInit();

        }
        internal DevExpress.XtraReports.UI.DetailBand Detail;
        internal DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        internal DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        internal DevExpress.DataAccess.Sql.SqlDataSource SqlDataSource1;
        internal DevExpress.XtraReports.UI.PageHeaderBand PageHeaderBand1;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeaderBand1;
        internal DevExpress.XtraReports.UI.PageFooterBand PageFooterBand1;
        internal DevExpress.XtraReports.UI.ReportHeaderBand ReportHeaderBand1;
        internal DevExpress.XtraReports.UI.XRControlStyle Title;
        internal DevExpress.XtraReports.UI.XRControlStyle FieldCaption;
        internal DevExpress.XtraReports.UI.XRControlStyle PageInfo;
        internal DevExpress.XtraReports.UI.XRControlStyle DataField;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel3;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel2;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel1;
        internal DevExpress.XtraReports.UI.XRTable XrTable2;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow2;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell4;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell5;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell12;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell14;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell16;
        internal DevExpress.XtraReports.UI.XRTable XrTable1;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell3;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell7;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell8;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell9;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell13;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell15;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell17;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell18;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell6;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell10;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell11;
    }
}