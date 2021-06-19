using FamilyTreeBackend.Core.Application.Helpers.ConfigModels;
using FamilyTreeBackend.Core.Application.Helpers.Exceptions.EmailExceptions;
using FamilyTreeBackend.Core.Application.Interfaces;
using FamilyTreeBackend.Core.Domain.Constants;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeBackend.Infrastructure.Service.InternalServices
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        
        public EmailService(IOptions<SmtpSettings> settings)
        {
            _smtpSettings = settings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);

                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            } 
            catch(Exception)
            {
                throw new SendEmailFailException(SendEmailExceptionMessages.SendEmailFailed, email);
            }
            

            //await Task.CompletedTask;
        }

        public async Task SendResetPasswordEmail(string email, string emailContent)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Orgigin Keeper: Reset your password";
                message.Body = new TextPart("html")
                {
                    Text = emailContent,
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);

                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception)
            {
                throw new SendEmailFailException(SendEmailExceptionMessages.SendEmailFailed, email);
            }
        }

        public async Task SendEmailConfirmationEmail(string email, string emailContent)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Orgigin Keeper: Email Confirmation";
                message.Body = new TextPart("html")
                {
                    Text = emailContent,
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, true);

                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception)
            {
                throw new SendEmailFailException(SendEmailExceptionMessages.SendEmailFailed, email);
            }
        }



    }
}
