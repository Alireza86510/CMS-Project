using System.Net;
using System.Net.Mail;
using System.Text;

namespace CMS.Core.Senders;

public class EmailSender
{
    public static bool SendEmail(string user, string subject, string body)
    {
        try
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("alireza.khosravi.1386.5.10@gmail.com", "jroc eywl fjkg pkln");

            MailAddress from = new MailAddress("alireza.khosravi.1386.5.10@gmail.com");
            MailAddress to = new MailAddress(user);
            MailMessage message = new MailMessage(from, to);
            message.Body = body;
            message.Subject = subject;
            message.IsBodyHtml = true;
            client.Send(message);
            message.Dispose();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}