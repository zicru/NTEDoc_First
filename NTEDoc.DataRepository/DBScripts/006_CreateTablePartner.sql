CREATE VIEW [dbo].[Partneri_UZZPROSIF] AS
	  SELECT 
	  [IDFirme]
	  ,[Konto]
      ,[naziv]
      ,[Mesto]
      ,[Adresa]
      ,[PIB]
      ,[Maticni]
      ,[ziro]
      ,[Telefon]
      ,[ptt]
      ,[Opstina]
      
	  FROM [UZZPROSIF].[dbo].[FIRME]
GO

 CREATE TABLE [data].[Partner](
	[Id] [int] IDENTITY(1,1) primary key NOT NULL,
	[IDFIrme] [int] NOT NULL,
	[Konto] [varchar](13) NULL,
	[naziv] [nvarchar](50) NULL,
	[Mesto] [nvarchar](20) NULL,
	[Adresa] [nvarchar](42) NULL,
	[PIB] [nvarchar](13) NULL,
	[Maticni] [nvarchar](13) NULL,
	[ziro] [nvarchar](30) NULL,
	[Telefon] [nvarchar](20) NULL,
	[ptt] [nvarchar](5) NULL,
	[Opstina] [char](3) NULL)

  ALTER TABLE [data].[Dokument]
  ADD PartnerId int
  CONSTRAINT FK_dataDocument_PartnerId
  FOREIGN KEY (PartnerId) REFERENCES [data].[Partner](Id)

  INSERT INTO [NTEDoc].[data].[Partner]
  SELECT * FROM [dbo].[Partneri_UZZPROSIF]