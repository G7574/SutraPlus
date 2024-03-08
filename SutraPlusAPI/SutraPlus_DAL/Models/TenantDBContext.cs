using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SutraPlus_DAL.Models
{
    public partial class TenantDBContext : DbContext, IDisposable
    {
        private string _connectionString = null;
        public static string staticConnectionString = "";
        public TenantDBContext(string connectionString)
        {
            _connectionString = connectionString;
            staticConnectionString = connectionString;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public TenantDBContext(DbContextOptions<TenantDBContext> options)
            : base(options)
        {
        }

        #region
        public virtual DbSet<AccounitngGroup> AccounitngGroups { get; set; } = null!;
        public virtual DbSet<BagWeight> BagWeights { get; set; } = null!;
        public virtual DbSet<BillSummary> BillSummaries { get; set; } = null!;
        public virtual DbSet<BillwiseReceipt> BillwiseReceipts { get; set; } = null!;
        public virtual DbSet<BusinessType> BusinessTypes { get; set; } = null!;
        public virtual DbSet<CashBook> CashBooks { get; set; } = null!;
        public virtual DbSet<CashBookk> CashBookks { get; set; } = null!;
        public virtual DbSet<ChequeSetting> ChequeSettings { get; set; } = null!;
        public virtual DbSet<CloseSession> CloseSessions { get; set; } = null!;
        public virtual DbSet<Commodity> Commodities { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<DailyCollection> DailyCollections { get; set; } = null!;
        public virtual DbSet<DailyMaster> DailyMasters { get; set; } = null!;
        public virtual DbSet<DailyMasterr> DailyMasterrs { get; set; } = null!;
        public virtual DbSet<DclineItem> DclineItems { get; set; } = null!;
        public virtual DbSet<Dcsummary> Dcsummaries { get; set; } = null!;
        public virtual DbSet<DeclaredValue> DeclaredValues { get; set; } = null!;
        public virtual DbSet<EmailBackup> EmailBackups { get; set; } = null!;
        public virtual DbSet<Expn> Expns { get; set; } = null!;
        public virtual DbSet<Expnss> Expnsses { get; set; } = null!;
        public virtual DbSet<ExtendedDate> ExtendedDates { get; set; } = null!;
        public virtual DbSet<ExtendedDates2> ExtendedDates2s { get; set; } = null!;
        public virtual DbSet<Gstin> Gstins { get; set; } = null!;
        public virtual DbSet<Gstquantra> Gstquantras { get; set; } = null!;
        public virtual DbSet<IndWtMasterCmn> IndWtMasterCmns { get; set; } = null!;
        public virtual DbSet<IndWtMasterr> IndWtMasterrs { get; set; } = null!;
        public virtual DbSet<Inventory> Inventory { get; set; } = null!;
        public virtual DbSet<InwardEntry> InwardEntries { get; set; } = null!;
        public virtual DbSet<Ledger> Ledgers { get; set; } = null!;
        public virtual DbSet<Loginn> Loginns { get; set; } = null!;
        public virtual DbSet<OldPayment> OldPayments { get; set; } = null!;
        public virtual DbSet<OpeningStock> OpeningStocks { get; set; } = null!;
        public virtual DbSet<PadatalMaster> PadatalMasters { get; set; } = null!;
        public virtual DbSet<PadatalRate> PadatalRates { get; set; } = null!;
        public virtual DbSet<ProductionMaster> ProductionMasters { get; set; } = null!;
        public virtual DbSet<QntrMaster> QntrMasters { get; set; } = null!;
        public virtual DbSet<RateMaster> RateMasters { get; set; } = null!;
        public virtual DbSet<RateMasterr> RateMasterrs { get; set; } = null!;
        public virtual DbSet<ReFilled> ReFilleds { get; set; } = null!;
        public virtual DbSet<SalesSummary> SalesSummaries { get; set; } = null!;
        public virtual DbSet<ServicesPurchased> ServicesPurchaseds { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<Summary> Summaries { get; set; } = null!;
        public virtual DbSet<TableHistory> TableHistories { get; set; } = null!;
        public virtual DbSet<TblUserRight> TblUserRights { get; set; } = null!;
        public virtual DbSet<TblYear> TblYears { get; set; } = null!;
        public virtual DbSet<Tdsmaster> Tdsmasters { get; set; } = null!;
        public virtual DbSet<TempMarkwise> TempMarkwises { get; set; } = null!;
        public virtual DbSet<TmpBl> TmpBls { get; set; } = null!;
        public virtual DbSet<TmpCashBook> TmpCashBooks { get; set; } = null!;
        public virtual DbSet<TmpCess> TmpCesses { get; set; } = null!;
        public virtual DbSet<TmpCessWk> TmpCessWks { get; set; } = null!;
        public virtual DbSet<TmpChopadaSummary> TmpChopadaSummaries { get; set; } = null!;
        public virtual DbSet<TmpChpd> TmpChpds { get; set; } = null!;
        public virtual DbSet<TmpChpd1> TmpChpd1s { get; set; } = null!;
        public virtual DbSet<TmpCshbk> TmpCshbks { get; set; } = null!;
        public virtual DbSet<TmpDelNote> TmpDelNotes { get; set; } = null!;
        public virtual DbSet<TmpDt> TmpDts { get; set; } = null!;
        public virtual DbSet<TmpDtss> TmpDtsses { get; set; } = null!;
        public virtual DbSet<TmpFinalize> TmpFinalizes { get; set; } = null!;
        public virtual DbSet<TmpHdDtl> TmpHdDtls { get; set; } = null!;
        public virtual DbSet<TmpKpYadi> TmpKpYadis { get; set; } = null!;
        public virtual DbSet<TmpPaymentList> TmpPaymentList { get; set; } = null!;
        public virtual DbSet<TmpLedger> TmpLedgers { get; set; } = null!;
        public virtual DbSet<TmpPl> TmpPls { get; set; } = null!;
        public virtual DbSet<TmpReceivable> TmpReceivables { get; set; } = null!;
        public virtual DbSet<TmpStockLedger> TmpStockLedgers { get; set; } = null!;
        public virtual DbSet<TmpSummary> TmpSummaries { get; set; } = null!;
        public virtual DbSet<TmpTaxSlab> TmpTaxSlabs { get; set; } = null!;
        public virtual DbSet<TmpTd> TmpTds { get; set; } = null!;
        public virtual DbSet<TmpTdsList> TmpTdsLists { get; set; } = null!;
        public virtual DbSet<TmpTndrFrm> TmpTndrFrms { get; set; } = null!;
        public virtual DbSet<TmpTrialBalance> TmpTrialBalances { get; set; } = null!;
        public virtual DbSet<TmpWtBill> TmpWtBills { get; set; } = null!;
        public virtual DbSet<TmpYadiCmn> TmpYadiCmns { get; set; } = null!;
        public virtual DbSet<Tmptdslist1> Tmptdslists1 { get; set; } = null!;
        public virtual DbSet<Tmpyadi> Tmpyadis { get; set; } = null!;
        public virtual DbSet<TndrFrm> TndrFrms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Vareity> Vareities { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<VoucherType> VoucherTypes { get; set; } = null!;
        public virtual DbSet<FormMaster> FormMasters { get; set; } = null!;
        public virtual DbSet<UserAccesMaster> UserAccesMasters { get; set; } = null!;
        public virtual DbSet<Dealer_Type> Dealer_Types { get; set; } = null!;
        public virtual DbSet<Ledger_Type> Ledger_Types { get; set; } = null!;
        // public virtual DbSet<SalesInvoice> SalesInvoices { get; set; } = null!; ///UI Model


        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccounitngGroup>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AccontingGroupId).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LiaorAsset)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PandLposition).HasColumnName("PandLPosition");

                entity.Property(e => e.UnderHead)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BagWeight>(entity =>
            {
                entity.HasKey(e => e.id);

                entity.Property(e => e.BagWeight1).HasColumnName("BagWeight");

                entity.Property(e => e.BuyerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<BillSummary>(entity =>
            {

                entity.ToTable("BillSummary");

                //modelBuilder.Entity<MyTable>()
                entity.Property(e => e._Id)
               .HasMaxLength(150)
               .IsUnicode(false);
                entity.Property(e => e.CompanyId)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.LedgerId)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.LedgerName)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.VochType)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.VoucherName)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VochNo)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.EwayBillNo)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.Ponumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Transporter)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.LorryNo)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.LorryOwnerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DriverName)
               .HasMaxLength(50)
               .IsUnicode(false);
                entity.Property(e => e.Dlno)
                      .HasMaxLength(50)
                      .IsUnicode(false);
                entity.Property(e => e.CheckPost)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FrieghtPerBag)
                      .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TotalFrieght)
                      .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Advance)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Balance)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AREDate)
                         .HasMaxLength(50)
                     .IsUnicode(false);
                entity.Property(e => e.ARENo)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsLessOrPlus)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExpenseName1)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExpenseName2)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExpenseName3)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExpenseAmount1)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExpenseAmount2)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.ExpenseAmount3)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DeliveryName)
                      .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DeliveryAddress1)
                       .HasMaxLength(50)
                       .IsUnicode(false);
                entity.Property(e => e.DeliveryAddress2)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DeliveryPlace)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DeliveryState)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DeliveryStateCode)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.BillAmount)
                      .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.INWords)
                        .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FrieghtAmount)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.RoundOff)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.TotalBags)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.TotalWeight)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.TotalAmount)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.PackingValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.HamaliValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.WeighnamFeeValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DalaliValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.CessValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.TaxableValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.SGSTValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.CSGSTValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.IGSTValue)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.UserId)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.SessionID)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.PaymentBy)
                       .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.AmountReceived)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Change)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CardDetails)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BillTime)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TranctDate)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CustomerName)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CustomerPlace)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CustomerContactNo)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PartyInvoiceNumber)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OtherCreated)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Recent)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentReceived)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TagName)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TagDate)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ToPrint)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.frieghtPlus)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateCode1)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StateCode2)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExpenseTax)
                 .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FrieghtinBill)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.LastOpenend)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriCommission)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriULH)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriCashAdvance)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriFrieght)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriEmpty)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriNet)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.CashToFarmer)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriOther1)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriOther2)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriOther1Name)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.VikriOther2Name)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.Note1)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.FromPlace)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.ToPlace)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DisplayNo)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DisplayinvNo)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.FrieghtLabel)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.FormName)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.DCNote)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.WeightInString)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.tdsperc)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.CommodityID)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.TCSValue)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.TCSPerc)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.SGSTLabel)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.CGSTLabel)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.IGSTLabel)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.TCSLabel)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.QrCode)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.IsGSTUpload)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DelPinCode)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DispatcherName)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DispatcherAddress1)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DispatcherAddress2)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DispatcherPlace)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DispatcherPIN)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DispatcherStatecode)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.CountryCode)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ShipBillNo)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ForCur)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.PortName)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.RefClaim)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.ShipBillDate)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.ExpDuty)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.monthNo)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.IsSEZ)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.id)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.Distance)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.EInvoiceNo)
                .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.GSTR1SectionName)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.GSTR1InvoiceType)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.OriginalInvNo)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.AprClosed)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.OriginalInvDate)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.Discount)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.TwelveValue)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.FiveValue)
               .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.ACKNO)
              .HasMaxLength(50)
               .IsUnicode(false);
                entity.Property(e => e.IRNNO)
              .HasMaxLength(50)
               .IsUnicode(false);
                entity.Property(e => e.SignQRCODE)
              .HasMaxLength(50)
               .IsUnicode(false);
                entity.Property(e => e.IsActive)
            .HasMaxLength(50)
             .IsUnicode(false);
                entity.Property(e => e.IsServiceInvoice)
           .HasMaxLength(50)
            .IsUnicode(false);

            });

            modelBuilder.Entity<BillwiseReceipt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BillwiseReceipt");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.LedgerId).HasColumnType("money");

                entity.Property(e => e.Narration).HasColumnType("money");

                entity.Property(e => e.ReceiptVochNo).HasColumnType("money");

                entity.Property(e => e.VochId).HasColumnType("money");

                entity.Property(e => e.VochNo).HasColumnType("money");
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BusinessType");

                entity.Property(e => e.BusinessTypename)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CashBook>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cash_book");

                entity.Property(e => e.AcPay).HasColumnName("ac_pay");

                entity.Property(e => e.AgPay).HasColumnName("ag_pay");

                entity.Property(e => e.AssoFee)
                    .HasColumnType("money")
                    .HasColumnName("Asso_fee");

                entity.Property(e => e.AverageRate)
                    .HasColumnType("money")
                    .HasColumnName("average_rate");

                entity.Property(e => e.BasicValue)
                    .HasColumnType("money")
                    .HasColumnName("Basic_value");

                entity.Property(e => e.BatchId).HasColumnName("Batch_id");

                entity.Property(e => e.BatchNo).HasColumnName("batch_no");

                entity.Property(e => e.BillNumber).HasColumnName("bill_number");

                entity.Property(e => e.BnkId).HasColumnName("bnk_id");

                entity.Property(e => e.Cess).HasColumnType("money");

                entity.Property(e => e.Cgst)
                    .HasColumnType("money")
                    .HasColumnName("cgst");

                entity.Property(e => e.ChkNo).HasColumnName("chk_no");

                entity.Property(e => e.Chno).HasColumnName("chno");

                entity.Property(e => e.ClosingEntry).HasColumnName("closing_entry");

                entity.Property(e => e.ClosingStock).HasColumnName("closing_stock");

                entity.Property(e => e.Commission).HasColumnType("money");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Credit)
                    .HasColumnType("money")
                    .HasColumnName("credit");

                entity.Property(e => e.CreditNoteNo).HasColumnName("Credit_Note_No");

                entity.Property(e => e.Csgt)
                    .HasColumnType("money")
                    .HasColumnName("csgt");

                entity.Property(e => e.DdlPaid).HasColumnName("ddl_paid");

                entity.Property(e => e.DdlTp).HasColumnName("ddl_tp");

                entity.Property(e => e.Debit)
                    .HasColumnType("money")
                    .HasColumnName("debit");

                entity.Property(e => e.DebitNoteNo).HasColumnName("debit_note_no");

                entity.Property(e => e.DnNo).HasColumnName("dn_no");

                entity.Property(e => e.EditId).HasColumnName("edit_id");

                entity.Property(e => e.EntrTp).HasColumnName("entr_tp");

                entity.Property(e => e.EntryId).HasColumnName("entry_id");

                entity.Property(e => e.ExportInvNo).HasColumnName("Export_Inv_No");

                entity.Property(e => e.FarPay).HasColumnName("FAR_PAY");

                entity.Property(e => e.ForBuy).HasColumnName("for_buy");

                entity.Property(e => e.FromPr).HasColumnName("from_pr");

                entity.Property(e => e.FromStkTrnsfrQty).HasColumnName("from_Stk_trnsfr_qty");

                entity.Property(e => e.GinInvNo).HasColumnName("gin_inv_no");

                entity.Property(e => e.Gng).HasColumnName("gng");

                entity.Property(e => e.Hamali).HasColumnType("money");

                entity.Property(e => e.Igst)
                    .HasColumnType("money")
                    .HasColumnName("igst");

                entity.Property(e => e.InTheBillOf)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("in_the_bill_of");

                entity.Property(e => e.InvNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("inv_no");

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Inwrds)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("inwrds");

                entity.Property(e => e.IsCommissionTds).HasColumnName("is_commission_tds");

                entity.Property(e => e.IsGinningTds).HasColumnName("is_ginning_tds");

                entity.Property(e => e.IsIgnore)
                    .HasColumnType("money")
                    .HasColumnName("is_ignore");

                entity.Property(e => e.IsLorryTds).HasColumnName("is_lorry_tds");

                entity.Property(e => e.IsMulti).HasColumnName("is_multi");

                entity.Property(e => e.ItId).HasColumnName("It_id");

                entity.Property(e => e.Jrnl).HasColumnName("jrnl");

                entity.Property(e => e.KBlNo).HasColumnName("k_bl_no");

                entity.Property(e => e.LedgerId).HasColumnName("LedgerID");

                entity.Property(e => e.Narration)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("narration");

                entity.Property(e => e.NewEntry).HasColumnName("new_entry");

                entity.Property(e => e.ObQty).HasColumnName("Ob_Qty");

                entity.Property(e => e.OrgDt)
                    .HasColumnType("datetime")
                    .HasColumnName("org_dt");

                entity.Property(e => e.OrgInv).HasColumnName("org_inv");

                entity.Property(e => e.Others).HasColumnType("money");

                entity.Property(e => e.PBlNo).HasColumnName("p_bl_no");

                entity.Property(e => e.Packing).HasColumnType("money");

                entity.Property(e => e.Paid).HasColumnName("paid");

                entity.Property(e => e.PartyInvoiceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PartyInvoiceNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("party_invoice_number");

                entity.Property(e => e.PartyNameforNrtn)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PcaSales).HasColumnName("pca_Sales");

                entity.Property(e => e.PrchRet).HasColumnName("prch_ret");

                entity.Property(e => e.PrchReturn).HasColumnName("prch_return");

                entity.Property(e => e.Prcs).HasColumnName("prcs");

                entity.Property(e => e.PrintNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductionDcNo).HasColumnName("Production_Dc_no");

                entity.Property(e => e.Prt)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("prt");

                entity.Property(e => e.PurchaseQty).HasColumnName("Purchase_Qty");

                entity.Property(e => e.QId).HasColumnName("q_id");

                entity.Property(e => e.Rndf)
                    .HasColumnType("money")
                    .HasColumnName("rndf");

                entity.Property(e => e.SalesBasicValue)
                    .HasColumnType("money")
                    .HasColumnName("SALES_Basic_value");

                entity.Property(e => e.SalesExpenses)
                    .HasColumnType("money")
                    .HasColumnName("sales_expenses");

                entity.Property(e => e.SalesQty).HasColumnName("sales_Qty");

                entity.Property(e => e.SalesWeight).HasColumnName("sales_weight");

                entity.Property(e => e.Sgst)
                    .HasColumnType("money")
                    .HasColumnName("sgst");

                entity.Property(e => e.ShortageQty).HasColumnName("shortage_qty");

                entity.Property(e => e.Slf).HasColumnName("slf");

                entity.Property(e => e.SrBasicValue)
                    .HasColumnType("money")
                    .HasColumnName("sr_basic_value");

                entity.Property(e => e.SrInvNo).HasColumnName("sr_inv_no");

                entity.Property(e => e.SrNo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sr_No");

                entity.Property(e => e.SrQty).HasColumnName("SR_qty");

                entity.Property(e => e.SrTax)
                    .HasColumnType("money")
                    .HasColumnName("sr_tax");

                entity.Property(e => e.StemlessBatchNo).HasColumnName("Stemless_batch_no");

                entity.Property(e => e.StkTrns).HasColumnName("stk_trns");

                entity.Property(e => e.TaxInSales)
                    .HasColumnType("money")
                    .HasColumnName("tax_in_sales");

                entity.Property(e => e.TaxRate)
                    .HasColumnType("money")
                    .HasColumnName("Tax_Rate");

                entity.Property(e => e.TaxRateInSales)
                    .HasColumnType("money")
                    .HasColumnName("Tax_Rate_in_sales");

                entity.Property(e => e.TaxableValue)
                    .HasColumnType("money")
                    .HasColumnName("Taxable_value");

                entity.Property(e => e.ToPr).HasColumnName("to_pr");

                entity.Property(e => e.TotalBags).HasColumnName("Total_bags");

                entity.Property(e => e.TotalWt).HasColumnName("Total_wt");

                entity.Property(e => e.Tp).HasColumnName("tp");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.TradingPq).HasColumnName("trading_pq");

                entity.Property(e => e.TradingSq).HasColumnName("trading_sq");

                entity.Property(e => e.TranType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TranctDate)
                    .HasColumnType("datetime")
                    .HasColumnName("tranct_date");

                entity.Property(e => e.TransType).HasColumnName("Trans_Type");

                entity.Property(e => e.TypeInReg)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Type_in_reg");

                entity.Property(e => e.TypeInSaleReg)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Type_in_SALE_reg");

                entity.Property(e => e.Upl).HasColumnName("upl");

                entity.Property(e => e.VBlNo).HasColumnName("v_bl_no");

                entity.Property(e => e.VMax).HasColumnName("v_max");

                entity.Property(e => e.VMin).HasColumnName("v_min");

                entity.Property(e => e.Vat)
                    .HasColumnType("money")
                    .HasColumnName("VAT");

                entity.Property(e => e.VikriAsf)
                    .HasColumnType("money")
                    .HasColumnName("vikri_asf");

                entity.Property(e => e.VikriBags).HasColumnName("vikri_bags");

                entity.Property(e => e.VikriBastani)
                    .HasColumnType("money")
                    .HasColumnName("vikri_bastani");

                entity.Property(e => e.VikriBlNo).HasColumnName("vikri_bl_no");

                entity.Property(e => e.VikriCess)
                    .HasColumnType("money")
                    .HasColumnName("vikri_cess");

                entity.Property(e => e.VikriCommission)
                    .HasColumnType("money")
                    .HasColumnName("vikri_commission");

                entity.Property(e => e.VikriHamali)
                    .HasColumnType("money")
                    .HasColumnName("vikri_hamali");

                entity.Property(e => e.VikriPacking)
                    .HasColumnType("money")
                    .HasColumnName("vikri_packing");

                entity.Property(e => e.VikriRndf)
                    .HasColumnType("money")
                    .HasColumnName("vikri_rndf");

                entity.Property(e => e.VikriTaxable)
                    .HasColumnType("money")
                    .HasColumnName("vikri_taxable");

                entity.Property(e => e.VikriVat)
                    .HasColumnType("money")
                    .HasColumnName("vikri_vat");

                entity.Property(e => e.VikriWt).HasColumnName("vikri_wt");

                entity.Property(e => e.VikriWtf)
                    .HasColumnType("money")
                    .HasColumnName("vikri_wtf");

                entity.Property(e => e.VochNo).HasColumnName("voch_no");

                entity.Property(e => e.Vochtype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("vochtype");

                entity.Property(e => e.WeighmanFee)
                    .HasColumnType("money")
                    .HasColumnName("Weighman_fee");
            });

            modelBuilder.Entity<CashBookk>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cash_bookk");

                entity.Property(e => e.Credit)
                    .HasColumnType("money")
                    .HasColumnName("credit");

                entity.Property(e => e.Debit)
                    .HasColumnType("money")
                    .HasColumnName("debit");

                entity.Property(e => e.EntrTp).HasColumnName("entr_tp");

                entity.Property(e => e.FarPay).HasColumnName("far_pay");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.HdId).HasColumnName("hd_id");

                entity.Property(e => e.KBlNo).HasColumnName("k_bl_no");

                entity.Property(e => e.Paid).HasColumnName("paid");

                entity.Property(e => e.Prt)
                    .HasMaxLength(2500)
                    .IsUnicode(false)
                    .HasColumnName("prt");

                entity.Property(e => e.QId).HasColumnName("q_id");

                entity.Property(e => e.SrNo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sr_no");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.VBlNo).HasColumnName("v_bl_no");

                entity.Property(e => e.VMax).HasColumnName("v_max");

                entity.Property(e => e.VMin).HasColumnName("v_min");

                entity.Property(e => e.VikriAsf)
                    .HasColumnType("money")
                    .HasColumnName("vikri_asf");

                entity.Property(e => e.VikriBags).HasColumnName("vikri_bags");

                entity.Property(e => e.VikriBastani)
                    .HasColumnType("money")
                    .HasColumnName("vikri_bastani");

                entity.Property(e => e.VikriBlNo).HasColumnName("vikri_bl_no");

                entity.Property(e => e.VikriCess)
                    .HasColumnType("money")
                    .HasColumnName("vikri_cess");

                entity.Property(e => e.VikriCommission)
                    .HasColumnType("money")
                    .HasColumnName("vikri_commission");

                entity.Property(e => e.VikriHamali)
                    .HasColumnType("money")
                    .HasColumnName("vikri_hamali");

                entity.Property(e => e.VikriPacking)
                    .HasColumnType("money")
                    .HasColumnName("vikri_packing");

                entity.Property(e => e.VikriRndf)
                    .HasColumnType("money")
                    .HasColumnName("vikri_rndf");

                entity.Property(e => e.VikriTaxable)
                    .HasColumnType("money")
                    .HasColumnName("vikri_taxable");

                entity.Property(e => e.VikriVat)
                    .HasColumnType("money")
                    .HasColumnName("vikri_vat");

                entity.Property(e => e.VikriWt).HasColumnName("vikri_wt");

                entity.Property(e => e.VikriWtf)
                    .HasColumnType("money")
                    .HasColumnName("vikri_wtf");
            });

            modelBuilder.Entity<ChequeSetting>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<CloseSession>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CloseSession");

                entity.Property(e => e.ClosedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Commodity>(entity =>
            {

                entity.ToTable("Commodity");
                entity.Property(e => e._Id)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CommodityId)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CommodityName)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CompanyId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mou)
                     .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IGST)
                      .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SGST)
                     .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HSN)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnName("HSN");
                entity.Property(e => e.CGST)
                      .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsTrading)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedBy)
                         .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.OpeningStock)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.PcsPerBox)
                   .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RecentPurchaseRate)
                .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MinStockLevel)
                       .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Cess)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Mrp)
                     .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SellingRate)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LocalName)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.IsVikryCommodity)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.IsVikriCommodity)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Obval)
                       .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BarCode)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ShortageWeight)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ClosingWeight)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AvaregeRate)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ClosingValue)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OutLetId);
                entity.Property(e => e.Sno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToPrint)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.DeductTds)
                  .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SearchName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsService)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e._Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CompanyId)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.CompanyName)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.AddressLine1)
                 .HasMaxLength(200)
                 .IsUnicode(false);
                entity.Property(e => e.AddressLine2)
                 .HasMaxLength(200)
                 .IsUnicode(false);
                entity.Property(e => e.AddressLine3)
                 .HasMaxLength(200)
                  .IsUnicode(false);
                entity.Property(e => e.Place)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.Gstin)
                  .HasMaxLength(50)
                  .IsUnicode(false);


                entity.Property(e => e.ContactDetails)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Shree)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.KannadaName)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Pan)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.FirmCode)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Apmccode)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Iec)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Fln)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Bin)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Bank1)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Ifsc1)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AccountNo1)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Bank2)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Ifsc2)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AccountNo2)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Bank3)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Ifsc3)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Account3)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Title)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.District)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.State)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Email)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.CellPhone)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Tan)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Cst)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Tin)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.StandardBilling)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AutoDeductTds)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.CashEntrySystem)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.RateInclusiveTax)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.FarmerBill)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.TraderBill)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.PrintVochour)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Sender)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                //entity.Property(e => e.UserName)
                //  .HasMaxLength(50)
                //  .IsUnicode(false);
                //entity.Property(e => e.UserPass)
                //  .HasMaxLength(50)
                //  .IsUnicode(false);
                entity.Property(e => e.SecondLineForReport)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ThirdLineForReport)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ReportTile1)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ReportTile2)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.CreatedBy)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Logo)
                  .HasMaxLength(2000)
                  .IsUnicode(false);
                entity.Property(e => e.LastOpenend)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.BillNo)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.KannadaAddress)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.KannadaPlace)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Jurisdiction)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Fssai)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.OldFid)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.JurisLine)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                //entity.Property(e => e.RptTitle)
                //  .HasMaxLength(50)
                //  .IsUnicode(false);
                //entity.Property(e => e.HsnCode)
                //  .HasMaxLength(50)
                //  .IsUnicode(false);
                entity.Property(e => e.NameColor)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Printweights)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Cp)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.Gid)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Gpw)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.SelfEmailId)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.SelfWhatsUpNo)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Tcsreq)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.TcsreqinReceipt)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Lutno)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.WebId)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.LicenceKey)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.EinvoiceKey)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.EinvoiceSkey)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.EinvoiceUserName)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.EinvoicePassword)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.EinvoiceReq)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Pin)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.LegalName)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.PortaluserName)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.PortalPw)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.PortalEmail)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.BillCode)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.VochNo)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.VochType)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.LedgerId)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.InvType)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Iscrystal)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ReportName)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Dbname)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DontTaxFrieght)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.NeftAcno)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.IsStandard)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AskTcs)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Setcmdac)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AskPdf)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.IsShopSent)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.HexCode)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.CustomerId)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.IsVps)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ExpiryDate)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AddColumns1)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AskEinvoice)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DeleteWeight)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.VrtinForm)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DirectPrint)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.PrintNumber)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                //entity.Property(e => e.QtyBilling)
                //  .HasMaxLength(50)
                //  .IsUnicode(false);
                entity.Property(e => e.IsDuplicateDeleted)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                //entity.Property(e => e.IsTaxIncl)
                //  .HasMaxLength(50)
                //  .IsUnicode(false);
                entity.Property(e => e.IsVikriReq)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AutoTds)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.Tcstotaxable)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.AskOnlineBackUpe)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.DeleteEx)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.PhyPath)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.ResetPacking)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.NoOfCopies)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.IsActive)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.InvoiceString)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                //entity.HasNoKey();

                //entity.ToTable("Company");

                //entity.Property(e => e.Account3)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.AccountNo1)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.AccountNo2)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.AddressLine1)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.AddressLine2)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.AddressLine3)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.Apmccode)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("APMCCode");

                //entity.Property(e => e.AskEinvoice).HasColumnName("AskEInvoice");

                //entity.Property(e => e.AskPdf).HasColumnName("AskPDF");

                //entity.Property(e => e.AskTcs).HasColumnName("AskTCS");

                //entity.Property(e => e.AutoDeductTds).HasColumnName("AutoDeductTDS");

                //entity.Property(e => e.AutoTds).HasColumnName("AutoTDS");

                //entity.Property(e => e.Bank1)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.Bank2)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.Bank3)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.BillCode)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.Bin)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("BIN");

                //entity.Property(e => e.CellPhone)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.CompanyId).ValueGeneratedOnAdd();

                //entity.Property(e => e.CompanyName)
                //    .HasMaxLength(100)
                //    .IsUnicode(false);

                //entity.Property(e => e.ContactDetails)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                //entity.Property(e => e.Cst)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("CST");

                //entity.Property(e => e.CustomerId)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("CustomerID");

                //entity.Property(e => e.Dbname)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("DBNAME");

                //entity.Property(e => e.District).HasMaxLength(50);

                //entity.Property(e => e.EinvoiceKey)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.EinvoicePassword)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.EinvoiceReq).HasColumnName("EInvoiceReq");

                //entity.Property(e => e.EinvoiceSkey)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.EinvoiceUserName)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.Email)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                //entity.Property(e => e.FirmCode)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.Fln)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("FLN");

                //entity.Property(e => e.Fssai)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("FSSAI");

                //entity.Property(e => e.Gid)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("GID");

                //entity.Property(e => e.Gpw)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("GPW");

                //entity.Property(e => e.Gstin)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("GSTIN");

                //entity.Property(e => e.HexCode)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.HsnCode)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("hsn_code");

                //entity.Property(e => e.Iec)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("IEC");

                //entity.Property(e => e.Ifsc1)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("IFSC1");

                //entity.Property(e => e.Ifsc2)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("IFSC2");

                //entity.Property(e => e.Ifsc3)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("IFSC3");

                //entity.Property(e => e.InvType)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.IsTaxIncl).HasColumnName("is_tax_incl");

                //entity.Property(e => e.JurisLine)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.Jurisdiction)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.KannadaAddress)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.KannadaName)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.KannadaPlace)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.LastOpenend).HasColumnType("datetime");

                //entity.Property(e => e.LegalName)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.LicenceKey)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.Logo)
                //    .HasColumnType("image")
                //    .HasColumnName("logo");

                //entity.Property(e => e.Lutno)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("LUTNO");

                //entity.Property(e => e.NameColor)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.NeftAcno)
                //    .HasMaxLength(250)
                //    .IsUnicode(false);

                //entity.Property(e => e.OldFid).HasColumnName("oldFid");

                //entity.Property(e => e.Pan)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("PAN");

                //entity.Property(e => e.PhyPath)
                //    .HasMaxLength(500)
                //    .IsUnicode(false);

                //entity.Property(e => e.Pin)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("PIN");

                //entity.Property(e => e.Place)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.PortalEmail)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.PortalPw)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.PortaluserName)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.PrintNumber)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.QtyBilling).HasColumnName("qty_billing");

                //entity.Property(e => e.ReportName)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.ReportTile1)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.ReportTile2)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.RptTitle)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("Rpt_Title");

                //entity.Property(e => e.SecondLineForReport)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.SelfEmailId)
                //    .HasMaxLength(150)
                //    .IsUnicode(false)
                //    .HasColumnName("SelfEmailID");

                //entity.Property(e => e.SelfWhatsUpNo)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.Sender)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.Setcmdac).HasColumnName("SETCMDAC");

                //entity.Property(e => e.Shree)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.State)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);

                //entity.Property(e => e.Tan)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("TAN");

                //entity.Property(e => e.Tcsreq).HasColumnName("TCSREQ");

                //entity.Property(e => e.TcsreqinReceipt).HasColumnName("TCSREQinReceipt");

                //entity.Property(e => e.Tcstotaxable).HasColumnName("TCSTOTaxable");

                //entity.Property(e => e.ThirdLineForReport)
                //    .HasMaxLength(150)
                //    .IsUnicode(false);

                //entity.Property(e => e.Tin)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("TIN");

                //entity.Property(e => e.Title).HasMaxLength(150);

                //entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                //entity.Property(e => e.UserName)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("User_Name");

                //entity.Property(e => e.UserPass)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("User_Pass");
                //entity.Property(e => e.IsActive)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("IsActive");


                //entity.Property(e => e.WebId).HasColumnName("WebID");
            });

            modelBuilder.Entity<DailyCollection>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DailyCollection");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AgentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LedgerId).HasColumnName("LedgerID");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DailyMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("daily_master");

                entity.Property(e => e.Adv)
                    .HasColumnType("money")
                    .HasColumnName("adv");

                entity.Property(e => e.AltLotNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("alt_lot_no");

                entity.Property(e => e.Bd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bd");

                entity.Property(e => e.Bgs).HasColumnName("bgs");

                entity.Property(e => e.Frght)
                    .HasColumnType("money")
                    .HasColumnName("frght");

                entity.Property(e => e.KNo).HasColumnName("k_no");

                entity.Property(e => e.Kchl)
                    .HasColumnType("money")
                    .HasColumnName("kchl");

                entity.Property(e => e.LotNo).HasColumnName("lot_no");

                entity.Property(e => e.MerId).HasColumnName("mer_id");

                entity.Property(e => e.Rt)
                    .HasColumnType("money")
                    .HasColumnName("rt");

                entity.Property(e => e.Str)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("str");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.Unl)
                    .HasColumnType("money")
                    .HasColumnName("unl");

                entity.Property(e => e.VNo).HasColumnName("v_no");

                entity.Property(e => e.Vrt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vrt");

                entity.Property(e => e.VrtId).HasColumnName("vrt_id");

                entity.Property(e => e.YrId).HasColumnName("yr_id");
            });

            modelBuilder.Entity<DailyMasterr>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("daily_masterr");

                entity.Property(e => e.Adv)
                    .HasColumnType("money")
                    .HasColumnName("adv");

                entity.Property(e => e.AltLotNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("alt_lot_No");

                entity.Property(e => e.Bgs).HasColumnName("bgs");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Frght)
                    .HasColumnType("money")
                    .HasColumnName("frght");

                entity.Property(e => e.HdId).HasColumnName("hd_id");

                entity.Property(e => e.Kchl)
                    .HasColumnType("money")
                    .HasColumnName("kchl");

                entity.Property(e => e.LotNo).HasColumnName("lot_no");

                entity.Property(e => e.MerId).HasColumnName("mer_id");

                entity.Property(e => e.Rt)
                    .HasColumnType("money")
                    .HasColumnName("rt");

                entity.Property(e => e.Str)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("str");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.Unl).HasColumnName("unl");

                entity.Property(e => e.VNo).HasColumnName("v_no");

                entity.Property(e => e.YrId).HasColumnName("yr_id");
            });

            modelBuilder.Entity<DclineItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("DCLineItems");

                entity.Property(e => e.ActualWeightKg)
                    .HasColumnType("money")
                    .HasColumnName("actualWeightKg");

                entity.Property(e => e.Bno).HasColumnName("BNo");

                entity.Property(e => e.ChangedWeightKg)
                    .HasColumnType("money")
                    .HasColumnName("changedWeightKg");

                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Freight)
                    .HasColumnType("money")
                    .HasColumnName("freight");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MethodOfPacking)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("methodOfPacking");

                entity.Property(e => e.PrivateMark)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("privateMark");

                entity.Property(e => e.Total)
                    .HasColumnType("money")
                    .HasColumnName("total");
            });

            modelBuilder.Entity<Dcsummary>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("DCSummary");

                entity.Property(e => e.Advance).HasColumnType("money");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.Bno).HasColumnName("BNo");

                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                entity.Property(e => e.ConsigneesAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("consigneesAddress");

                entity.Property(e => e.ConsigneesName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("consigneesName");

                entity.Property(e => e.ConsignorsAddress).HasColumnName("consignorsAddress");

                entity.Property(e => e.ConsignorsName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("consignorsName");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.DeliveryAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("deliveryAddress");

                entity.Property(e => e.DeliveryDetails)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("deliveryDetails");

                entity.Property(e => e.FromLocation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fromLocation");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.LorryNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lorryNumber");

                entity.Property(e => e.ReceiverDetails)
                    .HasMaxLength(300)
                    .HasColumnName("receiverDetails");

                entity.Property(e => e.SenderDetails)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("senderDetails");

                entity.Property(e => e.ToLocation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("toLocation");

                entity.Property(e => e.TotalFrieght).HasColumnType("money");
            });

            modelBuilder.Entity<DeclaredValue>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AverageRate).HasColumnType("money");

                entity.Property(e => e.ClosingValue).HasColumnType("money");

                entity.Property(e => e.CommodityId).HasColumnName("CommodityID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmailBackup>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.BackupName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.TranctDate).HasColumnType("date");
            });

            modelBuilder.Entity<Expn>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("expns");

                entity.Property(e => e.Adv)
                    .HasColumnType("money")
                    .HasColumnName("adv");

                entity.Property(e => e.Advrt)
                    .HasColumnType("money")
                    .HasColumnName("advrt");

                entity.Property(e => e.Bamt)
                    .HasColumnType("money")
                    .HasColumnName("bamt");

                entity.Property(e => e.Blno).HasColumnName("blno");

                entity.Property(e => e.Blttl)
                    .HasColumnType("money")
                    .HasColumnName("blttl");

                entity.Property(e => e.Exp1)
                    .HasColumnType("money")
                    .HasColumnName("exp1");

                entity.Property(e => e.Exp2)
                    .HasColumnType("money")
                    .HasColumnName("exp2");

                entity.Property(e => e.Exp3)
                    .HasColumnType("money")
                    .HasColumnName("exp3");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Lots)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lots");

                entity.Property(e => e.Prt1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("prt1");

                entity.Property(e => e.Prt2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("prt2");

                entity.Property(e => e.Prt3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("prt3");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");
            });

            modelBuilder.Entity<Expnss>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("expnss");

                entity.Property(e => e.Adv)
                    .HasColumnType("money")
                    .HasColumnName("adv");

                entity.Property(e => e.Advrt)
                    .HasColumnType("money")
                    .HasColumnName("advrt");

                entity.Property(e => e.Bamt)
                    .HasColumnType("money")
                    .HasColumnName("bamt");

                entity.Property(e => e.Blno).HasColumnName("blno");

                entity.Property(e => e.Blttl)
                    .HasColumnType("money")
                    .HasColumnName("blttl");

                entity.Property(e => e.Exp1)
                    .HasColumnType("money")
                    .HasColumnName("exp1");

                entity.Property(e => e.Exp2)
                    .HasColumnType("money")
                    .HasColumnName("exp2");

                entity.Property(e => e.Exp3)
                    .HasColumnType("money")
                    .HasColumnName("exp3");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Prt1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("prt1");

                entity.Property(e => e.Prt2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("prt2");

                entity.Property(e => e.Prt3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("prt3");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");
            });

            modelBuilder.Entity<ExtendedDate>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ExtendedDates2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ExtendedDates2");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Gstin>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GSTINS");

                entity.Property(e => e.Gstin1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GSTIN");
            });

            modelBuilder.Entity<Gstquantra>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GSTQuantra");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.MonthNo).HasColumnType("datetime");
            });

            modelBuilder.Entity<IndWtMasterCmn>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ind_wt_master_cmn");

                entity.Property(e => e.BuyerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("buyer_code");

                entity.Property(e => e.EditId).HasColumnName("edit_id");

                entity.Property(e => e.EntryId).HasColumnName("entry_id");

                entity.Property(e => e.KBlNo).HasColumnName("k_bl_no");

                entity.Property(e => e.KBlNoC)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("k_bl_no_c");

                entity.Property(e => e.LotNo).HasColumnName("lot_no");

                entity.Property(e => e.MId).HasColumnName("m_id");

                entity.Property(e => e.PrintNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rate)
                    .HasColumnType("money")
                    .HasColumnName("rate");

                entity.Property(e => e.SellerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("seller_code");

                entity.Property(e => e.SrNo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sr_no");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.TtlWt).HasColumnName("ttl_wt");

                entity.Property(e => e.Wt).HasColumnName("wt");

                entity.Property(e => e.YrId).HasColumnName("yr_id");
            });

            modelBuilder.Entity<IndWtMasterr>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ind_wt_masterr");

                entity.Property(e => e.Cess).HasColumnName("cess");

                entity.Property(e => e.Dd).HasColumnName("dd");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.KBlNo).HasColumnName("k_bl_no");

                entity.Property(e => e.LotNo).HasColumnName("lot_no");

                entity.Property(e => e.MId).HasColumnName("m_id");

                entity.Property(e => e.Pkng).HasColumnName("pkng");

                entity.Property(e => e.SrNo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sr_no");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.TtlWt).HasColumnName("ttl_wt");

                entity.Property(e => e.Wt).HasColumnName("wt");

                entity.Property(e => e.YrId).HasColumnName("yr_id");
            });
            modelBuilder.Entity<Inventory>(entity =>
            {

                entity.Property(e => e._Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CompanyId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.VochType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.VochNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LedgerId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CommodityId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CommodityName)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.TranctDate)
                     .HasMaxLength(50)
                     .IsUnicode(false);
                entity.Property(e => e.PoNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.EwaybillNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LotNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WeightPerBag)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NoOfBags)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NoOfDocra)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NoOfBodhs)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TotalWeight)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Rate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Amount)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Mark)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PartyInvoiceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Discount)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NetAmount)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Cess)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SGST)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CGST)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IGST)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Createdby)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SchemeDiscount)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FreeQty)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IGSTRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SGSTRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CGSTRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CessRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UnloadHamali)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.VikriFrieght)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.VikriCashAdvance)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.EmptyGunnybags)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BuyerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ToPrint)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Taxable)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DisplayNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DisplayinvNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsTender)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsTotalWeight)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SellingRate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.WeightInString)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.InputCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.InputLot)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OutputCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OutputLot)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MonthNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsTaxableValueupdt)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OriginalInvNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.AprReturned)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ISUSED)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OriginalInvDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Frieght)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<InwardEntry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("InwardEntry");

                entity.Property(e => e.ChargeTypeId).HasColumnName("ChargeTypeID");

                entity.Property(e => e.CommodityId).HasColumnName("CommodityID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.EntryDate).HasColumnType("date");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.InwardTypeId).HasColumnName("InwardTypeID");

                entity.Property(e => e.LedgerId).HasColumnName("LedgerID");

                entity.Property(e => e.NoOfBags).HasColumnType("money");

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.Property(e => e.StaffName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StockPointId).HasColumnName("StockPointID");

                entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            });

            modelBuilder.Entity<Ledger>(entity =>
            {
                entity.ToTable("Ledger");


                entity.Property(e => e._Id)
                         .HasMaxLength(150)
                         .IsUnicode(true);
                entity.Property(e => e.CompanyId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LedgerType)
                .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.LedgerId)
                   .ValueGeneratedNever();
                entity.Property(e => e.LedgerName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Address1)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Address2)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.State)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Place)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Country)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Gstin)
                  .HasMaxLength(250)
                  .IsUnicode(false);
                entity.Property(e => e.DealerType)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.KannadaName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.KannadaPlace)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.LedgerCode)
                   .HasMaxLength(250)
                   .IsUnicode(false);
                entity.Property(e => e.ContactDetails)
                  .HasMaxLength(250)
                  .IsUnicode(false);
                entity.Property(e => e.Pan)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.BankName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Ifsc)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.AccountNo)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.AccountingGroupId)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NameAndPlace)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.PackingRate)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.HamaliRate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.WeighManFeeRate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.DalaliRate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.CessRate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.DeprPerc)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.CaplPerc)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.ExpiryDate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.RenewDate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.CellNo)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.OtherCreated)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.LocalName)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.LocalAddress)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.UnloadHamaliRate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.OldHdid)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Gstn)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Dlr_Type)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Tp)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Ist)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Bname)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.EmailId)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.PrintAcpay)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.ExclPay)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.AgentCode)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.OldLedgerId)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.Pin)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.StateCode)
             .HasMaxLength(150)
             .IsUnicode(false);
                entity.Property(e => e.LegalName)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.NeftAcno)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.ChequeNo)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.AskIndtcs)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.ApplyTds)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.Fssai)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.CommodityAccount)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.Dperc)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.IsSelected)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.RentTdsperc)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.DeductFrieghtTds)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.ToPrint)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.TotalCommission)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.Tdsdeducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.IsExported)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.TotalForTds2)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.Tds2deducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.ManualBookPageNo)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.QtoBeDeducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.Qtdsdeducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.TotalTv)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.BalanceQtds)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.TotalTurnoverforTcs)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.Tcsdeducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.TcstoBeDeducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.BalanceTcs)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.RentToBeDeducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.RentTdsdeducted)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.BalanceRentTds)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.ClosingBalanceCr)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.TotalTransaction)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.ClosingBalanceDr)
            .HasMaxLength(150)
            .IsUnicode(false);
                entity.Property(e => e.TotalContactTv)
            .HasMaxLength(150)
            .IsUnicode(false);

                entity.Property(e => e.OpeningBalance)
           .HasMaxLength(150)
           .IsUnicode(false);

                entity.Property(e => e.CrDr)
           .HasMaxLength(150)
           .IsUnicode(false);

                entity.Property(e => e.IsActive)
            .HasMaxLength(150)
            .IsUnicode(false);



            });

            modelBuilder.Entity<Loginn>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("loginn");

                entity.Property(e => e.PW)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("p_w");

                entity.Property(e => e.UNm)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("u_nm");
            });

            modelBuilder.Entity<OldPayment>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Old_Payments");

                entity.Property(e => e.Debit)
                    .HasColumnType("money")
                    .HasColumnName("debit");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.HdId).HasColumnName("hd_id");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.Vno).HasColumnName("vno");
            });

            modelBuilder.Entity<OpeningStock>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OpeningStock");

                entity.Property(e => e.Amount).HasColumnType("money");
            });

            modelBuilder.Entity<PadatalMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PadatalMaster");

                entity.Property(e => e.ActualPurchaseDate).HasColumnType("datetime");

                entity.Property(e => e.AgentName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("agentName");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LedgerId).HasColumnName("LedgerID");

                entity.Property(e => e.PadatalName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PurchaseDate)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnType("money");
            });

            modelBuilder.Entity<PadatalRate>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.BazarCommission).HasColumnType("money");

                entity.Property(e => e.CartageRate).HasColumnType("money");

                entity.Property(e => e.Cess).HasColumnType("money");

                entity.Property(e => e.Cgst)
                    .HasColumnType("money")
                    .HasColumnName("CGST");

                entity.Property(e => e.Hamali).HasColumnType("money");

                entity.Property(e => e.Packing).HasColumnType("money");

                entity.Property(e => e.PurchseCommission).HasColumnType("money");

                entity.Property(e => e.RefillRate).HasColumnType("money");

                entity.Property(e => e.RepairRate).HasColumnType("money");

                entity.Property(e => e.Sgst)
                    .HasColumnType("money")
                    .HasColumnName("SGST");

                entity.Property(e => e.WeighmanFee).HasColumnType("money");
            });

            modelBuilder.Entity<ProductionMaster>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.SrNo);

                entity.ToTable("ProductionMaster");

                entity.Property(e => e.GoodsValue).HasColumnType("money");

                entity.Property(e => e.QtyRcd).HasColumnName("qty_rcd");

                entity.Property(e => e.QtySent).HasColumnName("qty_Sent");

                entity.Property(e => e.ReceivedItId).HasColumnName("Received_it_id");

                entity.Property(e => e.ReceivedItemName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Refno)
                    .HasColumnType("money")
                    .HasColumnName("REFNO");

                entity.Property(e => e.SentItemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sentItemName");

                entity.Property(e => e.SrNo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sr_no");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<QntrMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("qntr_master");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");
            });

            modelBuilder.Entity<RateMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rate_master");

                entity.Property(e => e.AdvRt)
                    .HasColumnType("money")
                    .HasColumnName("adv_rt");

                entity.Property(e => e.Anfhml).HasColumnName("anfhml");

                entity.Property(e => e.Anfwtf).HasColumnName("anfwtf");

                entity.Property(e => e.Anpkng).HasColumnName("anpkng");

                entity.Property(e => e.Asf).HasColumnName("asf");

                entity.Property(e => e.Asso)
                    .HasColumnType("money")
                    .HasColumnName("asso");

                entity.Property(e => e.Bagfhml).HasColumnName("bagfhml");

                entity.Property(e => e.Bagfwtf).HasColumnName("bagfwtf");

                entity.Property(e => e.Bagpkng).HasColumnName("bagpkng");

                entity.Property(e => e.Bodhfhml).HasColumnName("bodhfhml");

                entity.Property(e => e.Bodhfwtf).HasColumnName("bodhfwtf");

                entity.Property(e => e.Ces)
                    .HasColumnType("money")
                    .HasColumnName("ces");

                entity.Property(e => e.Cess)
                    .HasColumnType("money")
                    .HasColumnName("cess");

                entity.Property(e => e.Cmn).HasColumnType("money");

                entity.Property(e => e.Dl)
                    .HasColumnType("money")
                    .HasColumnName("dl");

                entity.Property(e => e.ItId).HasColumnName("it_id");

                entity.Property(e => e.KHml)
                    .HasColumnType("money")
                    .HasColumnName("k_hml");

                entity.Property(e => e.KPk)
                    .HasColumnType("money")
                    .HasColumnName("k_pk");

                entity.Property(e => e.Mnth).HasColumnName("mnth");

                entity.Property(e => e.Perbg)
                    .HasColumnType("money")
                    .HasColumnName("perbg");

                entity.Property(e => e.Plstfhml).HasColumnName("plstfhml");

                entity.Property(e => e.Plstfwtf).HasColumnName("plstfwtf");

                entity.Property(e => e.Plstpkng).HasColumnName("plstpkng");

                entity.Property(e => e.RndTo)
                    .HasColumnType("money")
                    .HasColumnName("rnd_to");

                entity.Property(e => e.Sls)
                    .HasColumnType("money")
                    .HasColumnName("sls");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.Unld)
                    .HasColumnType("money")
                    .HasColumnName("unld");

                entity.Property(e => e.VHml)
                    .HasColumnType("money")
                    .HasColumnName("v_hml");

                entity.Property(e => e.VPk)
                    .HasColumnType("money")
                    .HasColumnName("v_pk");

                entity.Property(e => e.Vat).HasColumnName("vat");

                entity.Property(e => e.Wt)
                    .HasColumnType("money")
                    .HasColumnName("wt");

                entity.Property(e => e.Wtf)
                    .HasColumnType("money")
                    .HasColumnName("wtf");

                entity.Property(e => e.Yr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("yr");
            });

            modelBuilder.Entity<RateMasterr>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("rate_masterr");

                entity.Property(e => e.AdvRt)
                    .HasColumnType("money")
                    .HasColumnName("adv_rt");

                entity.Property(e => e.Asso)
                    .HasColumnType("money")
                    .HasColumnName("asso");

                entity.Property(e => e.Ces)
                    .HasColumnType("money")
                    .HasColumnName("ces");

                entity.Property(e => e.Dl)
                    .HasColumnType("money")
                    .HasColumnName("dl");

                entity.Property(e => e.KHml)
                    .HasColumnType("money")
                    .HasColumnName("k_hml");

                entity.Property(e => e.KPk)
                    .HasColumnType("money")
                    .HasColumnName("k_pk");

                entity.Property(e => e.Mnth).HasColumnName("mnth");

                entity.Property(e => e.Perbg)
                    .HasColumnType("money")
                    .HasColumnName("perbg");

                entity.Property(e => e.RndTo)
                    .HasColumnType("money")
                    .HasColumnName("rnd_to");

                entity.Property(e => e.Sls)
                    .HasColumnType("money")
                    .HasColumnName("sls");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.Unld)
                    .HasColumnType("money")
                    .HasColumnName("unld");

                entity.Property(e => e.VHml)
                    .HasColumnType("money")
                    .HasColumnName("v_hml");

                entity.Property(e => e.VPk)
                    .HasColumnType("money")
                    .HasColumnName("v_pk");

                entity.Property(e => e.Wt)
                    .HasColumnType("money")
                    .HasColumnName("wt");

                entity.Property(e => e.Yr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("yr");
            });

            modelBuilder.Entity<ReFilled>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ReFilled");

                entity.Property(e => e.PadatalName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalesSummary>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SalesSummary");

                entity.Property(e => e.Advance).HasColumnType("money");

                entity.Property(e => e.Aredate)
                    .HasColumnType("money")
                    .HasColumnName("AREDate");

                entity.Property(e => e.Areno)
                    .HasColumnType("datetime")
                    .HasColumnName("ARENo");

                entity.Property(e => e.Balance).HasColumnType("money");

                entity.Property(e => e.BillAmount).HasColumnType("money");

                entity.Property(e => e.CheckPost)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.DeliveryAddress1)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryAddress2)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryPlace)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryState)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryStateCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dlno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DLNo");

                entity.Property(e => e.DriverName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EwayBillNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpenseAmount1).HasColumnType("money");

                entity.Property(e => e.ExpenseAmount2).HasColumnType("money");

                entity.Property(e => e.ExpenseAmount3).HasColumnType("money");

                entity.Property(e => e.ExpenseName1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpenseName2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpenseName3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FrieghtAmount).HasColumnType("money");

                entity.Property(e => e.FrieghtPerBag).HasColumnType("money");

                entity.Property(e => e.Inwords)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("INWords");

                entity.Property(e => e.LorryNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LorryOwnerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ponumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PONumber");

                entity.Property(e => e.RoundOff).HasColumnType("money");

                entity.Property(e => e.TotalFrieght).HasColumnType("money");

                entity.Property(e => e.Transporter)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ServicesPurchased>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ServicesPurchased");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.ServiceCost).HasColumnType("money");

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("states");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Statecode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("statecode");

                entity.Property(e => e.Statename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("statename");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Summary>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Summary");

                entity.Property(e => e.AverageRate).HasColumnType("money");

                entity.Property(e => e.BagAmount).HasColumnType("money");

                entity.Property(e => e.BagRate).HasColumnType("money");

                entity.Property(e => e.Bastani).HasColumnType("money");

                entity.Property(e => e.BazarCommissionRate).HasColumnType("money");

                entity.Property(e => e.BazarCommissionValue).HasColumnType("money");

                entity.Property(e => e.BillTotal).HasColumnType("money");

                entity.Property(e => e.CartageValue).HasColumnType("money");

                entity.Property(e => e.CessRate).HasColumnType("money");

                entity.Property(e => e.CessValue).HasColumnType("money");

                entity.Property(e => e.Cgstrate)
                    .HasColumnType("money")
                    .HasColumnName("CGSTRate");

                entity.Property(e => e.Cgstvalue)
                    .HasColumnType("money")
                    .HasColumnName("CGSTValue");

                entity.Property(e => e.CommissionTo)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CrushingCharges).HasColumnType("money");

                entity.Property(e => e.Expense1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expense1Amount).HasColumnType("decimal(30, 2)");

                entity.Property(e => e.Expense2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expense2Amount).HasColumnType("decimal(30, 2)");

                entity.Property(e => e.Expense3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expense3Amount).HasColumnType("decimal(30, 2)");

                entity.Property(e => e.Expense4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Expense4Amount).HasColumnType("decimal(30, 2)");

                entity.Property(e => e.FirstRoundOff).HasColumnType("money");

                entity.Property(e => e.GrandToal).HasColumnType("money");

                entity.Property(e => e.GrindingCharges).HasColumnType("money");

                entity.Property(e => e.HamaliRate).HasColumnType("money");

                entity.Property(e => e.HamaliValues).HasColumnType("money");

                entity.Property(e => e.LooseRate).HasColumnType("money");

                entity.Property(e => e.LorryFrieght).HasColumnType("money");

                entity.Property(e => e.NoofEmptyBags).HasColumnType("money");

                entity.Property(e => e.PackingRate).HasColumnType("money");

                entity.Property(e => e.PackingValue).HasColumnType("money");

                entity.Property(e => e.PadatalName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PadatalRate).HasColumnType("money");

                entity.Property(e => e.PurchaseCommissionRate).HasColumnType("money");

                entity.Property(e => e.PurchaseCommissionValues).HasColumnType("money");

                entity.Property(e => e.Refilled1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("refilled1");

                entity.Property(e => e.Refilled2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("refilled2");

                entity.Property(e => e.Refilled3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("refilled3");

                entity.Property(e => e.Refilled4)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("refilled4");

                entity.Property(e => e.Refilled5)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("refilled5");

                entity.Property(e => e.Refilled6)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("refilled6");

                entity.Property(e => e.Refilled7)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("refilled7");

                entity.Property(e => e.Refillvalue).HasColumnType("money");

                entity.Property(e => e.RepairValue).HasColumnType("money");

                entity.Property(e => e.SecondRoundOff).HasColumnType("money");

                entity.Property(e => e.Sgstrate)
                    .HasColumnType("money")
                    .HasColumnName("SGSTRate");

                entity.Property(e => e.Sgstvalue)
                    .HasColumnType("money")
                    .HasColumnName("SGSTValue");

                entity.Property(e => e.Shortage).HasColumnType("money");

                entity.Property(e => e.StemlessCharges).HasColumnType("money");

                entity.Property(e => e.TaxableValue).HasColumnType("money");

                entity.Property(e => e.TotalExpenses).HasColumnType("money");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");

                entity.Property(e => e.WeighmanFeeRate).HasColumnType("money");

                entity.Property(e => e.WeighmanFeeValue).HasColumnType("money");
            });

            modelBuilder.Entity<TableHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TableHistory");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TransDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblUserRight>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblUserRights");

                entity.Property(e => e.BtnDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("btnDescription");

                entity.Property(e => e.BtnName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("btnName");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.UserId).HasColumnName("userID");
            });

            modelBuilder.Entity<TblYear>(entity =>
            {
                entity.ToTable("TblYear");

                entity.Property(e => e.Dbname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DBName");

                entity.Property(e => e.FinYear)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tdsmaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TDSMaster");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.SavedDate).HasColumnType("date");

                entity.Property(e => e.TdsAmount).HasColumnType("money");

                entity.Property(e => e.TdsType)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.Property(e => e.ToTalCommission).HasColumnType("money");
            });

            modelBuilder.Entity<TempMarkwise>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TempMarkwise");

                entity.Property(e => e.BastaniAverage).HasColumnType("money");

                entity.Property(e => e.Batani).HasColumnType("money");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Expense).HasColumnType("money");

                entity.Property(e => e.FinalAverage).HasColumnType("money");

                entity.Property(e => e.Mark)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MaxRate).HasColumnType("money");

                entity.Property(e => e.MinRate).HasColumnType("money");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");

                entity.Property(e => e.Weight).HasColumnType("money");
            });

            modelBuilder.Entity<TmpBl>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_bl");

                entity.Property(e => e.Adv)
                    .HasColumnType("money")
                    .HasColumnName("adv");

                entity.Property(e => e.AltLotNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("alt_lot_no");

                entity.Property(e => e.Amt)
                    .HasColumnType("money")
                    .HasColumnName("amt");

                entity.Property(e => e.Asf)
                    .HasColumnType("money")
                    .HasColumnName("asf");

                entity.Property(e => e.Bgs).HasColumnName("bgs");

                entity.Property(e => e.BlNo).HasColumnName("bl_no");

                entity.Property(e => e.Ces)
                    .HasColumnType("money")
                    .HasColumnName("ces");

                entity.Property(e => e.Cgst)
                    .HasColumnType("money")
                    .HasColumnName("cgst");

                entity.Property(e => e.Cmn)
                    .HasColumnType("money")
                    .HasColumnName("cmn");

                entity.Property(e => e.Csgt)
                    .HasColumnType("money")
                    .HasColumnName("csgt");

                entity.Property(e => e.Dalali)
                    .HasColumnType("money")
                    .HasColumnName("dalali");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Frght)
                    .HasColumnType("money")
                    .HasColumnName("frght");

                entity.Property(e => e.Hid).HasColumnName("hid");

                entity.Property(e => e.Hml)
                    .HasColumnType("money")
                    .HasColumnName("hml");

                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kchl)
                    .HasColumnType("money")
                    .HasColumnName("kchl");

                entity.Property(e => e.LotNo).HasColumnName("lot_no");

                entity.Property(e => e.Pkng)
                    .HasColumnType("money")
                    .HasColumnName("pkng");

                entity.Property(e => e.PrintNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rndf)
                    .HasColumnType("money")
                    .HasColumnName("rndf");

                entity.Property(e => e.Rt)
                    .HasColumnType("money")
                    .HasColumnName("rt");

                entity.Property(e => e.Sgst)
                    .HasColumnType("money")
                    .HasColumnName("sgst");

                entity.Property(e => e.Str)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("str");

                entity.Property(e => e.TinNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tin_no");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.TtlWtForAMer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ttl_wt_for_a_mer");

                entity.Property(e => e.Vat)
                    .HasColumnType("money")
                    .HasColumnName("vat");

                entity.Property(e => e.Wt).HasColumnName("wt");

                entity.Property(e => e.Wtf)
                    .HasColumnType("money")
                    .HasColumnName("wtf");
            });

            modelBuilder.Entity<TmpCashBook>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TmpCashBook");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Narration)
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.Sno).HasColumnName("SNo");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TmpCess>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_cess");

                entity.Property(e => e.BastForKBl)
                    .HasColumnType("money")
                    .HasColumnName("bast_for_k_bl");

                entity.Property(e => e.Bgs).HasColumnName("bgs");

                entity.Property(e => e.Blno).HasColumnName("blno");

                entity.Property(e => e.Camt)
                    .HasColumnType("money")
                    .HasColumnName("camt");

                entity.Property(e => e.Fdt)
                    .HasColumnType("datetime")
                    .HasColumnName("fdt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Mid).HasColumnName("mid");

                entity.Property(e => e.Todt)
                    .HasColumnType("datetime")
                    .HasColumnName("todt");

                entity.Property(e => e.Trdt)
                    .HasColumnType("datetime")
                    .HasColumnName("trdt");

                entity.Property(e => e.Vmax).HasColumnName("vmax");

                entity.Property(e => e.Vmin).HasColumnName("vmin");

                entity.Property(e => e.Wamt)
                    .HasColumnType("money")
                    .HasColumnName("wamt");

                entity.Property(e => e.Wt).HasColumnName("wt");
            });

            modelBuilder.Entity<TmpCessWk>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_cess_wk");

                entity.Property(e => e.Amt)
                    .HasColumnType("money")
                    .HasColumnName("amt");

                entity.Property(e => e.Bgs).HasColumnName("bgs");

                entity.Property(e => e.BillNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bill_no");

                entity.Property(e => e.Cess)
                    .HasColumnType("money")
                    .HasColumnName("cess");

                entity.Property(e => e.Dt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Wt).HasColumnName("wt");

                entity.Property(e => e.WtFee)
                    .HasColumnType("money")
                    .HasColumnName("wt_fee");
            });

            modelBuilder.Entity<TmpChopadaSummary>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TmpChopadaSummary");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Igst)
                    .HasColumnType("money")
                    .HasColumnName("IGST");

                entity.Property(e => e.Others).HasColumnType("money");

                entity.Property(e => e.TotalBastani).HasColumnType("money");

                entity.Property(e => e.TotalBillAmount).HasColumnType("money");

                entity.Property(e => e.TotalCess).HasColumnType("money");

                entity.Property(e => e.TotalCgst)
                    .HasColumnType("money")
                    .HasColumnName("TotalCGST");

                entity.Property(e => e.TotalCommission).HasColumnType("money");

                entity.Property(e => e.TotalHamali).HasColumnType("money");

                entity.Property(e => e.TotalPacking).HasColumnType("money");

                entity.Property(e => e.TotalRoff)
                    .HasColumnType("money")
                    .HasColumnName("TotalROff");

                entity.Property(e => e.TotalSgst)
                    .HasColumnType("money")
                    .HasColumnName("TotalSGST");

                entity.Property(e => e.TotalTaxable).HasColumnType("money");

                entity.Property(e => e.Totalw).HasColumnType("money");
            });

            modelBuilder.Entity<TmpChpd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_chpd");

                entity.Property(e => e.Bgs).HasColumnName("bgs");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Hid).HasColumnName("hid");

                entity.Property(e => e.Lt).HasColumnName("lt");

                entity.Property(e => e.MNoOfBags).HasColumnName("m_no_of_bags");

                entity.Property(e => e.Mcd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mcd");

                entity.Property(e => e.Mwt)
                    .HasMaxLength(3800)
                    .IsUnicode(false)
                    .HasColumnName("mwt");

                entity.Property(e => e.RfBgs).HasColumnName("rf_bgs");

                entity.Property(e => e.Rt)
                    .HasColumnType("money")
                    .HasColumnName("rt");

                entity.Property(e => e.Tdt)
                    .HasColumnType("datetime")
                    .HasColumnName("tdt");

                entity.Property(e => e.TtlAmt)
                    .HasColumnType("money")
                    .HasColumnName("ttl_amt");

                entity.Property(e => e.TtlBags).HasColumnName("ttl_bags");

                entity.Property(e => e.TtlBast)
                    .HasColumnType("money")
                    .HasColumnName("ttl_bast");

                entity.Property(e => e.TtlWt).HasColumnName("ttl_wt");

                entity.Property(e => e.TtlWtForM).HasColumnName("ttl_wt_for_m");

                entity.Property(e => e.VBlNo).HasColumnName("v_bl_no");

                entity.Property(e => e.Wt)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("wt");
            });

            modelBuilder.Entity<TmpChpd1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_chpd_1");

                entity.Property(e => e.Bags).HasColumnName("bags");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Hid).HasColumnName("hid");

                entity.Property(e => e.Lt).HasColumnName("lt");

                entity.Property(e => e.Mcd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mcd");

                entity.Property(e => e.Ord).HasColumnName("ord");

                entity.Property(e => e.Pg).HasColumnName("pg");

                entity.Property(e => e.Rt)
                    .HasColumnType("money")
                    .HasColumnName("rt");

                entity.Property(e => e.SrNo).HasColumnName("sr_no");

                entity.Property(e => e.Wt)
                    .HasMaxLength(2500)
                    .IsUnicode(false)
                    .HasColumnName("wt");
            });

            modelBuilder.Entity<TmpCshbk>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_cshbk");

                entity.Property(e => e.Clb)
                    .HasColumnType("money")
                    .HasColumnName("clb");

                entity.Property(e => e.Cr)
                    .HasColumnType("money")
                    .HasColumnName("cr");

                entity.Property(e => e.Db)
                    .HasColumnType("money")
                    .HasColumnName("db");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt");

                entity.Property(e => e.Fdt)
                    .HasColumnType("datetime")
                    .HasColumnName("fdt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Op)
                    .HasColumnType("money")
                    .HasColumnName("op");

                entity.Property(e => e.Prt)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("prt");

                entity.Property(e => e.Srno).HasColumnName("srno");

                entity.Property(e => e.Tdt)
                    .HasColumnType("datetime")
                    .HasColumnName("tdt");
            });

            modelBuilder.Entity<TmpDelNote>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_del_note");

                entity.Property(e => e.Bags).HasColumnName("bags");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Hid).HasColumnName("hid");
            });

            modelBuilder.Entity<TmpDt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_dts");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt");

                entity.Property(e => e.Fid).HasColumnName("fid");
            });

            modelBuilder.Entity<TmpDtss>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_dtss");

                entity.Property(e => e.Fdt)
                    .HasColumnType("datetime")
                    .HasColumnName("fdt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Tdt)
                    .HasColumnType("datetime")
                    .HasColumnName("tdt");

                entity.Property(e => e.Wt).HasColumnName("wt");
            });

            modelBuilder.Entity<TmpFinalize>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmpFinalize");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ActualLedgerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.LedgerId).HasColumnName("LedgerID");

                entity.Property(e => e.Particulars)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpHdDtl>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_hd_dtls");

                entity.Property(e => e.CMinus)
                    .HasColumnType("money")
                    .HasColumnName("c_minus");

                entity.Property(e => e.ClsC)
                    .HasColumnType("money")
                    .HasColumnName("cls_c");

                entity.Property(e => e.ClsD)
                    .HasColumnType("money")
                    .HasColumnName("cls_D");

                entity.Property(e => e.Credit)
                    .HasColumnType("money")
                    .HasColumnName("credit");

                entity.Property(e => e.DMinus)
                    .HasColumnType("money")
                    .HasColumnName("d_minus");

                entity.Property(e => e.Debit)
                    .HasColumnType("money")
                    .HasColumnName("debit");

                entity.Property(e => e.Fdt)
                    .HasColumnType("datetime")
                    .HasColumnName("fdt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.FnlC)
                    .HasColumnType("money")
                    .HasColumnName("fnl_c");

                entity.Property(e => e.FnlD)
                    .HasColumnType("money")
                    .HasColumnName("fnl_D");

                entity.Property(e => e.Hid).HasColumnName("hid");

                entity.Property(e => e.ItsC)
                    .HasColumnType("money")
                    .HasColumnName("its_c");

                entity.Property(e => e.ItsD)
                    .HasColumnType("money")
                    .HasColumnName("its_d");

                entity.Property(e => e.Prt)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("prt");

                entity.Property(e => e.RunCb)
                    .HasColumnType("money")
                    .HasColumnName("Run_cb");

                entity.Property(e => e.SrNo).HasColumnName("sr_no");

                entity.Property(e => e.Tdt)
                    .HasColumnType("datetime")
                    .HasColumnName("tdt");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.TtlC)
                    .HasColumnType("money")
                    .HasColumnName("ttl_C");

                entity.Property(e => e.TtlD)
                    .HasColumnType("money")
                    .HasColumnName("ttl_d");
            });

            modelBuilder.Entity<TmpKpYadi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_kp_yadi");

                entity.Property(e => e.Amt)
                    .HasColumnType("money")
                    .HasColumnName("amt");

                entity.Property(e => e.Blno).HasColumnName("blno");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.HdId).HasColumnName("hd_id");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");
            });

            modelBuilder.Entity<TmpLedger>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TmpLedger");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Narration)
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.Sno).ValueGeneratedOnAdd();

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TmpPl>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TmpPl");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PurchaseAmount).HasColumnType("money");

                entity.Property(e => e.PurchaseParticulars)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SaleAmount).HasColumnType("money");

                entity.Property(e => e.SaleParticulars)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("money");
            });

            modelBuilder.Entity<TmpReceivable>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TmpStockLedger>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmpStockLedger");

                entity.Property(e => e.Average).HasColumnType("money");

                entity.Property(e => e.ClosingStock).HasColumnType("money");

                entity.Property(e => e.ClosingValue).HasColumnType("money");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.FromProductionQty).HasColumnType("money");

                entity.Property(e => e.FromProductionValue).HasColumnType("money");

                entity.Property(e => e.Obstock).HasColumnName("OBStock");

                entity.Property(e => e.Obvalue)
                    .HasColumnType("money")
                    .HasColumnName("OBValue");

                entity.Property(e => e.OnwSalesQty).HasColumnType("money");

                entity.Property(e => e.OnwSalesValue).HasColumnType("money");

                entity.Property(e => e.PurchaseValue).HasColumnType("money");

                entity.Property(e => e.SalesReturnValue).HasColumnType("money");

                entity.Property(e => e.ToProduction).HasColumnType("money");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TmpSummary>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmpSummary");

                entity.Property(e => e.Ason)
                    .HasColumnType("datetime")
                    .HasColumnName("ason");

                entity.Property(e => e.Cbcr)
                    .HasColumnType("money")
                    .HasColumnName("CBCr");

                entity.Property(e => e.Cbdr)
                    .HasColumnType("money")
                    .HasColumnName("CBDr");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.HdId).HasColumnName("hd_id");

                entity.Property(e => e.Obcr)
                    .HasColumnType("money")
                    .HasColumnName("OBCr");

                entity.Property(e => e.Obdr)
                    .HasColumnType("money")
                    .HasColumnName("OBDr");
            });

            modelBuilder.Entity<TmpTaxSlab>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmpTaxSlab");

                entity.Property(e => e.EighteenTaxCgst)
                    .HasColumnType("money")
                    .HasColumnName("EighteenTaxCGST");

                entity.Property(e => e.EighteenTaxIgst)
                    .HasColumnType("money")
                    .HasColumnName("EighteenTaxIGST");

                entity.Property(e => e.EighteenTaxSgst)
                    .HasColumnType("money")
                    .HasColumnName("EighteenTaxSGST");

                entity.Property(e => e.EighteenValue).HasColumnType("money");

                entity.Property(e => e.FiveTaxCgst)
                    .HasColumnType("money")
                    .HasColumnName("FiveTaxCGST");

                entity.Property(e => e.FiveTaxIgst)
                    .HasColumnType("money")
                    .HasColumnName("FiveTaxIGST");

                entity.Property(e => e.FiveTaxSgst)
                    .HasColumnType("money")
                    .HasColumnName("FiveTaxSGST");

                entity.Property(e => e.FiveValue).HasColumnType("money");

                entity.Property(e => e.T8cgsttax)
                    .HasColumnType("money")
                    .HasColumnName("T8CGSTTax");

                entity.Property(e => e.T8sgsttax)
                    .HasColumnType("money")
                    .HasColumnName("T8SGSTTAx");

                entity.Property(e => e.T8taxIgst)
                    .HasColumnType("money")
                    .HasColumnName("T8TaxIGST");

                entity.Property(e => e.T8value)
                    .HasColumnType("money")
                    .HasColumnName("T8Value");

                entity.Property(e => e.TwelveTaxCgst)
                    .HasColumnType("money")
                    .HasColumnName("TwelveTaxCGST");

                entity.Property(e => e.TwelveTaxSgst)
                    .HasColumnType("money")
                    .HasColumnName("TwelveTaxSGST");

                entity.Property(e => e.TwelveValue).HasColumnType("money");

                entity.Property(e => e.TwlveTaxIgst)
                    .HasColumnType("money")
                    .HasColumnName("TwlveTaxIGST");

                entity.Property(e => e.ZeroTax).HasColumnType("money");

                entity.Property(e => e.ZeroValue).HasColumnType("money");
            });

            modelBuilder.Entity<TmpTd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TmpTDS");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("TDS");

                entity.Property(e => e.Tdstitle)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("TDSTitle");

                entity.Property(e => e.Tdstype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TDSType");
            });

            modelBuilder.Entity<TmpTdsList>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_tds_list");

                entity.Property(e => e.Amt)
                    .HasColumnType("money")
                    .HasColumnName("amt");

                entity.Property(e => e.Bl).HasColumnName("bl");

                entity.Property(e => e.Cmn)
                    .HasColumnType("money")
                    .HasColumnName("cmn");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt");

                entity.Property(e => e.Fdt)
                    .HasColumnType("datetime")
                    .HasColumnName("fdt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Mid).HasColumnName("mid");

                entity.Property(e => e.Tds)
                    .HasColumnType("money")
                    .HasColumnName("tds");

                entity.Property(e => e.Tdt)
                    .HasColumnType("datetime")
                    .HasColumnName("tdt");
            });

            modelBuilder.Entity<TmpTndrFrm>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_tndr_frm");

                entity.Property(e => e.Apmccode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apmccode");

                entity.Property(e => e.Bg).HasColumnName("bg");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Fnm)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("fnm");

                entity.Property(e => e.Lt).HasColumnName("lt");

                entity.Property(e => e.SrNo).HasColumnName("sr_no");

                entity.Property(e => e.Tp).HasColumnName("tp");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.Vrt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vrt");
            });

            modelBuilder.Entity<TmpTrialBalance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TmpTrialBalance");

                entity.Property(e => e.AsOnDate).HasColumnType("datetime");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.LedgerId).HasColumnName("LedgerID");

                entity.Property(e => e.OpeningBalance)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpWtBill>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_wt_bill");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.HdD).HasColumnName("hd_d");

                entity.Property(e => e.LotNo).HasColumnName("lot_no");

                entity.Property(e => e.Lots)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("lots");
            });

            modelBuilder.Entity<TmpYadiCmn>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_yadi_cmn");

                entity.Property(e => e.Amt)
                    .HasColumnType("money")
                    .HasColumnName("amt");

                entity.Property(e => e.Dt)
                    .HasColumnType("datetime")
                    .HasColumnName("dt");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Hid).HasColumnName("hid");

                entity.Property(e => e.Todt)
                    .HasColumnType("datetime")
                    .HasColumnName("todt");

                entity.Property(e => e.TodtAmt)
                    .HasColumnType("money")
                    .HasColumnName("todt_amt");
            });

            modelBuilder.Entity<Tmptdslist1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmptdslist");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Tr_dt");
            });

            modelBuilder.Entity<Tmpyadi>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmpyadi");

                entity.Property(e => e.Ason)
                    .HasColumnType("datetime")
                    .HasColumnName("ason");

                entity.Property(e => e.Credit)
                    .HasColumnType("money")
                    .HasColumnName("credit");

                entity.Property(e => e.Days).HasColumnName("days");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.HdId).HasColumnName("hd_id");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");
            });

            modelBuilder.Entity<TndrFrm>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tndr_frm");

                entity.Property(e => e.Bg).HasColumnName("bg");

                entity.Property(e => e.Fid).HasColumnName("fid");

                entity.Property(e => e.Lt).HasColumnName("lt");

                entity.Property(e => e.SrNo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("sr_no");

                entity.Property(e => e.Tp).HasColumnName("tp");

                entity.Property(e => e.TrDt)
                    .HasColumnType("datetime")
                    .HasColumnName("tr_dt");

                entity.Property(e => e.Vrt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vrt");
            });



            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UserId)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.UserType)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.UserName)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.Password)
                 .HasMaxLength(50)
                 .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                 .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                  .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                        .HasMaxLength(50)
                        .IsUnicode(false);
                entity.Property(e => e.PhoneNo)
                        .HasMaxLength(50)
                        .IsUnicode(false);
                entity.Property(e => e.IsActive)
                  .HasMaxLength(50)
                  .IsUnicode(false);
            });


            //modelBuilder.Entity<User>(entity =>
            //{

            //    entity.Property(e => e.Id).HasColumnName("Id");

            //    entity.Property(e => e.CreatedDate).HasColumnName("datetime");
            //    entity.Property(e => e.Id)
            //       .HasMaxLength(50)
            //       .IsUnicode(false);

            //    entity.Property(e => e.Password)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            //    entity.Property(e => e.UserId).ValueGeneratedOnAdd();

            //    entity.Property(e => e.UserName)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.UserType)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);
            //});

            modelBuilder.Entity<Vareity>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.VrtNm)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vrt_nm");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                //entity.HasNoKey();

                entity.Property(e => e.BrandName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cmnamount)
                    .HasColumnType("money")
                    .HasColumnName("CMNAmount");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Credit).HasColumnType("money");

                entity.Property(e => e.Debit).HasColumnType("money");

                entity.Property(e => e.EntryGroup)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Fhamali)
                    .HasColumnType("money")
                    .HasColumnName("FHamali");

                entity.Property(e => e.FnoOfBags).HasColumnName("FNoOfBags");

                entity.Property(e => e.ForEdit)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fpacking)
                    .HasColumnType("money")
                    .HasColumnName("FPacking");

                entity.Property(e => e.Fvalue).HasColumnName("FValue");

                entity.Property(e => e.Fweight).HasColumnName("FWeight");

                entity.Property(e => e.GstqunatryName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("GSTQunatryName");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Inwords)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Kcess)
                    .HasColumnType("money")
                    .HasColumnName("KCess");

                entity.Property(e => e.Kcgst)
                    .HasColumnType("money")
                    .HasColumnName("KCGST");

                entity.Property(e => e.Kcommission)
                    .HasColumnType("money")
                    .HasColumnName("KCommission");

                entity.Property(e => e.Khamali)
                    .HasColumnType("money")
                    .HasColumnName("KHamali");

                entity.Property(e => e.Kpacking).HasColumnType("money");

                entity.Property(e => e.Kroundoff).HasColumnType("money");

                entity.Property(e => e.Ksgst)
                    .HasColumnType("money")
                    .HasColumnName("KSGST");

                entity.Property(e => e.Kvalue).HasColumnType("money");

                entity.Property(e => e.KweighmanFee)
                    .HasColumnType("money")
                    .HasColumnName("KWeighmanFee");

                entity.Property(e => e.LedgerNameForNarration)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Narration)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PartyId).HasColumnName("PartyID");

                entity.Property(e => e.PartyInvoiceNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tdstype).HasColumnName("TDSType");

                entity.Property(e => e.TranctDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VoucherType>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.VoucherId).ValueGeneratedOnAdd();

                entity.Property(e => e.VoucherName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FormMaster>(entity =>
            {

                entity.ToTable("FormMaster");

                entity.Property(e => e.Id)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FormId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.GroupName)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.FormName)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<UserAccesMaster>(entity =>
            {

                entity.ToTable("UserAccesMaster");

                entity.Property(e => e.Id)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FormId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UserId)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.IsAccess)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.PreparedBy)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.PreparedDate)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive)
                   .HasMaxLength(250)
                   .IsUnicode(false);
            });
            modelBuilder.Entity<Dealer_Type>(entity =>
            {

                entity.ToTable("Dealer_Type");

                entity.Property(e => e.Id)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DealerType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ledger_Type>(entity =>
            {

                entity.ToTable("Ledger_Type");

                entity.Property(e => e.Id)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.LedgerType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(150)
                   .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(150)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            //modelBuilder.Entity<SalesInvoice>(entity =>
            //{

            //    entity.Property(e => e.billSummary)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.inventory)
            //        .ValueGeneratedOnAdd()
            //        .IsUnicode(false);


            //});
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
