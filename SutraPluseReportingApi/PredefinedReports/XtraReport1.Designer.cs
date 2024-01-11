using DevExpress.XtraReports.Expressions;
using DevExpress.XtraReports.Parameters;
using System;
using System.Linq;

namespace SutraPlusReportApi.PredefinedReports
{
    partial class XtraReport1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //private System.ComponentModel.IContainer components;

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
            DevExpress.DataAccess.Sql.RelationInfo RelationInfo5 = new DevExpress.DataAccess.Sql.RelationInfo();
            DevExpress.DataAccess.Sql.RelationColumnInfo RelationColumnInfo5 = new DevExpress.DataAccess.Sql.RelationColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo1 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo1 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo2 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo3 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo4 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo5 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo6 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo7 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo2 = new DevExpress.DataAccess.Sql.TableInfo();
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
            DevExpress.DataAccess.Sql.TableInfo TableInfo4 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo23 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo24 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo25 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.TableInfo TableInfo5 = new DevExpress.DataAccess.Sql.TableInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo26 = new DevExpress.DataAccess.Sql.ColumnInfo();
            DevExpress.DataAccess.Sql.ColumnInfo ColumnInfo27 = new DevExpress.DataAccess.Sql.ColumnInfo();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraReport1));
            DevExpress.XtraReports.UI.XRSummary XrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary8 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary9 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary10 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary11 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary12 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary13 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary14 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary15 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary16 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary17 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary18 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary19 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary20 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary21 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary22 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary23 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary XrSummary24 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.XrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.SqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblHeader = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.XrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.XrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.IsTrading = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.XrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.XrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.XrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader3 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupHeader4 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.XrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader5 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupFooter3 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.GroupFooter4 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.GroupFooter5 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.XrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.XrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.XrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ItemName = new DevExpress.XtraReports.UI.CalculatedField();
            this.PartyName = new DevExpress.XtraReports.UI.CalculatedField();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.XrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.HideWeight = new DevExpress.XtraReports.UI.FormattingRule();
            this.GroupHeader6 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            var dateRangeParam = new Parameter();            dateRangeParam.Name = "dateRange";            dateRangeParam.Description = "Date Range:";            dateRangeParam.Type = typeof(System.DateTime);

            // Create a RangeParametersSettings instance and set up its properties.
            var dateRangeSettings = new RangeParametersSettings();

            // Specify the start date and end date parameters.
            dateRangeSettings.StartParameter.Name = "dateRangeStart";            dateRangeSettings.StartParameter.ExpressionBindings.Add(                new BasicExpressionBinding("Value", new System.DateTime(2022, 4, 1).ToString())            );            dateRangeSettings.EndParameter.Name = "dateRangeEnd";            dateRangeSettings.EndParameter.ExpressionBindings.Add(                new BasicExpressionBinding("Value", new System.DateTime(2022, 5, 1).ToString())            );
            this.Report.FilterString = "GetDate([TranctDate]) Between(?dateRangeStart,?dateRangeEnd)";
            //
            //Detail
            //
            this.Detail.HeightF = 0.0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            //
            //XrTable1
            //
            this.XrTable1.Borders = (DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right);
            this.XrTable1.LocationFloat = new DevExpress.Utils.PointFloat(7.881228F, 0.0F);
            this.XrTable1.Name = "XrTable1";
            this.XrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow1 });
            this.XrTable1.SizeF = new System.Drawing.SizeF(1115.418F, 15.0F);
            this.XrTable1.StylePriority.UseBorders = false;
            //
            //XrTableRow1
            //
            this.XrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell1, this.XrTableCell11, this.XrTableCell10, this.XrTableCell2, this.XrTableCell3, this.XrTableCell4, this.XrTableCell5, this.XrTableCell6, this.XrTableCell7, this.XrTableCell8 });
            this.XrTableRow1.Name = "XrTableRow1";
            this.XrTableRow1.Weight = 1.0D;
            //
            //XrTableCell1
            //
            //here date is adding
           
            this.XrTableCell1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TranctDate", "{0:dd-MM-yyyy}") });
            this.XrTableCell1.Name = "XrTableCell1";
            this.XrTableCell1.Weight = 0.77870399797875722D;

            //
            //XrTableCell11
            //
            this.XrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.LedgerName") });
            this.XrTableCell11.Name = "XrTableCell11";
            this.XrTableCell11.Weight = 1.873259596954153D;
            //
            //XrTableCell10
            //
            this.XrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Ledger_GSTIN") });
            this.XrTableCell10.Name = "XrTableCell10";
            this.XrTableCell10.Weight = 1.8767881666100266D;
            //
            //XrTableCell2
            //
            this.XrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.PartyInvoiceNumber") });
            this.XrTableCell2.Name = "XrTableCell2";
            this.XrTableCell2.StylePriority.UseTextAlignment = false;
            this.XrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell2.Weight = 2.1901043939561857D;
            //
            //XrTableCell3
            //
            this.XrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.NoOfBags") });
            this.XrTableCell3.Name = "XrTableCell3";
            this.XrTableCell3.StylePriority.UseTextAlignment = false;
            XrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell3.Summary = XrSummary1;
            this.XrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell3.Weight = 0.62555595664366592D;
            //
            //XrTableCell4
            //
            this.XrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TotalWeight") });
            this.XrTableCell4.Name = "XrTableCell4";
            this.XrTableCell4.StylePriority.UseTextAlignment = false;
            XrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell4.Summary = XrSummary2;
            this.XrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell4.Weight = 0.74775555619291223D;
            //
            //XrTableCell5
            //
            this.XrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Taxable") });
            this.XrTableCell5.Name = "XrTableCell5";
            this.XrTableCell5.StylePriority.UseTextAlignment = false;
            XrSummary3.FormatString = "{0:##,##,###.00}";
            XrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell5.Summary = XrSummary3;
            this.XrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell5.Weight = 1.0011240479142089D;
            //
            //XrTableCell6
            //
            this.XrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.SGST") });
            this.XrTableCell6.Name = "XrTableCell6";
            this.XrTableCell6.StylePriority.UseTextAlignment = false;
            XrSummary4.FormatString = "{0:##,##,###.00}";
            XrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell6.Summary = XrSummary4;
            this.XrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell6.Weight = 0.79228047764316156D;
            //
            //XrTableCell7
            //
            this.XrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CGST") });
            this.XrTableCell7.Name = "XrTableCell7";
            this.XrTableCell7.StylePriority.UseTextAlignment = false;
            XrSummary5.FormatString = "{0:##,##,###.00}";
            XrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell7.Summary = XrSummary5;
            this.XrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell7.Weight = 0.85218275011785194D;
            //
            //XrTableCell8
            //
            this.XrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IGST") });
            this.XrTableCell8.Name = "XrTableCell8";
            this.XrTableCell8.StylePriority.UseTextAlignment = false;
            XrSummary6.FormatString = "{0:##,##,###.00}";
            XrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell8.Summary = XrSummary6;
            this.XrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell8.Weight = 0.84769809812116037D;
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
            this.BottomMargin.HeightF = 16.66667F;
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
            RelationInfo1.NestedTable = "Inventory";
            RelationInfo1.ParentTable = "Company";
            RelationColumnInfo2.NestedKeyColumn = "CommodityId";
            RelationColumnInfo2.ParentKeyColumn = "CommodityId";
            RelationInfo2.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo2 });
            RelationInfo2.NestedTable = "Commodity";
            RelationInfo2.ParentTable = "Inventory";
            RelationColumnInfo3.NestedKeyColumn = "CompanyId";
            RelationColumnInfo3.ParentKeyColumn = "CompanyId";
            RelationInfo3.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo3 });
            RelationInfo3.NestedTable = "Ledger";
            RelationInfo3.ParentTable = "Company";
            RelationColumnInfo4.NestedKeyColumn = "LedgerId";
            RelationColumnInfo4.ParentKeyColumn = "LedgerId";
            RelationInfo4.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo4 });
            RelationInfo4.NestedTable = "Ledger";
            RelationInfo4.ParentTable = "Inventory";
            RelationColumnInfo5.NestedKeyColumn = "VoucherId";
            RelationColumnInfo5.ParentKeyColumn = "VochType";
            RelationInfo5.KeyColumns.AddRange(new DevExpress.DataAccess.Sql.RelationColumnInfo[] { RelationColumnInfo5 });
            RelationInfo5.NestedTable = "VoucherTypes";
            RelationInfo5.ParentTable = "Inventory";
            TableQuery1.Relations.AddRange(new DevExpress.DataAccess.Sql.RelationInfo[] { RelationInfo1, RelationInfo2, RelationInfo3, RelationInfo4, RelationInfo5 });
            TableInfo1.Name = "Company";
            ColumnInfo1.Name = "CompanyId";
            ColumnInfo2.Name = "CompanyName";
            ColumnInfo3.Name = "AddressLine1";
            ColumnInfo4.Name = "AddressLine2";
            ColumnInfo5.Name = "AddressLine3";
            ColumnInfo6.Name = "Place";
            ColumnInfo7.Name = "GSTIN";
            TableInfo1.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo1, ColumnInfo2, ColumnInfo3, ColumnInfo4, ColumnInfo5, ColumnInfo6, ColumnInfo7 });
            TableInfo2.Name = "Inventory";
            ColumnInfo8.Name = "VochType";
            ColumnInfo9.Name = "VochNo";
            ColumnInfo10.Name = "TranctDate";
            ColumnInfo11.Name = "NoOfBags";
            ColumnInfo12.Name = "TotalWeight";
            ColumnInfo13.Name = "Amount";
            ColumnInfo14.Name = "PartyInvoiceNumber";
            ColumnInfo15.Name = "SGST";
            ColumnInfo16.Name = "CGST";
            ColumnInfo17.Name = "IGST";
            ColumnInfo18.Name = "Taxable";
            ColumnInfo19.Name = "ToPrint";
            TableInfo2.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo8, ColumnInfo9, ColumnInfo10, ColumnInfo11, ColumnInfo12, ColumnInfo13, ColumnInfo14, ColumnInfo15, ColumnInfo16, ColumnInfo17, ColumnInfo18, ColumnInfo19 });
            TableInfo3.Name = "Commodity";
            ColumnInfo20.Name = "CommodityId";
            ColumnInfo21.Name = "CommodityName";
            ColumnInfo22.Name = "HSN";
            TableInfo3.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo20, ColumnInfo21, ColumnInfo22 });
            TableInfo4.Name = "Ledger";
            ColumnInfo23.Name = "LedgerName";
            ColumnInfo24.Alias = "Ledger_Place";
            ColumnInfo24.Name = "Place";
            ColumnInfo25.Alias = "Ledger_GSTIN";
            ColumnInfo25.Name = "GSTIN";
            TableInfo4.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo23, ColumnInfo24, ColumnInfo25 });
            TableInfo5.Name = "VoucherTypes";
            ColumnInfo26.Name = "VoucherId";
            ColumnInfo27.Name = "VoucherName";
            TableInfo5.SelectedColumns.AddRange(new DevExpress.DataAccess.Sql.ColumnInfo[] { ColumnInfo26, ColumnInfo27 });
            TableQuery1.Tables.AddRange(new DevExpress.DataAccess.Sql.TableInfo[] { TableInfo1, TableInfo2, TableInfo3, TableInfo4, TableInfo5 });
            this.SqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] { TableQuery1 });
            this.SqlDataSource1.ResultSchemaSerializable = resources.GetString("SqlDataSource1.ResultSchemaSerializable");
            //
            //PageHeader
            //
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.lblHeader, this.XrLabel5, this.XrLabel4, this.XrLabel3, this.XrLabel2, this.XrTable2, this.XrPageInfo2 });
            this.PageHeader.HeightF = 127.0833F;
            this.PageHeader.Name = "PageHeader";
            //
            //lblHeader
            //
            this.lblHeader.Font = new System.Drawing.Font("Times New Roman", 10.0F, System.Drawing.FontStyle.Bold);
            this.lblHeader.LocationFloat = new DevExpress.Utils.PointFloat(111.4584F, 89.08335F);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.lblHeader.SizeF = new System.Drawing.SizeF(902.6667F, 15.0F);
            this.lblHeader.StylePriority.UseFont = false;
            this.lblHeader.StylePriority.UseTextAlignment = false;
            this.lblHeader.Text = "lblHeader";
            this.lblHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel5
            //
            this.XrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.GSTIN") });
            this.XrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 74.08336F);
            this.XrLabel5.Name = "XrLabel5";
            this.XrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel5.SizeF = new System.Drawing.SizeF(1110.0F, 15.0F);
            this.XrLabel5.StylePriority.UseTextAlignment = false;
            this.XrLabel5.Text = "XrLabel5";
            this.XrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel4
            //
            this.XrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Place") });
            this.XrLabel4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(10.00003F, 59.08337F);
            this.XrLabel4.Name = "XrLabel4";
            this.XrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel4.SizeF = new System.Drawing.SizeF(1110.0F, 15.0F);
            this.XrLabel4.StylePriority.UseFont = false;
            this.XrLabel4.StylePriority.UseTextAlignment = false;
            this.XrLabel4.Text = "XrLabel4";
            this.XrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel3
            //
            this.XrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.AddressLine1") });
            this.XrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 44.08337F);
            this.XrLabel3.Name = "XrLabel3";
            this.XrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel3.SizeF = new System.Drawing.SizeF(1110.0F, 15.0F);
            this.XrLabel3.StylePriority.UseTextAlignment = false;
            this.XrLabel3.Text = "XrLabel3";
            this.XrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrLabel2
            //
            this.XrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CompanyName") });
            this.XrLabel2.Font = new System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Bold);
            this.XrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 21.08337F);
            this.XrLabel2.Name = "XrLabel2";
            this.XrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel2.SizeF = new System.Drawing.SizeF(1111.0F, 23.0F);
            this.XrLabel2.StylePriority.UseFont = false;
            this.XrLabel2.StylePriority.UseTextAlignment = false;
            this.XrLabel2.Text = "XrLabel2";
            this.XrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTable2
            //
            this.XrTable2.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrTable2.LocationFloat = new DevExpress.Utils.PointFloat(9.99999F, 112.0833F);
            this.XrTable2.Name = "XrTable2";
            this.XrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow2 });
            this.XrTable2.SizeF = new System.Drawing.SizeF(1113.299F, 15.00001F);
            this.XrTable2.StylePriority.UseBorders = false;
            this.XrTable2.StylePriority.UseFont = false;
            this.XrTable2.StylePriority.UseTextAlignment = false;
            this.XrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //XrTableRow2
            //
            this.XrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell12, this.XrTableCell13, this.XrTableCell9, this.XrTableCell14, this.XrTableCell15, this.XrTableCell16, this.XrTableCell17, this.XrTableCell18, this.XrTableCell19, this.XrTableCell20 });
            this.XrTableRow2.Name = "XrTableRow2";
            this.XrTableRow2.Weight = 1.0D;
            //
            //XrTableCell12
            //
            this.XrTableCell12.Name = "XrTableCell12";
            this.XrTableCell12.Text = "Date";
            this.XrTableCell12.Weight = 0.77753168610359513D;
            //
            //XrTableCell13
            //
            this.XrTableCell13.Name = "XrTableCell13";
            this.XrTableCell13.Text = "Party Name";
            this.XrTableCell13.Weight = 1.9374985518260106D;
            //
            //XrTableCell9
            //
            this.XrTableCell9.Name = "XrTableCell9";
            this.XrTableCell9.Text = "GSTIN";
            this.XrTableCell9.Weight = 1.9374985518260106D;
            //
            //XrTableCell14
            //
            this.XrTableCell14.Name = "XrTableCell14";
            this.XrTableCell14.StylePriority.UseTextAlignment = false;
            this.XrTableCell14.Text = "Bill Number";
            this.XrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell14.Weight = 2.2609510323735988D;
            //
            //XrTableCell15
            //
            this.XrTableCell15.Name = "XrTableCell15";
            this.XrTableCell15.StylePriority.UseTextAlignment = false;
            this.XrTableCell15.Text = "No Of Bags";
            this.XrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell15.Weight = 0.64578886448587891D;
            //
            //XrTableCell16
            //
            this.XrTableCell16.Name = "XrTableCell16";
            this.XrTableCell16.StylePriority.UseTextAlignment = false;
            this.XrTableCell16.Text = "Weight";
            this.XrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell16.Weight = 0.9394851313904371D;
            //
            //XrTableCell17
            //
            this.XrTableCell17.Name = "XrTableCell17";
            this.XrTableCell17.StylePriority.UseTextAlignment = false;
            this.XrTableCell17.Text = "Taxable Value";
            this.XrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell17.Weight = 1.0000003840679048D;
            //
            //XrTableCell18
            //
            this.XrTableCell18.Name = "XrTableCell18";
            this.XrTableCell18.StylePriority.UseTextAlignment = false;
            this.XrTableCell18.Text = "SGST";
            this.XrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell18.Weight = 0.79556956454991057D;
            //
            //XrTableCell19
            //
            this.XrTableCell19.Name = "XrTableCell19";
            this.XrTableCell19.StylePriority.UseTextAlignment = false;
            this.XrTableCell19.Text = "CGST";
            this.XrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell19.Weight = 0.76805696454984518D;
            //
            //XrTableCell20
            //
            this.XrTableCell20.Name = "XrTableCell20";
            this.XrTableCell20.StylePriority.UseTextAlignment = false;
            this.XrTableCell20.Text = "IGST";
            this.XrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.XrTableCell20.Weight = 0.87511752636683893D;
            //
            //XrPageInfo2
            //
            this.XrPageInfo2.Format = "Page {0} of {1}";
            this.XrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(1031.25F, 89.08335F);
            this.XrPageInfo2.Name = "XrPageInfo2";
            this.XrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrPageInfo2.SizeF = new System.Drawing.SizeF(72.95819F, 14.37502F);
            this.XrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            //
            //ReportFooter
            //
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable4 });
            this.ReportFooter.HeightF = 15.0F;
            this.ReportFooter.Name = "ReportFooter";
            //
            //XrTable4
            //
            this.XrTable4.Borders = (DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable4.Font = new System.Drawing.Font("Times New Roman", 10.0F, System.Drawing.FontStyle.Bold);
            this.XrTable4.LocationFloat = new DevExpress.Utils.PointFloat(7.88121F, 0.0F);
            this.XrTable4.Name = "XrTable4";
            this.XrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow4 });
            this.XrTable4.SizeF = new System.Drawing.SizeF(1112.119F, 15.0F);
            this.XrTable4.StylePriority.UseBorders = false;
            this.XrTable4.StylePriority.UseFont = false;
            //
            //XrTableRow4
            //
            this.XrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell26, this.XrTableCell21, this.XrTableCell22, this.XrTableCell23, this.XrTableCell24, this.XrTableCell31, this.XrTableCell32 });
            this.XrTableRow4.Name = "XrTableRow4";
            this.XrTableRow4.Weight = 1.0D;
            //
            //XrTableCell26
            //
            this.XrTableCell26.Name = "XrTableCell26";
            this.XrTableCell26.StylePriority.UseTextAlignment = false;
            this.XrTableCell26.Text = "Grand Total  : ";
            this.XrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell26.Weight = 5.6515593628602527D;
            //
            //XrTableCell21
            //
            this.XrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.NoOfBags") });
            this.XrTableCell21.Name = "XrTableCell21";
            this.XrTableCell21.StylePriority.UseTextAlignment = false;
            XrSummary7.FormatString = "{0:#}";
            XrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell21.Summary = XrSummary7;
            this.XrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell21.Weight = 0.52618294199246018D;
            //
            //XrTableCell22
            //
            this.XrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TotalWeight") });
            this.XrTableCell22.Name = "XrTableCell22";
            this.XrTableCell22.StylePriority.UseTextAlignment = false;
            XrSummary8.FormatString = "{0:##,##,###.00}";
            XrSummary8.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell22.Summary = XrSummary8;
            this.XrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell22.Weight = 0.62897543467857631D;
            //
            //XrTableCell23
            //
            this.XrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Taxable") });
            this.XrTableCell23.Name = "XrTableCell23";
            this.XrTableCell23.StylePriority.UseTextAlignment = false;
            XrSummary9.FormatString = "{0:##,##,###.00}";
            XrSummary9.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell23.Summary = XrSummary9;
            this.XrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell23.Weight = 0.842094511973338D;
            //
            //XrTableCell24
            //
            this.XrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.SGST") });
            this.XrTableCell24.Name = "XrTableCell24";
            this.XrTableCell24.StylePriority.UseTextAlignment = false;
            XrSummary10.FormatString = "{0:##,##,###.00}";
            XrSummary10.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell24.Summary = XrSummary10;
            this.XrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell24.Weight = 0.666426435966372D;
            //
            //XrTableCell31
            //
            this.XrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CGST") });
            this.XrTableCell31.Name = "XrTableCell31";
            this.XrTableCell31.StylePriority.UseTextAlignment = false;
            XrSummary11.FormatString = "{0:##,##,###.00}";
            XrSummary11.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell31.Summary = XrSummary11;
            this.XrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell31.Weight = 0.71681020287995856D;
            //
            //XrTableCell32
            //
            this.XrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IGST") });
            this.XrTableCell32.Name = "XrTableCell32";
            this.XrTableCell32.StylePriority.UseTextAlignment = false;
            XrSummary12.FormatString = "{0:##,##,###.00}";
            XrSummary12.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.XrTableCell32.Summary = XrSummary12;
            this.XrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell32.Weight = 0.68422097272576821D;
            //
            //IsTrading
            //
            this.IsTrading.DataMember = "Company";
            this.IsTrading.Expression = "Iif([TotalWeight]>0,' Stock Transactions'  ,'Other Transactions' )";
            this.IsTrading.Name = "IsTrading";
            //
            //GroupHeader1
            //
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrLabel1 });
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("CommodityId", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader1.HeightF = 23.0F;
            this.GroupHeader1.Level = 5;
            this.GroupHeader1.Name = "GroupHeader1";
            //
            //XrLabel1
            //
            this.XrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.ItemName") });
            this.XrLabel1.Font = new System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Bold);
            this.XrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0.0F);
            this.XrLabel1.Name = "XrLabel1";
            this.XrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel1.SizeF = new System.Drawing.SizeF(1094.208F, 23.0F);
            this.XrLabel1.StylePriority.UseFont = false;
            this.XrLabel1.StylePriority.UseTextAlignment = false;
            this.XrLabel1.Text = "XrLabel1";
            this.XrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //GroupFooter1
            //
            this.GroupFooter1.HeightF = 0.0F;
            this.GroupFooter1.Level = 5;
            this.GroupFooter1.Name = "GroupFooter1";
            //
            //GroupHeader2
            //
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrLabel6 });
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("IsTrading", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader2.HeightF = 23.0F;
            this.GroupHeader2.Level = 4;
            this.GroupHeader2.Name = "GroupHeader2";
            //
            //XrLabel6
            //
            this.XrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IsTrading") });
            this.XrLabel6.Font = new System.Drawing.Font("Times New Roman", 12.0F, System.Drawing.FontStyle.Bold);
            this.XrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(10.00003F, 0.0F);
            this.XrLabel6.Name = "XrLabel6";
            this.XrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel6.SizeF = new System.Drawing.SizeF(1094.208F, 23.0F);
            this.XrLabel6.StylePriority.UseFont = false;
            this.XrLabel6.StylePriority.UseTextAlignment = false;
            this.XrLabel6.Text = "XrLabel6";
            this.XrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            //
            //GroupFooter2
            //
            this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable5 });
            this.GroupFooter2.HeightF = 15.0F;
            this.GroupFooter2.Level = 4;
            this.GroupFooter2.Name = "GroupFooter2";
            //
            //XrTable5
            //
            this.XrTable5.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable5.Font = new System.Drawing.Font("Times New Roman", 10.0F, System.Drawing.FontStyle.Bold);
            this.XrTable5.LocationFloat = new DevExpress.Utils.PointFloat(7.881226F, 0.0F);
            this.XrTable5.Name = "XrTable5";
            this.XrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow5 });
            this.XrTable5.SizeF = new System.Drawing.SizeF(1112.119F, 15.0F);
            this.XrTable5.StylePriority.UseBorders = false;
            this.XrTable5.StylePriority.UseFont = false;
            //
            //XrTableRow5
            //
            this.XrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell39, this.XrTableCell33, this.XrTableCell34, this.XrTableCell35, this.XrTableCell36, this.XrTableCell37, this.XrTableCell38 });
            this.XrTableRow5.Name = "XrTableRow5";
            this.XrTableRow5.Weight = 1.0D;
            //
            //XrTableCell39
            //
            this.XrTableCell39.Font = new System.Drawing.Font("Times New Roman", 10.0F, System.Drawing.FontStyle.Bold);
            this.XrTableCell39.Name = "XrTableCell39";
            this.XrTableCell39.StylePriority.UseFont = false;
            this.XrTableCell39.StylePriority.UseTextAlignment = false;
            this.XrTableCell39.Text = "Item wise Total  : ";
            this.XrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell39.Weight = 5.708748052639443D;
            //
            //XrTableCell33
            //
            this.XrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.NoOfBags") });
            this.XrTableCell33.Name = "XrTableCell33";
            this.XrTableCell33.StylePriority.UseTextAlignment = false;
            XrSummary13.FormatString = "{0:#}";
            XrSummary13.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell33.Summary = XrSummary13;
            this.XrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell33.Weight = 0.53150692336103222D;
            //
            //XrTableCell34
            //
            this.XrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TotalWeight") });
            this.XrTableCell34.Name = "XrTableCell34";
            this.XrTableCell34.StylePriority.UseTextAlignment = false;
            XrSummary14.FormatString = "{0:##,##,###.00}";
            XrSummary14.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell34.Summary = XrSummary14;
            this.XrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell34.Weight = 0.6353405064588622D;
            //
            //XrTableCell35
            //
            this.XrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Taxable") });
            this.XrTableCell35.Name = "XrTableCell35";
            this.XrTableCell35.StylePriority.UseTextAlignment = false;
            XrSummary15.FormatString = "{0:##,##,###.00}";
            XrSummary15.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell35.Summary = XrSummary15;
            this.XrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell35.Weight = 0.8506150312716213D;
            //
            //XrTableCell36
            //
            this.XrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.SGST") });
            this.XrTableCell36.Name = "XrTableCell36";
            this.XrTableCell36.StylePriority.UseTextAlignment = false;
            XrSummary16.FormatString = "{0:##,##,###.00}";
            XrSummary16.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell36.Summary = XrSummary16;
            this.XrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell36.Weight = 0.67316993617926557D;
            //
            //XrTableCell37
            //
            this.XrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CGST") });
            this.XrTableCell37.Name = "XrTableCell37";
            this.XrTableCell37.StylePriority.UseTextAlignment = false;
            XrSummary17.FormatString = "{0:##,##,###.00}";
            XrSummary17.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell37.Summary = XrSummary17;
            this.XrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell37.Weight = 0.724064699655347D;
            //
            //XrTableCell38
            //
            this.XrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IGST") });
            this.XrTableCell38.Name = "XrTableCell38";
            this.XrTableCell38.StylePriority.UseTextAlignment = false;
            XrSummary18.FormatString = "{0:##,##,###.00}";
            XrSummary18.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell38.Summary = XrSummary18;
            this.XrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell38.Weight = 0.6911431044498545D;
            //
            //GroupHeader3
            //
            this.GroupHeader3.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("TranctDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader3.HeightF = 0.0F;
            this.GroupHeader3.Level = 2;
            this.GroupHeader3.Name = "GroupHeader3";
            //
            //GroupHeader4
            //
            this.GroupHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrLabel7 });
            this.GroupHeader4.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("VochType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader4.HeightF = 23.0F;
            this.GroupHeader4.Level = 3;
            this.GroupHeader4.Name = "GroupHeader4";
            //
            //XrLabel7
            //
            this.XrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.VoucherName") });
            this.XrLabel7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.XrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(10.00003F, 0.0F);
            this.XrLabel7.Name = "XrLabel7";
            this.XrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0F);
            this.XrLabel7.SizeF = new System.Drawing.SizeF(353.125F, 23.0F);
            this.XrLabel7.StylePriority.UseFont = false;
            this.XrLabel7.Text = "XrLabel7";
            //
            //GroupHeader5
            //
            this.GroupHeader5.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("LedgerName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader5.HeightF = 0.0F;
            this.GroupHeader5.Level = 1;
            this.GroupHeader5.Name = "GroupHeader5";
            //
            //GroupFooter3
            //
            this.GroupFooter3.HeightF = 1.041667F;
            this.GroupFooter3.Level = 1;
            this.GroupFooter3.Name = "GroupFooter3";
            //
            //GroupFooter4
            //
            this.GroupFooter4.HeightF = 0.0F;
            this.GroupFooter4.Level = 2;
            this.GroupFooter4.Name = "GroupFooter4";
            //
            //GroupFooter5
            //
            this.GroupFooter5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable7 });
            this.GroupFooter5.HeightF = 15.0F;
            this.GroupFooter5.Level = 3;
            this.GroupFooter5.Name = "GroupFooter5";
            //
            //XrTable7
            //
            this.XrTable7.Borders = (DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom);
            this.XrTable7.Font = new System.Drawing.Font("Times New Roman", 10.0F, System.Drawing.FontStyle.Bold);
            this.XrTable7.LocationFloat = new DevExpress.Utils.PointFloat(7.88121F, 0.0F);
            this.XrTable7.Name = "XrTable7";
            this.XrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] { this.XrTableRow7 });
            this.XrTable7.SizeF = new System.Drawing.SizeF(1113.119F, 15.0F);
            this.XrTable7.StylePriority.UseBorders = false;
            this.XrTable7.StylePriority.UseFont = false;
            //
            //XrTableRow7
            //
            this.XrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] { this.XrTableCell25, this.XrTableCell43, this.XrTableCell44, this.XrTableCell45, this.XrTableCell46, this.XrTableCell47, this.XrTableCell48 });
            this.XrTableRow7.Name = "XrTableRow7";
            this.XrTableRow7.Weight = 1.0D;
            //
            //XrTableCell25
            //
            this.XrTableCell25.Name = "XrTableCell25";
            this.XrTableCell25.StylePriority.UseTextAlignment = false;
            this.XrTableCell25.Text = "Voucher Type wise Total  :         ";
            this.XrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell25.Weight = 5.7705141391183794D;
            //
            //XrTableCell43
            //
            this.XrTableCell43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.NoOfBags") });
            this.XrTableCell43.Name = "XrTableCell43";
            this.XrTableCell43.StylePriority.UseTextAlignment = false;
            XrSummary19.FormatString = "{0:#}";
            XrSummary19.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell43.Summary = XrSummary19;
            this.XrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell43.Weight = 0.5372586691069956D;
            //
            //XrTableCell44
            //
            this.XrTableCell44.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.TotalWeight") });
            this.XrTableCell44.Name = "XrTableCell44";
            this.XrTableCell44.StylePriority.UseTextAlignment = false;
            XrSummary20.FormatString = "{0:##,##,###.00}";
            XrSummary20.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell44.Summary = XrSummary20;
            this.XrTableCell44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell44.Weight = 0.64221362996802911D;
            //
            //XrTableCell45
            //
            this.XrTableCell45.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.Taxable") });
            this.XrTableCell45.Name = "XrTableCell45";
            this.XrTableCell45.StylePriority.UseTextAlignment = false;
            XrSummary21.FormatString = "{0:##,##,###.00}";
            XrSummary21.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell45.Summary = XrSummary21;
            this.XrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell45.Weight = 0.85981953704901859D;
            //
            //XrTableCell46
            //
            this.XrTableCell46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.SGST") });
            this.XrTableCell46.Name = "XrTableCell46";
            this.XrTableCell46.StylePriority.UseTextAlignment = false;
            XrSummary22.FormatString = "{0:##,##,###.00}";
            XrSummary22.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell46.Summary = XrSummary22;
            this.XrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell46.Weight = 0.68045233424382245D;
            //
            //XrTableCell47
            //
            this.XrTableCell47.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.CGST") });
            this.XrTableCell47.Name = "XrTableCell47";
            this.XrTableCell47.StylePriority.UseTextAlignment = false;
            XrSummary23.FormatString = "{0:##,##,###.00}";
            XrSummary23.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell47.Summary = XrSummary23;
            this.XrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell47.Weight = 0.73190094143631568D;
            //
            //XrTableCell48
            //
            this.XrTableCell48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Company.IGST") });
            this.XrTableCell48.Name = "XrTableCell48";
            this.XrTableCell48.StylePriority.UseTextAlignment = false;
            XrSummary24.FormatString = "{0:##,##,###.00}";
            XrSummary24.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.XrTableCell48.Summary = XrSummary24;
            this.XrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.XrTableCell48.Weight = 0.70754026885038279D;
            //
            //ItemName
            //
            this.ItemName.DataMember = "Company";
            this.ItemName.Expression = "[CommodityName]+'   HSN:'+[HSN]";
            this.ItemName.Name = "ItemName";
            //
            //PartyName
            //
            this.PartyName.DataMember = "Company";
            this.PartyName.Expression = "' '+[LedgerName]";
            this.PartyName.Name = "PartyName";
            //
            //PageFooter
            //
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrLine1 });
            this.PageFooter.HeightF = 7.374954F;
            this.PageFooter.Name = "PageFooter";
            //
            //XrLine1
            //
            this.XrLine1.LocationFloat = new DevExpress.Utils.PointFloat(10.00005F, 0.0F);
            this.XrLine1.Name = "XrLine1";
            this.XrLine1.SizeF = new System.Drawing.SizeF(1111.0F, 2.0F);
            //
            //HideWeight
            //
            this.HideWeight.Name = "HideWeight";
            //
            //GroupHeader6
            //
            this.GroupHeader6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.XrTable1 });
            this.GroupHeader6.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] { new DevExpress.XtraReports.UI.GroupField("PartyInvoiceNumber", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending) });
            this.GroupHeader6.HeightF = 16.66667F;
            this.GroupHeader6.Name = "GroupHeader6";
            //
            //rptHSNWiseReport
            //
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] { this.Detail, this.TopMargin, this.BottomMargin, this.PageHeader, this.ReportFooter, this.GroupHeader1, this.GroupFooter1, this.GroupHeader2, this.GroupFooter2, this.GroupHeader3, this.GroupHeader4, this.GroupHeader5, this.GroupFooter3, this.GroupFooter4, this.GroupFooter5, this.PageFooter, this.GroupHeader6 });
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] { this.IsTrading, this.ItemName, this.PartyName });
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] { this.SqlDataSource1 });
            this.DataMember = "Company";
            this.DataSource = this.SqlDataSource1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] { this.HideWeight });
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(24, 14, 0, 17);
            this.PageHeight = 827;
            this.PageWidth = 1169;
            this.PaperKind = (DevExpress.Drawing.Printing.DXPaperKind)System.Drawing.Printing.PaperKind.A4;
            this.ScriptLanguage = DevExpress.XtraReports.ScriptLanguage.VisualBasic;
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)this.XrTable1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable2).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable4).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable5).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.XrTable7).EndInit();
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
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell11;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell2;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell3;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell4;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell5;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell6;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell7;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell8;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel5;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel4;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel3;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel2;
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
        internal DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        internal DevExpress.XtraReports.UI.XRLabel lblHeader;
        internal DevExpress.XtraReports.UI.XRTable XrTable4;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow4;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell21;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell22;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell23;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell24;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell31;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell32;
        internal DevExpress.XtraReports.UI.XRPageInfo XrPageInfo2;
        internal DevExpress.XtraReports.UI.CalculatedField IsTrading;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel6;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter2;
        internal DevExpress.XtraReports.UI.XRTable XrTable5;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow5;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell33;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell34;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell35;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell36;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell37;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell38;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader3;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader4;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader5;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell39;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel7;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter3;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter4;
        internal DevExpress.XtraReports.UI.GroupFooterBand GroupFooter5;
        internal DevExpress.XtraReports.UI.XRTable XrTable7;
        internal DevExpress.XtraReports.UI.XRTableRow XrTableRow7;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell43;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell44;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell45;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell46;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell47;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell48;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell25;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell26;
        internal DevExpress.XtraReports.UI.XRLabel XrLabel1;
        internal DevExpress.XtraReports.UI.CalculatedField ItemName;
        internal DevExpress.XtraReports.UI.CalculatedField PartyName;
        internal DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        internal DevExpress.XtraReports.UI.XRLine XrLine1;
        internal DevExpress.XtraReports.UI.FormattingRule HideWeight;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell10;
        internal DevExpress.XtraReports.UI.XRTableCell XrTableCell9;
        internal DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader6;
    }
}
