CREATE PROCEDURE [dbo].[gopi_AddEmail]
	@EmailData ntext
AS
INSERT INTO [dbo].[gopi_Emails]
 ([dbo].[gopi_Emails].[EmailData])
VALUES
 (@EmailData) 
RETURN 0;