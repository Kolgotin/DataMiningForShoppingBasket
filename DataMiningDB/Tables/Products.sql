CREATE TABLE [dbo].[Products](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [ProductName] [nvarchar](32) NOT NULL,
	    [ProductTypeid] [int] NULL,
	    [Cost] [decimal](9, 2) NULL,
	    [FractionalAllowed] bit NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Products_ProductTypes] FOREIGN KEY([ProductTypeid]) REFERENCES [dbo].[ProductTypes] ([Id])
    ) ON [PRIMARY]
GO
