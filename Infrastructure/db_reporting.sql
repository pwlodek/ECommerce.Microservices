CREATE TABLE [dbo].[Orders](
    [OrderId] [int] NOT NULL,
    [CustomerId] [int] NOT NULL,
    [FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
    [Status] [int] NOT NULL,
    [Total] [float] NOT NULL,
CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
    [OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
GO