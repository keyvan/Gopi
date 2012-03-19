using System;
using System.Configuration.Provider;
using System.Net.Mail;

namespace Gopi.Provider
{
    public abstract class DataProvider : ProviderBase
    {
        public abstract void AddEmail(MailMessage email);

        public abstract MailCollection GetEmails(int emailsCount);

        public abstract void RemoveEmail(Guid id);
    }
}
