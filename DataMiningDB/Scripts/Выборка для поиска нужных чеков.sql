
declare @id as int
set @id = 284

SELECT TOP (100) [SaleRows].SaleId, Products.*
from 
(
SELECT  [SaleId]
      ,[ProductId]
  FROM [MyboxDb].[dbo].[SaleRows]
  where [ProductId] = @id
) focused
join [SaleRows]
on [SaleRows].SaleId = focused.SaleId
join Products
on [SaleRows].ProductId = Products.Id
