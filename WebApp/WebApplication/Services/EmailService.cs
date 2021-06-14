using System;
using System.Net.Mail;

namespace WebApplication.Services
{
    public class EmailService : IEmailService
    {
        public bool SendNewPasswordEmail(string emailAddress, string password)
        {
            var mail = new MailMessage();
            mail.To.Add(emailAddress);
            mail.From = new MailAddress("vdg.warehouse.logistic@gmail.com", "VDG logistic", System.Text.Encoding.UTF8);
            mail.Subject = "VDG logistic - password reset";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = @"<html>
                      <body>
                      <p>Dear Mr/Ms. ,</p>
                      <p> Here is your new password: " + password + @"</p>
                      <p>Please use it to login and change it right after. </br></p>
                      <p>Sincerely,<br>-Your VDG logistic IT support</br></p>
                      </body>
                      </html>
                     ";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            var client = new SmtpClient
            {
                Credentials =
                    new System.Net.NetworkCredential("vdg.warehouse.logistic@gmail.com", "vdglogistic123!"),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true
            };
            try
            {
                client.Send(mail);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}