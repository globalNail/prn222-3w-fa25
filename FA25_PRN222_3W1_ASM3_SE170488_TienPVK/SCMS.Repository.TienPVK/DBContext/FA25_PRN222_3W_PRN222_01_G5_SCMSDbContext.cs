using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SCMS.Domain.TienPVK.Models;

namespace SCMS.Repository.TienPVK.DBContext;

public partial class FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext : DbContext
{
    public FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext()
    {
    }

    public FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext(DbContextOptions<FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityRegistration> ActivityRegistrations { get; set; }

    public virtual DbSet<AttendanceCheck> AttendanceChecks { get; set; }

    public virtual DbSet<AttendanceSession> AttendanceSessions { get; set; }

    public virtual DbSet<ClubCategoriesTienPvk> ClubCategoriesTienPvks { get; set; }

    public virtual DbSet<ClubFeePolicy> ClubFeePolicies { get; set; }

    public virtual DbSet<ClubsTienPvk> ClubsTienPvks { get; set; }

    public virtual DbSet<DisciplinaryAction> DisciplinaryActions { get; set; }

    public virtual DbSet<DisciplinaryCase> DisciplinaryCases { get; set; }

    public virtual DbSet<FeeInvoice> FeeInvoices { get; set; }

    public virtual DbSet<FeeInvoiceLine> FeeInvoiceLines { get; set; }

    public virtual DbSet<JoinRequest> JoinRequests { get; set; }

    public virtual DbSet<JoinRequestReview> JoinRequestReviews { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberDocument> MemberDocuments { get; set; }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; }

    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activiti__45F4A7F13D2107BC");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.CancelReason).HasMaxLength(255);
            entity.Property(e => e.ClubIdtienPvk).HasColumnName("ClubIDTienPVK");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FeePerPerson).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Planned");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.ClubIdtienPvkNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ClubIdtienPvk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Activities_ClubsTienPVK");
        });

        modelBuilder.Entity<ActivityRegistration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Activity__6EF5883058DE3AC3");

            entity.HasIndex(e => e.ActivityId, "IX_ActivityRegs_Activity");

            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.RegisteredAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivityRegistrations)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityRegs_Activities");

            entity.HasOne(d => d.Member).WithMany(p => p.ActivityRegistrations)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityRegs_Members");
        });

        modelBuilder.Entity<AttendanceCheck>(entity =>
        {
            entity.HasKey(e => e.AttendanceCheckId).HasName("PK__Attendan__232CA08E601776AB");

            entity.Property(e => e.AttendanceCheckId).HasColumnName("AttendanceCheckID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.AttendanceSession).WithMany(p => p.AttendanceChecks)
                .HasForeignKey(d => d.AttendanceSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttChecks_Session");

            entity.HasOne(d => d.Member).WithMany(p => p.AttendanceChecks)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttChecks_Member");
        });

        modelBuilder.Entity<AttendanceSession>(entity =>
        {
            entity.HasKey(e => e.AttendanceSessionId).HasName("PK__Attendan__AF3ABC576A9FE331");

            entity.Property(e => e.AttendanceSessionId).HasColumnName("AttendanceSessionID");
            entity.Property(e => e.ClubIdtienPvk).HasColumnName("ClubIDTienPVK");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Open");
            entity.Property(e => e.Title).HasMaxLength(150);

            entity.HasOne(d => d.Activity).WithMany(p => p.AttendanceSessions)
                .HasForeignKey(d => d.ActivityId)
                .HasConstraintName("FK_AttSes_Activities");

            entity.HasOne(d => d.ClubIdtienPvkNavigation).WithMany(p => p.AttendanceSessions)
                .HasForeignKey(d => d.ClubIdtienPvk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttSes_ClubsTienPVK");

            entity.HasOne(d => d.HostUser).WithMany(p => p.AttendanceSessions)
                .HasForeignKey(d => d.HostUserId)
                .HasConstraintName("FK_AttSes_SystemAccount");
        });

        modelBuilder.Entity<ClubCategoriesTienPvk>(entity =>
        {
            entity.HasKey(e => e.CategoryIdtienPvk);

            entity.ToTable("ClubCategoriesTienPVK");

            entity.HasIndex(e => e.CategoryCode, "UQ__ClubCate__371BA955E5F89157").IsUnique();

            entity.Property(e => e.CategoryIdtienPvk).HasColumnName("CategoryIDTienPVK");
            entity.Property(e => e.CategoryCode).HasMaxLength(10);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<ClubFeePolicy>(entity =>
        {
            entity.HasKey(e => e.FeePolicyId).HasName("PK__ClubFeeP__ADCA960A50E43B3B");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.ClubIdtienPvk).HasColumnName("ClubIDTienPVK");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FeeType).HasMaxLength(64);
            entity.Property(e => e.IsMandatory).HasDefaultValue(true);
            entity.Property(e => e.Period).HasMaxLength(32);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.ClubIdtienPvkNavigation).WithMany(p => p.ClubFeePolicies)
                .HasForeignKey(d => d.ClubIdtienPvk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClubFeePolicies_ClubsTienPVK");
        });

        modelBuilder.Entity<ClubsTienPvk>(entity =>
        {
            entity.HasKey(e => e.ClubIdtienPvk);

            entity.ToTable("ClubsTienPVK");

            entity.HasIndex(e => e.ClubCode, "UQ__ClubsTie__3B436E80640C4767").IsUnique();

            entity.Property(e => e.ClubIdtienPvk).HasColumnName("ClubIDTienPVK");
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CategoryIdtienPvk).HasColumnName("CategoryIDTienPVK");
            entity.Property(e => e.ClubCode).HasMaxLength(20);
            entity.Property(e => e.ClubName).HasMaxLength(150);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.IsOpenToJoin).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RequiresApproval).HasDefaultValue(true);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Category).WithMany(p => p.ClubsTienPvks)
                .HasForeignKey(d => d.CategoryIdtienPvk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClubsTienPVK_Category");

            entity.HasOne(d => d.ManagerUser).WithMany(p => p.ClubsTienPvks)
                .HasForeignKey(d => d.ManagerUserId)
                .HasConstraintName("FK_ClubsTienPVK_ManagerUser");
        });

        modelBuilder.Entity<DisciplinaryAction>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK__Discipli__FFE3F4B9B43D79BC");

            entity.Property(e => e.ActionId).HasColumnName("ActionID");
            entity.Property(e => e.ActionType).HasMaxLength(32);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Note).HasMaxLength(500);

            entity.HasOne(d => d.Case).WithMany(p => p.DisciplinaryActions)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscActions_Case");
        });

        modelBuilder.Entity<DisciplinaryCase>(entity =>
        {
            entity.HasKey(e => e.CaseId).HasName("PK__Discipli__6CAE526C79C00DF0");

            entity.Property(e => e.CaseId).HasColumnName("CaseID");
            entity.Property(e => e.ClubIdtienPvk).HasColumnName("ClubIDTienPVK");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.OpenedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Severity).HasMaxLength(16);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Open");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.ClubIdtienPvkNavigation).WithMany(p => p.DisciplinaryCases)
                .HasForeignKey(d => d.ClubIdtienPvk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscCases_ClubsTienPVK");

            entity.HasOne(d => d.Member).WithMany(p => p.DisciplinaryCases)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscCases_Members");

            entity.HasOne(d => d.ReportedByUser).WithMany(p => p.DisciplinaryCases)
                .HasForeignKey(d => d.ReportedByUserId)
                .HasConstraintName("FK_DiscCases_SystemAcc");
        });

        modelBuilder.Entity<FeeInvoice>(entity =>
        {
            entity.HasKey(e => e.FeeInvoiceId).HasName("PK__FeeInvoi__AE7A61BE457D2639");

            entity.HasIndex(e => new { e.ClubIdtienPvk, e.IssuedAt }, "IX_FeeInvoices_Club_Dates");

            entity.HasIndex(e => e.InvoiceNumber, "UQ__FeeInvoi__D776E98137E94B81").IsUnique();

            entity.Property(e => e.FeeInvoiceId).HasColumnName("FeeInvoiceID");
            entity.Property(e => e.ClubIdtienPvk).HasColumnName("ClubIDTienPVK");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(30);
            entity.Property(e => e.IssuedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PaymentMethod).HasMaxLength(32);
            entity.Property(e => e.ProviderTxnId).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Draft");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.ClubIdtienPvkNavigation).WithMany(p => p.FeeInvoices)
                .HasForeignKey(d => d.ClubIdtienPvk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeeInvoices_ClubsTienPVK");

            entity.HasOne(d => d.PayerMember).WithMany(p => p.FeeInvoices)
                .HasForeignKey(d => d.PayerMemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeeInvoices_Members");
        });

        modelBuilder.Entity<FeeInvoiceLine>(entity =>
        {
            entity.HasKey(e => e.FeeInvoiceLineId).HasName("PK__FeeInvoi__5F956242C0369987");

            entity.Property(e => e.FeeInvoiceLineId).HasColumnName("FeeInvoiceLineID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ItemName).HasMaxLength(150);
            entity.Property(e => e.LineTotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.FeeInvoice).WithMany(p => p.FeeInvoiceLines)
                .HasForeignKey(d => d.FeeInvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeeInvoiceLines_Invoices");

            entity.HasOne(d => d.FeePolicy).WithMany(p => p.FeeInvoiceLines)
                .HasForeignKey(d => d.FeePolicyId)
                .HasConstraintName("FK_FeeInvoiceLines_FeePolicy");
        });

        modelBuilder.Entity<JoinRequest>(entity =>
        {
            entity.HasKey(e => e.JoinRequestId).HasName("PK__JoinRequ__2573934A5CF05FE4");

            entity.Property(e => e.JoinRequestId).HasColumnName("JoinRequestID");
            entity.Property(e => e.ClubIdtienPvk).HasColumnName("ClubIDTienPVK");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.RequiredFeeAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.ApprovedByUser).WithMany(p => p.JoinRequests)
                .HasForeignKey(d => d.ApprovedByUserId)
                .HasConstraintName("FK_JoinRequests_Users");

            entity.HasOne(d => d.ClubIdtienPvkNavigation).WithMany(p => p.JoinRequests)
                .HasForeignKey(d => d.ClubIdtienPvk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JoinRequests_ClubsTienPVK");

            entity.HasOne(d => d.Member).WithMany(p => p.JoinRequests)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JoinRequests_Members");
        });

        modelBuilder.Entity<JoinRequestReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__JoinRequ__74BC79AE0AB8A8E8");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Decision)
                .HasMaxLength(16)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.JoinRequest).WithMany(p => p.JoinRequestReviews)
                .HasForeignKey(d => d.JoinRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JoinRequestReviews_JoinRequests");

            entity.HasOne(d => d.ReviewerUser).WithMany(p => p.JoinRequestReviews)
                .HasForeignKey(d => d.ReviewerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JoinRequestReviews_SystemAccount");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Members__0CF04B386A4428C2");

            entity.HasIndex(e => e.UserId, "IX_Members_User");

            entity.HasIndex(e => e.StudentCode, "UQ__Members__1FC88604D0A1CAEE").IsUnique();

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Faculty).HasMaxLength(120);
            entity.Property(e => e.Gender).HasMaxLength(16);
            entity.Property(e => e.Gpa)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("GPA");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Major).HasMaxLength(120);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.StudentCode).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany(p => p.Members)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Members_SystemAccount");
        });

        modelBuilder.Entity<MemberDocument>(entity =>
        {
            entity.HasKey(e => e.MemberDocumentId).HasName("PK__MemberDo__19D99DEE6F81865A");

            entity.Property(e => e.MemberDocumentId).HasColumnName("MemberDocumentID");
            entity.Property(e => e.DocType).HasMaxLength(50);
            entity.Property(e => e.FileUrl).HasMaxLength(500);
            entity.Property(e => e.Note).HasMaxLength(300);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberDocuments)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberDocuments_Members");
        });

        modelBuilder.Entity<SystemAccount>(entity =>
        {
            entity.HasKey(e => e.UserAccountId).HasName("PK_UserAccount");

            entity.ToTable("SystemAccount");

            entity.Property(e => e.UserAccountId).HasColumnName("UserAccountID");
            entity.Property(e => e.ApplicationCode).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.RequestCode).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
