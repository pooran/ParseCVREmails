using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace ParseCVREmails.Controllers
{
    public class EmailController : ApiController
    {
        private readonly QueueManager _queueManager = new QueueManager();
        //public async Task<HttpResponseMessage> PostFormData()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

            
        //    var provider = new MultipartFormDataMemoryStreamProvider();

        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        // This illustrates how to get the file names.
        //        foreach (HttpContent file in provider.Files)
        //        {
        //            Trace.WriteLine(file.Headers.ContentDisposition.FileName);                    
        //        }

        //        // Show all the key-value pairs.
        //        foreach (var key in provider.FormData.AllKeys)
        //        {
        //            foreach (var val in provider.FormData.GetValues(key))
        //            {
        //                Trace.WriteLine(string.Format("{0}: {1}", key, val));
        //            }
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    catch (System.Exception e)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //    }
        //}

        public async Task<HttpResponseMessage> Post([MultipartFormData(typeof(EmailData))] EmailData data)
        {
            await _queueManager.SendMessage(Map(data), await _queueManager.GetQueue("emails"));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private ParseCVREmails.Lib.EmailData Map(EmailData data)
        {
            return new Lib.EmailData()
            {
                AttachmentX = data.AttachmentX,
                Attachmentinfo = data.Attachmentinfo,
                Attachments = data.Attachments,
                Cc = data.Cc,
                Charset = data.Charset,
                Dkim = data.Dkim,
                Envelope = data.Envelope,
                Files = new List<string>(),
                From = data.From,
                Headers = data.Headers,
                Html = data.Html,
                SPF = data.SPF,
                Spam_report = data.Spam_report,
                Spam_score = data.Spam_score,
                Subject = data.Subject,
                Text = data.Text,
                To = data.To
            };
        }
    }

    public class EmailData : MultipartFormData
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
    }

    public abstract class MultipartFormData
    {
        public List<HttpContent> Files { get; set; }
    }
}
