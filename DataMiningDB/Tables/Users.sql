CREATE TABLE [dbo].[Users](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [UserName] [nvarchar](32) NOT NULL,
	    [UserPassword] [nvarchar](16) NOT NULL,
	    [UserTypeId] [int] NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id]) ON [PRIMARY],
        CONSTRAINT [FK_Users_UserTypes] FOREIGN KEY([UserTypeId]) REFERENCES [dbo].[UserTypes] ([Id])
    ) ON [PRIMARY]
GO
