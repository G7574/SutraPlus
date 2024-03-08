using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SutraPlus_DAL.Models
{
    public partial class MasterDBContext : DbContext
    {
        public MasterDBContext()
        {
        }

        public MasterDBContext(DbContextOptions<MasterDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerFinancialYear> CustomerFinancialYears { get; set; } = null!;
        public virtual DbSet<UserSession> UserSessions { get; set; } = null!;
        public virtual DbSet<SuperAdminLogins> SuperAdminLogins { get; set; } = null!;
        public virtual DbSet<EmailConfig> EmailConfig { get; set; } = null!;
        //public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<OTPTrans> OTPTrans { get; set; } = null!;
        public virtual DbSet<StateMaster> StateMaster { get; set; } = null!;
        public virtual DbSet<YearMaster> YearMaster { get; set; } = null!;

        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<ThemCode> ThemCodes { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //this is not in use
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=103.50.212.163;Database=SutraPlus;uid=sa;Password=root@123;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");


                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Pin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PIN");
                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.UpdatedDate)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.IsActive)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<CustomerFinancialYear>(entity =>
            {
                entity.ToTable("CustomerFinancialYear");

                entity.Property(e => e.Id)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Id");
               
                entity.Property(e => e.CustomerId)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CustomerId");

                entity.Property(e => e.Year)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Year");

                entity.Property(e => e.StartDate)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("StartDate");

                entity.Property(e => e.EndDate)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EndDate");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Description");

                entity.Property(e => e.DatabaseUri)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DatabaseURI");

                entity.Property(e => e.ThemeCode)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ThemeCode");


                entity.Property(e => e.BackCode)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("BackCode");

                entity.Property(e => e.WebUrl)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("WebUrl");

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("CreatedDate");

                entity.Property(e => e.UpdatedDate)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("UpdatedDate");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.ToTable("UserSession");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<YearMaster>(entity =>
            {
                entity.ToTable("YearMaster");

                entity.Property(e => e.Year)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FinYear)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.IsActive)
                  .HasMaxLength(50)
                  .IsUnicode(false);
            });

            modelBuilder.Entity<SuperAdminLogins>(entity =>
            {
                entity.ToTable("SuperAdminLogin");

                entity.Property(e => e.Id)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.UserType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreparedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PreparedDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<OTPTrans>(entity =>
            {
                entity.ToTable("OTPTrans");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .IsUnicode(false);
                entity.Property(e => e.OTP)
               .HasMaxLength(50)
               .IsUnicode(false);
                entity.Property(e => e.CreatedDate)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.ExpireDate)
             .HasMaxLength(50)
             .IsUnicode(false);
                entity.Property(e => e.IsActive)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            });

            modelBuilder.Entity<StateMaster>(entity =>
            {
                entity.ToTable("StateMaster");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .IsUnicode(false);
                entity.Property(e => e.Statename)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Statecode)
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
                entity.Property(e => e.IsActive)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            });
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("Country");

                entity.Property(e => e._Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CountryName)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.IsActive)
                  .HasMaxLength(50)
                  .IsUnicode(false);
            });
            modelBuilder.Entity<ThemCode>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("ThemeMaster");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Year)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.ThemeCode)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.BackCode)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.IsActive)
                  .HasMaxLength(50)
                  .IsUnicode(false);
            });
            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
