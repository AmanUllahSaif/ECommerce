using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace ECommerce.UI.Util
{
    public class EmailManager
    {
        /// <summary>
        /// Custom Email Sending
        /// </summary>
        /// <param name="to">Email address to sent email</param>
        /// <param name="message">Message content to send</param>
        /// <param name="subject">Subject of Mail</param>

        public static void SendMail(string to, string message, string subject)
        {
            string from = ConfigurationManager.AppSettings["from"].ToString();
            string host = ConfigurationManager.AppSettings["host"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

            MailMessage mail = new MailMessage(from, to);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Port = port;
            client.Host = host;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(from, password);
            client.Send(mail);
        }

        public static void SendNewOrderEmail(long Id, string to, decimal amount)
        {
            string message = "You have placed an order. Order number is " +Id+" and amount is "+ amount;
            SendMail(to, message, "New order");
        }

        public static void SendInprocessMail(long Id, string to)
        {
            string message = "Your Order Number" + Id + " is in-proccess. ";
            SendMail(to, message, "Order In-Proccess");
        }

        public static void SendRejectMail(long Id, string to)
        {
            string message = "Your Order Number" + Id + " is Rejected by admin. ";
            SendMail(to, message, "Order Rejected");
        }

        public static void SendOrderReturnedMail(long Id, string to)
        {
            string message = "Your Order Number" + Id + " is return. ";
            SendMail(to, message, "Order Returned");
        }
        public static void SendOrderShippedMail(long Id, string to)
        {
            string message = "Your Order Number" + Id + " is shipped. ";
            SendMail(to, message, "Order Returned");
        }

    }
}