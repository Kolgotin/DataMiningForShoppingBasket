DROP TRIGGER IF EXISTS SaleRows_INSERT
GO

CREATE TRIGGER SaleRows_INSERT
ON SaleRows
AFTER INSERT
AS
BEGIN

SELECT Products.Id,
    ResultWarehouseQuantity = Products.WarehouseQuantity - INSERTED.SaleQuantity,
    Products.FractionalAllowed,
    INSERTED.SaleQuantity
INTO #Prepare
FROM Products
INNER JOIN
INSERTED
ON Products.Id = INSERTED.ProductId

DECLARE @failCount INT;

IF (
SELECT COUNT(*)
FROM #Prepare
WHERE #Prepare.ResultWarehouseQuantity < 0
OR (#Prepare.SaleQuantity != ROUND(#Prepare.SaleQuantity , 0) AND #Prepare.FractionalAllowed = 0)
) > 0
THROW 51000, 'WarehouseQuantity не может быть отрицательным', 1;

UPDATE Products
SET 
WarehouseQuantity=ResultWarehouseQuantity
FROM Products
INNER JOIN
#Prepare
ON #Prepare.Id = Products.Id

END
GO
