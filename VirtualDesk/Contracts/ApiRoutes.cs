using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualDesk.Contracts
{
    public static class ApiRoutes
    {
        public const string Base = "api/v1/";

        public static class Users
        {
            public const string Add = Base + "users";
            public const string Login = Base + "users/login";
            public const string LoginSwagger = Base + "users/login/swagger";
        }
        public static class Word2PDF
        {
            public const string WordToPDF = Base + "Word2PDF";
        }
        public static class MailSender
        {
            //MailSend
            public const string MailSend = Base + "MailSenderPost";
            public const string MailSendFromContact = Base + "MailSenderSendFromList/{id}";
            public const string MailSendByAccAndCont = Base + "MailSender/SendByAccount/{id}/{loginId}";
            //MailGet
            public const string MailGet = Base + "MailSenderGet";
            public const string MailGetByAcc = Base + "MailSenderGet/{id}";
            //Contacts
            public const string AddContact = Base + "MailSender";
            public const string GetAllContacts = Base + "MailSender";
            public const string GetContactById = Base + "MailSender/{id}";
            public const string UpdateContact = Base + "MailSender/{id}";
            public const string DeleteContact = Base + "MailSender/{id}";
            //Accounts
            public const string AddAccount = Base + "MailSender/AddAccount";
            public const string GetAllAccounts = Base + "MailSender/GetAllAccounts";
            public const string GetAccountById = Base + "MailSender/GetAccountById/{id}";
            public const string UpdateAccount = Base + "MailSender/UpdateAccount/{id}";
            public const string DeleteAccount = Base + "MailSender/DeleteAccount/{id}";
        }
        public static class SlackBot
        {
            public const string SlackBot_ = Base + "SlackBot";
            public const string SlackBotMsg = Base + "SlackBot/msg";
            public const string SlackBotFile = Base + "SlackBot/file";
            public const string SlackBotMessages = Base + "SlackBot/{channel}";
        }
    }
}
