using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using MiniDropbox.Domain;
using MiniDropbox.Domain.Services;
using NHibernate;
using NHibernate.Linq;

namespace MiniDropbox.Data
{
    public class EmailHandler 
    {

        public string SendEmail(string address, string subject, string message)
        {

            string email = "postmaster@app5907.mailgun.org";
            string password = "3ipcsv86ayd9";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.mailgun.org", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
            return address + "," + subject + "," + message;
        }
    }
}
