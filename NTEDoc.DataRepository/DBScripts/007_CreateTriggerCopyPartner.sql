USE [UZZPROSIF]
GO

/****** Object:  Trigger [dbo].[copy_Partner]    Script Date: 3/31/2020 2:04:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[copy_Partner]
   ON [dbo].[FIRME] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @IDFirme int, @Konto varchar(13), @naziv nvarchar(50), @Mesto nvarchar(20),@adresa nvarchar(42), @PIB nvarchar(13), @Maticni nvarchar(13),
	@ziro nvarchar(30), @Telefon nvarchar(20), @ptt nvarchar(5), @Opstina char(3)

	SELECT @IDFirme = IDFirme, @Konto = Konto, @naziv = naziv, @Mesto=Mesto, @adresa = adresa, @PIB = PIB, @Maticni = Maticni,
	@ziro = ziro, @Telefon = Telefon,  @ptt = ptt, @Opstina = Opstina from inserted 

	IF EXISTS(SELECT * FROM DELETED)
	BEGIN
	UPDATE [NTEDoc].[data].[Partner]
	SET Konto = @Konto, naziv = @naziv, Mesto = @Mesto, Adresa = @adresa, PIB = @PIB, Maticni = @Maticni, ziro = @ziro, Telefon = @Telefon,
	 ptt = @ptt, Opstina = @Opstina
	WHERE IDFirme = @IDFirme
	END
	else
	BEGIN
	insert into [NTEDoc].[data].[Partner]
	select * from [NTEDoc].[dbo].[Partneri_UZZPROSIF]
	where Konto = @Konto
	END
	
    -- Insert statements for trigger here

END
GO

ALTER TABLE [dbo].[FIRME] ENABLE TRIGGER [copy_Partner]
GO


