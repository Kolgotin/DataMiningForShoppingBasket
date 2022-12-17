CREATE TABLE [dbo].[Products](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [ProductName] [nvarchar](32) NOT NULL,
	    [ProductTypeId] [int] NULL,
	    [Cost] [decimal](9, 2) NULL,
	    [FractionalAllowed] bit NOT NULL CONSTRAINT [DF_Products_FractionalAllowed] default (0),
	    [WarehouseQuantity] decimal(18,4) NOT NULL CONSTRAINT [DF_Products_WarehouseQuantity] default (0),
        CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Products_ProductTypes] FOREIGN KEY([ProductTypeId]) REFERENCES [dbo].[ProductTypes] ([Id])
    ) ON [PRIMARY]
GO
