 alter table [dbo].[AspNetUsers]
  add SektorId int 
  constraint FK_AspNetUsers_SektorId
  foreign key (SektorId) references [def].[Sektor](Id);