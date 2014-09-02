using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseCVREmails.Lib
{
    public class EmailData 
    {
        /// <summary>
        /// The raw headers of the email.
        /// </summary>
        public string Headers { get; set; }
        /// <summary>
        /// Text body of email. If not set, email did not have a text body.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// HTML body of email. If not set, email did not have an HTML body.
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// Email sender, as taken from the message headers.
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Email recipient field, as taken from the message headers.
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Email cc field, as taken from the message headers.
        /// </summary>
        public string Cc { get; set; }
        /// <summary>
        /// Email Subject.
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// A JSON string containing the verification results of any dkim and domain keys signatures in the message.
        /// </summary>
        public string Dkim { get; set; }
        /// <summary>
        /// The results of the Sender Policy Framework verification of the message sender and receiving IP address.
        /// </summary>
        public string SPF { get; set; }
        /// <summary>
        /// A JSON string containing the SMTP envelope. This will have two variables: *to*, which is a single-element array containing the address that we recieved the email to, and *from*, which is the return path for the message.
        /// </summary>
        public string Envelope { get; set; }
        /// <summary>
        /// A JSON string containing the character sets of the fields extracted from the message.
        /// </summary>
        public string Charset { get; set; }
        /// <summary>
        /// Spam Assassin’s rating for whether or not this is spam.
        /// </summary>
        public string Spam_score { get; set; }
        /// <summary>
        /// Spam Assassin’s spam report.
        /// </summary>
        public string Spam_report { get; set; }
        /// <summary>
        /// Number of attachments included in email.
        /// </summary>
        public string Attachments { get; set; }
        /// <summary>
        /// A JSON string containing the attachmentX (see below) keys with another JSON string as the value. This string will contain the keys *filename*, which is the name of the file and *type*, which is the [media type](http://en.wikipedia.org/wiki/Internet_media_type) of the file.
        /// </summary>
        public string Attachmentinfo { get; set; }
        /// <summary>
        /// These are file upload names, where N is the total number of attachments. For example, if the number of attachments is 0, there will be no attachment files. If the number of attachments is 3, parameters attachment1, attachment2, and attachment3 will have file uploads. Attachments provided with this parameter, are provided in the form of file uploads. TNEF files (winmail.dat) will be extracted and have any attachments posted.
        /// </summary>
        public string AttachmentX { get; set; }

        public List<string> Files { get; set; }
    }
}
