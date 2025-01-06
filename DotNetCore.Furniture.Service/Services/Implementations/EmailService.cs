using DotNetCore.Furniture.Domain.Common.Generics;
using DotNetCore.Furniture.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DotNetCore.Furniture.Service.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<NewResult<string>> SendActivationEmail(string emailAddress, string verificationCode)
        {
            NewResult<string> result = new NewResult<string>();
            try
            {
                // Get SMTP server settings from appsettings
                var smtpServer = configuration["EmailSettings:SmtpHost"];
                var port = int.Parse(configuration["EmailSettings:SmtpPort"]);
                var username = configuration["EmailSettings:SmtpUser"];
                var password = configuration["EmailSettings:SmtpPass"];

                using (var client = new SmtpClient())
                {
                    // Specify SMTP server settings
                    client.Host = smtpServer;
                    client.Port = port;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(username, password);
                    client.EnableSsl = true;

                    // Create and configure the email message
                    var message = new MailMessage();
                    message.From = new MailAddress(username); // Sender email address
                    message.To.Add(emailAddress);
                    message.Subject = "Account Activation"; // Email subject
                    message.Body = $"Please click the following link to activate your account: {verificationCode}"; // Email body

                    // Send email asynchronously
                    await client.SendMailAsync(message);

                }
                return NewResult<string>.Success(null, "Email sent successfully");
            }
            catch (Exception)
            {

                return NewResult<string>.Failed(null, "unable to send email");
            }
        }
    }
}
