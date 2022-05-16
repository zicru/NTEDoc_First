create table Likvidatori (
Id int not null primary key  identity(1,1),
FirstName nvarchar(50),
LastName nvarchar(50)
)

 alter table [data].[Dokument]
  add LikvidatorId int
  constraint FK_dataDocument_LikvidatorId
  foreign key (LikvidatorId) references [dbo].[Likvidatori](Id)

  alter table [dbo].[Likvidatori]
  add UserId nvarchar(450)

  alter table [dbo].[Likvidatori]
  add ExtId int