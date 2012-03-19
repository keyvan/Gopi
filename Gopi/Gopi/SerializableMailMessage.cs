using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Gopi
{
    /// <summary>
    /// Represents a serializable mail message.
    /// </summary>
    public class SerializableMailMessage : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Identifier
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Main MailMessage object
        /// </summary>
        public MailMessage Email { get; set; }

        /// <summary>
        /// Date and time when this object has been added to database
        /// </summary>
        public DateTime DateAdded { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Public consturctor
        /// </summary>
        public SerializableMailMessage()
        {
            this.Email = new MailMessage();
        }

        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// Implementation of GetSchema for serialization
        /// </summary>
        /// <returns>An XML schema</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Implementation of ReadXml for serialization
        /// </summary>
        /// <param name="reader">XmlReader that contains the data of serialized object</param>
        public void ReadXml(XmlReader reader)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(reader);

            // Properties
            XmlNode rootNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage");
            this.Email.IsBodyHtml = Convert.ToBoolean(rootNode.Attributes["IsBodyHtml"].Value);
            this.Email.Priority = (MailPriority)Convert.ToInt16(rootNode.Attributes["Priority"].Value);

            // From
            XmlNode fromNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/From");
            string fromDisplayName = string.Empty;
            if (fromNode.Attributes["DisplayName"] != null)
                fromDisplayName = fromNode.Attributes["DisplayName"].Value;
            MailAddress fromAddress = new MailAddress(fromNode.InnerText, fromDisplayName);
            this.Email.From = fromAddress;

            // To
            XmlNode toNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/To/Addresses");
            foreach (XmlNode node in toNode.ChildNodes)
            {
                string toDisplayName = string.Empty;
                if (node.Attributes["DisplayName"] != null)
                    toDisplayName = node.Attributes["DisplayName"].Value;
                MailAddress toAddress = new MailAddress(node.InnerText, toDisplayName);
                this.Email.To.Add(toAddress);
            }

            // CC
            XmlNode ccNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/CC/Addresses");
            if (ccNode != null)
            {
                foreach (XmlNode node in ccNode.ChildNodes)
                {
                    string ccDisplayName = string.Empty;
                    if (node.Attributes["DisplayName"] != null)
                        ccDisplayName = node.Attributes["DisplayName"].Value;
                    MailAddress ccAddress = new MailAddress(node.InnerText, ccDisplayName);
                    this.Email.CC.Add(ccAddress);
                }
            }

            // Bcc
            XmlNode bccNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/Bcc/Addresses");
            if (bccNode != null)
            {
                foreach (XmlNode node in bccNode.ChildNodes)
                {
                    string bccDisplayName = string.Empty;
                    if (node.Attributes["DisplayName"] != null)
                        bccDisplayName = node.Attributes["DisplayName"].Value;
                    MailAddress bccAddress = new MailAddress(node.InnerText, bccDisplayName);
                    this.Email.Bcc.Add(bccAddress);
                }
            }

            // Subject
            XmlNode subjectNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/Subject");
            this.Email.Subject = subjectNode.InnerText;

            // Body
            XmlNode bodyNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/Body");
            this.Email.Body = bodyNode.InnerText;

            // ReplyTo
            XmlNode replyToNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/ReplyTo");
            if (replyToNode != null)
            {
                string replytoDisplayName = string.Empty;
                if (replyToNode.Attributes["DisplayName"] != null)
                    replytoDisplayName = replyToNode.Attributes["DisplayName"].Value;
                MailAddress replytoAddress = new MailAddress(replyToNode.InnerText, replytoDisplayName);
                this.Email.ReplyTo = replytoAddress;
            }

            // Sender
            XmlNode senderNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/Sender");
            if (senderNode != null)
            {
                string senderDisplayName = string.Empty;
                if (senderNode.Attributes["DisplayName"] != null)
                    senderDisplayName = senderNode.Attributes["DisplayName"].Value;
                MailAddress senderAddress = new MailAddress(senderNode.InnerText, senderDisplayName);
                this.Email.Sender = senderAddress;
            }

            // Attachments
            XmlNode attachmentsNode = GetConfigSection(xml, "SerializableMailMessage/MailMessage/Attachments");
            if (attachmentsNode != null)
            {
                foreach (XmlNode node in attachmentsNode.ChildNodes)
                {
                    string contentTypeString = string.Empty;
                    if (node.Attributes["ContentType"] != null)
                        contentTypeString = node.Attributes["ContentType"].Value;

                    ContentType contentType = new ContentType(contentTypeString);

                    MemoryStream stream = new MemoryStream();
                    byte[] data = Encoding.UTF8.GetBytes(node.InnerText);
                    stream.Write(data, 0, data.Length);

                    Attachment attachment = new Attachment(stream, contentType);
                    this.Email.Attachments.Add(attachment);
                }
            }
        }

        /// <summary>
        /// Implementation of WriteXml for serialization
        /// </summary>
        /// <param name="writer">XmlWriter that can be used to serialize the object</param>
        public void WriteXml(XmlWriter writer)
        {
            if (this.Email != null)
            {
                writer.WriteStartElement("MailMessage");
                writer.WriteAttributeString("Priority", Convert.ToInt16(this.Email.Priority).ToString());
                writer.WriteAttributeString("IsBodyHtml", this.Email.IsBodyHtml.ToString());

                // From
                writer.WriteStartElement("From");
                if (!string.IsNullOrEmpty(this.Email.From.DisplayName))
                    writer.WriteAttributeString("DisplayName", this.Email.From.DisplayName);
                writer.WriteRaw(this.Email.From.Address);
                writer.WriteEndElement();

                // To
                writer.WriteStartElement("To");
                writer.WriteStartElement("Addresses");
                foreach (MailAddress address in this.Email.To)
                {
                    writer.WriteStartElement("Address");
                    if (!string.IsNullOrEmpty(address.DisplayName))
                        writer.WriteAttributeString("DisplayName", address.DisplayName);
                    writer.WriteRaw(address.Address);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();

                // CC
                if (this.Email.CC.Count > 0)
                {
                    writer.WriteStartElement("CC");
                    writer.WriteStartElement("Addresses");
                    foreach (MailAddress address in this.Email.CC)
                    {
                        writer.WriteStartElement("Address");
                        if (!string.IsNullOrEmpty(address.DisplayName))
                            writer.WriteAttributeString("DisplayName", address.DisplayName);
                        writer.WriteRaw(address.Address);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                // Bcc
                if (this.Email.Bcc.Count > 0)
                {
                    writer.WriteStartElement("Bcc");
                    writer.WriteStartElement("Addresses");
                    foreach (MailAddress address in this.Email.Bcc)
                    {
                        writer.WriteStartElement("Address");
                        if (!string.IsNullOrEmpty(address.DisplayName))
                            writer.WriteAttributeString("DisplayName", address.DisplayName);
                        writer.WriteRaw(address.Address);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                // Subject
                writer.WriteStartElement("Subject");
                writer.WriteRaw(this.Email.Subject);
                writer.WriteEndElement();

                // Body
                writer.WriteStartElement("Body");
                writer.WriteCData(this.Email.Body);
                writer.WriteEndElement();

                // ReplyTo
                if (this.Email.ReplyTo != null)
                {
                    writer.WriteStartElement("ReplyTo");
                    if (!string.IsNullOrEmpty(this.Email.ReplyTo.DisplayName))
                        writer.WriteAttributeString("DisplayName", this.Email.ReplyTo.DisplayName);
                    writer.WriteRaw(this.Email.ReplyTo.Address);
                    writer.WriteEndElement();
                }

                // Sender
                if (this.Email.Sender != null)
                {
                    writer.WriteStartElement("Sender");
                    if (!string.IsNullOrEmpty(this.Email.Sender.DisplayName))
                        writer.WriteAttributeString("DisplayName", this.Email.Sender.DisplayName);
                    writer.WriteRaw(this.Email.Sender.Address);
                    writer.WriteEndElement();
                }

                // Attachments
                if (this.Email.Attachments.Count > 0)
                {
                    writer.WriteStartElement("Attachments");

                    foreach (Attachment attachment in this.Email.Attachments)
                    {
                        writer.WriteStartElement("Attachment");

                        if (!string.IsNullOrEmpty(attachment.Name))
                            writer.WriteAttributeString("ContentType", attachment.ContentType.ToString());

                        using (BinaryReader reader = new BinaryReader(attachment.ContentStream))
                        {
                            byte[] data = reader.ReadBytes((int)attachment.ContentStream.Length);

                            writer.WriteBase64(data, 0, data.Length);
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// REturns a specific node in the serialized XML
        /// </summary>
        /// <param name="xml">XML document</param>
        /// <param name="nodePath">XPath expression for the node path</param>
        /// <returns>An XML node</returns>
        private XmlNode GetConfigSection(XmlDocument xml, string nodePath)
        {
            return xml.SelectSingleNode(nodePath);
        }

        #endregion
    }
}
