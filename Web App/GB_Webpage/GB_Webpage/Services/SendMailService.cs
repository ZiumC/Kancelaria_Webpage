using GB_Webpage.Models;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;

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

                var fromAddress = new MailAddress(_emailFormProvider, "Kontakt kancelaria");
                var toAddress = new MailAddress(_emailRecivesForm, "Użytkownik kancelarii");
                string fromPassword = _emailKey;
                string subject = $"Osoba {_contact.Name} napisał/a wiadomość.";
                string htmlContent =
                     "<div style=\"padding: 5ex 10ex 5ex 10ex; border-radius: 2ex;\">" +
                        "<center> <h1 style=\"font-size: 5ex;\">Formularz kontaktowy</h1></center>" +
                        "<b><p style=\"margin-top: 4ex; margin-bottom: 1ex; font-size: 3ex;\">Osoba: Janusz napisał/a wiadomość o treści:</p></b>" +
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