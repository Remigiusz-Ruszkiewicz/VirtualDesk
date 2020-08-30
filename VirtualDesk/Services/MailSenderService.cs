using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDesk.Contracts.Requests;
using VirtualDesk.Data;
using VirtualDesk.Models;

namespace VirtualDesk.Services
{
    public class MailSenderService : IMailSenderService
    {
        public MailSenderService(IEmailConfiguration emailConfiguration, DataContext dbContext)
        {
            _emailConfiguration = emailConfiguration;
            DbContext = dbContext;
        }
        public IEmailConfiguration _emailConfiguration { get; }
        public DataContext DbContext { get; }
        public async Task<EmailAccountList> AddContactAsync(EmailAccountList emailAccountList)
        {
            emailAccountList.Id = Guid.NewGuid();
            DbContext.emailAccountLists.Add(emailAccountList);
            await DbContext.SaveChangesAsync();
            var emailToReturn = await GetContactByIdAsync(emailAccountList.Id);
            return emailToReturn;
        }
        public async Task<bool> DeleteContactAsync(Guid id)
        {
            var account = await DbContext.emailAccountLists.SingleOrDefaultAsync(x => x.Id == id);
            if (account == null)
                return false;
            DbContext.emailAccountLists.Remove(account);
            await DbContext.SaveChangesAsync();
            return true;
        }
        public async Task<AccountsList> GetAccountByIdAsync(Guid id)
        {
            return await DbContext.accountsLists.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<ICollection<EmailAccountList>> GetAllContactsAsync()
        {
            var result = await DbContext.emailAccountLists.ToListAsync();
            return result;
        }
        public async Task<EmailAccountList> GetContactByIdAsync(Guid id)
        {
            return await DbContext.emailAccountLists.SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<AccountsList> AddAccountAsync(AccountsList accountsList)
        {
            accountsList.Id = Guid.NewGuid();
            DbContext.accountsLists.Add(accountsList);
            await DbContext.SaveChangesAsync();
            var accountToReturn = await GetAccountByIdAsync(accountsList.Id);
            return accountToReturn;
        }
        public async Task<bool> MailSendFromContactAsync(SendMailByIdRequest sendMailByIdRequest, Guid id)
        {
            var mail = await DbContext.emailAccountLists.SingleOrDefaultAsync(x => x.Id == id);
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(mail.Name, mail.Address));
            message.From.Add(new MailboxAddress(sendMailByIdRequest.NameFrom, mail.Address));
            message.Subject = sendMailByIdRequest.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = sendMailByIdRequest.Content
            };
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
            return true;
        }
        public async Task<EmailAccountList> UpdateContactAsync(EmailAccountList emailAccountList)
        {
            DbContext.emailAccountLists.Update(emailAccountList);
            await DbContext.SaveChangesAsync();
            return emailAccountList;
        }
        public int CheckMailQuantity()
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);
                return emailClient.Count();
            }
        }
        List<EmailMessage> IMailSenderService.MailGet(int number)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);
                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = (emailClient.Count - 1); i >= emailClient.Count - number; i--)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new MailSender { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new MailSender { Address = x.Address, Name = x.Name }));
                    emails.Add(emailMessage);
                }
                return emails;
            }
        }
        bool IMailSenderService.MailSend(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
            return true;
        }
        public async Task<bool> MailSendByAccAndContAsync(SendMailByIdRequest sendMailByIdRequest, Guid id, Guid loginId)
        {
            var mail = await DbContext.emailAccountLists.SingleOrDefaultAsync(x => x.Id == id);
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(mail.Name, mail.Address));
            message.From.Add(new MailboxAddress(sendMailByIdRequest.NameFrom, mail.Address));
            message.Subject = sendMailByIdRequest.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = sendMailByIdRequest.Content
            };
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(GetAccountByIdAsync(loginId).Result.SmtpServer, GetAccountByIdAsync(loginId).Result.SmtpPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(GetAccountByIdAsync(loginId).Result.EmailAdress, GetAccountByIdAsync(loginId).Result.Pass);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
            return true;
        }

        public async Task<ICollection<AccountsList>> GetAllAccountsAsync()
        {
            var result = await DbContext.accountsLists.ToListAsync();
            return result;
        }

        public async Task<AccountsList> UpdateAccountAsync(AccountsList accountsList)
        {
            DbContext.accountsLists.Update(accountsList);
            await DbContext.SaveChangesAsync();
            return accountsList;
        }

        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            var account = await DbContext.accountsLists.SingleOrDefaultAsync(x => x.Id == id);
            if (account == null)
                return false;
            DbContext.accountsLists.Remove(account);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmailMessage>> MailGetByAcc(Guid id, int maxCount = 10)
        {
            var mail = await DbContext.accountsLists.SingleOrDefaultAsync(x => x.Id == id);
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(mail.PopServer, mail.PopPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(mail.EmailAdress, mail.Pass);
                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = (emailClient.Count - 1); i >= emailClient.Count - maxCount; i--)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new MailSender { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new MailSender { Address = x.Address, Name = x.Name }));
                    emails.Add(emailMessage);
                }
                return emails;
            }
        }
    }
}
