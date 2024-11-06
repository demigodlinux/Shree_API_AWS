//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using Shree_API_AWS.Models;

//namespace Shree_API_AWS.Context;

//public partial class MasterContext : DbContext
//{
//    public MasterContext()
//    {
//    }

//    public MasterContext(DbContextOptions<MasterContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<ClientDetail> ClientDetails { get; set; }

//    public virtual DbSet<Employee> Employees { get; set; }

//    public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }

//    public virtual DbSet<EmployeeLoanDetail> EmployeeLoanDetails { get; set; }

//    public virtual DbSet<LogEmployeeAttendance> LogEmployeeAttendances { get; set; }

//    public virtual DbSet<OvertimeWorking> OvertimeWorkings { get; set; }

//    public virtual DbSet<UserDetailsTable> UserDetailsTables { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=AADIMACHINE\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<ClientDetail>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__client_d__3214EC27E297EBD6");

//            entity.ToTable("client_details");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.ClientAddress).HasMaxLength(1500);
//            entity.Property(e => e.ClientId)
//                .HasMaxLength(100)
//                .HasColumnName("ClientID");
//            entity.Property(e => e.ClientLocation).HasMaxLength(200);
//            entity.Property(e => e.ClientName).HasMaxLength(500);
//            entity.Property(e => e.ClientServiceType).HasMaxLength(200);
//            entity.Property(e => e.ContractAmount).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.ContractEndDate).HasColumnType("datetime");
//            entity.Property(e => e.ContractStartDate).HasColumnType("datetime");
//            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
//            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
//            entity.Property(e => e.IsGstincluded).HasColumnName("IsGSTIncluded");
//            entity.Property(e => e.SiteManagerName).HasMaxLength(200);
//        });

//        modelBuilder.Entity<Employee>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC2722DC49B1");

//            entity.HasIndex(e => e.EmployeeId, "UQ__Employee__7AD04FF06BBA4AD2").IsUnique();

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.Basic).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
//            entity.Property(e => e.Ecname)
//                .HasMaxLength(50)
//                .HasColumnName("ECName");
//            entity.Property(e => e.Ecnumber).HasColumnName("ECNumber");
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(50)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.FirstName).HasMaxLength(50);
//            entity.Property(e => e.HireDate).HasColumnType("datetime");
//            entity.Property(e => e.Hra)
//                .HasColumnType("decimal(10, 2)")
//                .HasColumnName("HRA");
//            entity.Property(e => e.LastName).HasMaxLength(50);
//            entity.Property(e => e.NativePlace).HasMaxLength(100);
//            entity.Property(e => e.Position).HasMaxLength(50);
//            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.SpecialAllowance).HasColumnType("decimal(10, 2)");
//        });

//        modelBuilder.Entity<EmployeeAttendance>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27FF8321BE");

//            entity.ToTable("EmployeeAttendance");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
//            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(50)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.EntryForMonth).HasMaxLength(50);
//            entity.Property(e => e.LastAbsentDate).HasColumnType("datetime");
//            entity.Property(e => e.LastHalfDayDate).HasColumnType("datetime");
//            entity.Property(e => e.LastLateDay).HasColumnType("datetime");
//            entity.Property(e => e.LastPaidLeaveDate).HasColumnType("datetime");
//            entity.Property(e => e.LastPresentDate).HasColumnType("datetime");

//            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeAttendances)
//                .HasPrincipalKey(p => p.EmployeeId)
//                .HasForeignKey(d => d.EmployeeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__EmployeeA__Emplo__3118447E");
//        });

//        modelBuilder.Entity<EmployeeLoanDetail>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC2760E6EF3E");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
//            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(50)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.EntryForMonth).HasMaxLength(50);
//            entity.Property(e => e.InstallmentAmount).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.LastLoanAmountPaid).HasColumnType("datetime");
//            entity.Property(e => e.LoanAmount).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.LoanIssuedOn).HasColumnType("datetime");
//            entity.Property(e => e.OutstandingLoanAmount).HasColumnType("decimal(10, 2)");

//            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeLoanDetails)
//                .HasPrincipalKey(p => p.EmployeeId)
//                .HasForeignKey(d => d.EmployeeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__EmployeeL__Emplo__320C68B7");
//        });

//        modelBuilder.Entity<LogEmployeeAttendance>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Log_Empl__3214EC27C4E713EE");

//            entity.ToTable("Log_EmployeeAttendance");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.AttendanceDate).HasColumnType("datetime");
//            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
//            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
//            entity.Property(e => e.EmployeeAttendId).HasColumnName("EmployeeAttendID");
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(50)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.EntryForMonth).HasMaxLength(50);

//            entity.HasOne(d => d.EmployeeAttend).WithMany(p => p.LogEmployeeAttendances)
//                .HasForeignKey(d => d.EmployeeAttendId)
//                .HasConstraintName("FK__Log_Emplo__Emplo__33F4B129");

//            entity.HasOne(d => d.Employee).WithMany(p => p.LogEmployeeAttendances)
//                .HasPrincipalKey(p => p.EmployeeId)
//                .HasForeignKey(d => d.EmployeeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__Log_Emplo__Emplo__33008CF0");
//        });

//        modelBuilder.Entity<OvertimeWorking>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Overtime__3214EC27B7DDB65A");

//            entity.ToTable("OvertimeWorking");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
//            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(50)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.EntryForMonth).HasMaxLength(50);
//            entity.Property(e => e.LastPublicHolidayDuty).HasColumnType("datetime");
//            entity.Property(e => e.LastSundayDuty).HasColumnType("datetime");

//            entity.HasOne(d => d.Employee).WithMany(p => p.OvertimeWorkings)
//                .HasPrincipalKey(p => p.EmployeeId)
//                .HasForeignKey(d => d.EmployeeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__OvertimeW__Emplo__34E8D562");
//        });

//        modelBuilder.Entity<UserDetailsTable>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__UserDeta__3214EC27E0B23A66");

//            entity.ToTable("UserDetailsTable");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
//            entity.Property(e => e.Email).HasMaxLength(50);
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(50)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.Password)
//                .HasMaxLength(10)
//                .HasColumnName("password");
//            entity.Property(e => e.Role).HasMaxLength(50);
//            entity.Property(e => e.UserName)
//                .HasMaxLength(50)
//                .HasColumnName("userName");

//            entity.HasOne(d => d.Employee).WithMany(p => p.UserDetailsTables)
//                .HasPrincipalKey(p => p.EmployeeId)
//                .HasForeignKey(d => d.EmployeeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__UserDetai__Emplo__35DCF99B");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
