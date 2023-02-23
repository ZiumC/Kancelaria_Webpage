using GB_Webpage.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GB_Webpage.Services
{
    public class SendMailService
    {

        private readonly string _apiKey;
        private readonly string _emailFormProvider;
        private readonly string _emailRecivesForm;
        private readonly ContactModel _contact;

        public SendMailService(string apiKey, string emailFormProvider, string emailRecivesForm, ContactModel contact)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _emailFormProvider = emailFormProvider ?? throw new ArgumentNullException(nameof(emailFormProvider));
            _emailRecivesForm = emailRecivesForm ?? throw new ArgumentNullException(nameof(emailRecivesForm));
            _contact = contact ?? throw new ArgumentNullException(nameof(contact));
        }

        public bool sendMail() {

            try
            {
                var client = new SendGridClient(_apiKey);

                var from = new EmailAddress(_emailFormProvider, "Kontakt Kancelaria");
                var to = new EmailAddress(_emailRecivesForm, "Użytkownik");

                var subject = $"Formularz - osoba {_contact.Name} napisał/a wiadomość.";
                var plainTextContent = "Form";
                var htmlContent = 
                    "<div style=\"font-family: Oxygen, sans-serif\">" +
                        "<center><h1 style=\"font-size: 5ex;\">Formularz kontaktowy</h1></center>" +
                        "<div>" +
                            $"<p style=\"font-size: 3ex; margin-top: 3ex\">Osoba {_contact.Name} napisał/a wiadomość o treści:</p>" +
                            $"<center><i><p> {_contact.Message} </p></i></center>" +
                            "<p style=\"font-size: 3ex; margin-top: 7ex\">Dane kontaktowe tej osoby to:</p>" +
                            "<center>" +
                            "<b><p>Imię i nazwisko: </p></b>" +
                            $"<p> {_contact.Name} </p><b>" +
                            "<p>Adres e-mail: </p></b>" +
                            $"<p> {_contact.Email} </p>" +
                            "</center>" +
                        "</div>" +
                    "</div>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                var response = client.SendEmailAsync(msg);

                Console.WriteLine(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("error?" + e);
                return false;
            }

            return true;
        }
    }
}