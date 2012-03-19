using System.Net;
using System.Net.Mail;
using Gopi.Provider;

namespace Gopi
{
    /// <summary>
    /// Represents the main class in Gopi component
    /// </summary>
    public class Sender
    {
        #region Private Members

        private string _server;
        private string _username;
        private string _password;
        private int _timeOut = -1;
        private int _port = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Public constructor
        /// </summary>
        public Sender()
        {
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="server">SMTP server</param>
        public Sender(string server)
        {
            this._server = server;
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="server">SMTP server</param>
        /// <param name="username">SMTP username</param>
        /// <param name="password">SMTP password</param>
        public Sender(string server, string username, string password)
        {
            this._server = server;
            this._username = username;
            this._password = password;
        }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="server">SMTP server</param>
        /// <param name="username">SMTP username</param>
        /// <param name="password">SMTP password</param>
        /// <param name="timeOut">Operation timeout</param>
        /// <param name="port">SMTP port</param>
        public Sender(string server, string username, string password, int timeOut, int port)
        {
            this._server = server;
            this._username = username;
            this._password = password;
            this._timeOut = timeOut;
            this._port = port;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an email to database
        /// </summary>
        /// <param name="email">Email to add</param>
        public void AddNewEmail(MailMessage email)
        {
            DataProviderManager.Provider.AddEmail(email);
        }

        /// <summary>
        /// Sends all emails stored in database
        /// </summary>
        /// <returns>Number of emails that are sent successfully</returns>
        public int SendAllEmails()
        {
            return SendQueue(-1);
        }

        /// <summary>
        /// Sends a specified number of emails stored in database
        /// </summary>
        /// <param name="emailsCount">Number of emails to send</param>
        /// <returns>Number of emails that are sent successfully</returns>
        public int SendAllEmails(int emailsCount)
        {
            return SendQueue(emailsCount);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sends specified number of emails stored in the queue
        /// </summary>
        /// <param name="emailsCount">Number of emails to send</param>
        /// <returns>Number of emails that are sent successfully</returns>
        private int SendQueue(int emailsCount)
        {
            int count = 0;

            MailCollection emails = DataProviderManager.Provider.GetEmails(emailsCount);

            foreach (SerializableMailMessage email in emails)
            {
                if (SendMail(email.Email))
                {
                    count++;
                    DataProviderManager.Provider.RemoveEmail(email.ID);
                }
            }

            return count;
        } 

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="email">Email to send</param>
        /// <returns>Boolean value specifying if the email could be sent</returns>
        private bool SendMail(MailMessage email)
        {
            try
            {
                SmtpClient objSmtp = new SmtpClient(this._server);

                objSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (this._username != null && this._password != null)
                {
                    NetworkCredential objCredential = new NetworkCredential(this._username, this._password);
                    objSmtp.UseDefaultCredentials = false;
                    objSmtp.Credentials = objCredential;
                }

                if (this._port != -1)
                    objSmtp.Port = this._port;

                if (this._timeOut != -1)
                    objSmtp.Timeout = this._timeOut;

                objSmtp.Send(email);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
