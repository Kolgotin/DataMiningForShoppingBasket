CREATE TABLE [dbo].[ProductTypes](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [ProductTypeName] [nvarchar](32) NOT NULL,
	    [ProductTypeDescription] [nvarchar](max) NULL,
        CONSTRAINT [PK_ProductTypes] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
