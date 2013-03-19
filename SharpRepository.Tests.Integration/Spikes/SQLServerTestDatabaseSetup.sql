USE [SharpRepositoryTest]
GO
/****** Object:  Table [dbo].[SomeType]    Script Date: 03/18/2013 22:13:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SomeType](
	[SomeTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SomeType] PRIMARY KEY CLUSTERED 
(
	[SomeTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SomeObject]    Script Date: 03/18/2013 22:13:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SomeObject](
	[SomeObjectId] [int] IDENTITY(1,1) NOT NULL,
	[SomeTypeId] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SomeObject] PRIMARY KEY CLUSTERED 
(
	[SomeObjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MainItem]    Script Date: 03/18/2013 22:13:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MainItem](
	[MainItemId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SomeTypeId] [int] NOT NULL,
 CONSTRAINT [PK_MainItem] PRIMARY KEY CLUSTERED 
(
	[MainItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_MainItem_SomeType]    Script Date: 03/18/2013 22:13:55 ******/
ALTER TABLE [dbo].[MainItem]  WITH CHECK ADD  CONSTRAINT [FK_MainItem_SomeType] FOREIGN KEY([SomeTypeId])
REFERENCES [dbo].[SomeType] ([SomeTypeId])
GO
ALTER TABLE [dbo].[MainItem] CHECK CONSTRAINT [FK_MainItem_SomeType]
GO
/****** Object:  ForeignKey [FK_SomeObject_SomeType]    Script Date: 03/18/2013 22:13:55 ******/
ALTER TABLE [dbo].[SomeObject]  WITH CHECK ADD  CONSTRAINT [FK_SomeObject_SomeType] FOREIGN KEY([SomeTypeId])
REFERENCES [dbo].[SomeType] ([SomeTypeId])
GO
ALTER TABLE [dbo].[SomeObject] CHECK CONSTRAINT [FK_SomeObject_SomeType]
GO


insert into SomeType (Title)
select 'Type 1'
union
select 'Type 2'
union
select 'Type 3'
union
select 'Type 4'

insert into SomeObject (SomeTypeId, Title)
select 1, 'Object 1'
union
select 1, 'Object 2'
union
select 1, 'Object 3'
union
select 2, 'Object 4'
union
select 2, 'Object 5'
union
select 3, 'Object 6'


insert into MainItem (Name, SomeTypeId)
select 'Item 1', 1
union
select 'Item 2', 1
union
select 'Item 3', 2
union
select 'Item 4', 2
union
select 'Item 5', 3
union
select 'Item 6', 3
union
select 'Item 7', 3