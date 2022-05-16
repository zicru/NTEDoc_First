USE [UZZPROSIF]
GO

/****** Object:  View [dbo].[V_izvodi]    Script Date: 22.5.2020 14:05:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[V_firme_ugovori]
AS
select arhiv_br,f.naziv,f.IDFirme,f.Konto   from ugovori u inner join Ugovori_firme uf on  u.IDUgovora =uf.IDUgovora 
inner join FIRME f on uf.Analitika =f.Konto  where u.Aktivan=1



GO