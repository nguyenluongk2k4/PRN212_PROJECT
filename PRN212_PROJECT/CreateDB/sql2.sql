CREATE TABLE Supplier (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(11),
    Address NVARCHAR(250),
    Email VARCHAR(50)
);

CREATE TABLE SupplierOrder (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SupplierId INT NOT NULL,
    OrderDate DATE,
    DeliverDate DATE,
    Status INT,
    IsPaid BIT,
    Total DECIMAL(18,2), 
    FOREIGN KEY (SupplierId) REFERENCES Supplier(Id)
);
create table Feedback(
 id int identity(1,1) primary key,
 rate int,
 Content nvarchar(max)
)

CREATE TABLE SupplierOrderDetail (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SupplierOrderId INT NOT NULL,
    ProductName NVARCHAR(100),
    Amount INT,
    UnitPrice FLOAT,
    FOREIGN KEY (SupplierOrderId) REFERENCES SupplierOrder(Id)
);

CREATE TABLE Expenditure (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    SupplierOrderId INT NULL,
    FOREIGN KEY (SupplierOrderId) REFERENCES SupplierOrder(Id)
);
alter table OrderTable
add PhoneNumber varchar(11)

CREATE TABLE Role(
role_id INT,
role_name NVARCHAR(255),
PRIMARY KEY(role_id)
)

INSERT INTO dbo.Role(role_id, role_name)
VALUES(1,'Admin')

INSERT INTO dbo.Role(role_id, role_name)
VALUES(2,'Staff')

INSERT INTO dbo.Role(role_id, role_name)
VALUES(3,'Cooker')

INSERT INTO dbo.Role(role_id, role_name)
VALUES(4,'Customer')


CREATE TABLE Accounts(
account_id INT IDENTITY(1,1),
username NVARCHAR(255) NOT NULL,
password NVARCHAR(255) NOT NULL,
role_id INT,
fullname NVARCHAR(255),
)

INSERT INTO Accounts (username, password, role_id, fullname)
VALUES
('admin_nam', 'password123', 1, 'Nguyễn Văn Nam'),
('admin_lan', 'password123', 1, 'Trần Thị Lan'),
('staff_hai', 'password123', 2, 'Phạm Văn Hải'),
('staff_hoa', 'password123', 2, 'Lê Thị Hoa'),
('staff_phong', 'password123', 2, 'Đặng Minh Phong'),
('cooker_quang', 'password123', 3, 'Hoàng Văn Quang'),
('cooker_mai', 'password123', 3, 'Bùi Thị Mai'),
('customer_dung', 'password123', 4, 'Đỗ Mạnh Dũng'),
('customer_anh', 'password123', 4, 'Nguyễn Hoàng Anh'),
('customer_ly', 'password123', 4, 'Trịnh Thuỳ Ly');
