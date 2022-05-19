CREATE TABLE [dbo].[Warehouse](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [ProductId] [int] NOT NULL,
	    [IncomeQuantity] [int] NOT NULL,
        CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Warehouse_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
    ) ON [PRIMARY]
GO
