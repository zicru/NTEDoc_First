alter table [data].[Dokument]
  add ContractNumber nvarchar(50)

  alter table [data].[Dokument]
  add CurrencyDate date

  alter table [data].[Dokument]
  add Amount decimal(18,2)