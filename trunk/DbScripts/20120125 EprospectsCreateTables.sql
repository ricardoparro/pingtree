
Drop Table LeadRejected
Drop Table LeadBought
Drop Table Lead
Drop Table Billing
Drop Table BillingType
Drop Table Status
Drop table Affiliate
Drop Table Seller
Drop Table Buyer
Drop Table Log






--Buyer table

CREATE TABLE dbo.Buyer
	(
	BuyerId int NOT NULL IDENTITY (1, 1),
	Name varchar(150) NOT NULL,
	Username varchar(150) NOT NULL,
	Password varchar(50) NOT NULL,
	Alias varchar(50) NULL,
	EmailAddress varchar(150) NOT NULL,
	Mobile varchar(150) NULL,
	RefKey varchar(50) NULL,
	FixedAmount float NULL,
	Country varchar(100) NOT NULL,
	IntegrationUrl varchar(100) NOT NULL,
	Active bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Buyer ADD CONSTRAINT
	PK_Buyer PRIMARY KEY CLUSTERED 
	(
	BuyerId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO


--Seller table
	
	CREATE TABLE dbo.Seller
	(
	SellerId int NOT NULL IDENTITY (1, 1),
	SellerName varchar(150) NOT NULL,
	Country varchar(100) NOT NULL,
	Active bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Seller ADD CONSTRAINT
	PK_Seller PRIMARY KEY CLUSTERED 
	(
	SellerId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO


--Create Affiliate table
CREATE TABLE dbo.Affiliate
	(
	AffiliateId int NOT NULL IDENTITY (1, 1),
	AffiliateIdRef varchar(50) NOT NULL,
	AffiliateName varchar(100) NOT NULL,
	ContactName varchar(150) NOT NULL,
	ContactEmail varchar(150) NOT NULL,
	ContactPhone varchar(150) NOT NULL,
	AlternatePhone varchar(150) NULL,
	CompanyName varchar(150) NOT NULL,
	Address1 varchar(200) NOT NULL,
	Address2 varchar(150) NULL,
	Town varchar(50) NULL,
	Postcode varchar(50) NULL,
	County varchar(150) NULL,
	BankName varchar(150) NULL,
	Sortcode varchar(50) NULL,
	AccountNumber varchar(50) NULL,
	PaymentType varchar(50) NULL,
	Username varchar(150) NOT NULL,
	Password varchar(150) NOT NULL,
	SellerId int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Affiliate ADD CONSTRAINT
	PK_Table_1 PRIMARY KEY CLUSTERED 
	(
	AffiliateId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
ALTER TABLE dbo.Affiliate ADD CONSTRAINT
	FK_Seller_Affiliate FOREIGN KEY
	(
	SellerId
	) REFERENCES dbo.Seller
	(
	SellerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO
--BillingType Table

CREATE TABLE dbo.BillingType
	(
	BillingTypeId int NOT NULL IDENTITY (1, 1),
	Description varchar(150) NOT NULL,
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.BillingType ADD CONSTRAINT
	PK_BillingType PRIMARY KEY CLUSTERED 
	(
	BillingTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

--BillingTable
CREATE TABLE dbo.Billing
	(
	BillingId int NOT NULL IDENTITY (1, 1),
	BillingTypeId int NOT NULL,
	SellerId int NOT NULL,
	BuyerId int NOT NULL,
	Value decimal(18, 0) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Billing ADD CONSTRAINT
	PK_Billing PRIMARY KEY CLUSTERED 
	(
	BillingId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
	ALTER TABLE dbo.Billing ADD CONSTRAINT
	FK_Billing_SellerId FOREIGN KEY
	(
	SellerId
	) REFERENCES dbo.Seller
	(
	SellerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
	ALTER TABLE dbo.Billing ADD CONSTRAINT
	FK_Billing_BuyerId FOREIGN KEY
	(
	BuyerId
	) REFERENCES dbo.Buyer
	(
	BuyerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
	ALTER TABLE dbo.Billing ADD CONSTRAINT
	FK_Billing_BillingType FOREIGN KEY
	(
	BillingTypeId
	) REFERENCES dbo.BillingType
	(
	BillingTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO


--Status table
CREATE TABLE dbo.Status
	(
	StatusId int NOT NULL IDENTITY (1, 1),
	Description varchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Status ADD CONSTRAINT
	PK_Status PRIMARY KEY CLUSTERED 
	(
	StatusId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO



--Lead Table

CREATE TABLE dbo.Lead
	(
	LeadId int NOT NULL IDENTITY (1, 1),
	Gender varchar(50) NULL,
	Title varchar(20) NULL,
	Forename varchar(150) NOT NULL,
	Surname varchar(150) NOT NULL,
	Dob datetime NOT NULL,
	AddressLine1 varchar(250) NULL,
	AddressLine2 varchar(250) NOT NULL,
	Town varchar(50) NULL,
	County varchar(50) NOT NULL,
	Postcode varchar(10) NOT NULL,
	HomeStatus varchar(20) NOT NULL,
	MonthsAtAddress int NULL,
	EmployersName varchar(50) NOT NULL,
	MonthsAtEmployer int NULL,
	IncomeFrequency varchar(50) NOT NULL,
	MonthlyIncome int NOT NULL,
	EmploymentStatus varchar(50) NOT NULL,
	DirectDeposit bit NOT NULL,
	BankAccountNumber varchar(10) NOT NULL,
	BankSortcode varchar(10) NOT NULL,
	HomePhone varchar(150) NULL,
	WorkPhone varchar(150) NULL,
	MobilePhone varchar(150) NULL,
	EmailAddress varchar(150) NOT NULL,
	ApplicationDate datetime NOT NULL,
	Source varchar(150) NULL,
	SubSource varchar(150) NULL,
	IpAddress varchar(150) NULL,
	Country varchar(100) NULL,
	SellerId int NOT NULL,
	LoanAmount int NOT NULL,
	DebitCardType varchar(50) NULL,
	PaybackDate datetime NOT NULL,
	NexPaybackDate datetime NULL,
	StatusId int NOT NULL
	)
	ALTER TABLE dbo.Lead ADD CONSTRAINT
	PK_Lead PRIMARY KEY CLUSTERED 
	(
	LeadId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Lead ADD CONSTRAINT
	FK_Lead_Seller FOREIGN KEY
	(
	SellerId
	) REFERENCES dbo.Seller
	(
	SellerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
	ALTER TABLE dbo.Lead ADD CONSTRAINT
	FK_Lead_Status FOREIGN KEY
	(
	StatusId
	) REFERENCES dbo.Status
	(
	StatusId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO


-- LeadRejected table

CREATE TABLE dbo.LeadRejected
	(
	LeadRejectedId int NOT NULL IDENTITY (1, 1),
	LeadId int NOT NULL,
	Reason varchar(150) NULL,
	BuyerId int NOT NULL,
	BuyerLeadReference varchar(200) NULL,
	TimeRejected datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.LeadRejected ADD CONSTRAINT
	PK_LeadRejected PRIMARY KEY CLUSTERED 
	(
	LeadRejectedId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.LeadRejected ADD CONSTRAINT
	FK_Lead_LeadRejected FOREIGN KEY
	(
	LeadId
	) REFERENCES dbo.Lead
	(
	LeadId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.LeadRejected ADD CONSTRAINT
	FK_Buyer_LeadRejected FOREIGN KEY
	(
	BuyerId
	) REFERENCES dbo.Buyer
	(
	BuyerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO

-- LeadBought table

CREATE TABLE dbo.LeadBought
	(
	LeadBoughtId int NOT NULL IDENTITY (1, 1),
	LeadId int NOT NULL,
	BuyerId int NOT NULL,
	Amount smallmoney NOT NULL,
	ExchangeRate decimal(18, 0) NULL,
	BuyerLeadReference varchar(200) NULL,
	RedirectionLink varchar(200) NOT NULL,
	TimeBought datetime NOT NULL,
	BillingValue decimal(18, 0) NOT NULL, --This billing value is the percentage or fixed payout at The time of the sale 
	BillingTypeId int -- This is the billing type at the time of the sale
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.LeadBought ADD CONSTRAINT
	PK_LeadBought PRIMARY KEY CLUSTERED 
	(
	LeadBoughtId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.LeadBought ADD CONSTRAINT
	FK_Lead_LeadBought FOREIGN KEY
	(
	LeadId
	) REFERENCES dbo.Lead
	(
	LeadId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.LeadBought ADD CONSTRAINT
	FK_Buyer_LeadBought FOREIGN KEY
	(
	BuyerId
	) REFERENCES dbo.Buyer
	(
	BuyerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.LeadBought ADD CONSTRAINT
	FK_BillingType_LeadBought FOREIGN KEY
	(
	BillingTypeId
	) REFERENCES dbo.BillingType
	(
	BillingTypeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

--Log table for Log4NET

CREATE TABLE [dbo].[Log] (

    [Id] [int] IDENTITY (1, 1) NOT NULL,

    [Date] [datetime] NOT NULL,

    [Level] [varchar] (50) NOT NULL,

    [Logger] [varchar] (255) NOT NULL,

    [Message] [varchar] (4000) NOT NULL,

    [Exception] [varchar] (2000) NULL,
    [BuyerId] [int] NULL,
    [LeadId] [int] NULL

)
ALTER TABLE dbo.Log ADD CONSTRAINT
	PK_Log PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
--INSERT DictionaryData Status

INSERT INTO [dbo].[Status]
           ([Description])
     VALUES
           ('New')
GO

INSERT INTO [dbo].[Status]
           ([Description])
     VALUES
           ('Accepted')
GO

INSERT INTO [dbo].[Status]
           ([Description])
     VALUES
           ('Invalid')
GO

INSERT INTO [dbo].[Status]
           ([Description])
     VALUES
           ('Rejected')
GO

--INSERT BillingTypes

INSERT INTO [dbo].[BillingType]
           ([Description])
     VALUES
           ('Percentage')
GO


INSERT INTO [dbo].[BillingType]
           ([Description])
     VALUES
           ('FixedPrice')
GO
