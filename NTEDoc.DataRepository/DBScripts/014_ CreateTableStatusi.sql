update [data].[Dokument]
set Status = null


alter table [data].[Dokument]
drop Column Status

create table def.Statusi (
Id int not null Primary Key identity(1,1),
Name nvarchar(50)
)

alter table [data].[Dokument]
add StatusId int
constraint fk_dataDocument_StatusId
foreign key (StatusId) references [def].[Statusi](Id)

INSERT INTO [def].[Statusi]
           ([Name])
     VALUES
           ('U obradi'),
		   ('Prosledjeno likvidatoru'),
		   ('Likvidator primio'),
		   ('Prosledjeno nadle�nom sektoru'),
		   ('Vra�eno na doradu'),
		   ('Vra�eno dobavlja�u'),
		   ('Odobreno'),
		   ('Odbijeno')
GO
