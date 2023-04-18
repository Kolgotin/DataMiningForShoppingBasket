CREATE TABLE [dbo].[FocusProducts](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [varchar](max) NOT NULL,
		[StartDate] [datetime] NOT NULL,
		[FinishDate] [datetime] NOT NULL,
		[ProductId] [int] NOT NULL,
		[DiscountCost] [decimal](9, 2) NOT NULL,
		CONSTRAINT [PK_FocusProducts] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
		CONSTRAINT [FK_Products_FocusProducts] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
