/******************************************************************
 Student Club Management System - Full DDL (One file, LITE sub tables)
 Pairs for 7 members:
  #1 - ClubsTienPVK + ClubCategoriesTienPVK
  #2 - Members + MemberDocuments
  #3 - JoinRequests + JoinRequestReviews
  #4 - Activities + ActivityRegistrations
  #5 - FeeInvoices + FeeInvoiceLines
  #6 - AttendanceSessions + AttendanceChecks
  #7 - DisciplinaryCases + DisciplinaryActions
******************************************************************/

/* DB */
USE master
GO

IF DB_ID('B3W_PRN222_01_PRN222_SEXXXXXX_SCMS') IS NOT NULL
BEGIN
    ALTER DATABASE B3W_PRN222_01_PRN222_SEXXXXXX_SCMS SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE B3W_PRN222_01_PRN222_SEXXXXXX_SCMS;
END
GO

CREATE DATABASE B3W_PRN222_01_PRN222_SEXXXXXX_SCMS;
GO

USE B3W_PRN222_01_PRN222_SEXXXXXX_SCMS;
GO

/* ================== USER ACCOUNT (chung hệ thống) ================== */
IF OBJECT_ID(N'dbo.SystemAccount', N'U') IS NOT NULL DROP TABLE dbo.SystemAccount;
CREATE TABLE dbo.SystemAccount
(
    UserAccountID int IDENTITY(1,1) NOT NULL,
    UserName nvarchar(50) NOT NULL,
    [Password] nvarchar(100) NOT NULL,
    FullName nvarchar(100) NOT NULL,
    Email nvarchar(150) NOT NULL,
    Phone nvarchar(50) NOT NULL,
    EmployeeCode nvarchar(50) NOT NULL,
    RoleId int NOT NULL,
    RequestCode nvarchar(50) NULL,
    CreatedDate datetime NULL,
    ApplicationCode nvarchar(50) NULL,
    CreatedBy nvarchar(50) NULL,
    ModifiedDate datetime NULL,
    ModifiedBy nvarchar(50) NULL,
    IsActive bit NOT NULL,
    CONSTRAINT [PK_UserAccount] PRIMARY KEY CLUSTERED (UserAccountID ASC)
);
GO

/* ================================================================
   #1 - ClubsTienPVK (Main) + ClubFeePolicies (Sub LITE)
================================================================ */
IF OBJECT_ID(N'dbo.ClubCategoriesTienPVK', N'U') IS NOT NULL DROP TABLE dbo.ClubCategoriesTienPVK;
IF OBJECT_ID(N'dbo.ClubsTienPVK', N'U') IS NOT NULL DROP TABLE dbo.ClubsTienPVK;
GO

CREATE TABLE dbo.ClubCategoriesTienPVK
(
    CategoryIDTienPVK INT IDENTITY,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryCode NVARCHAR(10) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy NVARCHAR(50) NULL,
    ModifiedBy NVARCHAR(50) NULL,
    ModifiedAt DATETIME2 NULL,

    CONSTRAINT PK_ClubCategoriesTienPVK PRIMARY KEY(CategoryIDTienPVK)
);
GO

CREATE TABLE dbo.ClubsTienPVK
(
    ClubIDTienPVK INT IDENTITY(1,1),
    ClubCode NVARCHAR(20) NOT NULL UNIQUE,
    ClubName NVARCHAR(150) NOT NULL,
    CategoryIDTienPVK INT NOT NULL,
    Description NVARCHAR(MAX) NULL,
    FoundedDate DATETIME2 NOT NULL,
    Email NVARCHAR(150) NULL,
    Phone NVARCHAR(15) NULL,
    Address NVARCHAR(250) NULL,
    ManagerUserId INT NULL,
    MemberLimit INT NOT NULL DEFAULT 0,
    IsOpenToJoin BIT NOT NULL DEFAULT 1,
    RequiresApproval BIT NOT NULL DEFAULT 1,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Active',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy NVARCHAR(50) NULL,
    ModifiedBy NVARCHAR(50) NULL,
    ModifiedAt DATETIME2 NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,

    CONSTRAINT PK_ClubsTienPVK PRIMARY KEY(ClubIDTienPVK),
    CONSTRAINT FK_ClubsTienPVK_ManagerUser FOREIGN KEY (ManagerUserId) REFERENCES dbo.SystemAccount(UserAccountID),
    CONSTRAINT FK_ClubsTienPVK_Category FOREIGN KEY (CategoryIDTienPVK) REFERENCES dbo.ClubCategoriesTienPVK(CategoryIDTienPVK)
);
GO


/* LITE: 11 cột */
IF OBJECT_ID(N'dbo.ClubFeePolicies', N'U') IS NOT NULL DROP TABLE dbo.ClubFeePolicies;
CREATE TABLE dbo.ClubFeePolicies
(
    FeePolicyId INT IDENTITY(1,1) PRIMARY KEY,
    ClubIDTienPVK INT NOT NULL,
    FeeType NVARCHAR(64) NOT NULL,
    -- Membership/Monthly/Event
    Amount DECIMAL(12,2) NOT NULL,
    Period NVARCHAR(32) NOT NULL,
    -- Monthly/Annual/PerEvent
    EffectiveFrom DATETIME2 NOT NULL,
    EffectiveTo DATETIME2 NULL,
    IsMandatory BIT NOT NULL DEFAULT 1,
    IsRefundable BIT NOT NULL DEFAULT 0,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Active',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
);
GO

ALTER TABLE dbo.ClubFeePolicies
ADD CONSTRAINT FK_ClubFeePolicies_ClubsTienPVK
FOREIGN KEY (ClubIDTienPVK) REFERENCES dbo.ClubsTienPVK(ClubIDTienPVK);
GO

/* ================================================================
   #2 - Members (Main) + MemberDocuments (Sub LITE)
================================================================ */
IF OBJECT_ID(N'dbo.MemberDocuments', N'U') IS NOT NULL DROP TABLE dbo.MemberDocuments;
IF OBJECT_ID(N'dbo.Members', N'U') IS NOT NULL DROP TABLE dbo.Members;
GO

CREATE TABLE dbo.Members
(
    MemberID INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    -- FK -> SystemAccount
    StudentCode NVARCHAR(20) NOT NULL UNIQUE,
    Faculty NVARCHAR(120) NULL,
    Major NVARCHAR(120) NULL,
    IntakeYear INT NULL,
    Gender NVARCHAR(16) NULL,
    DOB DATETIME2 NULL,
    Address NVARCHAR(250) NULL,
    AvatarUrl NVARCHAR(500) NULL,
    GPA DECIMAL(3,2) NULL,
    IsVerified BIT NOT NULL DEFAULT 0,
    IsGraduated BIT NOT NULL DEFAULT 0,
    JoinedAt DATETIME2 NULL,
    LeftAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL,
    CreatedBy NVARCHAR(50) NULL,
    ModifiedBy NVARCHAR(50) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0
);
GO

ALTER TABLE dbo.Members
ADD CONSTRAINT FK_Members_SystemAccount FOREIGN KEY(UserId)
REFERENCES dbo.SystemAccount(UserAccountID);
GO

/* LITE: 10 cột */
CREATE TABLE dbo.MemberDocuments
(
    MemberDocumentID INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    DocType NVARCHAR(50) NOT NULL,
    FileUrl NVARCHAR(500) NOT NULL,
    UploadedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    VerifiedAt DATETIME2 NULL,
    IsVerified BIT NOT NULL DEFAULT 0,
    IsExpired BIT NOT NULL DEFAULT 0,
    ExpiredAt DATETIME2 NULL,
    Note NVARCHAR(300) NULL
);
GO

ALTER TABLE dbo.MemberDocuments
ADD CONSTRAINT FK_MemberDocuments_Members FOREIGN KEY(MemberId) REFERENCES dbo.Members(MemberID);
GO

/* ================================================================
   #3 - JoinRequests (Main) + JoinRequestReviews (Sub LITE)
================================================================ */
IF OBJECT_ID(N'dbo.JoinRequestReviews', N'U') IS NOT NULL DROP TABLE dbo.JoinRequestReviews;
IF OBJECT_ID(N'dbo.JoinRequests', N'U') IS NOT NULL DROP TABLE dbo.JoinRequests;
GO

CREATE TABLE dbo.JoinRequests
(
    JoinRequestID INT IDENTITY(1,1) PRIMARY KEY,
    ClubIDTienPVK INT NOT NULL,
    MemberId INT NOT NULL,
    SubmittedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Message NVARCHAR(500) NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Pending',
    -- Pending/Approved/Rejected/Cancelled
    ApprovedAt DATETIME2 NULL,
    RejectedAt DATETIME2 NULL,
    ApprovedByUserId INT NULL,
    -- FK -> SystemAccount
    IsFeePaid BIT NOT NULL DEFAULT 0,
    RequiredFeeAmount DECIMAL(12,2) NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.JoinRequests
  ADD CONSTRAINT FK_JoinRequests_ClubsTienPVK   FOREIGN KEY(ClubIDTienPVK)   REFERENCES dbo.ClubsTienPVK(ClubIDTienPVK),
      CONSTRAINT FK_JoinRequests_Members FOREIGN KEY(MemberId) REFERENCES dbo.Members(MemberID),
      CONSTRAINT FK_JoinRequests_Users   FOREIGN KEY(ApprovedByUserId) REFERENCES dbo.SystemAccount(UserAccountID);
GO

/* LITE: 10 cột */
CREATE TABLE dbo.JoinRequestReviews
(
    ReviewID INT IDENTITY(1,1) PRIMARY KEY,
    JoinRequestId INT NOT NULL,
    ReviewerUserId INT NOT NULL,
    -- FK -> SystemAccount
    StepNo INT NOT NULL,
    Decision NVARCHAR(16) NOT NULL DEFAULT N'Pending',
    -- Pending/Approve/Reject
    Comment NVARCHAR(500) NULL,
    ReviewedAt DATETIME2 NULL,
    IsFinal BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.JoinRequestReviews
  ADD CONSTRAINT FK_JoinRequestReviews_JoinRequests FOREIGN KEY(JoinRequestId) REFERENCES dbo.JoinRequests(JoinRequestID),
      CONSTRAINT FK_JoinRequestReviews_SystemAccount  FOREIGN KEY(ReviewerUserId) REFERENCES dbo.SystemAccount(UserAccountID);
GO

/* ================================================================
   #4 - Activities (Main) + ActivityRegistrations (Sub LITE)
================================================================ */
IF OBJECT_ID(N'dbo.ActivityRegistrations', N'U') IS NOT NULL DROP TABLE dbo.ActivityRegistrations;
IF OBJECT_ID(N'dbo.Activities', N'U') IS NOT NULL DROP TABLE dbo.Activities;
GO

CREATE TABLE dbo.Activities
(
    ActivityID INT IDENTITY(1,1) PRIMARY KEY,
    ClubIDTienPVK INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    StartAt DATETIME2 NOT NULL,
    EndAt DATETIME2 NOT NULL,
    Location NVARCHAR(200) NULL,
    MaxParticipants INT NOT NULL DEFAULT 0,
    FeePerPerson DECIMAL(12,2) NOT NULL DEFAULT 0,
    IsPublic BIT NOT NULL DEFAULT 1,
    IsOnline BIT NOT NULL DEFAULT 0,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Planned',
    IsCancelled BIT NOT NULL DEFAULT 0,
    CancelReason NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.Activities
ADD CONSTRAINT FK_Activities_ClubsTienPVK FOREIGN KEY(ClubIDTienPVK) REFERENCES dbo.ClubsTienPVK(ClubIDTienPVK);
GO

/* LITE: 10 cột */
CREATE TABLE dbo.ActivityRegistrations
(
    RegistrationID INT IDENTITY(1,1) PRIMARY KEY,
    ActivityId INT NOT NULL,
    MemberId INT NOT NULL,
    RegisteredAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    IsCheckedIn BIT NOT NULL DEFAULT 0,
    CheckedInAt DATETIME2 NULL,
    IsPaid BIT NOT NULL DEFAULT 0,
    AmountPaid DECIMAL(12,2) NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.ActivityRegistrations
  ADD CONSTRAINT FK_ActivityRegs_Activities FOREIGN KEY(ActivityId) REFERENCES dbo.Activities(ActivityID),
      CONSTRAINT FK_ActivityRegs_Members    FOREIGN KEY(MemberId)   REFERENCES dbo.Members(MemberID);
GO

/* ================================================================
   #5 - FeeInvoices (Main) + FeeInvoiceLines (Sub LITE)
================================================================ */
IF OBJECT_ID(N'dbo.FeeInvoiceLines', N'U') IS NOT NULL DROP TABLE dbo.FeeInvoiceLines;
IF OBJECT_ID(N'dbo.FeeInvoices', N'U') IS NOT NULL DROP TABLE dbo.FeeInvoices;
GO

CREATE TABLE dbo.FeeInvoices
(
    FeeInvoiceID INT IDENTITY(1,1) PRIMARY KEY,
    ClubIDTienPVK INT NOT NULL,
    PayerMemberId INT NOT NULL,
    InvoiceNumber NVARCHAR(30) NOT NULL UNIQUE,
    IssuedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    DueAt DATETIME2 NULL,
    PaidAt DATETIME2 NULL,
    Subtotal DECIMAL(12,2) NOT NULL DEFAULT 0,
    DiscountAmount DECIMAL(12,2) NOT NULL DEFAULT 0,
    TotalAmount DECIMAL(12,2) NOT NULL DEFAULT 0,
    PaymentMethod NVARCHAR(32) NULL,
    ProviderTxnId NVARCHAR(100) NULL,
    IsPaid BIT NOT NULL DEFAULT 0,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Draft',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.FeeInvoices
  ADD CONSTRAINT FK_FeeInvoices_ClubsTienPVK   FOREIGN KEY(ClubIDTienPVK)        REFERENCES dbo.ClubsTienPVK(ClubIDTienPVK),
      CONSTRAINT FK_FeeInvoices_Members FOREIGN KEY(PayerMemberId) REFERENCES dbo.Members(MemberID);
GO

/* LITE: 10 cột */
CREATE TABLE dbo.FeeInvoiceLines
(
    FeeInvoiceLineID INT IDENTITY(1,1) PRIMARY KEY,
    FeeInvoiceId INT NOT NULL,
    FeePolicyId INT NULL,
    -- vẫn giữ link policy
    ItemName NVARCHAR(150) NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    UnitPrice DECIMAL(12,2) NOT NULL DEFAULT 0,
    LineTotal DECIMAL(12,2) NOT NULL DEFAULT 0,
    IsDiscountLine BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.FeeInvoiceLines
  ADD CONSTRAINT FK_FeeInvoiceLines_Invoices   FOREIGN KEY(FeeInvoiceId) REFERENCES dbo.FeeInvoices(FeeInvoiceID),
      CONSTRAINT FK_FeeInvoiceLines_FeePolicy  FOREIGN KEY(FeePolicyId)  REFERENCES dbo.ClubFeePolicies(FeePolicyId);
GO

/* ================================================================
   #6 - AttendanceSessions (Main) + AttendanceChecks (Sub LITE)
================================================================ */
IF OBJECT_ID(N'dbo.AttendanceChecks', N'U') IS NOT NULL DROP TABLE dbo.AttendanceChecks;
IF OBJECT_ID(N'dbo.AttendanceSessions', N'U') IS NOT NULL DROP TABLE dbo.AttendanceSessions;
GO

CREATE TABLE dbo.AttendanceSessions
(
    AttendanceSessionID INT IDENTITY(1,1) PRIMARY KEY,
    ClubIDTienPVK INT NOT NULL,
    ActivityId INT NULL,
    Title NVARCHAR(150) NOT NULL,
    SessionDate DATETIME2 NOT NULL,
    StartAt DATETIME2 NOT NULL,
    EndAt DATETIME2 NOT NULL,
    Location NVARCHAR(200) NULL,
    HostUserId INT NULL,
    -- FK -> SystemAccount
    IsMandatory BIT NOT NULL DEFAULT 0,
    Status NVARCHAR(30) NOT NULL DEFAULT N'Open',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.AttendanceSessions
  ADD CONSTRAINT FK_AttSes_ClubsTienPVK       FOREIGN KEY(ClubIDTienPVK)      REFERENCES dbo.ClubsTienPVK(ClubIDTienPVK),
      CONSTRAINT FK_AttSes_Activities  FOREIGN KEY(ActivityId)  REFERENCES dbo.Activities(ActivityID),
      CONSTRAINT FK_AttSes_SystemAccount FOREIGN KEY(HostUserId)  REFERENCES dbo.SystemAccount(UserAccountID);
GO

/* LITE: 11 cột */
CREATE TABLE dbo.AttendanceChecks
(
    AttendanceCheckID INT IDENTITY(1,1) PRIMARY KEY,
    AttendanceSessionId INT NOT NULL,
    MemberId INT NOT NULL,
    CheckedInAt DATETIME2 NULL,
    CheckedOutAt DATETIME2 NULL,
    IsLate BIT NOT NULL DEFAULT 0,
    IsExcused BIT NOT NULL DEFAULT 0,
    IsPresent BIT NOT NULL DEFAULT 0,
    IsAbsent BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.AttendanceChecks
  ADD CONSTRAINT FK_AttChecks_Session FOREIGN KEY(AttendanceSessionId) REFERENCES dbo.AttendanceSessions(AttendanceSessionID),
      CONSTRAINT FK_AttChecks_Member  FOREIGN KEY(MemberId)            REFERENCES dbo.Members(MemberID);
GO

/* ================================================================
   #7 - DisciplinaryCases (Main) + DisciplinaryActions (Sub LITE)
================================================================ */
IF OBJECT_ID(N'dbo.DisciplinaryActions', N'U') IS NOT NULL DROP TABLE dbo.DisciplinaryActions;
IF OBJECT_ID(N'dbo.DisciplinaryCases', N'U') IS NOT NULL DROP TABLE dbo.DisciplinaryCases;
GO

CREATE TABLE dbo.DisciplinaryCases
(
    CaseID INT IDENTITY(1,1) PRIMARY KEY,
    ClubIDTienPVK INT NOT NULL,
    MemberId INT NOT NULL,
    OpenedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    ReportedByUserId INT NULL,
    -- FK -> SystemAccount
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Severity NVARCHAR(16) NOT NULL,
    -- Low/Medium/High
    Status NVARCHAR(30) NOT NULL DEFAULT N'Open',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

ALTER TABLE dbo.DisciplinaryCases
  ADD CONSTRAINT FK_DiscCases_ClubsTienPVK   FOREIGN KEY(ClubIDTienPVK)   REFERENCES dbo.ClubsTienPVK(ClubIDTienPVK),
      CONSTRAINT FK_DiscCases_Members FOREIGN KEY(MemberId) REFERENCES dbo.Members(MemberID),
      CONSTRAINT FK_DiscCases_SystemAcc FOREIGN KEY(ReportedByUserId) REFERENCES dbo.SystemAccount(UserAccountID);
GO

/* LITE: 10 cột */
CREATE TABLE dbo.DisciplinaryActions
(
    ActionID INT IDENTITY(1,1) PRIMARY KEY,
    CaseId INT NOT NULL,
    ActionType NVARCHAR(32) NOT NULL,
    -- Warn/Fine/Suspend/Expel
    ActionDate DATETIME2 NOT NULL,
    IsNotified BIT NOT NULL DEFAULT 0,
    NotifiedAt DATETIME2 NULL,
    IsCompleted BIT NOT NULL DEFAULT 0,
    CompletedAt DATETIME2 NULL,
    Note NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

ALTER TABLE dbo.DisciplinaryActions
  ADD CONSTRAINT FK_DiscActions_Case  FOREIGN KEY(CaseId) REFERENCES dbo.DisciplinaryCases(CaseID);
GO

/* ================= CHECKS & INDEXES ================= */

-- Activities: EndAt >= StartAt
IF NOT EXISTS (SELECT 1
FROM sys.check_constraints
WHERE name = 'CK_Activities_Time')
BEGIN
    ALTER TABLE dbo.Activities
    ADD CONSTRAINT CK_Activities_Time CHECK (EndAt >= StartAt);
END
GO

-- FeeInvoices: DueAt >= IssuedAt
IF NOT EXISTS (SELECT 1
FROM sys.check_constraints
WHERE name = 'CK_FeeInvoices_Dates')
BEGIN
    ALTER TABLE dbo.FeeInvoices
    ADD CONSTRAINT CK_FeeInvoices_Dates CHECK (DueAt IS NULL OR DueAt >= IssuedAt);
END
GO

-- FeeInvoiceLines: positive numbers
IF NOT EXISTS (SELECT 1
FROM sys.check_constraints
WHERE name = 'CK_FeeInvoiceLines_Positive_LITE')
BEGIN
    ALTER TABLE dbo.FeeInvoiceLines
    ADD CONSTRAINT CK_FeeInvoiceLines_Positive_LITE 
    CHECK (Quantity > 0 AND UnitPrice >= 0 AND LineTotal >= 0);
END
GO

-- AttendanceChecks: cannot be both Present & Absent
IF NOT EXISTS (SELECT 1
FROM sys.check_constraints
WHERE name = 'CK_AttChecks_Presence')
BEGIN
    ALTER TABLE dbo.AttendanceChecks
    ADD CONSTRAINT CK_AttChecks_Presence CHECK (NOT (IsPresent = 1 AND IsAbsent = 1));
END
GO

-- Helpful indexes
IF NOT EXISTS (SELECT 1
FROM sys.indexes
WHERE name = 'IX_FeeInvoices_Club_Dates' AND object_id = OBJECT_ID('dbo.FeeInvoices'))
    CREATE INDEX IX_FeeInvoices_Club_Dates ON dbo.FeeInvoices(ClubIDTienPVK, IssuedAt) INCLUDE (TotalAmount, IsPaid);
GO

IF NOT EXISTS (SELECT 1
FROM sys.indexes
WHERE name = 'IX_ActivityRegs_Activity' AND object_id = OBJECT_ID('dbo.ActivityRegistrations'))
    CREATE INDEX IX_ActivityRegs_Activity ON dbo.ActivityRegistrations(ActivityId) INCLUDE (IsPaid);
GO

IF NOT EXISTS (SELECT 1
FROM sys.indexes
WHERE name = 'IX_Members_User' AND object_id = OBJECT_ID('dbo.Members'))
    CREATE INDEX IX_Members_User ON dbo.Members(UserId);
GO

/* ========================== END OF FILE ======================== */


---------------------------------------------------------
-- SYSTEMACCOUNT
---------------------------------------------------------
INSERT INTO dbo.SystemAccount
    (UserName, [Password], FullName, Email, Phone, EmployeeCode, RoleId, IsActive, CreatedDate)
VALUES
    ('admin', '123456', N'Administrator', 'admin@university.edu', '0123456789', 'EMP001', 1, 1, SYSDATETIME()),
    ('clubmanager1', '123456', N'Manager One', 'mgr1@university.edu', '0851234567', 'EMP002', 2, 1, SYSDATETIME()),
    ('reviewer1', '123456', N'Reviewer User', 'reviewer@university.edu', '0912345678', 'EMP003', 3, 1, SYSDATETIME());

---------------------------------------------------------
-- CLUB CATEGORIES
---------------------------------------------------------
INSERT INTO dbo.ClubCategoriesTienPVK
    (CategoryName, CategoryCode)
VALUES
    (N'Sports', 'SPT'),
    (N'Art & Music', 'ART'),
    (N'Technology', 'TEC'),
    (N'Volunteer', 'VOL');

---------------------------------------------------------
-- CLUBS
---------------------------------------------------------
INSERT INTO dbo.ClubsTienPVK
    (ClubCode, ClubName, CategoryIDTienPVK, Description, FoundedDate, Email, Phone, Address, ManagerUserId)
VALUES
    ('CLB_FOOTBALL', N'Football Club', 1, N'University football activities', '2018-09-01', 'football@uni.edu', '0901112222', N'Stadium zone A', 2),
    ('CLB_MUSIC', N'Music Club', 2, N'Activities for music lovers', '2019-03-15', 'music@uni.edu', '0902223333', N'Room B201', 2),
    ('CLB_TECH', N'Tech & Coding Club', 3, N'Programming, AI, IoT', '2020-01-10', 'tech@uni.edu', '0903334444', N'Lab C105', 1);

---------------------------------------------------------
-- CLUB FEE POLICIES
---------------------------------------------------------
INSERT INTO dbo.ClubFeePolicies
    (ClubIDTienPVK, FeeType, Amount, Period, EffectiveFrom)
VALUES
    (1, 'Membership', 200000, 'Annual', '2025-01-01'),
    (2, 'Monthly', 50000, 'Monthly', '2025-01-01'),
    (3, 'Event', 30000, 'PerEvent', '2025-01-01');

---------------------------------------------------------
-- MEMBERS
---------------------------------------------------------
INSERT INTO dbo.Members
    (UserId, StudentCode, Faculty, Major, IntakeYear, Gender, DOB, Address, GPA, JoinedAt)
VALUES
    (1, 'SE150001', N'IT Faculty', N'Software Engineering', 2021, 'Male', '2003-03-03', N'District 1', 3.2, SYSDATETIME()),
    (2, 'SE150002', N'Business Faculty', N'Marketing', 2020, 'Female', '2002-07-21', N'District 7', 3.6, SYSDATETIME()),
    (3, 'SE150003', N'IT Faculty', N'AI Engineering', 2022, 'Male', '2004-01-11', N'Thu Duc City', 3.8, SYSDATETIME());

---------------------------------------------------------
-- MEMBER DOCUMENTS
---------------------------------------------------------
INSERT INTO dbo.MemberDocuments
    (MemberId, DocType, FileUrl)
VALUES
    (1, 'StudentID', 'https://example.com/docs/1_id.png'),
    (2, 'StudentID', 'https://example.com/docs/2_id.png'),
    (3, 'StudentID', 'https://example.com/docs/3_id.png');

---------------------------------------------------------
-- JOIN REQUESTS
---------------------------------------------------------
INSERT INTO dbo.JoinRequests
    (ClubIDTienPVK, MemberId, Message, RequiredFeeAmount)
VALUES
    (1, 1, N'I want to join the football club.', 200000),
    (2, 2, N'I love music and want to join.', 50000),
    (3, 3, N'Passionate about AI & coding.', 0);

---------------------------------------------------------
-- JOIN REQUEST REVIEWS
---------------------------------------------------------
INSERT INTO dbo.JoinRequestReviews
    (JoinRequestId, ReviewerUserId, StepNo, Decision)
VALUES
    (1, 3, 1, 'Approve'),
    (2, 3, 1, 'Approve'),
    (3, 1, 1, 'Approve');

---------------------------------------------------------
-- ACTIVITIES
---------------------------------------------------------
INSERT INTO dbo.Activities
    (ClubIDTienPVK, Title, Description, StartAt, EndAt, Location, MaxParticipants, FeePerPerson)
VALUES
    (1, N'Football Tournament 2025', N'Annual football competition', '2025-03-10 08:00', '2025-03-10 17:00', N'Stadium A', 100, 0),
    (2, N'Music Live Show', N'Performance event', '2025-04-05 18:00', '2025-04-05 21:00', N'Auditorium B', 80, 30000),
    (3, N'AI Bootcamp', N'Deep learning workshop', '2025-05-12 09:00', '2025-05-12 16:00', N'Lab C105', 40, 50000);

---------------------------------------------------------
-- ACTIVITY REGISTRATIONS
---------------------------------------------------------
INSERT INTO dbo.ActivityRegistrations
    (ActivityId, MemberId, IsPaid, AmountPaid)
VALUES
    (1, 1, 1, 0),
    (2, 2, 1, 30000),
    (3, 3, 1, 50000);

---------------------------------------------------------
-- FEE INVOICES
---------------------------------------------------------
INSERT INTO dbo.FeeInvoices
    (ClubIDTienPVK, PayerMemberId, InvoiceNumber, Subtotal, TotalAmount, IsPaid)
VALUES
    (1, 1, 'INV001', 200000, 200000, 1),
    (2, 2, 'INV002', 50000, 50000, 1),
    (3, 3, 'INV003', 30000, 30000, 1);

---------------------------------------------------------
-- FEE INVOICE LINES
---------------------------------------------------------
INSERT INTO dbo.FeeInvoiceLines
    (FeeInvoiceId, ItemName, Quantity, UnitPrice, LineTotal)
VALUES
    (1, N'Membership Fee', 1, 200000, 200000),
    (2, N'Monthly Fee', 1, 50000, 50000),
    (3, N'Event Registration Fee', 1, 30000, 30000);

---------------------------------------------------------
-- ATTENDANCE SESSIONS
---------------------------------------------------------
INSERT INTO dbo.AttendanceSessions
    (ClubIDTienPVK, ActivityId, Title, SessionDate, StartAt, EndAt, Location, HostUserId)
VALUES
    (1, 1, N'Training Session', '2025-03-01', '2025-03-01 08:00', '2025-03-01 10:00', N'Field 1', 2),
    (2, 2, N'Rehearsal', '2025-04-01', '2025-04-01 17:00', '2025-04-01 19:00', N'Room B201', 2);

---------------------------------------------------------
-- ATTENDANCE CHECKS
---------------------------------------------------------
INSERT INTO dbo.AttendanceChecks
    (AttendanceSessionId, MemberId, IsPresent)
VALUES
    (1, 1, 1),
    (2, 2, 1);

---------------------------------------------------------
-- DISCIPLINARY CASES
---------------------------------------------------------
INSERT INTO dbo.DisciplinaryCases
    (ClubIDTienPVK, MemberId, ReportedByUserId, Title, Description, Severity)
VALUES
    (1, 1, 2, N'Late Attendance', N'Member arrived 30 minutes late', 'Low'),
    (2, 2, 3, N'Missing Rehearsal', N'Member skipped rehearsal without notice', 'Medium');

---------------------------------------------------------
-- DISCIPLINARY ACTIONS
---------------------------------------------------------
INSERT INTO dbo.DisciplinaryActions
    (CaseId, ActionType, ActionDate, Note)
VALUES
    (1, 'Warn', SYSDATETIME(), N'Verbal warning issued'),
    (2, 'Warn', SYSDATETIME(), N'First official warning');
