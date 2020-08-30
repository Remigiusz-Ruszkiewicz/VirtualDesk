using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualDesk.Contracts.Requests;
using VirtualDesk.Models;

namespace VirtualDesk.Services
{
    public interface IMailSenderService
    {
        //MailSend
        public bool MailSend(EmailMessage emailMessage);
        public Task<bool> MailSendFromContactAsync(SendMailByIdRequest sendMailByIdRequest, Guid id);
        public Task<bool> MailSendByAccAndContAsync(SendMailByIdRequest sendMailByIdRequest, Guid id, Guid loginId);
        //MailGet
        public List<EmailMessage> MailGet(int maxCount = 10);
        public Task<List<EmailMessage>> MailGetByAcc(Guid id,int maxCount = 10);
        //Contacts
        Task<EmailAccountList> AddContactAsync(EmailAccountList emailAccountList);
        Task<ICollection<EmailAccountList>> GetAllContactsAsync();
        Task<EmailAccountList> GetContactByIdAsync(Guid id);
        Task<EmailAccountList> UpdateContactAsync(EmailAccountList emailAccountList);
        Task<bool> DeleteContactAsync(Guid id);
        //Accounts
        Task<AccountsList> AddAccountAsync(AccountsList accountsList);
        Task<ICollection<AccountsList>> GetAllAccountsAsync();
        Task<AccountsList> GetAccountByIdAsync(Guid id);
        Task<AccountsList> UpdateAccountAsync(AccountsList accountsList);
        Task<bool> DeleteAccountAsync(Guid id);
        //Inne
        public int CheckMailQuantity();
    }
}
