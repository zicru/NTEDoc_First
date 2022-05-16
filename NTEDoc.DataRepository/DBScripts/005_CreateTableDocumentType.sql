CREATE VIEW [dbo].[VrsteFaktura_UZZPROSIF] AS
	  SELECT 
	  
	  [NAZIV]
      
      
      
	  FROM [UZZPROSIF].[dbo].[Fakture_vrsta]
GO

 CREATE TABLE def.DocumentType(
  Id int primary key identity(1,1),
  NAZIV nvarchar(256)
  )

  ALTER TABLE [data].[Dokument]
  ADD TypeId int
  CONSTRAINT FK_dataDocument_TypeId
  FOREIGN KEY (TypeId) REFERENCES [def].[DocumentType](Id);

 

   INSERT INTO [def].[DocumentType]
  SELECT *  FROM [dbo].[VrsteFaktura_UZZPROSIF]

  INSERT INTO [def].[DocumentType]
           ([NAZIV])
     VALUES
           ('Prepis raèuna')
GO