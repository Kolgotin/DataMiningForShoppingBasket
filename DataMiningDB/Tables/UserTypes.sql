CREATE TABLE [dbo].[UserTypes](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [UserTypeName] [nvarchar](16) NOT NULL,
        CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY]
    ) ON [PRIMARY]
GO
