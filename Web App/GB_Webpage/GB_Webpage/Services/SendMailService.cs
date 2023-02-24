using GB_Webpage.Models;
using System.Net;
using System.Net.Mail;

namespace GB_Webpage.Services
{
    public class SendMailService
    {

        private readonly string _emailKey;
        private readonly string _emailFormProvider;
        private readonly string _emailRecivesForm;
        private readonly ContactModel _contact;

        public SendMailService(string emailKey, string emailFormProvider, string emailRecivesForm, ContactModel contact)
        {
            _emailKey = emailKey ?? throw new ArgumentNullException(nameof(_emailKey));
            _emailFormProvider = emailFormProvider ?? throw new ArgumentNullException(nameof(emailFormProvider));
            _emailRecivesForm = emailRecivesForm ?? throw new ArgumentNullException(nameof(emailRecivesForm));
            _contact = contact ?? throw new ArgumentNullException(nameof(contact));
        }

        public Task<bool> sendMail() {

            try
            {

                var fromAddress = new MailAddress(_emailFormProvider, "Kontakc kancelaria");
                var toAddress = new MailAddress(_emailRecivesForm, "Użytkoniwk kancelarii");
                string fromPassword = _emailKey;
                string subject = $"soba {_contact.Name} napisał/a wiadomość.";
                string htmlContent =
                    "<div style=\"font-family: Oxygen, sans-serif\">" +
                        "<h1 style=\"font-size: 5ex;\">Formularz kontaktowy</h1>" +
                        "<div>" +
                            $"<p style=\"font-size: 2ex; margin-top: 3ex\">Osoba {_contact.Name} napisał/a wiadomość o treści:</p>" +
                            $"<i><p style=\"font-size: 1.6ex;\"> {_contact.Message} </p></i>" +
                            "<p style=\"font-size: 2ex; margin-top: 7ex\">Dane kontaktowe tej osoby to:</p>" +
                            "<b><p>Imię i nazwisko: </p></b>" +
                            $"<p> {_contact.Name} </p><b>" +
                            "<p>Adres e-mail: </p></b>" +
                            $"<p> {_contact.Email} </p>" +
                        "</div>" +
                    "</div>";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = htmlContent,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}