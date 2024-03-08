
using PassParameterExample.Services;
using System;

namespace SutraPlusReportApi.PredefinedReports
{

    public partial class ListAndRegisters
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
            DevExpress.XtraReports.UI.XRSummary XrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary8 = new DevExpress.XtraReports.UI.XRSummary();
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
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo16 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo17 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo18 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo19 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo3 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo20 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo21 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo22 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo51 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo4 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo23 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo24 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo25 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo26 = new DevExpress.DataAccess.Sql.ColumnInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListAndRegisters));
            DevExpress.XtraReports.UI.XRSummary XrSummary9 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary10 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary11 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary12 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary13 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary14 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary15 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary16 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.XrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hidezeroweight = new DevExpress.XtraReports.UI.FormattingRule();
            this.XrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.XrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.XrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblHeader = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Others = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.XrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.XrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CalculatedField1 = new DevExpress.XtraReports.UI.CalculatedField();
            this.Quintal = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupHeader3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.GroupFooter3 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.CalculatedField2 = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            //
            //Detail
            //
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable1 });
            this.Detail.HeightF = 15.0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            //XrTable1
            //
            this.XrTable1.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable1.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.XrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0.0F, 0.0F);
            this.XrTable1.Name = "XrTable1";
            this.XrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow1 });
            this.XrTable1.SizeF = new System.Drawing.SizeF(1091.0F, 15.0F);
            this.XrTable1.StylePriority.UseBorders = false;
            this.XrTable1.StylePriority.UseFont = false;
            this.XrTable1.StylePriority.UseTextAlignment = false;
            this.XrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTableRow1
            //
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell1, this.XrTableCell2, this.XrTableCell3, this.XrTableCell4, this.XrTableCell35, this.XrTableCell5, this.XrTableCell6, this.XrTableCell7, this.XrTableCell8, this.XrTableCell9, this.XrTableCell10, this.XrTableCell11 });
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 1.0D;
            //
            //XrTableCell1
            //
            this.XrTableCell1.Name = "XrTableCell1";
            XrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            XrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell1.Summary = XrSummary1;
            this.XrTableCell1.Weight = 0.35416667938232421D;
            //
            //XrTableCell2
            //
            this.XrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TranctDate", "{0:dd-MM-yyyy}") });
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.Weight = 0.71874984741210957D;
            //
            //XrTableCell3
            //
            this.XrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.LedgerName") });
            this.XrTableCell3.Name = "XrTableCell3";
            this.XrTableCell3.StylePriority.UseTextAlignment = false;
            this.XrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.XrTableCell3.Weight = 2.0937500381469722D;
            //
            //XrTableCell4
            //
            this.XrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Ledger_GSTIN") });
            this.XrTableCell4.Name = "XrTableCell4";
            this.XrTableCell4.StylePriority.UseTextAlignment = false;
            this.XrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell4.Weight = 1.3958328304658334D;
            //
            //XrTableCell35
            //
            this.XrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.DisplayinvNo") });
            this.XrTableCell35.Name = "XrTableCell35";
            this.XrTableCell35.StylePriority.UseTextAlignment = false;
            this.XrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell35.Weight = 0.91666747420826433D;
            //
            //XrTableCell5
            //
            this.XrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Quintal", "{0:#.000}") });
            this.XrTableCell5.FormattingRules.Add(this.hidezeroweight);
            this.XrTableCell5.Name = "XrTableCell5";
            this.XrTableCell5.StylePriority.UseTextAlignment = false;
            this.XrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell5.Weight = 0.68708282520869957D;

            //this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate)";
            //this.Report.FilterString = "CompanyId=?companyidrecord";

            //this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate)";
          
            
            //this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate) and (VochType Between(?vochtype1,?vochtype2) or (VochType = ?vochtype1) or (VochType = ?vochtype2) ) and CompanyId=?companyidrecord and (LedgerId=?ledgerId)";



            if (CustomReportStorageWebExtension.doWeHaveLedgerId)
            {
                this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate) and (VochType Between(?vochtype1,?vochtype2) or (VochType = ?vochtype1) or (VochType = ?vochtype2) ) and CompanyId=?companyidrecord and (LedgerId=?ledgerId)";
            }
            else
            {
                this.Report.FilterString = "(GetDate([TranctDate]) Between(?StartDate,?EndDate) or GetDate([TranctDate]) = ?StartDate or GetDate([TranctDate]) = ?EndDate) and (VochType Between(?vochtype1,?vochtype2) or (VochType = ?vochtype1) or (VochType = ?vochtype2) ) and CompanyId=?companyidrecord";
            }
             

            //
            //hidezeroweight
            //
            this.hidezeroweight.Condition = "[TotalWeight]=0";
            //
            //
            //
            this.hidezeroweight.Formatting.ForeColor = System.Drawing.Color.Transparent;
            this.hidezeroweight.Name = "hidezeroweight";
            //
            //XrTableCell6
            //
            this.XrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TaxableValue", "{0:##,##,###.00}") });
            this.XrTableCell6.Name = "XrTableCell6";
            this.XrTableCell6.StylePriority.UseTextAlignment = false;
            this.XrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell6.Weight = 0.98958373997309557D;
            //
            //XrTableCell7
            //
            this.XrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.SGSTValue", "{0:##,##,###.00}") });
            this.XrTableCell7.Name = "XrTableCell7";
            this.XrTableCell7.StylePriority.UseTextAlignment = false;
            this.XrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell7.Weight = 0.687915641585801D;
            //
            //XrTableCell8
            //
            this.XrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CSGSTValue", "{0:##,##,###.00}") });
            this.XrTableCell8.Name = "XrTableCell8";
            this.XrTableCell8.StylePriority.UseTextAlignment = false;
            this.XrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell8.Weight = 0.78124938416134093D;
            //
            //XrTableCell9
            //
            this.XrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IGSTValue", "{0:##,##,###.00}") });
            this.XrTableCell9.Name = "XrTableCell9";
            this.XrTableCell9.StylePriority.UseTextAlignment = false;
            this.XrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell9.Weight = 0.76000121468304371D;
            //
            //XrTableCell10
            //
            this.XrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Others", "{0:c2}") });
            this.XrTableCell10.Name = "XrTableCell10";
            this.XrTableCell10.StylePriority.UseTextAlignment = false;
            this.XrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell10.Weight = 0.62499938024199719D;
            //
            //XrTableCell11
            //
            this.XrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.BillAmount", "{0:##,##,###.00}") });
            this.XrTableCell11.Name = "XrTableCell11";
            this.XrTableCell11.StylePriority.UseTextAlignment = false;
            this.XrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell11.Weight = 0.90000121819477563D;
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
            //XrTable3
            //
            this.XrTable3.Borders = (DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable3.LocationFloat = new DevExpress.Utils.PointFloat(547.9167F, 0.0F);
            this.XrTable3.Name = "XrTable3";
            this.XrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow3 });
            this.XrTable3.SizeF = new System.Drawing.SizeF(542.0833F, 15.0F);
            this.XrTable3.StylePriority.UseBorders = false;
            this.XrTable3.StylePriority.UseFont = false;
            //
            //XrTableRow3
            //
            this.XrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell27, this.XrTableCell28, this.XrTableCell29, this.XrTableCell30, this.XrTableCell31, this.XrTableCell32, this.XrTableCell33 });
            this.XrTableRow3.Name = "XrTableRow3";
            this.XrTableRow3.Weight = 1.0D;
            //
            //XrTableCell27
            //
            this.XrTableCell27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Quintal") });
            this.XrTableCell27.Name = "XrTableCell27";
            this.XrTableCell27.StylePriority.UseTextAlignment = false;
            XrSummary2.FormatString = "{0:n2}";
            XrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell27.Summary = XrSummary2;
            this.XrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell27.Weight = 0.67708251542208187D;
            //
            //XrTableCell28
            //
            this.XrTableCell28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TaxableValue") });
            this.XrTableCell28.Name = "XrTableCell28";
            this.XrTableCell28.StylePriority.UseTextAlignment = false;
            XrSummary3.FormatString = "{0:##,##,###.00}";
            XrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell28.Summary = XrSummary3;
            this.XrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell28.Weight = 0.99958435057861839D;
            //
            //XrTableCell29
            //
            this.XrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.SGSTValue") });
            this.XrTableCell29.Name = "XrTableCell29";
            this.XrTableCell29.StylePriority.UseTextAlignment = false;
            XrSummary4.FormatString = "{0:##,##,###.00}";
            XrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell29.Summary = XrSummary4;
            this.XrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell29.Weight = 0.68791510138533063D;
            //
            //XrTableCell30
            //
            this.XrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CSGSTValue") });
            this.XrTableCell30.Name = "XrTableCell30";
            this.XrTableCell30.StylePriority.UseTextAlignment = false;
            XrSummary5.FormatString = "{0:##,##,###.00}";
            XrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell30.Summary = XrSummary5;
            this.XrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell30.Weight = 0.78124938052846915D;
            //
            //XrTableCell31
            //
            this.XrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IGSTValue") });
            this.XrTableCell31.Name = "XrTableCell31";
            this.XrTableCell31.StylePriority.UseTextAlignment = false;
            XrSummary6.FormatString = "{0:##,##,###.00}";
            XrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell31.Summary = XrSummary6;
            this.XrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell31.Weight = 0.76000060611390607D;
            //
            //XrTableCell32
            //
            this.XrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Others") });
            this.XrTableCell32.Name = "XrTableCell32";
            this.XrTableCell32.StylePriority.UseTextAlignment = false;
            XrSummary7.FormatString = "{0:c2}";
            XrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell32.Summary = XrSummary7;
            this.XrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell32.Weight = 0.62500001720286547D;
            //
            //XrTableCell33
            //
            this.XrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.BillAmount") });
            this.XrTableCell33.Name = "XrTableCell33";
            this.XrTableCell33.StylePriority.UseTextAlignment = false;
            XrSummary8.FormatString = "{0:##,##,###.00}";
            XrSummary8.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell33.Summary = XrSummary8;
            this.XrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell33.Weight = 0.8900012541035579D;
            //
            //SqlDataSource1
            //
            this.SqlDataSource1.ConnectionName = ".\\SQLEXPRESS_K2122_Connection 2";
            MsSqlConnectionParameters1.AuthorizationType = DevExpress.DataAccess.ConnectionParameters.MsSqlAuthorizationType.SqlServer;
            //MsSqlConnectionParameters1.DatabaseName = "K2223RGP";
            //MsSqlConnectionParameters1.DatabaseName = "K2223RGP";
            //MsSqlConnectionParameters1.UserName = "sa";
            //MsSqlConnectionParameters1.Password = "root@123";
            //MsSqlConnectionParameters1.ServerName = "103.50.212.163";

            MsSqlConnectionParameters1.DatabaseName = CustomReportStorageWebExtension.databaseName;
            MsSqlConnectionParameters1.UserName = CustomReportStorageWebExtension.UserID;
            MsSqlConnectionParameters1.Password = CustomReportStorageWebExtension.Password;
            MsSqlConnectionParameters1.ServerName = CustomReportStorageWebExtension.DataSource;
            this.SqlDataSource1.ConnectionParameters = MsSqlConnectionParameters1;
            this.SqlDataSource1.Name = "SqlDataSource1";

            TableQuery1.Name = "Company";
            RelationColumnInfo1.NestedKeyColumn = "CompanyID";
            RelationColumnInfo1.ParentKeyColumn = "CompanyId";
            RelationInfo1.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo1 });
            RelationInfo1.NestedTable = "BillSummary";
            RelationInfo1.ParentTable = "Company";
            RelationColumnInfo2.NestedKeyColumn = "CompanyId";
            RelationColumnInfo2.ParentKeyColumn = "CompanyId";
            RelationInfo2.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo2 });
            RelationInfo2.NestedTable = "Ledger";
            RelationInfo2.ParentTable = "Company";
            RelationColumnInfo3.NestedKeyColumn = "LedgerId";
            RelationColumnInfo3.ParentKeyColumn = "LedgerId";
            RelationInfo3.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo3 });
            RelationInfo3.NestedTable = "Ledger";
            RelationInfo3.ParentTable = "BillSummary";
            RelationColumnInfo4.NestedKeyColumn = "VoucherId";
            RelationColumnInfo4.ParentKeyColumn = "VochType";
            RelationInfo4.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo4 });
            RelationInfo4.NestedTable = "VoucherTypes";
            RelationInfo4.ParentTable = "BillSummary";
            TableQuery1.Relations.AddRange(new DevExpress.DataAccess.Sql.RelationInfo[] { RelationInfo1, RelationInfo2, RelationInfo3, RelationInfo4 });
            TableInfo1.Name = "Company";
            ColumnInfo1.Name = "CompanyName";
            ColumnInfo2.Name = "AddressLine1";
            ColumnInfo3.Name = "Place";
            ColumnInfo4.Name = "GSTIN";
            TableInfo1.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo1, ColumnInfo2, ColumnInfo3, ColumnInfo4 });
            TableInfo2.Name = "BillSummary";
            ColumnInfo5.Name = "VochType";
            ColumnInfo6.Name = "VochNo";
            ColumnInfo7.Name = "RoundOff";
            ColumnInfo8.Name = "TotalBags";
            ColumnInfo9.Name = "TotalWeight";
            ColumnInfo10.Name = "TotalAmount";
            ColumnInfo11.Name = "BillAmount";
            ColumnInfo12.Name = "SGSTValue";
            ColumnInfo13.Name = "CSGSTValue";
            ColumnInfo14.Name = "IGSTValue";
            ColumnInfo15.Name = "TaxableValue";
            ColumnInfo16.Name = "DisplayinvNo";
            ColumnInfo17.Name = "ToPrint";
            ColumnInfo18.Name = "TranctDate";
            ColumnInfo19.Name = "IsGSTUpload";
            ColumnInfo25.Name = "CompanyID";
            TableInfo2.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo5, ColumnInfo6, ColumnInfo7, ColumnInfo8, ColumnInfo9, ColumnInfo10, ColumnInfo11, ColumnInfo12, ColumnInfo13, ColumnInfo14, ColumnInfo15, ColumnInfo16, ColumnInfo17, ColumnInfo18, ColumnInfo19, ColumnInfo25 });
            TableInfo3.Name = "Ledger";
            ColumnInfo20.Name = "LedgerName";
            ColumnInfo21.Alias = "Ledger_Place";
            ColumnInfo21.Name = "Place";
            ColumnInfo22.Alias = "Ledger_GSTIN";
            ColumnInfo22.Name = "GSTIN";
            ColumnInfo51.Name = "LedgerId";
            TableInfo3.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo20, ColumnInfo21, ColumnInfo22, ColumnInfo51 });
            TableInfo4.Name = "VoucherTypes";
            ColumnInfo23.Name = "VoucherId";
            ColumnInfo24.Name = "VoucherName";

            TableInfo4.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo23, ColumnInfo24,});
            TableQuery1.Tables.AddRange(new DevExpress.DataAccess.Sql.TableInfo[] { TableInfo1, TableInfo2, TableInfo3, TableInfo4 });
            this.SqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] { TableQuery1 });
            this.SqlDataSource1.ResultSchemaSerializable = resources.GetString("SqlDataSource1.ResultSchemaSerializable");
            //
            //PageHeader
            //
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrPageInfo2, this.lblHeader, this.XrLabel4, this.XrLabel3, this.XrLabel2, this.XrLabel1, this.XrTable2 });
            this.PageHeader.HeightF = 162.5F;
            this.PageHeader.Name = "PageHeader";
            //
            //XrPageInfo2
            //
            this.XrPageInfo2.Format = "Page {0} of {1}";
            this.XrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(1001.0F, 121.625F);
            this.XrPageInfo2.Name = "XrPageInfo2";
            this.XrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrPageInfo2.SizeF = new System.Drawing.SizeF(72.95819F, 14.37502F);
            this.XrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            //
            //lblHeader
            //
            this.lblHeader.LocationFloat = new DevExpress.Utils.PointFloat(102.4166F, 121.625F);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.lblHeader.SizeF = new System.Drawing.SizeF(856.6251F, 23.0F);
            this.lblHeader.StylePriority.UseTextAlignment = false;
            this.lblHeader.Text = "List and Register Report";
            this.lblHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel4
            //
            this.XrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.GSTIN") });
            this.XrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(9.999959F, 106.625F);
            this.XrLabel4.Name = "XrLabel4";
            this.XrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel4.SizeF = new System.Drawing.SizeF(1063.958F, 15.0F);
            this.XrLabel4.StylePriority.UseTextAlignment = false;
            this.XrLabel4.Text = "XrLabel4";
            this.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel3
            //
            this.XrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Place") });
            this.XrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(9.999959F, 83.62499F);
            this.XrLabel3.Name = "XrLabel3";
            this.XrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel3.SizeF = new System.Drawing.SizeF(1063.958F, 23.0F);
            this.XrLabel3.StylePriority.UseTextAlignment = false;
            this.XrLabel3.Text = "XrLabel3";
            this.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel2
            //
            this.XrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.AddressLine1") });
            this.XrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 60.62499F);
            this.XrLabel2.Name = "XrLabel2";
            this.XrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel2.SizeF = new System.Drawing.SizeF(1063.958F, 23.0F);
            this.XrLabel2.StylePriority.UseTextAlignment = false;
            this.XrLabel2.Text = "XrLabel2";
            this.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel1
            //
            this.XrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CompanyName") });
            this.XrLabel1.Font = new System.Drawing.Font("Times New Roman", 11.0F, System.Drawing.FontStyle.Bold);
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999959F, 37.62499F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(1063.958F, 23.0F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "XrLabel1";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTable2
            //
            this.XrTable2.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0.0F, 144.625F);
            this.XrTable2.Name = "XrTable2";
            this.XrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow2 });
            this.XrTable2.SizeF = new System.Drawing.SizeF(1091.0F, 15.0F);
            this.XrTable2.StylePriority.UseBorders = false;
            this.XrTable2.StylePriority.UseFont = false;
            this.XrTable2.StylePriority.UseTextAlignment = false;
            this.XrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTableRow2
            //
            this.XrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell12, this.XrTableCell13, this.XrTableCell14, this.XrTableCell15, this.XrTableCell34, this.XrTableCell16, this.XrTableCell17, this.XrTableCell18, this.XrTableCell19, this.XrTableCell20, this.XrTableCell21, this.XrTableCell22 });
            this.XrTableRow2.Name = "XrTableRow2";
            this.XrTableRow2.Weight = 1.0D;
            //
            //XrTableCell12
            //
            this.XrTableCell12.Name = "XrTableCell12";
            XrSummary9.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            this.XrTableCell12.Summary = XrSummary9;
            this.XrTableCell12.Text = "S.No";
            this.XrTableCell12.Weight = 0.35416667938232421D;
            //
            //XrTableCell13
            //
            this.XrTableCell13.Name = "XrTableCell13";
            this.XrTableCell13.Text = "Date";
            this.XrTableCell13.Weight = 0.71874980926513676D;
            //
            //XrTableCell14
            //
            this.XrTableCell14.Name = "XrTableCell14";
            this.XrTableCell14.Text = "Party Name";
            this.XrTableCell14.Weight = 2.0937500762939454D;
            //
            //XrTableCell15
            //
            this.XrTableCell15.Name = "XrTableCell15";
            this.XrTableCell15.StylePriority.UseTextAlignment = false;
            this.XrTableCell15.Text = "GSTIN";
            this.XrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell15.Weight = 1.3958330669921568D;
            //
            //XrTableCell34
            //
            this.XrTableCell34.Name = "XrTableCell34";
            this.XrTableCell34.StylePriority.UseTextAlignment = false;
            this.XrTableCell34.Text = "Bill Number";
            this.XrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell34.Weight = 0.91666693300784308D;
            //
            //XrTableCell16
            //
            this.XrTableCell16.Name = "XrTableCell16";
            this.XrTableCell16.StylePriority.UseTextAlignment = false;
            this.XrTableCell16.Text = "Weight";
            this.XrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell16.Weight = 0.68708367268174575D;
            //
            //XrTableCell17
            //
            this.XrTableCell17.Font = new System.Drawing.Font("Times New Roman", 8.0F, System.Drawing.FontStyle.Bold);
            this.XrTableCell17.Name = "XrTableCell17";
            this.XrTableCell17.StylePriority.UseFont = false;
            this.XrTableCell17.StylePriority.UseTextAlignment = false;
            this.XrTableCell17.Text = "Taxable Value";
            this.XrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell17.Weight = 0.98958381594559486D;
            //
            //XrTableCell18
            //
            this.XrTableCell18.Name = "XrTableCell18";
            this.XrTableCell18.StylePriority.UseTextAlignment = false;
            this.XrTableCell18.Text = "SGST";
            this.XrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell18.Weight = 0.687916335476811D;
            //
            //XrTableCell19
            //
            this.XrTableCell19.Name = "XrTableCell19";
            this.XrTableCell19.StylePriority.UseTextAlignment = false;
            this.XrTableCell19.Text = "CGST";
            this.XrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell19.Weight = 0.78124862731489053D;
            //
            //XrTableCell20
            //
            this.XrTableCell20.Name = "XrTableCell20";
            this.XrTableCell20.StylePriority.UseTextAlignment = false;
            this.XrTableCell20.Text = "IGST";
            this.XrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell20.Weight = 0.76000068487400141D;
            //
            //XrTableCell21
            //
            this.XrTableCell21.Font = new System.Drawing.Font("Times New Roman", 8.0F, System.Drawing.FontStyle.Bold);
            this.XrTableCell21.Name = "XrTableCell21";
            this.XrTableCell21.StylePriority.UseFont = false;
            this.XrTableCell21.StylePriority.UseTextAlignment = false;
            this.XrTableCell21.Text = "Others";
            this.XrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell21.Weight = 0.62499879034582662D;
            //
            //XrTableCell22
            //
            this.XrTableCell22.Name = "XrTableCell22";
            this.XrTableCell22.StylePriority.UseTextAlignment = false;
            this.XrTableCell22.Text = "Bill Amount";
            this.XrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell22.Weight = 0.90000211877128622D;
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
            this.ReportFooter.HeightF = 18.75F;
            this.ReportFooter.Name = "ReportFooter";
            //
            //GroupHeader1
            //
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrLabel5 });
            this.GroupHeader1.Expanded = false;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("VoucherId", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader1.HeightF = 23.95833F;
            this.GroupHeader1.Level = 2;
            this.GroupHeader1.Name = "GroupHeader1";
            //
            //XrLabel5
            //
            this.XrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.VoucherName") });
            this.XrLabel5.Font = new System.Drawing.Font("Times New Roman", 11.0F, System.Drawing.FontStyle.Bold);
            this.XrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(35.41667F, 0.0F);
            this.XrLabel5.Name = "XrLabel5";
            this.XrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel5.SizeF = new System.Drawing.SizeF(173.9583F, 23.0F);
            this.XrLabel5.StylePriority.UseFont = false;
            this.XrLabel5.Text = "XrLabel5";
            //
            //GroupFooter1
            //
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable4 });
            this.GroupFooter1.HeightF = 15.0F;
            this.GroupFooter1.Level = 2;
            this.GroupFooter1.Name = "GroupFooter1";
            //
            //XrTable4
            //
            this.XrTable4.Borders = (DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable4.LocationFloat = new DevExpress.Utils.PointFloat(545.9167F, 0.0F);
            this.XrTable4.Name = "XrTable4";
            this.XrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow4 });
            this.XrTable4.SizeF = new System.Drawing.SizeF(545.0832F, 15.0F);
            this.XrTable4.StylePriority.UseBorders = false;
            this.XrTable4.StylePriority.UseFont = false;
            //
            //XrTableRow4
            //
            this.XrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell23, this.XrTableCell24, this.XrTableCell25, this.XrTableCell26, this.XrTableCell36, this.XrTableCell37, this.XrTableCell38 });
            this.XrTableRow4.Name = "XrTableRow4";
            this.XrTableRow4.Weight = 1.0D;
            //
            //XrTableCell23
            //
            this.XrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Quintal") });
            this.XrTableCell23.Name = "XrTableCell23";
            this.XrTableCell23.StylePriority.UseTextAlignment = false;
            XrSummary10.FormatString = "{0:n2}";
            XrSummary10.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell23.Summary = XrSummary10;
            this.XrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell23.Weight = 0.70448822569563418D;
            //
            //XrTableCell24
            //
            this.XrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TaxableValue") });
            this.XrTableCell24.Name = "XrTableCell24";
            this.XrTableCell24.StylePriority.UseTextAlignment = false;
            XrSummary11.FormatString = "{0:##,##,###.00}";
            XrSummary11.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell24.Summary = XrSummary11;
            this.XrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell24.Weight = 0.98595354073038843D;
            //
            //XrTableCell25
            //
            this.XrTableCell25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.SGSTValue") });
            this.XrTableCell25.Name = "XrTableCell25";
            this.XrTableCell25.StylePriority.UseTextAlignment = false;
            XrSummary12.FormatString = "{0:##,##,###.00}";
            XrSummary12.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell25.Summary = XrSummary12;
            this.XrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell25.Weight = 0.68539227626969124D;
            //
            //XrTableCell26
            //
            this.XrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CSGSTValue") });
            this.XrTableCell26.Name = "XrTableCell26";
            this.XrTableCell26.StylePriority.UseTextAlignment = false;
            XrSummary13.FormatString = "{0:##,##,###.00}";
            XrSummary13.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell26.Summary = XrSummary13;
            this.XrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell26.Weight = 0.77838235749131091D;
            //
            //XrTableCell36
            //
            this.XrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IGSTValue") });
            this.XrTableCell36.Name = "XrTableCell36";
            this.XrTableCell36.StylePriority.UseTextAlignment = false;
            XrSummary14.FormatString = "{0:##,##,###.00}";
            XrSummary14.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell36.Summary = XrSummary14;
            this.XrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell36.Weight = 0.75721263989710919D;
            //
            //XrTableCell37
            //
            this.XrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Others") });
            this.XrTableCell37.Name = "XrTableCell37";
            this.XrTableCell37.StylePriority.UseTextAlignment = false;
            XrSummary15.FormatString = "{0:c2}";
            XrSummary15.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell37.Summary = XrSummary15;
            this.XrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell37.Weight = 0.62270656019411086D;
            //
            //XrTableCell38
            //
            this.XrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.BillAmount") });
            this.XrTableCell38.Name = "XrTableCell38";
            this.XrTableCell38.StylePriority.UseTextAlignment = false;
            XrSummary16.FormatString = "{0:##,##,###.00}";
            XrSummary16.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell38.Summary = XrSummary16;
            this.XrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell38.Weight = 0.896697014881095D;
            //
            //CalculatedField1
            //
            this.CalculatedField1.DataMember = "Company";
            this.CalculatedField1.Name = "CalculatedField1";
            //
            //Quintal
            //
            this.Quintal.DataMember = "Company";
            this.Quintal.Expression = "round([TotalWeight]/100,3)";
            this.Quintal.Name = "Quintal";
            //
            //GroupHeader2
            //
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("TranctDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader2.HeightF = 0.0F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            //
            //GroupHeader3
            //
            this.GroupHeader3.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("VochNo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader3.HeightF = 0.0F;
            this.GroupHeader3.Name = "GroupHeader3";
            //
            //GroupFooter2
            //
            this.GroupFooter2.HeightF = 0.0F;
            this.GroupFooter2.Name = "GroupFooter2";
            //
            //GroupFooter3
            //
            this.GroupFooter3.HeightF = 0.0F;
            this.GroupFooter3.Level = 1;
            this.GroupFooter3.Name = "GroupFooter3";
            //
            //CalculatedField2
            //
            this.CalculatedField2.Expression = "round([TotalWeight]/100,3)";
            this.CalculatedField2.Name = "CalculatedField2";
            //
            //rptAPMCRegister
            //
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { this.Detail, this.TopMargin, this.BottomMargin, this.PageHeader, this.ReportFooter, this.GroupHeader1, this.GroupFooter1, this.GroupHeader2, this.GroupHeader3, this.GroupFooter2, this.GroupFooter3 });
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] { this.Others, this.CalculatedField1, this.Quintal, this.CalculatedField2 });
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] { this.SqlDataSource1 });
            this.DataMember = "Company";
            this.DataSource = this.SqlDataSource1;
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] { this.hidezeroweight });
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(48, 20, 0, 14);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = (DevExpress.Drawing.Printing.DXPaperKind)System.Drawing.Printing.PaperKind.A4;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable3).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable4).EndInit();
            ((System.ComponentModel.ISupportInitialize)this).EndInit();

        }
        internal DevExpress.XtraReports.UI.DetailBand Detail;
        internal DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        internal DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        internal DevExpress.DataAccess.Sql.SqlDataSource SqlDataSource1;
        internal DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        internal DevExpress.XtraReports.UI.XRTable XrTable1;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow1;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell1;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell3;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell4;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell5;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell6;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell7;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell8;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell9;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell10;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell11;
        internal DevExpress.XtraReports.UI.CalculatedField Others;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel4;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel3;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel2;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel1;
        internal DevExpress.XtraReports.UI.XRTable XrTable2;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow2;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell12;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell13;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell14;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell15;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell16;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell17;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell18;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell19;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell20;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell21;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell22;
        internal DevExpress.XtraReports.UI.XRLabel lblHeader;
        internal DevExpress.XtraReports.UI.XRTable XrTable3;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow3;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell27;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell28;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell29;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell30;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell31;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell32;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell33;
        internal DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell35;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell34;
        internal DevExpress.XtraReports.UI.XRPageInfo XrPageInfo2;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel5;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        internal DevExpress.XtraReports.UI.CalculatedField CalculatedField1;
        internal DevExpress.XtraReports.UI.CalculatedField Quintal;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader3;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter2;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter3;
        internal DevExpress.XtraReports.UI.XRTable XrTable4;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow4;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell23;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell24;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell25;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell26;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell36;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell37;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell38;
        internal DevExpress.XtraReports.UI.CalculatedField CalculatedField2;
        internal DevExpress.XtraReports.UI.FormattingRule hidezeroweight;
    }
}