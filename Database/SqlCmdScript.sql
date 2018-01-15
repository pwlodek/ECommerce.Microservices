---
--- Products Database
---
CREATE DATABASE [Ecommerce.Products];
GO

USE [Ecommerce.Products];
GO

CREATE TABLE [dbo].[Products](
    [ProductID] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) NULL,
    [Quantity] [int] NULL,
    [Price] [float] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
    [ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO

INSERT INTO [dbo].[Products] VALUES ('Carrots', 15, 153)
INSERT INTO [dbo].[Products] VALUES ('Meat', 100, 200)
INSERT INTO [dbo].[Products] VALUES ('Beans', 150, 300)
GO

---
--- Customers Database
---
CREATE DATABASE [Ecommerce.Customers];
GO

USE [Ecommerce.Customers];
GO

CREATE TABLE [dbo].[Customers](
    [CustomerID] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
    [CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO

INSERT INTO [dbo].[Customers] VALUES ('John', 'Kowalski')
INSERT INTO [dbo].[Customers] VALUES ('Roman', 'Polanski')
INSERT INTO [dbo].[Customers] VALUES ('Jeff', 'Bridges')
GO

---
--- Sales Database
---
CREATE DATABASE [Ecommerce.Sales];
GO

USE [Ecommerce.Sales];
GO

CREATE TABLE [dbo].[Orders](
    [OrderId] [int] IDENTITY(1,1) NOT NULL,
    [CustomerId] [int] NOT NULL,
    [Status] [int] NOT NULL,
    [Total] [float] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
    [OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
GO

CREATE TABLE [dbo].[OrderItems](
    [OrderItemId] [int] IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
    [Name] [nvarchar](max) NULL,
    [Quantity] [int] NULL,
    [Price] [float] NULL,

CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED ([OrderItemId] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId])
)
GO
 