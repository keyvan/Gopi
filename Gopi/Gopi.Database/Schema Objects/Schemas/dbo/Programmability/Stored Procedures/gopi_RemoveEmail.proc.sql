CREATE PROCEDURE [dbo].[gopi_RemoveEmail]
	@EmailID uniqueidentifier
AS
DELETE FROM [dbo].[gopi_Emails] WHERE ID = @EmailID

RETURN 0;