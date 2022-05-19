    CREATE TABLE [dbo].[Clients](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [Name] [nvarchar](32) NOT NULL,
	    [Gender] [nvarchar](1) NULL,
	    [BirthDate] [datetime] NULL,
	    [Email] [nvarchar](64) NULL,
	    [Phone] [nvarchar](16) NULL,
        CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
    ) ON [PRIMARY]
GO
