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