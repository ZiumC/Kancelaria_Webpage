using GB_Webpage.Models;
using System.Net;
using System.Net.Mail;

namespace GB_Webpage.Services
{
    public class SendMailService
    {

        private readonly string _emailKey;
        private readonly string _emailSendsForm;
        private readonly string _emailRecivesForm;
        private readonly ContactModel _contact;
        private readonly ILogger _logger;

        public SendMailService(string emailKey, string emailSendsForm, string emailRecivesForm, ContactModel contact, ILogger logger)
        {
            _emailKey = emailKey ?? throw new ArgumentNullException(nameof(_emailKey));
            _emailSendsForm = emailSendsForm ?? throw new ArgumentNullException(nameof(emailSendsForm));
            _emailRecivesForm = emailRecivesForm ?? throw new ArgumentNullException(nameof(emailRecivesForm));
            _contact = contact ?? throw new ArgumentNullException(nameof(contact));
            _logger = logger;
        }

        private SmtpClient MakeClient(MailAddress emailFrom, string emailKey)
        {

            return new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailFrom.Address, emailKey)
            };

        }

        private MailMessage MakeMessage(string subject, string body, MailAddress addressFrom, MailAddress addresTo)
        {
            return new MailMessage(addressFrom, addresTo)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
        }

        public bool SendMailAsync()
        {
            try
            {
                MailAddress addressFrom = new MailAddress(_emailSendsForm, "Kontakt kancelaria");
                MailAddress addressTo = new MailAddress(_emailRecivesForm, "Użytkownik kancelarii");
                SmtpClient client = MakeClient(addressFrom, _emailKey);

                string subject = $"Osoba {_contact.Name} napisał/a wiadomość.";
                string htmlContent =
                     "<div style=\"padding: 5ex 10ex 5ex 10ex; border-radius: 2ex;\">" +
                        "<center> <h1 style=\"font-size: 5ex;\">Formularz kontaktowy</h1></center>" +
                        $"<b><p style=\"margin-top: 4ex; margin-bottom: 1ex; font-size: 3ex;\">{_contact.Name} napisał/a wiadomość o treści:</p></b>" +
                        "<table style=\"width:auto; text-align: left; content-align: left; font-size: 3ex;\">" +
                            "<tr>" +
                                "<th></th>" +
                                $"<td><i><p>{_contact.Message}</p></i></td>" +
                            "</tr>" +
                        "</table>" +
                        "<p style=\"margin-top: 10ex; margin-bottom: 1ex; font-size: 3ex;\"><b>Dane kontaktowe tej osoby to:</b></p>" +
                        "<table style=\"width:auto; text-align: left; content-align: left; font-size: 3ex;\">" +
                            "<tr>" +
                                "<th>Imię i nazwisko:</th>" +
                                $"<td>{_contact.Name}</td>" +
                           "</tr>" +
                           "<tr>" +
                                "<th>Aadres e-mail:</th>" +
                                $"<td>{_contact.Email}</td>" +
                           "</tr>" +
                       "</table>" +
                       "<b><p style=\"color: red;\">Wiadomość wygenerowana automatycznie, proszę na nią nie odpowiadać.</p></b>" +
                  "</div>";


                using (MailMessage message = MakeMessage(subject, htmlContent, addressFrom, addressTo))
                {
                    client.Send(message);
                }

            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}