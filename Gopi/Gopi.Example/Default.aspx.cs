using System;
using System.Net.Mail;

namespace Gopi.Example
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddEmail_Click(object sender, EventArgs e)
        {
            MailMessage email = new MailMessage();
            email.From = new MailAddress("i@keyvan.fm", "Keyvan");

            email.To.Add(new MailAddress("i@keyvan.io", "Keyvan Nayyeri"));

            email.Subject = "Test Email";
            email.Body = "<p>This is a test email for serialization!</p>";

            email.IsBodyHtml = true;
            email.Priority = MailPriority.High;

            email.Attachments.Add(new Attachment(Server.MapPath("~/Image.jpg")));

            Sender gopiSender = new Sender();
            gopiSender.AddNewEmail(email);

            lblResult.Text = "Email Added!";
        }

        protected void btnSendAll_Click(object sender, EventArgs e)
        {
            Sender gopiSender = new Sender("mail.nayyeri.net", "keyvan@nayyeri.net", "password");
            gopiSender.SendAllEmails();
            lblResult.Text = "Emails Sent!";
        }
    }
}
