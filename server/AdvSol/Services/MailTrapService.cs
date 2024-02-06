using System.Net.Mail;
using System.Net;
using AdvSol.Utils;

namespace AdvSol.Services
{
    public interface IMailTrapService
    {
        void SendEmail(string from, string to, string title, string body);
    }

    public class MailTrapService : IMailTrapService
    {
        private string _id;
        private string _password;

        public MailTrapService(IConfiguration configuration)
        {
            _id = configuration.GetValue<string>("MailTrapId");
            _password = configuration.GetValue<string>("MailTrapPwd");
        }

        public void SendEmail(string from, string to, string title, string body)
        {
            if (_id.IsEmpty() || _password.IsEmpty())
                return;

            using var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential(_id, _password),
                EnableSsl = true
            };

            client.Send(from, to, title, body);           
        }
    }
}
