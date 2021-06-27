using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailService
{
    public class SendGridMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }

        public SendGridMessage(string to, string subject, string plainTextContent, string htmlContent)
        {
            To = to;
            Subject = subject;
            PlainTextContent = plainTextContent;
            HtmlContent = htmlContent;
        }
    }
}
