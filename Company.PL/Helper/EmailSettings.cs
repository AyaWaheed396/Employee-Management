using System.Net;
using System.Net.Mail;

namespace Company.PL.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("eng.ayawaheedmohamed@gmail.com", "sbildxwpsnqmdhmv");

            client.Send("eng.ayawaheedmohamed@gmail.com", email.To, email.Title, email.Body);

        }
    }
}
