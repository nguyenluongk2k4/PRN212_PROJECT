USE [master]
GO
/****** Object:  Database [ChickenPRN]    Script Date: 01/04/2025 12:44:15 ******/
CREATE DATABASE [ChickenPRN]
GO
ALTER DATABASE [ChickenPRN] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ChickenPRN].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ChickenPRN] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ChickenPRN] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ChickenPRN] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ChickenPRN] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ChickenPRN] SET ARITHABORT OFF 
GO
ALTER DATABASE [ChickenPRN] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ChickenPRN] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ChickenPRN] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ChickenPRN] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ChickenPRN] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ChickenPRN] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ChickenPRN] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ChickenPRN] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ChickenPRN] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ChickenPRN] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ChickenPRN] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ChickenPRN] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ChickenPRN] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ChickenPRN] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ChickenPRN] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ChickenPRN] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ChickenPRN] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ChickenPRN] SET RECOVERY FULL 
GO
ALTER DATABASE [ChickenPRN] SET  MULTI_USER 
GO
ALTER DATABASE [ChickenPRN] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ChickenPRN] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ChickenPRN] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ChickenPRN] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ChickenPRN] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ChickenPRN] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ChickenPRN', N'ON'
GO
ALTER DATABASE [ChickenPRN] SET QUERY_STORE = ON
GO
ALTER DATABASE [ChickenPRN] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ChickenPRN]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[account_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
	[role_id] [int] NULL,
	[fullname] [nvarchar](255) NULL,
 CONSTRAINT [PK_account_id] PRIMARY KEY CLUSTERED 
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[id] [int] NOT NULL,
	[name] [nvarchar](50) NULL,
	[describe] [nvarchar](max) NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Combo]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Combo](
	[ComboID] [int] IDENTITY(1,1) NOT NULL,
	[ComboName] [nvarchar](50) NULL,
	[Status] [int] NULL,
	[Price] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ComboID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComboDetail]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComboDetail](
	[ComboDetailID] [int] IDENTITY(1,1) NOT NULL,
	[FoodID] [int] NULL,
	[Amount] [int] NULL,
	[ComboID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ComboDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenditure]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenditure](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Executor] [nvarchar](50) NULL,
	[Cost] [decimal](18, 2) NULL,
	[SupplierOrderId] [int] NULL,
	[date] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[rate] [int] NULL,
	[Content] [nvarchar](max) NULL,
	[TimeFeedback] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Food]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Food](
	[FoodID] [int] IDENTITY(1,1) NOT NULL,
	[FoodName] [nvarchar](50) NULL,
	[FoodType] [int] NULL,
	[Price] [float] NULL,
	[Table] [int] NULL,
	[Status] [int] NULL,
	[image] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[FoodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetailCombo]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetailCombo](
	[orderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[orderID] [int] NULL,
	[ComboID] [int] NULL,
	[Amount] [int] NULL,
	[Price] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[orderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetailFood]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetailFood](
	[orderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[orderID] [int] NULL,
	[FoodID] [int] NULL,
	[Amount] [int] NULL,
	[Price] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[orderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderTable]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderTable](
	[orderID] [int] IDENTITY(1,1) NOT NULL,
	[customerName] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[isPaid] [bit] NULL,
	[Done] [bit] NULL,
	[Total] [float] NULL,
	[Address] [nvarchar](255) NULL,
	[Shipping] [bit] NULL,
	[PhoneNumber] [varchar](11) NULL,
PRIMARY KEY CLUSTERED 
(
	[orderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionId] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[id] [nchar](10) NOT NULL,
	[name] [nvarchar](max) NULL,
	[quantity] [int] NULL,
	[price] [money] NULL,
	[releaseDate] [date] NULL,
	[describe] [nvarchar](max) NULL,
	[image] [nvarchar](max) NULL,
	[cid] [int] NULL,
 CONSTRAINT [PK_Phone] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[role_id] [int] NOT NULL,
	[role_name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissions](
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PhoneNumber] [varchar](11) NULL,
	[Address] [nvarchar](250) NULL,
	[Email] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierOrder]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[OrderDate] [date] NULL,
	[DeliverDate] [date] NULL,
	[IsPaid] [bit] NULL,
	[Total] [decimal](18, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierOrderDetail]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierOrderDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierOrderId] [int] NOT NULL,
	[ProductName] [nvarchar](100) NULL,
	[Amount] [float] NULL,
	[UnitPrice] [float] NULL,
	[CalculationUnit] [nvarchar](25) NULL,
 CONSTRAINT [PK__Supplier__3214EC07F140A67E] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeOfFood]    Script Date: 01/04/2025 12:44:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeOfFood](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Accounts] ON 

INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (1, N'admin_nam', N'password123', 1, N'Nguy?n Van Nam')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (2, N'admin_lan', N'password123', 1, N'Tr?n Th? Lan')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (3, N'staff_hai', N'password123', 2, N'Ph?m Van H?i')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (4, N'staff_hoa', N'password123', 2, N'Lê Th? Hoa')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (5, N'staff_phong', N'password123', 2, N'Ð?ng Minh Phong')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (6, N'cooker_quang', N'password123', 3, N'Hoàng Van Quang')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (7, N'cooker_mai', N'password123', 3, N'Bùi Th? Mai')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (8, N'customer_dung', N'password123', 4, N'Ð? M?nh Dung')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (9, N'customer_anh', N'password123', 4, N'Nguy?n Hoàng Anh')
INSERT [dbo].[Accounts] ([account_id], [username], [password], [role_id], [fullname]) VALUES (10, N'customer_ly', N'password123', 4, N'Tr?nh Thu? Ly')
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO
INSERT [dbo].[Categories] ([id], [name], [describe]) VALUES (1, N'iPhone', N'Đẹp rực rỡ rạng ngời')
INSERT [dbo].[Categories] ([id], [name], [describe]) VALUES (2, N'Samsung', N'thời thượng')
INSERT [dbo].[Categories] ([id], [name], [describe]) VALUES (3, N'Oppo', N'dành cho sinh viên sành điệu')
INSERT [dbo].[Categories] ([id], [name], [describe]) VALUES (4, N'Vsmart', N'cho 1 Việt Nam đẹp đẽ')
INSERT [dbo].[Categories] ([id], [name], [describe]) VALUES (5, N'SS7', N'Mong Manh dễ vỡ')
GO
SET IDENTITY_INSERT [dbo].[Combo] ON 

INSERT [dbo].[Combo] ([ComboID], [ComboName], [Status], [Price]) VALUES (1, N'Wings Meal', 1, 180000)
INSERT [dbo].[Combo] ([ComboID], [ComboName], [Status], [Price]) VALUES (2, N'Family Pack', 1, 260000)
INSERT [dbo].[Combo] ([ComboID], [ComboName], [Status], [Price]) VALUES (3, N'Snack Combo', 1, 200000)
INSERT [dbo].[Combo] ([ComboID], [ComboName], [Status], [Price]) VALUES (4, N'Quick Bite', 1, 160000)
INSERT [dbo].[Combo] ([ComboID], [ComboName], [Status], [Price]) VALUES (5, N'Full Feast', 1, 250000)
SET IDENTITY_INSERT [dbo].[Combo] OFF
GO
SET IDENTITY_INSERT [dbo].[ComboDetail] ON 

INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (1, 1, 1, 1)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (2, 2, 1, 1)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (3, 4, 1, 1)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (4, 1, 1, 2)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (5, 2, 1, 2)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (6, 3, 1, 2)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (7, 4, 1, 2)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (8, 2, 1, 3)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (9, 3, 1, 3)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (10, 4, 1, 3)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (11, 1, 1, 4)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (12, 2, 1, 4)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (13, 1, 1, 5)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (14, 2, 1, 5)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (15, 3, 1, 5)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (16, 4, 1, 5)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (17, 1, 2, 1)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (18, 2, 2, 2)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (19, 3, 2, 3)
INSERT [dbo].[ComboDetail] ([ComboDetailID], [FoodID], [Amount], [ComboID]) VALUES (20, 4, 2, 4)
SET IDENTITY_INSERT [dbo].[ComboDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Expenditure] ON 

INSERT [dbo].[Expenditure] ([Id], [Name], [Executor], [Cost], [SupplierOrderId], [date]) VALUES (1, N'trả nợ', N'Thành', CAST(2000.00 AS Decimal(18, 2)), NULL, NULL)
INSERT [dbo].[Expenditure] ([Id], [Name], [Executor], [Cost], [SupplierOrderId], [date]) VALUES (2, N'Thanh Toán Tiền Nhập Hàng Ngày System.Func`1[System.String]', N'Ko phai thanh', CAST(6365000.00 AS Decimal(18, 2)), 3, CAST(N'2025-03-28' AS Date))
INSERT [dbo].[Expenditure] ([Id], [Name], [Executor], [Cost], [SupplierOrderId], [date]) VALUES (3, N'Thanh Toán Tiền Nhập Hàng Ngày 08/03/2025', N'toi la toi', CAST(6365000.00 AS Decimal(18, 2)), 4, CAST(N'2025-03-28' AS Date))
INSERT [dbo].[Expenditure] ([Id], [Name], [Executor], [Cost], [SupplierOrderId], [date]) VALUES (4, N'Thanh Toán Tiền Nhập Hàng Ngày 26/02/2025', N'hui', CAST(6365000.00 AS Decimal(18, 2)), 5, NULL)
INSERT [dbo].[Expenditure] ([Id], [Name], [Executor], [Cost], [SupplierOrderId], [date]) VALUES (5, N'Thanh Toán Tiền Nhập Hàng Ngày 27/02/2025', N'Luong', CAST(6365000.00 AS Decimal(18, 2)), 6, CAST(N'2025-03-29' AS Date))
SET IDENTITY_INSERT [dbo].[Expenditure] OFF
GO
SET IDENTITY_INSERT [dbo].[Feedback] ON 

INSERT [dbo].[Feedback] ([id], [rate], [Content], [TimeFeedback]) VALUES (1, 4, N'ok', CAST(N'2025-03-29T00:11:15.883' AS DateTime))
SET IDENTITY_INSERT [dbo].[Feedback] OFF
GO
SET IDENTITY_INSERT [dbo].[Food] ON 

INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (1, N'Gà rán', 4, 9000000, NULL, 0, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\19-7571_36bc815e0478443e8ac5c9a52babb6fe_abc06a52c369400b985f7fd56bc5c760.png')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (2, N'Gà Rán 4lua', 4, 30000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\g_r_n_5-1_1_7c4be151879740fd8e5a6ec566b9ae2c_e05e0b8fbdb2441bb5cee4c43ea42958.jpg')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (3, N'Điện 220v', 3, 100000, NULL, 0, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\Screenshot 2024-10-22 111045_c2a183863e9b4ee797010b6572333ffd_7b9add0fe2e447918cb9a24173b99883.png')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (4, N'Gà sốt cay ngọt', 4, 120000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\g_r_n_5-1_1_7c4be151879740fd8e5a6ec566b9ae2c_68febb5eaa39407c867f396fcd81cd82.jpg')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (5, N'Cánh gà Chiên', 4, 45000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\19-7571_36bc815e0478443e8ac5c9a52babb6fe_04e25ef5de9d4d7f9042be27a5782f5c.png')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (6, N'Coca', 3, 15000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\bin\images\nuoc-ngot-coca-cola-pet-390ml-20220817_a96bc83bbc614cd89b457245ecb10cdb.jpg')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (7, N'Pép si', 3, 15000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\bin\images\pngtree-pepsi-produced-in-tyumen-russia-by-pepsico-many-photo-png-image_13418754_03383e90731a45f8bc30f4de30bd8953.png')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (8, N'Sờ pai', 3, 15000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\nuoc-ngot-sprite-320ml_7ecf825efaa9444d9becf658b3812d4d_2e59645d5246425eb9f1a5c557050217.jpg')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (9, N'Khoai tây Chiên', 6, 30000, NULL, 1, N'khoai-tay-chien_311aebaa2908439cbbaeb5a37717dd6c_780d5a8c7cbc4733816be1fcceb4e21b.jpg')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (10, N'Khoai Tây Chiên Premium', 6, 60000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\Khoai_tay_chien_co_lon_2f7694ff2af443f6a978dde67821467a.png')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (11, N'Garena', 4, 68000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\cat-4_3a1f301562304509b5d6517e10d0770c.jpg')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (12, N'Thịt Chó 4 Mùa', 6, 45000, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\vendor-1_9c4169ccd5fc43639b756fe49f31c1a4.jpg')
INSERT [dbo].[Food] ([FoodID], [FoodName], [FoodType], [Price], [Table], [Status], [image]) VALUES (13, N'gà cay', 4, 22222, NULL, 1, N'D:\PRN212_PROJECT\PRN212_PROJECT\images\19-7571_afbd07bd31794f02b544f78b4ce4f1e0.png')
SET IDENTITY_INSERT [dbo].[Food] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetailCombo] ON 

INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (1, 1, 3, 1, 200000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (2, 2, 1, 2, 360000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (3, 3, 1, 1, 200000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (4, 4, 2, 2, 300000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (5, 5, 3, 1, 250000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (6, 6, 4, 3, 400000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (7, 7, 1, 2, 350000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (8, 8, 2, 1, 180000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (9, 9, 3, 2, 320000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (10, 10, 4, 1, 280000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (11, 11, 1, 3, 450000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (12, 12, 2, 2, 220000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (13, 13, 1, 2, 100000)
INSERT [dbo].[OrderDetailCombo] ([orderDetailID], [orderID], [ComboID], [Amount], [Price]) VALUES (14, 20, 2, 1, 260000)
SET IDENTITY_INSERT [dbo].[OrderDetailCombo] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetailFood] ON 

INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (1, 1, 3, 2, 200000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (2, 2, 3, 1, 100000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (3, 3, 3, 1, 360000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (4, 4, 1, 2, 150000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (5, 5, 3, 3, 180000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (6, 6, 4, 1, 250000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (7, 7, 1, 2, 300000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (8, 8, 2, 1, 120000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (9, 9, 3, 4, 220000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (10, 10, 4, 2, 170000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (11, 11, 1, 1, 190000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (12, 12, 2, 3, 260000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (13, 13, 2, 1, 200000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (14, 14, 3, 1, 100000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (15, 14, 4, 1, 120000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (16, 15, 3, 1, 100000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (17, 15, 4, 2, 120000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (18, 16, 1, 1, 47000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (19, 16, 3, 2, 100000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (20, 17, 1, 1, 47000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (21, 17, 3, 2, 100000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (22, 18, 4, 1, 120000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (23, 19, 4, 1, 120000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (24, 20, 3, 2, 100000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (25, 21, 6, 1, 15000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (26, 22, 4, 1, 120000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (27, 23, 4, 1, 120000)
INSERT [dbo].[OrderDetailFood] ([orderDetailID], [orderID], [FoodID], [Amount], [Price]) VALUES (28, 24, 4, 1, 120000)
SET IDENTITY_INSERT [dbo].[OrderDetailFood] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderTable] ON 

INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (1, N'Nguyen Duy Luong', CAST(N'2025-03-23T14:30:00.000' AS DateTime), 1, 1, 400000, N'Thạch Hòa - Thạch Thất -  Hà Nội', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (2, N'Nguyen Duy Luong', CAST(N'2025-03-23T14:30:00.000' AS DateTime), 1, 1, 460000, N'Thạch Hòa - Thạch Thất -  Hà Nội', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (3, N'Nguyen Duy Luong', CAST(N'2025-03-23T14:30:00.000' AS DateTime), 1, 1, 460000, N'Nicolas', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (4, N'Tran Thi Mai', CAST(N'2025-02-01T09:00:00.000' AS DateTime), 1, 1, 300000, N'123 Le Loi, Hanoi', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (5, N'Le Van Nam', CAST(N'2025-02-05T10:15:00.000' AS DateTime), 0, 1, 750000, N'45 Tran Phu, HCMC', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (6, N'Pham Thi Hong', CAST(N'2025-02-10T13:20:00.000' AS DateTime), 1, 0, 500000, N'78 Nguyen Trai, Da Nang', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (7, N'Hoang Van Tuan', CAST(N'2025-02-15T15:30:00.000' AS DateTime), 0, 0, 200000, N'12 Ly Thuong Kiet, Hue', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (8, N'Nguyen Thi Lan', CAST(N'2025-02-20T11:45:00.000' AS DateTime), 1, 1, 900000, N'56 Pham Van Dong, Can Tho', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (9, N'Bui Van Hung', CAST(N'2025-02-25T16:00:00.000' AS DateTime), 0, 1, 350000, N'89 Vo Van Tan, Hanoi', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (10, N'Do Thi Thanh', CAST(N'2025-03-01T08:30:00.000' AS DateTime), 1, 0, 600000, N'34 Nguyen Hue, HCMC', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (11, N'Vu Van Long', CAST(N'2025-03-05T14:00:00.000' AS DateTime), 0, 1, 450000, N'67 Tran Hung Dao, Da Nang', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (12, N'Nguyen Van An', CAST(N'2025-03-10T12:10:00.000' AS DateTime), 1, 1, 800000, N'23 Le Duan, Hue', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (13, N'Tran Van Bao', CAST(N'2025-03-20T17:20:00.000' AS DateTime), 0, 1, 250000, N'90 Nguyen Thi Minh Khai, Can Tho', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (14, N'sdf', CAST(N'2025-03-24T10:15:11.733' AS DateTime), NULL, NULL, 100000, N'sdf', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (15, N'Nguyen Duy Luong', CAST(N'2025-03-24T03:13:09.390' AS DateTime), 1, 1, 374000, N'Ha Noi', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (16, N'Nicolas', CAST(N'2025-03-24T03:14:27.030' AS DateTime), 1, 1, 271700, N'NewYork', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (17, N'Nicolas', CAST(N'2025-03-24T03:19:33.960' AS DateTime), 1, 1, 271700, N'Heheh', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (18, N'ewr', CAST(N'2025-03-24T11:18:30.643' AS DateTime), 1, 1, 132000, N'wer', 1, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (19, N'Thanh', CAST(N'2025-03-24T11:36:58.253' AS DateTime), 0, 1, 120000, N'In-store delivery', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (20, N'bla', CAST(N'2025-03-24T11:38:03.027' AS DateTime), 0, 1, 460000, N'In-store delivery', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (21, N'cac', CAST(N'2025-03-24T11:39:00.410' AS DateTime), 0, 1, 15000, N'In-store delivery', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (22, N'hui', CAST(N'2025-03-24T12:53:47.290' AS DateTime), 1, 0, 120000, N'In-store delivery', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (23, N'hui', CAST(N'2025-03-24T12:53:52.280' AS DateTime), 1, 0, 120000, N'In-store delivery', 0, NULL)
INSERT [dbo].[OrderTable] ([orderID], [customerName], [Date], [isPaid], [Done], [Total], [Address], [Shipping], [PhoneNumber]) VALUES (24, N'hui', CAST(N'2025-03-24T12:53:57.280' AS DateTime), 1, 0, 120000, N'In-store delivery', 0, NULL)
SET IDENTITY_INSERT [dbo].[OrderTable] OFF
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (6, N'CreateOrder')
INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (5, N'ImportGoods')
INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (7, N'ManageComboAndFood')
INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (4, N'ManageFeedback')
INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (2, N'ManageOrders')
INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (3, N'ManageRoles')
INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (8, N'ViewAnalysis')
INSERT [dbo].[Permissions] ([PermissionId], [PermissionName]) VALUES (1, N'ViewStatistics')
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ip1       ', N'iphone 12 series', 15, 19000.0000, CAST(N'2021-10-04' AS Date), N'Chiếc iPhone siêu nhỏ gọn nhưng mang trên mình sức mạnh không đối thủ. iPhone 12 mini là sự lựa chọn hoàn hảo cho những ai đang cần một chiếc điện thoại có thể làm mọi thứ nhưng lại nằm gọn trong lòng bàn tay và độ tiện dụng đáng kinh ngạc.', N'images/ip1.jpg', 1)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ip2       ', N'iphone 11 series', 30, 16000.0000, CAST(N'2020-10-04' AS Date), N'Chiếc iPhone siêu nhỏ gọn nhưng mang trên mình sức mạnh không đối thủ. iPhone 12 mini là sự lựa chọn hoàn hảo cho những ai đang cần một chiếc điện thoại có thể làm mọi thứ nhưng lại nằm gọn trong lòng bàn tay và độ tiện dụng đáng kinh ngạc.', N'images/ip2.jpg', 1)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ip3       ', N'iphone X series', 22, 13000.0000, CAST(N'2019-10-04' AS Date), N'Chiếc iPhone siêu nhỏ gọn nhưng mang trên mình sức mạnh không đối thủ. iPhone 12 mini là sự lựa chọn hoàn hảo cho những ai đang cần một chiếc điện thoại có thể làm mọi thứ nhưng lại nằm gọn trong lòng bàn tay và độ tiện dụng đáng kinh ngạc.', N'images/ip3.jpg', 1)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'opp1      ', N'oppo find x series', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'OPPO Reno4 - Chiếc điện thoại với cấu hình mạnh mẽ và công nghệ sạc siêu nhanh sẽ giúp bạn có được hiệu suất cao để trải nghiệm những điều thú vị trong cuộc sống, nhất là trên bộ tứ camera đẳng cấp cùng thiết kế từ nhà OPPO mà ai cũng phải ngước nhìn.

', N'images/opp1.jpg', 3)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'opp2      ', N'oppo find x series', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'OPPO Reno4 - Chiếc điện thoại với cấu hình mạnh mẽ và công nghệ sạc siêu nhanh sẽ giúp bạn có được hiệu suất cao để trải nghiệm những điều thú vị trong cuộc sống, nhất là trên bộ tứ camera đẳng cấp cùng thiết kế từ nhà OPPO mà ai cũng phải ngước nhìn.

', N'images/opp2.jpg', 3)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'opp3      ', N'oppo find x series', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'OPPO Reno4 - Chiếc điện thoại với cấu hình mạnh mẽ và công nghệ sạc siêu nhanh sẽ giúp bạn có được hiệu suất cao để trải nghiệm những điều thú vị trong cuộc sống, nhất là trên bộ tứ camera đẳng cấp cùng thiết kế từ nhà OPPO mà ai cũng phải ngước nhìn.

', N'images/opp3.jpg', 3)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'opp4      ', N'oppo find x series', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'OPPO Reno4 - Chiếc điện thoại với cấu hình mạnh mẽ và công nghệ sạc siêu nhanh sẽ giúp bạn có được hiệu suất cao để trải nghiệm những điều thú vị trong cuộc sống, nhất là trên bộ tứ camera đẳng cấp cùng thiết kế từ nhà OPPO mà ai cũng phải ngước nhìn.

', N'images/opp4.jpg', 3)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'opp5      ', N'oppo find x series', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'OPPO Reno4 - Chiếc điện thoại với cấu hình mạnh mẽ và công nghệ sạc siêu nhanh sẽ giúp bạn có được hiệu suất cao để trải nghiệm những điều thú vị trong cuộc sống, nhất là trên bộ tứ camera đẳng cấp cùng thiết kế từ nhà OPPO mà ai cũng phải ngước nhìn.

', N'images/opp5.jpg', 3)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ss1       ', N'Galaxy Z series', 12, 20000.0000, CAST(N'2021-01-04' AS Date), N'Samsung Galaxy Note 20 Ultra được chế tác từ những vật liệu cao cấp hàng đầu hiện nay, với sự tỉ mỉ và chất lượng gia công thượng thừa, tạo nên chiếc điện thoại đẹp hơn những gì bạn có thể tưởng tượng.', N'images/ss1.jpg', 2)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ss2       ', N'galaxy note series', 12, 18000.0000, CAST(N'2020-10-08' AS Date), N'Samsung Galaxy Note 20 Ultra được chế tác từ những vật liệu cao cấp hàng đầu hiện nay, với sự tỉ mỉ và chất lượng gia công thượng thừa, tạo nên chiếc điện thoại đẹp hơn những gì bạn có thể tưởng tượng.', N'images/ss2.jpg', 2)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ss3       ', N'galaxy F series', 34, 16000.0000, CAST(N'2019-10-08' AS Date), N'Samsung Galaxy Note 20 Ultra được chế tác từ những vật liệu cao cấp hàng đầu hiện nay, với sự tỉ mỉ và chất lượng gia công thượng thừa, tạo nên chiếc điện thoại đẹp hơn những gì bạn có thể tưởng tượng.', N'images/ss3.jpg', 2)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ss4       ', N'galaxy HHH', 19, 15000.0000, CAST(N'2018-10-08' AS Date), N'Samsung Galaxy Note 20 Ultra được chế tác từ những vật liệu cao cấp hàng đầu hiện nay, với sự tỉ mỉ và chất lượng gia công thượng thừa, tạo nên chiếc điện thoại đẹp hơn những gì bạn có thể tưởng tượng.', N'images/ss4.jpg', 2)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ss5       ', N'galaxy KKK', 52, 14000.0000, CAST(N'2017-01-04' AS Date), N'Samsung Galaxy Note 20 Ultra được chế tác từ những vật liệu cao cấp hàng đầu hiện nay, với sự tỉ mỉ và chất lượng gia công thượng thừa, tạo nên chiếc điện thoại đẹp hơn những gì bạn có thể tưởng tượng.', N'images/ss5.jpg', 2)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ss6       ', N'galaxy note series', 12, 18000.0000, CAST(N'2020-10-08' AS Date), N'Samsung Galaxy Note 20 Ultra du?c ch? tác t? nh?ng v?t li?u cao c?p hàng d?u hi?n nay, v?i s? t? m? và ch?t lu?ng gia công thu?ng th?a, t?o nên chi?c di?n tho?i d?p hon nh?ng gì b?n có th? tu?ng tu?ng.', N'images/ss2.jpg', 2)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'ss7       ', N'galaxy note series', 12, 18000.0000, CAST(N'2020-10-08' AS Date), N'Samsung Galaxy Note 20 Ultra du?c ch? tác t? nh?ng v?t li?u cao c?p hàng d?u hi?n nay, v?i s? t? m? và ch?t lu?ng gia công thu?ng th?a, t?o nên chi?c di?n tho?i d?p hon nh?ng gì b?n có th? tu?ng tu?ng.', N'images/ss2.jpg', 2)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'vsm1      ', N'Vsmart Joy 4 3GB-64GB', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'smart Live 4 6GB-64GB sở hữu cấu hình cực đỉnh, đưa bạn đến trải nghiệm giải trí bất tận với dung lượng pin lớn, màn hình tuyệt đẹp và 4 camera sau AI đầy ấn tượng.

', N'images/vsm1.jpg', 4)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'vsm2      ', N'Vsmart Joy 4 3GB-64GB', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'smart Live 4 6GB-64GB sở hữu cấu hình cực đỉnh, đưa bạn đến trải nghiệm giải trí bất tận với dung lượng pin lớn, màn hình tuyệt đẹp và 4 camera sau AI đầy ấn tượng.

', N'images/vsm2.jpg', 4)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'vsm3      ', N'Vsmart Joy 4 3GB-64GB', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'smart Live 4 6GB-64GB sở hữu cấu hình cực đỉnh, đưa bạn đến trải nghiệm giải trí bất tận với dung lượng pin lớn, màn hình tuyệt đẹp và 4 camera sau AI đầy ấn tượng.

', N'images/vsm3.jpg', 4)
INSERT [dbo].[Products] ([id], [name], [quantity], [price], [releaseDate], [describe], [image], [cid]) VALUES (N'vsm4      ', N'Vsmart Joy 4 3GB-64GB', 60, 13000.0000, CAST(N'2020-01-04' AS Date), N'smart Live 4 6GB-64GB sở hữu cấu hình cực đỉnh, đưa bạn đến trải nghiệm giải trí bất tận với dung lượng pin lớn, màn hình tuyệt đẹp và 4 camera sau AI đầy ấn tượng.

', N'images/vsm4.jpg', 4)
GO
INSERT [dbo].[Role] ([role_id], [role_name]) VALUES (1, N'Admin')
INSERT [dbo].[Role] ([role_id], [role_name]) VALUES (2, N'Staff')
INSERT [dbo].[Role] ([role_id], [role_name]) VALUES (3, N'Cooker')
INSERT [dbo].[Role] ([role_id], [role_name]) VALUES (4, N'Customer')
GO
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (1, 1)
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (1, 2)
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (1, 3)
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (1, 4)
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (1, 8)
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (2, 5)
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (2, 6)
INSERT [dbo].[RolePermissions] ([RoleId], [PermissionId]) VALUES (2, 7)
GO
SET IDENTITY_INSERT [dbo].[Supplier] ON 

INSERT [dbo].[Supplier] ([Id], [Name], [PhoneNumber], [Address], [Email]) VALUES (1, N'Thanh daden', N'0986248126', N'abc', N'kophaithanh@gmail.com')
INSERT [dbo].[Supplier] ([Id], [Name], [PhoneNumber], [Address], [Email]) VALUES (2, N'Thanh datrang', N'0986248122', N'abc', N'kophaithanh123@gmail.com')
SET IDENTITY_INSERT [dbo].[Supplier] OFF
GO
SET IDENTITY_INSERT [dbo].[SupplierOrder] ON 

INSERT [dbo].[SupplierOrder] ([Id], [SupplierId], [OrderDate], [DeliverDate], [IsPaid], [Total]) VALUES (1, 1, CAST(N'2025-03-15' AS Date), CAST(N'2025-03-08' AS Date), 1, CAST(1275000.00 AS Decimal(18, 2)))
INSERT [dbo].[SupplierOrder] ([Id], [SupplierId], [OrderDate], [DeliverDate], [IsPaid], [Total]) VALUES (2, 2, CAST(N'2025-03-01' AS Date), CAST(N'2025-04-06' AS Date), 1, CAST(1275000.00 AS Decimal(18, 2)))
INSERT [dbo].[SupplierOrder] ([Id], [SupplierId], [OrderDate], [DeliverDate], [IsPaid], [Total]) VALUES (3, 1, CAST(N'2025-03-07' AS Date), CAST(N'2025-04-18' AS Date), 1, CAST(6365000.00 AS Decimal(18, 2)))
INSERT [dbo].[SupplierOrder] ([Id], [SupplierId], [OrderDate], [DeliverDate], [IsPaid], [Total]) VALUES (4, 2, CAST(N'2025-03-08' AS Date), CAST(N'2025-03-28' AS Date), 1, CAST(6365000.00 AS Decimal(18, 2)))
INSERT [dbo].[SupplierOrder] ([Id], [SupplierId], [OrderDate], [DeliverDate], [IsPaid], [Total]) VALUES (5, 1, CAST(N'2025-02-26' AS Date), CAST(N'2025-03-30' AS Date), 1, CAST(6365000.00 AS Decimal(18, 2)))
INSERT [dbo].[SupplierOrder] ([Id], [SupplierId], [OrderDate], [DeliverDate], [IsPaid], [Total]) VALUES (6, 1, CAST(N'2025-02-27' AS Date), CAST(N'2025-03-30' AS Date), 1, CAST(6365000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[SupplierOrder] OFF
GO
SET IDENTITY_INSERT [dbo].[SupplierOrderDetail] ON 

INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (1, 1, N'Rau Xanh', 1.5, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (2, 1, N'Gà Đông Lạnh', 126, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (3, 2, N'Rau Xanh', 1.5, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (4, 2, N'Gà Đông Lạnh', 126, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (5, 3, N'Rau Xanh', 1.5, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (6, 3, N'Gà Đông Lạnh', 126, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (7, 3, N'Lương 220v', 10, 100000, N'cái')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (8, 3, N'Sườn bò', 20.45, 200000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (9, 5, N'Rau Xanh', 1.5, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (10, 5, N'Gà Đông Lạnh', 126, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (11, 5, N'Lương 220v', 10, 100000, N'cái')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (12, 5, N'Sườn bò', 20.45, 200000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (13, 6, N'Rau Xanh', 1.5, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (14, 6, N'Gà Đông Lạnh', 126, 10000, N'kg')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (15, 6, N'Lương 220v', 10, 100000, N'cái')
INSERT [dbo].[SupplierOrderDetail] ([Id], [SupplierOrderId], [ProductName], [Amount], [UnitPrice], [CalculationUnit]) VALUES (16, 6, N'Sườn bò', 20.45, 200000, N'kg')
SET IDENTITY_INSERT [dbo].[SupplierOrderDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[TypeOfFood] ON 

INSERT [dbo].[TypeOfFood] ([TypeID], [TypeName]) VALUES (1, N'Tráng miệng')
INSERT [dbo].[TypeOfFood] ([TypeID], [TypeName]) VALUES (3, N'Nước Uống')
INSERT [dbo].[TypeOfFood] ([TypeID], [TypeName]) VALUES (4, N'Gà Rán')
INSERT [dbo].[TypeOfFood] ([TypeID], [TypeName]) VALUES (6, N'Sữa Đặc')
SET IDENTITY_INSERT [dbo].[TypeOfFood] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Permissi__0FFDA357B6FF8E91]    Script Date: 01/04/2025 12:44:15 ******/
ALTER TABLE [dbo].[Permissions] ADD UNIQUE NONCLUSTERED 
(
	[PermissionName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Role_id] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Role_id]
GO
ALTER TABLE [dbo].[ComboDetail]  WITH CHECK ADD FOREIGN KEY([ComboID])
REFERENCES [dbo].[Combo] ([ComboID])
GO
ALTER TABLE [dbo].[ComboDetail]  WITH CHECK ADD FOREIGN KEY([FoodID])
REFERENCES [dbo].[Food] ([FoodID])
GO
ALTER TABLE [dbo].[Expenditure]  WITH CHECK ADD FOREIGN KEY([SupplierOrderId])
REFERENCES [dbo].[SupplierOrder] ([Id])
GO
ALTER TABLE [dbo].[Food]  WITH CHECK ADD FOREIGN KEY([FoodType])
REFERENCES [dbo].[TypeOfFood] ([TypeID])
GO
ALTER TABLE [dbo].[OrderDetailCombo]  WITH CHECK ADD FOREIGN KEY([ComboID])
REFERENCES [dbo].[Combo] ([ComboID])
GO
ALTER TABLE [dbo].[OrderDetailCombo]  WITH CHECK ADD FOREIGN KEY([orderID])
REFERENCES [dbo].[OrderTable] ([orderID])
GO
ALTER TABLE [dbo].[OrderDetailFood]  WITH CHECK ADD FOREIGN KEY([FoodID])
REFERENCES [dbo].[Food] ([FoodID])
GO
ALTER TABLE [dbo].[OrderDetailFood]  WITH CHECK ADD FOREIGN KEY([orderID])
REFERENCES [dbo].[OrderTable] ([orderID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Phone_Category] FOREIGN KEY([cid])
REFERENCES [dbo].[Categories] ([id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Phone_Category]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([PermissionId])
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[SupplierOrder]  WITH CHECK ADD FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([Id])
GO
USE [master]
GO
ALTER DATABASE [ChickenPRN] SET  READ_WRITE 
GO
