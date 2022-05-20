--Создание таблиц

IF NOT EXISTS (select * from sys.tables where name='UserTypes')
BEGIN
    CREATE TABLE [dbo].[UserTypes](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [UserTypeName] [nvarchar](16) NOT NULL,
        CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='ProductTypes')
BEGIN
    CREATE TABLE [dbo].[ProductTypes](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [ProductTypeName] [nvarchar](32) NOT NULL,
	    [ProductTypeDescription] [nvarchar](max) NULL,
        CONSTRAINT [PK_ProductTypes] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='Users')
BEGIN
    CREATE TABLE [dbo].[Users](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [UserName] [nvarchar](32) NOT NULL,
	    [UserPassword] [nvarchar](16) NOT NULL,
	    [UserTypeId] [int] NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Users_UserTypes] FOREIGN KEY([UserTypeId]) REFERENCES [dbo].[UserTypes] ([Id])
    ) ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='Clients')
BEGIN
    CREATE TABLE [dbo].[Clients](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [Name] [nvarchar](32) NOT NULL,
	    [Gender] [nvarchar](1) NULL,
	    [BirthDate] [datetime] NULL,
	    [Email] [nvarchar](64) NULL,
	    [Phone] [nvarchar](16) NULL,
        CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='Products')
BEGIN
    CREATE TABLE [dbo].[Products](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [ProductName] [nvarchar](32) NOT NULL,
	    [ProductTypeid] [int] NULL,
	    [Cost] [decimal](9, 2) NULL,
	    [FractionalAllowed] bit NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Products_ProductTypes] FOREIGN KEY([ProductTypeid]) REFERENCES [dbo].[ProductTypes] ([Id])
    ) ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='Discounts')
BEGIN
    CREATE TABLE [dbo].[Discounts](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [DiscountName] [nvarchar](32) NOT NULL,
	    [DiscountDescription] [varchar](max) NOT NULL,
	    [StartDate] [datetime] NOT NULL,
	    [FinishDate] [datetime] NOT NULL,
	    [ProductId] [int] NOT NULL,
	    [Quantity] [int] NOT NULL,
	    [DiscountCost] decimal(9, 0) NOT NULL,
        CONSTRAINT [PK_Discounts] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Products_Discounts] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='Warehouse')
BEGIN
    CREATE TABLE [dbo].[Warehouse](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [ProductId] [int] NOT NULL,
	    [IncomeQuantity] [int] NOT NULL,
        CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Warehouse_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
    ) ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='SaleReceipts')
BEGIN
    CREATE TABLE [dbo].[SaleReceipts](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [SaleDateTime] [datetime] NOT NULL,
	    [CashierId] [int] NOT NULL,
	    [ClientId] [int] NULL,
        CONSTRAINT [PK_SaleReceipts] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY],
        CONSTRAINT [FK_SaleReceipts_Clients] FOREIGN KEY([ClientId]) REFERENCES [dbo].[Clients] ([Id]),
        CONSTRAINT [FK_SaleReceipts_Users] FOREIGN KEY([CashierId]) REFERENCES [dbo].[Users] ([Id])
    ) ON [PRIMARY]
END
GO


IF NOT EXISTS (select * from sys.tables where name='SaleRows')
BEGIN
    CREATE TABLE [dbo].[SaleRows](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [SaleId] [int] NOT NULL,
	    [ProductId] [int] NOT NULL,
	    [Quantity] decimal(18,4) NOT NULL,
        CONSTRAINT [PK_SaleRows] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_SaleRows_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]),
        CONSTRAINT [FK_SaleRows_SaleReceipts] FOREIGN KEY([SaleId]) REFERENCES [dbo].[SaleReceipts] ([Id])
    ) ON [PRIMARY]
END
GO

