CREATE DATABASE JwelrySystemDBMSFinal
GO
USE JwelrySystemDBMSFinal
GO

CREATE TABLE Customer (
	CustomerID INT IDENTITY(1,1) PRIMARY KEY,
	FullName NVARCHAR(255) NOT NULL,
	CustomerType NVARCHAR(50) NOT NULL CHECK (CustomerType IN (N'VIP', N'Regular')),
	Address NVARCHAR(255) NOT NULL,
	Phone NVARCHAR(15) NOT NULL UNIQUE
);
GO

CREATE TABLE Branch (
	BranchID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255),
	Address NVARCHAR(255),
	Phone NVARCHAR(15)
);
GO

CREATE TABLE ProductGroup (
	GroupID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100),
	Description NVARCHAR(255)
);
GO

CREATE TABLE Product (
	ProductID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL,
	Material NVARCHAR(100) NOT NULL,
	StockQuantity INT CHECK (StockQuantity >= 0),
	Price DECIMAL(18,2) CHECK (Price > 0),
	Weight FLOAT CHECK (Weight > 0),
	Description NVARCHAR(255),
	GroupID INT,
	BranchID INT,
	ProdImage IMAGE,
	FOREIGN KEY (GroupID) REFERENCES ProductGroup(GroupID),
	FOREIGN KEY (BranchID) REFERENCES Branch(BranchID)
);

GO

CREATE TABLE Application (
	AppID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL,
	DiscountRate DECIMAL(5,2) CHECK (DiscountRate BETWEEN 0 AND 100),
	PaymentMethods NVARCHAR(255) NOT NULL
);

GO

CREATE TABLE OrderTable (
	OrderID INT IDENTITY(1,1) PRIMARY KEY,
	OrderDate DATETIME NOT NULL,
	TotalAmount DECIMAL(18,2) CHECK (TotalAmount >= 0),
	PaymentMethod NVARCHAR(50) NOT NULL,
	OrderStatus NVARCHAR(100) CHECK (OrderStatus IN (N'Pending', N'Completed', N'Cancelled')),
	ShippingMethod NVARCHAR(100),
	BranchID INT,
	CustomerID INT,
	AppID INT,
	FOREIGN KEY (BranchID) REFERENCES Branch(BranchID),
	FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
	FOREIGN KEY (AppID) REFERENCES Application(AppID)
);

GO

CREATE TABLE OrderDetail (
	OrderID INT,
	ProductID INT,
	Quantity INT CHECK (Quantity > 0),
	UnitPrice DECIMAL(18,2) CHECK (UnitPrice > 0),
	SubTotal DECIMAL(18,2),
	PRIMARY KEY (OrderID, ProductID),
	FOREIGN KEY (OrderID) REFERENCES OrderTable(OrderID),
	FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

GO

CREATE TABLE Supplier (
	SupplierID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255),
	Address NVARCHAR(255),
	Phone NVARCHAR(15)
);
GO

CREATE TABLE TransactionTable (
	TransactionID INT IDENTITY(1,1) PRIMARY KEY,
	TransactionDate DATETIME NOT NULL,
	Amount DECIMAL(18,2) CHECK (Amount > 0),
	Type NVARCHAR(50),
	Description NVARCHAR(255),
	BranchID INT,
	OrderID INT,
	FOREIGN KEY (BranchID) REFERENCES Branch(BranchID),
	FOREIGN KEY (OrderID) REFERENCES OrderTable(OrderID)
);

GO

CREATE TABLE Promotion (
	PromotionID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL,
	Type NVARCHAR(50) CHECK (Type IN (N'Discount', N'Gift')),
	Condition NVARCHAR(255),
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	DiscountRate DECIMAL(5,2) CHECK (DiscountRate BETWEEN 0 AND 100)
);

GO

CREATE TABLE OrderPromotion (
	OrderID INT,
	PromotionID INT,
	DiscountValue DECIMAL(18,2),
	PRIMARY KEY (OrderID, PromotionID),
	FOREIGN KEY (OrderID) REFERENCES OrderTable(OrderID),
	FOREIGN KEY (PromotionID) REFERENCES Promotion(PromotionID)
);
GO

CREATE TABLE SupplierProduct (
	SupplierID INT,
	ProductID INT,
	SupplyDate DATETIME,
	PRIMARY KEY (SupplierID, ProductID),
	FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
	FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
GO

INSERT INTO Customer (FullName, CustomerType, Address, Phone) VALUES
(N'Trần Thị Bình', N'Regular', N'45 Lê Lợi, TP.HCM', N'0987654321'),
(N'Nguyễn Văn Hùng', N'VIP', N'123 Nguyễn Trãi, TP.HCM', N'0912345678'),
(N'Phạm Thị Lan', N'VIP', N'78 CMT8, TP.HCM', N'0935123456'),
(N'Lê Quốc Tuấn', N'Regular', N'56 Pasteur, TP.HCM', N'0909123456'),
(N'Hoàng Mai Anh', N'VIP', N'10 Hai Bà Trưng, TP.HCM', N'0978123456');
GO

INSERT INTO Branch (Name, Address, Phone) VALUES
(N'Chi nhánh Lê Lợi', N'45 Lê Lợi, TP.HCM', N'02812345678'),
(N'Chi nhánh Nguyễn Trãi', N'123 Nguyễn Trãi, TP.HCM', N'02887654321'),
(N'Chi nhánh CMT8', N'78 CMT8, TP.HCM', N'02856789012'),
(N'Chi nhánh Pasteur', N'56 Pasteur, TP.HCM', N'02834567890'),
(N'Chi nhánh Hai Bà Trưng', N'10 Hai Bà Trưng, TP.HCM', N'02890123456');
GO

INSERT INTO ProductGroup (Name, Description) VALUES
(N'Nhẫn', N'Nhẫn vàng, bạc, kim cương'),
(N'Dây chuyền', N'Dây chuyền trang sức cao cấp'),
(N'Bông tai', N'Bông tai thời trang'),
(N'Lắc tay', N'Lắc tay phong cách hiện đại'),
(N'Mặt dây chuyền', N'Mặt dây trang sức tinh xảo');
GO

INSERT INTO Product (Name, Material, StockQuantity, Price, Weight, Description, GroupID, BranchID, ProdImage) VALUES
(N'Nhẫn bạc', N'Bạc 925', 100, 800000.00, 1.8, N'Nhẫn đơn giản', 1, 1,null),
(N'Dây chuyền bạc', N'Bạc 925', 30, 1500000.00, 10.0, N'Dây chuyền thời trang', 2, 2,null),
(N'Bông tai bạc', N'Bạc 925', 50, 600000.00, 1.2, N'Bông tai thanh lịch', 3, 3,null),
(N'Lắc tay vàng', N'Vàng 18K', 20, 5000000.00, 5.0, N'Lắc tay cao cấp', 4, 4,null),
(N'Mặt dây ngọc trai', N'Ngọc trai', 15, 2000000.00, 3.0, N'Mặt dây sang trọng', 5, 5,null);
GO

INSERT INTO Application (Name, DiscountRate, PaymentMethods) VALUES
(N'JewelryApp', 5.00, N'Credit Card, Cash'),
(N'Shopee', 10.00, N'MoMo, ShopeePay, Cash on Delivery'),
(N'Lazada', 2.50, N'ZaloPay, Bank Transfer, Cash on Delivery'),
(N'TikTokShop', 3.00, N'MoMo, Bank Transfer, Cash on Delivery');
GO

INSERT INTO OrderTable (OrderDate, TotalAmount, PaymentMethod, OrderStatus, ShippingMethod, BranchID, CustomerID, AppID) VALUES
('2025-03-01 10:00:00', 800000.00, N'Cash', N'Completed', N'Standard', 1, 1, 1),
('2025-03-02 14:30:00', 1500000.00, N'MoMo', N'Pending', N'Express', 2, 2, 2),
('2025-03-02 09:15:00', 600000.00, N'Credit Card', N'Completed', N'Standard', 3, 3, 3),
('2025-03-03 11:45:00', 5000000.00, N'Bank Transfer', N'Completed', N'Express', 4, 4, 4),
('2025-03-03 15:00:00', 2000000.00, N'Cash', N'Pending', N'Standard', 5, 5, 4);
GO

INSERT INTO OrderDetail (OrderID, ProductID, Quantity, UnitPrice, SubTotal) VALUES
(1, 1, 1, 800000.00, 800000.00),
(2, 2, 1, 1500000.00, 1500000.00),
(3, 3, 1, 600000.00, 600000.00),
(4, 4, 1, 5000000.00, 5000000.00),
(5, 5, 1, 2000000.00, 2000000.00);
GO

INSERT INTO Supplier (Name, Address, Phone) VALUES
(N'Nhà cung cấp XYZ', N'20 Nguyễn Trãi, TP.HCM', N'0922222222'),
(N'Công ty Trang sức ABC', N'30 Lê Thánh Tôn, TP.HCM', N'0913333333'),
(N'Đá quý Minh Anh', N'15 Võ Thị Sáu, TP.HCM', N'0934444444'),
(N'Vàng SJC', N'50 Lê Duẩn, TP.HCM', N'0945555555'),
(N'Ngọc trai Phú Quốc', N'25 Đồng Khởi, TP.HCM', N'0956666666');
GO

INSERT INTO TransactionTable (TransactionDate, Amount, Type, Description, BranchID, OrderID) VALUES
('2025-03-01 12:00:00', 800000.00, N'Sale', N'Bán nhẫn bạc', 1, 1),
('2025-03-02 15:00:00', 1500000.00, N'Sale', N'Bán dây chuyền bạc', 2, 2),
('2025-03-02 10:00:00', 600000.00, N'Sale', N'Bán bông tai bạc', 3, 3),
('2025-03-03 12:00:00', 5000000.00, N'Sale', N'Bán lắc tay vàng', 4, 4),
('2025-03-03 16:00:00', 2000000.00, N'Sale', N'Bán mặt dây ngọc trai', 5, 5);
GO


INSERT INTO Promotion (Name, Type, Condition, StartDate, EndDate) VALUES
(N'Khuyến mãi tháng 3', N'Discount', N'Mua trên 1 triệu', '2025-03-01 00:00:00', '2025-03-31 23:59:59'),
(N'Quà tặng VIP', N'Gift', N'Khách VIP', '2025-03-01 00:00:00', '2025-03-15 23:59:59'),
(N'Sale 10%', N'Discount', N'Mua trên 2 triệu', '2025-03-05 00:00:00', '2025-03-20 23:59:59'),
(N'Tặng hộp quà', N'Gift', N'Mua 2 sản phẩm', '2025-03-10 00:00:00', '2025-03-25 23:59:59'),
(N'Giảm giá cuối tuần', N'Discount', N'Mua ngày thứ 7', '2025-03-01 00:00:00', '2025-03-31 23:59:59');
GO

INSERT INTO OrderPromotion (OrderID, PromotionID, DiscountValue) VALUES
(1, 1, 80000.00),
(2, 2, 150000.00),
(3, 3, 60000.00),
(4, 4, 500000.00),
(5, 5, 200000.00);
GO

INSERT INTO SupplierProduct (SupplierID, ProductID, SupplyDate) VALUES
(1, 1, '2025-02-15 09:00:00'),
(2, 2, '2025-02-20 10:00:00'),
(3, 3, '2025-02-25 08:00:00'),
(4, 4, '2025-02-28 11:00:00'),
(5, 5, '2025-03-01 09:00:00');
GO


-- TRIGGER
-- Kiểm tra số lượng tồn kho khi thêm đơn hàng
CREATE TRIGGER trg_CheckStockBeforeInsertOrderDetail
ON OrderDetail
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nếu có sản phẩm nào trong đơn hàng không đủ tồn kho
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN Product p ON i.ProductID = p.ProductID
        WHERE i.Quantity > p.StockQuantity
    )
    BEGIN
        THROW 50000, 'Số lượng sản phẩm không đủ trong kho!', 1;
        RETURN;
    END;

    -- Nếu đủ tồn kho, chèn dữ liệu vào bảng OrderDetail
    INSERT INTO OrderDetail (OrderID, ProductID, Quantity, UnitPrice, SubTotal)
    SELECT OrderID, ProductID, Quantity, UnitPrice, SubTotal FROM inserted;
END;
GO
--Cập nhật tồn kho khi thêm đơn hàng

CREATE TRIGGER trg_UpdateStockAfterInsertOrderDetail
ON OrderDetail
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Cập nhật số lượng tồn kho của sản phẩm khi có đơn hàng mới
    UPDATE Product
    SET StockQuantity = Product.StockQuantity - inserted.Quantity
    FROM Product
    INNER JOIN inserted ON Product.ProductID = inserted.ProductID;
END;
GO

--Cập nhật tồn kho khi xóa đơn hàng

CREATE TRIGGER trg_UpdateStockAfterDeleteOrderDetail
ON OrderDetail
AFTER DELETE
AS
BEGIN
    UPDATE Product
    SET StockQuantity = StockQuantity + d.Quantity
    FROM Product P
    INNER JOIN deleted d ON P.ProductID = d.ProductID;
END;
GO
-- Kiểm tra ngày bắt đầu và kết thúc của khuyến mãi

CREATE TRIGGER trg_CheckPromotionDate
ON Promotion
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE EndDate < StartDate)
    BEGIN
        RAISERROR (N'Ngày kết thúc không thể trước ngày bắt đầu!', 16, 1);
        ROLLBACK;
        RETURN;
    END
END;
GO
-- Kiểm tra tổng tiền của đơn hàng không âm

CREATE TRIGGER trg_CheckOrderTotalAmount
ON OrderTable
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE TotalAmount <= 0)
    BEGIN
        RAISERROR (N'Tổng tiền đơn hàng phải lớn hơn 0!', 16, 1);
        ROLLBACK;
        RETURN;
    END
END;
GO
-- Tự động cập nhật trạng thái đơn hàng khi có giao dịch thanh toán

CREATE TRIGGER trg_UpdateOrderStatusAfterTransaction
ON TransactionTable
AFTER INSERT
AS
BEGIN
    UPDATE O
    SET O.OrderStatus = N'Completed'
    FROM OrderTable O
    INNER JOIN inserted I ON O.OrderID = I.OrderID
    WHERE O.OrderStatus = N'Pending';
END;
GO

-- Ngăn chặn nhập dữ liệu trùng lặp cho số điện thoại khách hàng
CREATE TRIGGER trg_PreventDuplicatePhone
ON Customer
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Customer WHERE Phone IN (SELECT Phone FROM inserted))
    BEGIN
        RAISERROR (N'Số điện thoại đã tồn tại!', 16, 1);
        ROLLBACK;
        RETURN;
    END
    INSERT INTO Customer (FullName, CustomerType, Address, Phone)
    SELECT FullName, CustomerType, Address, Phone FROM inserted;
END;
Go

-- Tự động đặt ngày giao dịch là ngày hiện tại nếu không nhập vào
ALTER TABLE TransactionTable 
ADD CONSTRAINT DF_TransactionDate DEFAULT GETDATE() FOR TransactionDate;
Go

--VIEW
--Lấy thông tin đơn hàng kèm chi tiết khách hàng và chi nhánh
CREATE VIEW v_OrderDetails AS
SELECT 
    o.OrderID, 
    o.OrderDate, 
    o.TotalAmount, 
    o.PaymentMethod, 
    o.OrderStatus, 
    o.ShippingMethod, 
    c.FullName AS CustomerName, 
    b.Name AS BranchName
FROM OrderTable o
JOIN Customer c ON o.CustomerID = c.CustomerID
JOIN Branch b ON o.BranchID = b.BranchID;

GO

--Truy vấn sản phẩm theo nhóm (ví dụ Rings)
CREATE VIEW v_ProductList AS
SELECT 
    p.ProductID, 
    p.Name AS ProductName, 
    p.Material, 
    p.StockQuantity, 
    p.Price, 
    p.Weight, 
    p.Description, 
	p.ProdImage,
    g.Name AS GroupName, 
    b.Name AS BranchName
FROM Product p
LEFT JOIN ProductGroup g ON p.GroupID = g.GroupID
LEFT JOIN Branch b ON p.BranchID = b.BranchID;

GO
--Xem doanh thu tại mỗi chi nhánh (xếp theo giảm dần)
CREATE VIEW v_BranchRevenue AS
SELECT 
    b.BranchID, 
    b.Name AS BranchName, 
    SUM(o.TotalAmount) AS TotalRevenue
FROM OrderTable o
JOIN Branch b ON o.BranchID = b.BranchID
WHERE o.OrderStatus = 'Completed'
GROUP BY b.BranchID, b.Name;

GO
-- xem Danh sách đơn hàng có áp dụng khuyến mãi
CREATE VIEW v_OrderWithPromotion AS
SELECT 
    o.OrderID, 
    o.OrderDate, 
    c.FullName AS CustomerName, 
    p.Name AS PromotionName, 
    op.DiscountValue
FROM OrderTable o
JOIN OrderPromotion op ON o.OrderID = op.OrderID
JOIN Promotion p ON op.PromotionID = p.PromotionID
JOIN Customer c ON o.CustomerID = c.CustomerID;

GO
--thống kê số lượng sản phẩm theo nhóm
CREATE VIEW v_ProductStockByGroup AS
SELECT 
    g.GroupID, 
    g.Name AS GroupName, 
    COUNT(p.ProductID) AS ProductCount, 
    SUM(p.StockQuantity) AS TotalStock
FROM Product p
JOIN ProductGroup g ON p.GroupID = g.GroupID
GROUP BY g.GroupID, g.Name;

CREATE PROCEDURE [dbo].[sp_TimKhachHangTheoTen]
    @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * 
    FROM Customer
    WHERE RIGHT(LTRIM(RTRIM(FullName)), CHARINDEX(' ', REVERSE(LTRIM(RTRIM(FullName))) + ' ') - 1) = @Name;
END

CREATE PROCEDURE [dbo].[sp_TimKhachHangTheoDiaChi]
    @DiaChi NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * 
    FROM Customer
    WHERE Address LIKE '%' + @DiaChi + '%';
END

CREATE PROCEDURE sp_AddCustomer
    @FullName NVARCHAR(100),
    @CustomerType NVARCHAR(50),
    @Address NVARCHAR(200),
    @Phone VARCHAR(15)
AS
BEGIN
    INSERT INTO Customer (FullName, CustomerType, Address, Phone)
    VALUES (@FullName, @CustomerType, @Address, @Phone)
END
CREATE PROCEDURE sp_UpdateCustomer
    @CustomerID INT,
    @FullName NVARCHAR(100),
    @CustomerType NVARCHAR(50),
    @Address NVARCHAR(200),
    @Phone VARCHAR(15)
AS
BEGIN
    UPDATE Customer
    SET FullName = @FullName,
        CustomerType = @CustomerType,
        Address = @Address,
        Phone = @Phone
    WHERE CustomerID = @CustomerID
END
CREATE PROCEDURE sp_DeleteCustomer
    @CustomerID INT
AS
BEGIN
    DELETE FROM Customer
    WHERE CustomerID = @CustomerID
END

