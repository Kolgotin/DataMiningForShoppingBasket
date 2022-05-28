CREATE TABLE [dbo].[Discounts](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [DiscountName] [nvarchar](32) NOT NULL,
	    [DiscountDescription] [varchar](max) NOT NULL,
	    [StartDate] [datetime] NOT NULL,
	    [FinishDate] [datetime] NOT NULL,
	    [ProductId] [int] NOT NULL,
	    [Quantity] [int] NOT NULL,
	    [DiscountCost] decimal(9, 2) NOT NULL,
        CONSTRAINT [PK_Discounts] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Products_Discounts] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
