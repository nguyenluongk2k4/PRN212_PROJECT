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
