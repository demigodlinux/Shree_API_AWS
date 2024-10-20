using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Context;

public partial class ShreedbContext : DbContext
{
    public ShreedbContext()
    {
    }

    public ShreedbContext(DbContextOptions<ShreedbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }

    public virtual DbSet<EmployeeLoanDetail> EmployeeLoanDetails { get; set; }

    public virtual DbSet<LogEmployeeAttendance> LogEmployeeAttendances { get; set; }

    public virtual DbSet<OvertimeWorking> OvertimeWorkings { get; set; }

    public virtual DbSet<UserDetailsTable> UserDetailsTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=shreeelevatordb-2.c3y42am6oahf.ap-south-1.rds.amazonaws.com,1433;Database=shreedb;User Id=admin;Password=Aadinandika;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27D3F6F0A3");

            entity.HasIndex(e => e.EmployeeId, "UQ__Employee__7AD04FF031317FDD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Basic).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Ecname)
                .HasMaxLength(50)
                .HasColumnName("ECName");
            entity.Property(e => e.Ecnumber).HasColumnName("ECNumber");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.HireDate).HasColumnType("datetime");
            entity.Property(e => e.Hra)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("HRA");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.NativePlace).HasMaxLength(100);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SpecialAllowance).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<EmployeeAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC2772D45691");

            entity.ToTable("EmployeeAttendance");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EntryForMonth).HasMaxLength(50);
            entity.Property(e => e.LastAbsentDate).HasColumnType("datetime");
            entity.Property(e => e.LastHalfDayDate).HasColumnType("datetime");
            entity.Property(e => e.LastLateDay).HasColumnType("datetime");
            entity.Property(e => e.LastPaidLeaveDate).HasColumnType("datetime");
            entity.Property(e => e.LastPresentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeAttendances)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeA__Emplo__5DCAEF64");
        });

        modelBuilder.Entity<EmployeeLoanDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC274E4AC009");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EntryForMonth).HasMaxLength(50);
            entity.Property(e => e.InstallmentAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LastLoanAmountPaid).HasColumnType("datetime");
            entity.Property(e => e.LoanAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LoanIssuedOn).HasColumnType("datetime");
            entity.Property(e => e.OutstandingLoanAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeLoanDetails)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeL__Emplo__5AEE82B9");
        });

        modelBuilder.Entity<LogEmployeeAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log_Empl__3214EC27E9F50FFF");

            entity.ToTable("Log_EmployeeAttendance");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AttendanceDate).HasColumnType("datetime");
            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
            entity.Property(e => e.EmployeeAttendId).HasColumnName("EmployeeAttendID");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EntryForMonth).HasMaxLength(50);

            entity.HasOne(d => d.EmployeeAttend).WithMany(p => p.LogEmployeeAttendances)
                .HasForeignKey(d => d.EmployeeAttendId)
                .HasConstraintName("FK__Log_Emplo__Emplo__66603565");

            entity.HasOne(d => d.Employee).WithMany(p => p.LogEmployeeAttendances)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Log_Emplo__Emplo__656C112C");
        });

        modelBuilder.Entity<OvertimeWorking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Overtime__3214EC27DFB6BAB2");

            entity.ToTable("OvertimeWorking");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataEnteredBy).HasMaxLength(100);
            entity.Property(e => e.DataEnteredOn).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EntryForMonth).HasMaxLength(50);
            entity.Property(e => e.LastPublicHolidayDuty).HasColumnType("datetime");
            entity.Property(e => e.LastSundayDuty).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.OvertimeWorkings)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OvertimeW__Emplo__5812160E");
        });

        modelBuilder.Entity<UserDetailsTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserDeta__3214EC27F93F5EDE");

            entity.ToTable("UserDetailsTable");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .HasColumnName("password");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");

            entity.HasOne(d => d.Employee).WithMany(p => p.UserDetailsTables)
                .HasPrincipalKey(p => p.EmployeeId)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserDetai__Emplo__6EF57B66");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
