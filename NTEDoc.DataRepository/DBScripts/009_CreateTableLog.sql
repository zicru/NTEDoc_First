CREATE TABLE data.Log (
    Id int NOT NULL IDENTITY(1,1),
    ChangeType nvarchar(50) ,
    CreatedBy nvarchar(50),
	CreatedDate datetime,
	DocumentId int,
    PRIMARY KEY (Id),
    CONSTRAINT FK_DocumentId FOREIGN KEY (DocumentId)
    REFERENCES [data].[Dokument](Id)
);


  alter table [NTEDoc].[data].[Log]
  add Comment nvarchar(512)