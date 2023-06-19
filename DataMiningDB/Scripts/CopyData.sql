INSERT INTO [MyboxDb].[dbo].[UserTypes]
           ([UserTypeName])
SELECT [UserTypeName]
  FROM [DataMiningDB].[dbo].[UserTypes]
GO

INSERT INTO [MyboxDb].[dbo].[Users]
           ([UserName]
           ,[UserPassword]
           ,[UserTypeId])
SELECT [UserName]
      ,[UserPassword]
      ,[UserTypeId]
  FROM [DataMiningDB].[dbo].[Users]
GO

Set Identity_Insert [Products] On

INSERT INTO [dbo].[Products]
           ([Id]
          , [ProductName]
           ,[Cost]
           ,[FractionalAllowed]
           ,[WarehouseQuantity])
SELECT [ИдБлюдо]
      ,[Наименование]
      ,[Цена]
      ,0
      ,20
  FROM [MyboxDb].[dbo].[Products2]
  order by [ИдБлюдо]
           
Set Identity_Insert [Products] Off
GO

Set Identity_Insert [SaleReceipts] On
  
INSERT INTO [dbo].[SaleReceipts]
           ([Id]
           ,[SaleDateTime]
           ,[CashierId])
  SELECT distinct [id_order],
  [date],
  3
  FROM [MyboxDb].[dbo].[Sales]
  order by [id_order]

Set Identity_Insert [SaleReceipts] Off
GO
    
INSERT INTO [dbo].[SaleRows]
           ([SaleId]
           ,[ProductId]
           ,[Quantity]
           ,[TotalCost])
SELECT [id_order]
      ,[id_dish]
      ,1
      ,Products.Cost
  FROM [MyboxDb].[dbo].[Sales]
  join Products
  on Products.Id = [id_dish]
  order by [id_order] 

GO
