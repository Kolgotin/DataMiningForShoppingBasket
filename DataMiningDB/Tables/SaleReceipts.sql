CREATE TABLE [dbo].[SaleReceipts](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [SaleDateTime] [datetime] NOT NULL,
	    [CashierId] [int] NOT NULL,
	    [ClientId] [int] NULL,
        CONSTRAINT [PK_SaleReceipts] PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY],
        CONSTRAINT [FK_SaleReceipts_Clients] FOREIGN KEY([ClientId]) REFERENCES [dbo].[Clients] ([Id]),
        CONSTRAINT [FK_SaleReceipts_Users] FOREIGN KEY([CashierId]) REFERENCES [dbo].[Users] ([Id])
    ) ON [PRIMARY]
GO
