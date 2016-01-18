using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace D3Helper.A_SMTPClient
{
    class SMTPClient
    {
        private const string SMTPHost = "";
        private const string MailFrom = "";
        private const string MailFromPW = "";
        private const string MailTo = "";
        private const string MailHeader = "";

        public static void sendMail(string BodyText)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = SMTPHost;
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(MailFrom, MailFromPW);

                MailMessage mm = new MailMessage(MailFrom, MailTo, MailHeader, BodyText);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                client.Send(mm);
            }
            catch { }
        }
    }
}
