CREATE TABLE [dbo].[gopi_Emails]
(
	ID uniqueidentifier ROWGUIDCOL  NOT NULL DEFAULT (newid()), 
	DateAdded datetime NOT NULL DEFAULT ({fn now()}),
	EmailData ntext NOT NULL
);
