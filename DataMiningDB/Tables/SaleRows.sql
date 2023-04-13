CREATE TABLE [dbo].[SaleRows](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [SaleId] [int] NOT NULL,
	    [ProductId] [int] NOT NULL,
	    [Quantity] decimal(18,4) NOT NULL,
		[TotalCost] [decimal](9, 2) NOT NULL,
        CONSTRAINT [PK_SaleRows] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_SaleRows_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]),
        CONSTRAINT [FK_SaleRows_SaleReceipts] FOREIGN KEY([SaleId]) REFERENCES [dbo].[SaleReceipts] ([Id])
    ) ON [PRIMARY]
GO
