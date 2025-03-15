CREATE TABLE OrderTable (
    orderID INT PRIMARY KEY IDENTITY(1,1),
    customerName NVARCHAR(50),
    Date DATETIME,
    isPaid BIT,
    Done BIT,
    Total FLOAT
);

CREATE TABLE TypeOfFood (
    TypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(50)
);

CREATE TABLE Food (
    FoodID INT PRIMARY KEY IDENTITY(1,1),
    FoodName NVARCHAR(50),
    FoodType INT FOREIGN KEY REFERENCES TypeOfFood(TypeID),
    Price FLOAT,
    Status INT
);

CREATE TABLE OrderDetail (
    orderDetailID INT PRIMARY KEY IDENTITY(1,1),
    orderID INT FOREIGN KEY REFERENCES OrderTable(orderID),
    FoodID INT FOREIGN KEY REFERENCES Food(FoodID),
    Amount INT
);

CREATE TABLE Combo (
    ComboID INT PRIMARY KEY IDENTITY(1,1),
    ComboName NVARCHAR(50),
    Status INT,
    Price FLOAT
);

CREATE TABLE ComboDetail (
    ComboDetailID INT PRIMARY KEY IDENTITY(1,1),
    FoodID INT FOREIGN KEY REFERENCES Food(FoodID),
    Amount INT,
    ComboID INT FOREIGN KEY REFERENCES Combo(ComboID)
);
