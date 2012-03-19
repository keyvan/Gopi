CREATE PROCEDURE [dbo].[gopi_GetEmails]
	@Count int = -1
AS

IF (@Count = -1)
 BEGIN
 SELECT
 [dbo].[gopi_Emails].[ID],
 [dbo].[gopi_Emails].[DateAdded],
 [dbo].[gopi_Emails].[EmailData]
  FROM  [dbo].[gopi_Emails]
  ORDER BY DateAdded DESC
 END
ELSE
 BEGIN
 SET ROWCOUNT @Count
 SELECT
 [dbo].[gopi_Emails].[ID],
 [dbo].[gopi_Emails].[DateAdded],
 [dbo].[gopi_Emails].[EmailData]
  FROM  [dbo].[gopi_Emails]
  ORDER BY DateAdded DESC
 END

RETURN 0;