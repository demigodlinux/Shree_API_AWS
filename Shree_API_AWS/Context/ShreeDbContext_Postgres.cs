using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Context;

public partial class ShreeDbContext_Postgres : DbContext
{
    public ShreeDbContext_Postgres()
    {
    }

    public ShreeDbContext_Postgres(DbContextOptions<ShreeDbContext_Postgres> options)
        : base(options)
    {
    }

    public virtual DbSet<AlertNotification> AlertNotifications { get; set; }

    public virtual DbSet<Blobtable> Blobtables { get; set; }

    public virtual DbSet<ClientDetail> ClientDetails { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Employeeattendance> Employeeattendances { get; set; }

    public virtual DbSet<Employeeloandetail> Employeeloandetails { get; set; }

    public virtual DbSet<LogEmployeeattendance> LogEmployeeattendances { get; set; }

    public virtual DbSet<Overtimeworking> Overtimeworkings { get; set; }

    public virtual DbSet<TimesheetdetailsAdmin> TimesheetdetailsAdmins { get; set; }

    public virtual DbSet<TimesheetdetailsEmployee> TimesheetdetailsEmployees { get; set; }

    public virtual DbSet<Userdetailstable> Userdetailstables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=master;Username=postgres;Password=Aadinandika");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlertNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("alert_notifications_pkey");

            entity.ToTable("alert_notifications");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(20)
                .HasColumnName("employeeid");
            entity.Property(e => e.Isadminnotification).HasColumnName("isadminnotification");
            entity.Property(e => e.Isapproved).HasColumnName("isapproved");
            entity.Property(e => e.Isemployeenotification).HasColumnName("isemployeenotification");
            entity.Property(e => e.Isnotificationactive).HasColumnName("isnotificationactive");
            entity.Property(e => e.Issentback).HasColumnName("issentback");
            entity.Property(e => e.Notificationmessage)
                .HasMaxLength(8000)
                .HasColumnName("notificationmessage");
            entity.Property(e => e.TimesheetId).HasColumnName("timesheet_id");
        });

        modelBuilder.Entity<Blobtable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("blobtable_pkey");

            entity.ToTable("blobtable");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dateenteredby)
                .HasMaxLength(100)
                .HasColumnName("dateenteredby");
            entity.Property(e => e.Dateenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Filename)
                .HasMaxLength(255)
                .HasColumnName("filename");
            entity.Property(e => e.Filepath).HasColumnName("filepath");
            entity.Property(e => e.Servicetype)
                .HasMaxLength(100)
                .HasColumnName("servicetype");
            entity.Property(e => e.Sitelocation)
                .HasMaxLength(500)
                .HasColumnName("sitelocation");
            entity.Property(e => e.Sitename)
                .HasMaxLength(500)
                .HasColumnName("sitename");
            entity.Property(e => e.Typeofupload)
                .HasMaxLength(100)
                .HasColumnName("typeofupload");
        });

        modelBuilder.Entity<ClientDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_details_pkey");

            entity.ToTable("client_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clientaddress)
                .HasMaxLength(1500)
                .HasColumnName("clientaddress");
            entity.Property(e => e.Clientid)
                .HasMaxLength(100)
                .HasColumnName("clientid");
            entity.Property(e => e.Clientlocation)
                .HasMaxLength(200)
                .HasColumnName("clientlocation");
            entity.Property(e => e.Clientname)
                .HasMaxLength(500)
                .HasColumnName("clientname");
            entity.Property(e => e.Clientservicetype)
                .HasMaxLength(200)
                .HasColumnName("clientservicetype");
            entity.Property(e => e.Contractamount)
                .HasPrecision(10, 2)
                .HasColumnName("contractamount");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractperiod).HasColumnName("contractperiod");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isgstincluded).HasColumnName("isgstincluded");
            entity.Property(e => e.Numberoflifts).HasColumnName("numberoflifts");
            entity.Property(e => e.Sitemanagercontact).HasColumnName("sitemanagercontact");
            entity.Property(e => e.Sitemanagername)
                .HasMaxLength(200)
                .HasColumnName("sitemanagername");
            entity.Property(e => e.Termsofpayment)
                .HasMaxLength(200)
                .HasColumnName("termsofpayment");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.HasIndex(e => e.Employeeid, "employees_employeeid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Basic)
                .HasPrecision(10, 2)
                .HasColumnName("basic");
            entity.Property(e => e.Dateofbirth)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateofbirth");
            entity.Property(e => e.Ecname)
                .HasMaxLength(50)
                .HasColumnName("ecname");
            entity.Property(e => e.Ecnumber).HasColumnName("ecnumber");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Hiredate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("hiredate");
            entity.Property(e => e.Hra)
                .HasPrecision(10, 2)
                .HasColumnName("hra");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Mobilenumber).HasColumnName("mobilenumber");
            entity.Property(e => e.Nativeplace)
                .HasMaxLength(100)
                .HasColumnName("nativeplace");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .HasColumnName("position");
            entity.Property(e => e.Salary)
                .HasPrecision(10, 2)
                .HasColumnName("salary");
            entity.Property(e => e.Specialallowance)
                .HasPrecision(10, 2)
                .HasColumnName("specialallowance");
        });

        modelBuilder.Entity<Employeeattendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employeeattendance_pkey");

            entity.ToTable("employeeattendance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Entryformonth)
                .HasMaxLength(50)
                .HasColumnName("entryformonth");
            entity.Property(e => e.Isabsent).HasColumnName("isabsent");
            entity.Property(e => e.Ishalfday).HasColumnName("ishalfday");
            entity.Property(e => e.Islate).HasColumnName("islate");
            entity.Property(e => e.Ispaidleave).HasColumnName("ispaidleave");
            entity.Property(e => e.Ispresent).HasColumnName("ispresent");
            entity.Property(e => e.Lastabsentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastabsentdate");
            entity.Property(e => e.Lasthalfdaydate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lasthalfdaydate");
            entity.Property(e => e.Lastlateday)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastlateday");
            entity.Property(e => e.Lastpaidleavedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastpaidleavedate");
            entity.Property(e => e.Lastpresentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastpresentdate");
            entity.Property(e => e.Totaldaysabsent).HasColumnName("totaldaysabsent");
            entity.Property(e => e.Totaldayshalfdays).HasColumnName("totaldayshalfdays");
            entity.Property(e => e.Totaldayslateday).HasColumnName("totaldayslateday");
            entity.Property(e => e.Totaldayspresent).HasColumnName("totaldayspresent");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeeattendances)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_attendance_employee");
        });

        modelBuilder.Entity<Employeeloandetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employeeloandetails_pkey");

            entity.ToTable("employeeloandetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Entryformonth)
                .HasMaxLength(50)
                .HasColumnName("entryformonth");
            entity.Property(e => e.Installmentamount)
                .HasPrecision(10, 2)
                .HasColumnName("installmentamount");
            entity.Property(e => e.Installmentscompleted).HasColumnName("installmentscompleted");
            entity.Property(e => e.Isloanrepayed).HasColumnName("isloanrepayed");
            entity.Property(e => e.Lastloanamountpaid)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastloanamountpaid");
            entity.Property(e => e.Loanamount)
                .HasPrecision(10, 2)
                .HasColumnName("loanamount");
            entity.Property(e => e.Loanissuedon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("loanissuedon");
            entity.Property(e => e.Outstandingloanamount)
                .HasPrecision(10, 2)
                .HasColumnName("outstandingloanamount");
            entity.Property(e => e.Totalinstallments).HasColumnName("totalinstallments");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeeloandetails)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee_loan_details_employee");
        });

        modelBuilder.Entity<LogEmployeeattendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("log_employeeattendance_pkey");

            entity.ToTable("log_employeeattendance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attendancedate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("attendancedate");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeattendid).HasColumnName("employeeattendid");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Entryformonth)
                .HasMaxLength(50)
                .HasColumnName("entryformonth");
            entity.Property(e => e.Isabsent).HasColumnName("isabsent");
            entity.Property(e => e.Ishalfday).HasColumnName("ishalfday");
            entity.Property(e => e.Islate).HasColumnName("islate");
            entity.Property(e => e.Isonduty).HasColumnName("isonduty");
            entity.Property(e => e.Ispaidleave).HasColumnName("ispaidleave");
            entity.Property(e => e.Ispresent).HasColumnName("ispresent");

            entity.HasOne(d => d.Employeeattend).WithMany(p => p.LogEmployeeattendances)
                .HasForeignKey(d => d.Employeeattendid)
                .HasConstraintName("fk_log_employee_attendance_attendance");

            entity.HasOne(d => d.Employee).WithMany(p => p.LogEmployeeattendances)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_log_employee_attendance_employee");
        });

        modelBuilder.Entity<Overtimeworking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("overtimeworking_pkey");

            entity.ToTable("overtimeworking");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Entryformonth)
                .HasMaxLength(50)
                .HasColumnName("entryformonth");
            entity.Property(e => e.Ispublicholidayduty).HasColumnName("ispublicholidayduty");
            entity.Property(e => e.Issundayduty).HasColumnName("issundayduty");
            entity.Property(e => e.Lastpublicholidayduty)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastpublicholidayduty");
            entity.Property(e => e.Lastsundayduty)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastsundayduty");
            entity.Property(e => e.Totalpublicholidayduty).HasColumnName("totalpublicholidayduty");
            entity.Property(e => e.Totalsundayduty).HasColumnName("totalsundayduty");

            entity.HasOne(d => d.Employee).WithMany(p => p.Overtimeworkings)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_overtime_working_employee");
        });

        modelBuilder.Entity<TimesheetdetailsAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timesheetdetails_admin_pkey");

            entity.ToTable("timesheetdetails_admin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Timesheetdetailid).HasColumnName("timesheetdetailid");

            entity.HasOne(d => d.Employee).WithMany(p => p.TimesheetdetailsAdmins)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employeeid");

            entity.HasOne(d => d.Timesheetdetail).WithMany(p => p.TimesheetdetailsAdmins)
                .HasForeignKey(d => d.Timesheetdetailid)
                .HasConstraintName("fk_timesheetdetailid");
        });

        modelBuilder.Entity<TimesheetdetailsEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timesheetdetails_employees_pkey");

            entity.ToTable("timesheetdetails_employees");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(50)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Timesheetdetails).HasColumnName("timesheetdetails");

            entity.HasOne(d => d.Employee).WithMany(p => p.TimesheetdetailsEmployees)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .HasConstraintName("fk_employeeid");
        });

        modelBuilder.Entity<Userdetailstable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userdetailstable_pkey");

            entity.ToTable("userdetailstable");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Password)
                .HasMaxLength(10)
                .HasColumnName("password");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Employee).WithMany(p => p.Userdetailstables)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_details_table_employee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
