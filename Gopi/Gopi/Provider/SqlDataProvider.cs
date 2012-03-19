using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;

namespace Gopi.Provider
{
    public class SqlDataProvider : DataProvider
    {
        private string _connectionString = string.Empty;

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            this._connectionString =
                ConfigurationManager.ConnectionStrings[config["connectionString"]].ConnectionString; ;

            if (string.IsNullOrEmpty(this._connectionString))
                throw new ConfigurationErrorsException
                    ("connectionString must be set to the appropriate value");
        }

        public override void AddEmail(MailMessage email)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                SqlCommand command = new SqlCommand("gopi_AddEmail", connection);
                command.CommandType = CommandType.StoredProcedure;

                SerializableMailMessage mailToSerialize = new SerializableMailMessage();
                mailToSerialize.Email = email;

                XmlSerializer serializer = new XmlSerializer(typeof(SerializableMailMessage));
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, mailToSerialize);

                    command.Parameters.AddWithValue("EmailData",
                        writer.GetStringBuilder().ToString());
                }

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();
            }
        }

        public override MailCollection GetEmails(int emailsCount)
        {
            MailCollection emails = new MailCollection();

            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                SqlCommand command = new SqlCommand("gopi_GetEmails", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("Count", emailsCount);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializableMailMessage));
                    while (reader.Read())
                    {
                        SerializableMailMessage email = new SerializableMailMessage();

                        string stringData = (string)reader["EmailData"];
                        MemoryStream memoryStream = new MemoryStream();
                        byte[] data = Encoding.Unicode.GetBytes(stringData);
                        Encoding.Convert(Encoding.Default, Encoding.UTF8, data);
                        memoryStream.Write(data, 0, data.Length);

                        memoryStream.Position = 0;

                        email = (SerializableMailMessage)serializer.Deserialize(memoryStream);

                        email.ID = new Guid(reader["ID"].ToString());
                        email.DateAdded = (DateTime)reader["DateAdded"];

                        emails.Add(email);
                    }
                }

                connection.Close();
                command.Dispose();
            }

            return emails;
        }

        public override void RemoveEmail(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                SqlCommand command = new SqlCommand("gopi_RemoveEmail", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("EmailID", id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();
            }
        }
    }
}
