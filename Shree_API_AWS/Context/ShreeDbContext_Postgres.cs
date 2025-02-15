using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Models;

namespace Shree_API_AWS.Context;

public partial class ShreeDbContext_Postgres : DbContext
{
    private string connectionString;
    public ShreeDbContext_Postgres(DbContextOptions<ShreeDbContext_Postgres> options, IConfiguration config)
        : base(options)
    {
        connectionString = config.GetConnectionString("PostgresConnection");
    }

    public virtual DbSet<AlertNotification> AlertNotifications { get; set; }

    public virtual DbSet<Blobtable> Blobtables { get; set; }

    public virtual DbSet<ClientDetail> ClientDetails { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Employeeattendance> Employeeattendances { get; set; }

    public virtual DbSet<Employeeloandetail> Employeeloandetails { get; set; }

    public virtual DbSet<Employeeloandetailslog> Employeeloandetailslogs { get; set; }

    public virtual DbSet<Employeemiscdetail> Employeemiscdetails { get; set; }

    public virtual DbSet<Employeepettycashdetail> Employeepettycashdetails { get; set; }

    public virtual DbSet<Employeesalaryadvancedetail> Employeesalaryadvancedetails { get; set; }

    public virtual DbSet<Indianholidays2025> Indianholidays2025s { get; set; }

    public virtual DbSet<Inventorylist> Inventorylists { get; set; }

    public virtual DbSet<Locationtracker> Locationtrackers { get; set; }

    public virtual DbSet<LogEmployeeattendance> LogEmployeeattendances { get; set; }

    public virtual DbSet<Loginventorydatum> Loginventorydata { get; set; }

    public virtual DbSet<Overtimeworking> Overtimeworkings { get; set; }

    public virtual DbSet<TimesheetdetailsAdmin> TimesheetdetailsAdmins { get; set; }

    public virtual DbSet<TimesheetdetailsEmployee> TimesheetdetailsEmployees { get; set; }

    public virtual DbSet<Userdetailstable> Userdetailstables { get; set; }

    public virtual DbSet<Vendorlist> Vendorlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql(connectionString);

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

            entity.HasIndex(e => e.Clientid, "uq_clientdetails_clientid").IsUnique();

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
            entity.Property(e => e.Dataenteredon).HasColumnName("dataenteredon");
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
            entity.Property(e => e.Ispublicholidayduty).HasColumnName("ispublicholidayduty");
            entity.Property(e => e.Issundyduty).HasColumnName("issundyduty");
            entity.Property(e => e.Lastabsentdate).HasColumnName("lastabsentdate");
            entity.Property(e => e.Lasthalfdaydate).HasColumnName("lasthalfdaydate");
            entity.Property(e => e.Lastlateday).HasColumnName("lastlateday");
            entity.Property(e => e.Lastpaidleavedate).HasColumnName("lastpaidleavedate");
            entity.Property(e => e.Lastpresentdate).HasColumnName("lastpresentdate");
            entity.Property(e => e.Lastpublicholidaydutydate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("lastpublicholidaydutydate");
            entity.Property(e => e.Lastsundaydutydate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("lastsundaydutydate");
            entity.Property(e => e.Totaldaysabsent).HasColumnName("totaldaysabsent");
            entity.Property(e => e.Totaldayshalfdays).HasColumnName("totaldayshalfdays");
            entity.Property(e => e.Totaldayslateday).HasColumnName("totaldayslateday");
            entity.Property(e => e.Totaldayspresent).HasColumnName("totaldayspresent");
            entity.Property(e => e.Totalpublicholidaydutydays).HasColumnName("totalpublicholidaydutydays");
            entity.Property(e => e.Totalsundaydutydays).HasColumnName("totalsundaydutydays");

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
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Lastloancollecteddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastloancollecteddate");
            entity.Property(e => e.Loanamount)
                .HasPrecision(10, 2)
                .HasColumnName("loanamount");
            entity.Property(e => e.Loanprocessedon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("loanprocessedon");
            entity.Property(e => e.Partsofrepayment).HasColumnName("partsofrepayment");
            entity.Property(e => e.Remarks).HasColumnName("remarks");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeeloandetails)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .HasConstraintName("fk_employee");
        });

        modelBuilder.Entity<Employeeloandetailslog>(entity =>
        {
            entity.HasKey(e => e.Logid).HasName("employeeloandetailslog_pkey");

            entity.ToTable("employeeloandetailslog");

            entity.Property(e => e.Logid).HasColumnName("logid");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Employeeloanid).HasColumnName("employeeloanid");
            entity.Property(e => e.Isloanamountdeducted).HasColumnName("isloanamountdeducted");
            entity.Property(e => e.Loanamount)
                .HasPrecision(10, 2)
                .HasColumnName("loanamount");
            entity.Property(e => e.Partofpaymentremaining).HasColumnName("partofpaymentremaining");
            entity.Property(e => e.Transactiondate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transactiondate");
            entity.Property(e => e.Transactiontype)
                .HasMaxLength(10)
                .HasColumnName("transactiontype");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeeloandetailslogs)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .HasConstraintName("fk_employee_log");

            entity.HasOne(d => d.Employeeloan).WithMany(p => p.Employeeloandetailslogs)
                .HasForeignKey(d => d.Employeeloanid)
                .HasConstraintName("fk_employee_loan");
        });

        modelBuilder.Entity<Employeemiscdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employeemiscdetails_pkey");

            entity.ToTable("employeemiscdetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amountprocessedon).HasColumnName("amountprocessedon");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Iscashdeducted).HasColumnName("iscashdeducted");
            entity.Property(e => e.Misccashamount)
                .HasPrecision(10, 2)
                .HasColumnName("misccashamount");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Transactiontype)
                .HasMaxLength(100)
                .HasColumnName("transactiontype");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeemiscdetails)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .HasConstraintName("fk_employee");
        });

        modelBuilder.Entity<Employeepettycashdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employeepettycashdetails_pkey");

            entity.ToTable("employeepettycashdetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amountprocessedon).HasColumnName("amountprocessedon");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Iscashdeducted).HasColumnName("iscashdeducted");
            entity.Property(e => e.Pettycashamount)
                .HasPrecision(10, 2)
                .HasColumnName("pettycashamount");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Transactiontype)
                .HasMaxLength(100)
                .HasColumnName("transactiontype");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeepettycashdetails)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .HasConstraintName("fk_employee");
        });

        modelBuilder.Entity<Employeesalaryadvancedetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employeesalaryadvancedetails_pkey");

            entity.ToTable("employeesalaryadvancedetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Advanceamount)
                .HasPrecision(10, 2)
                .HasColumnName("advanceamount");
            entity.Property(e => e.Amountprocessedon).HasColumnName("amountprocessedon");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(50)
                .HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isadvancededucted).HasColumnName("isadvancededucted");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Transactiontype)
                .HasMaxLength(100)
                .HasColumnName("transactiontype");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeesalaryadvancedetails)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .HasConstraintName("fk_employee");
        });

        modelBuilder.Entity<Indianholidays2025>(entity =>
        {
            entity.HasKey(e => e.Holidayid).HasName("indianholidays_2025_pkey");

            entity.ToTable("indianholidays_2025");

            entity.Property(e => e.Holidayid).HasColumnName("holidayid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Holidaydate).HasColumnName("holidaydate");
            entity.Property(e => e.Holidayname)
                .HasMaxLength(100)
                .HasColumnName("holidayname");
            entity.Property(e => e.Holidaytype)
                .HasMaxLength(50)
                .HasColumnName("holidaytype");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasColumnName("region");
        });

        modelBuilder.Entity<Inventorylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inventorylist_pkey");

            entity.ToTable("inventorylist");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dateenteredby)
                .HasMaxLength(100)
                .HasColumnName("dateenteredby");
            entity.Property(e => e.Dateenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateenteredon");
            entity.Property(e => e.Materialdescription).HasColumnName("materialdescription");
            entity.Property(e => e.Materialid)
                .HasMaxLength(50)
                .HasColumnName("materialid");
            entity.Property(e => e.Materialname).HasColumnName("materialname");
            entity.Property(e => e.Materialpriceperunit)
                .HasPrecision(10, 2)
                .HasColumnName("materialpriceperunit");
            entity.Property(e => e.Measuringunit)
                .HasMaxLength(50)
                .HasColumnName("measuringunit");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Typeofmaterial)
                .HasMaxLength(50)
                .HasColumnName("typeofmaterial");
            entity.Property(e => e.Vendorid)
                .HasMaxLength(50)
                .HasColumnName("vendorid");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Inventorylists)
                .HasPrincipalKey(p => p.Vendorid)
                .HasForeignKey(d => d.Vendorid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_inventorylist_vendor");
        });

        modelBuilder.Entity<Locationtracker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("locationtracker_pkey");

            entity.ToTable("locationtracker");

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
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Locationlog).HasColumnName("locationlog");
        });

        modelBuilder.Entity<LogEmployeeattendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("log_employeeattendance_pkey");

            entity.ToTable("log_employeeattendance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attendancedate).HasColumnName("attendancedate");
            entity.Property(e => e.Checkintiming)
                .HasMaxLength(100)
                .HasColumnName("checkintiming");
            entity.Property(e => e.Checkouttiming)
                .HasMaxLength(100)
                .HasColumnName("checkouttiming");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
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

        modelBuilder.Entity<Loginventorydatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("loginventorydata_pkey");

            entity.ToTable("loginventorydata");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clientid)
                .HasMaxLength(100)
                .HasColumnName("clientid");
            entity.Property(e => e.Currentmaterialpriceperunit)
                .HasPrecision(10, 2)
                .HasColumnName("currentmaterialpriceperunit");
            entity.Property(e => e.Dateenteredby)
                .HasMaxLength(100)
                .HasColumnName("dateenteredby");
            entity.Property(e => e.Dateenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateenteredon");
            entity.Property(e => e.Dateoftransaction)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateoftransaction");
            entity.Property(e => e.Materialdescription).HasColumnName("materialdescription");
            entity.Property(e => e.Materialid)
                .HasMaxLength(50)
                .HasColumnName("materialid");
            entity.Property(e => e.Materialname).HasColumnName("materialname");
            entity.Property(e => e.Measuringunit)
                .HasMaxLength(50)
                .HasColumnName("measuringunit");
            entity.Property(e => e.Priceofunittransacted)
                .HasPrecision(10, 2)
                .HasColumnName("priceofunittransacted");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Typeofmaterial)
                .HasMaxLength(50)
                .HasColumnName("typeofmaterial");
            entity.Property(e => e.Typeoftransaction)
                .HasMaxLength(100)
                .HasColumnName("typeoftransaction");
            entity.Property(e => e.Vendorid)
                .HasMaxLength(50)
                .HasColumnName("vendorid");

            entity.HasOne(d => d.Client).WithMany(p => p.Loginventorydata)
                .HasPrincipalKey(p => p.Clientid)
                .HasForeignKey(d => d.Clientid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_loginventorydata_client");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Loginventorydata)
                .HasPrincipalKey(p => p.Vendorid)
                .HasForeignKey(d => d.Vendorid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_loginventorydata_vendor");
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
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Dateofotworking).HasColumnName("dateofotworking");
            entity.Property(e => e.Employeeid)
                .HasMaxLength(12)
                .HasColumnName("employeeid");
            entity.Property(e => e.Entryfor)
                .HasMaxLength(100)
                .HasColumnName("entryfor");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Othoursworked).HasColumnName("othoursworked");

            entity.HasOne(d => d.Employee).WithMany(p => p.Overtimeworkings)
                .HasPrincipalKey(p => p.Employeeid)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employee");
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

        modelBuilder.Entity<Vendorlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vendorlist_pkey");

            entity.ToTable("vendorlist");

            entity.HasIndex(e => e.Vendorid, "vendorlist_vendorid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dataenteredby)
                .HasMaxLength(100)
                .HasColumnName("dataenteredby");
            entity.Property(e => e.Dataenteredon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataenteredon");
            entity.Property(e => e.Gstnumber)
                .HasMaxLength(100)
                .HasColumnName("gstnumber");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Ownername)
                .HasMaxLength(200)
                .HasColumnName("ownername");
            entity.Property(e => e.Ownernumber).HasColumnName("ownernumber");
            entity.Property(e => e.Vendorid)
                .HasMaxLength(50)
                .HasColumnName("vendorid");
            entity.Property(e => e.Vendorlocation).HasColumnName("vendorlocation");
            entity.Property(e => e.Vendorname)
                .HasMaxLength(500)
                .HasColumnName("vendorname");
            entity.Property(e => e.Vendortype)
                .HasMaxLength(200)
                .HasColumnName("vendortype");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
