CREATE VIEW [dbo].[Sektori_UZZPROSIF] AS
	  SELECT 
	  [Naziv]
      
      
	  FROM [UZZPROSIF].[dbo].[Projekti]
GO

CREATE TABLE [def].[Sektor] (
Id int not null PRIMARY KEY IDENTITY (1, 1),
Naziv nvarchar(60) NULL,
)

ALTER TABLE [data].[Dokument]
ADD SectorId int
CONSTRAINT FK_dataDokument_SectorId
FOREIGN KEY (SectorId) REFERENCES [def].[Sektor](Id);

insert into [def].[Sektor]
select * from [dbo].[Sektori_UZZPROSIF]


