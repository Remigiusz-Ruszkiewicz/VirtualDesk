using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualDesk.Contracts;
using VirtualDesk.Contracts.Requests;
using VirtualDesk.Models;
using VirtualDesk.Services;

namespace VirtualDesk.Controllers
{
    [ApiController]
    public class MailSenderController : ControllerBase
    {
        public MailSenderController(IMailSenderService mailSenderService, IEmailConfiguration emailConfiguration)
        {
            MailSenderService = mailSenderService;
            _emailConfiguration = emailConfiguration;
        }
        public IEmailConfiguration _emailConfiguration;
        public IMailSenderService MailSenderService { get; }

        /// <summary>
        /// Wysyłanie Maila (Wypełniasz wszystko)
        /// </summary>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.MailSender.MailSend)]
        public IActionResult Send([FromBody]EmailMessage emailMessage)
        {
            var result = MailSenderService.MailSend(emailMessage);
            return Ok(result);
        }
        /// <summary>
        /// Wysyła wiadomośc Do osoby z Listy Kontaktów 
        /// </summary>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.MailSender.MailSendFromContact)]
        public IActionResult MailSendFromContact([FromRoute] Guid id, [FromBody]SendMailByIdRequest sendMailByIdRequest)
        {
            var result = MailSenderService.MailSendFromContactAsync(sendMailByIdRequest, id);
            return Ok(result);
        }
        /// <summary>
        /// Wysyla maila do osoby z kontaktów i przez uzytkownika z podanym id
        /// </summary>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.MailSender.MailSendByAccAndCont)]
        public IActionResult MailSendByAccAndCont([FromRoute] Guid id, [FromRoute] Guid loginId, [FromBody]SendMailByIdRequest sendMailByIdRequest)
        {
            var result = MailSenderService.MailSendByAccAndContAsync(sendMailByIdRequest, id, loginId);
            return Ok(result);
        }
        /// <summary>
        /// Pobiera Wiadomości
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.MailSender.MailGet)]
        public IActionResult ReceiveEmail(int maxCount = 10)
        {
            var result = MailSenderService.MailGet(maxCount);
            return Ok(result);
        }
        /// <summary>
        /// Pobiera Wiadomości od osoby z listy
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.MailSender.MailGetByAcc)]
        public IActionResult ReceiveEmailFromUser([FromRoute] Guid id, int maxCount = 10)
        {
            var result = MailSenderService.MailGetByAcc(id,maxCount);
            return Ok(result);
        }
        /// <summary>
        /// Dodaje Osobe do Listy Kontaktów
        /// </summary>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.MailSender.AddContact)]
        public async Task<IActionResult> AddContact([FromBody]EmailAccountList emailAccountList)
        {
            var product = await MailSenderService.AddContactAsync(emailAccountList);
            return Ok(product);
        }
        /// <summary>
        /// Pobiera wszystkie osoby z Listy Kontaktów
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.MailSender.GetAllContacts)]
        public async Task<IActionResult> GetAllContacts()
        {
            var products = await MailSenderService.GetAllContactsAsync();
            return Ok(products);
        }
        /// <summary>
        /// Pobiera Osobe z Listy kontaktów po id
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.MailSender.GetContactById)]
        public async Task<IActionResult> GetContactById([FromRoute]Guid id)
        {
            var product = await MailSenderService.GetContactByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        /// <summary>
        /// Aktualizuje Osobę z Listy Kontaktów 
        /// </summary>
        [AllowAnonymous]
        [HttpPut(ApiRoutes.MailSender.UpdateContact)]
        public async Task<IActionResult> UpdateContact([FromRoute]Guid id, [FromBody]EmailAccountList emailAccountList)
        {
            var product = await MailSenderService.GetContactByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = emailAccountList.Name;
            product.Address = emailAccountList.Address;
            var updatedAccount = await MailSenderService.UpdateContactAsync(product);
            return Ok(updatedAccount);
        }
        /// <summary>
        /// Usuwa osobe z Listy Kontaktów po id
        /// </summary>
        [AllowAnonymous]
        [HttpDelete(ApiRoutes.MailSender.DeleteContact)]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var deleted = await MailSenderService.DeleteContactAsync(id);
            if (deleted)
                return NoContent();
            return NotFound();
        }
        /// <summary>
        /// Dodaje Konto Uzytkownika
        /// </summary>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.MailSender.AddAccount)]
        public async Task<IActionResult> AddAccount([FromBody]AccountsList accountsList)
        {
            var product = await MailSenderService.AddAccountAsync(accountsList);
            return Ok(product);
        }
        /// <summary>
        /// Pobiera wszystkie osoby z Listy Użytkowników
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.MailSender.GetAllAccounts)]
        public async Task<IActionResult> GetAllAccounts()
        {
            var products = await MailSenderService.GetAllAccountsAsync();
            return Ok(products);
        }
        /// <summary>
        /// Pobiera konto Uzytkownika po id
        /// </summary>
        [AllowAnonymous]
        [HttpGet(ApiRoutes.MailSender.GetAccountById)]
        public async Task<IActionResult> GetAccountById([FromRoute]Guid id)
        {
            var loginAccount = await MailSenderService.GetAccountByIdAsync(id);
            if (loginAccount == null)
            {
                return NotFound();
            }
            return Ok(loginAccount);
        }
        /// <summary>
        /// Aktualizuje dane Uzytkownika
        /// </summary>
        [AllowAnonymous]
        [HttpPut(ApiRoutes.MailSender.UpdateAccount)]
        public async Task<IActionResult> UpdateAccount([FromRoute]Guid id, [FromBody]UpdateAccountListRequest updateAccountListRequest)
        {
            var product = await MailSenderService.GetAccountByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.EmailAdress = updateAccountListRequest.EmailAdress;
            product.Pass = updateAccountListRequest.Pass;
            product.IsPrivate = updateAccountListRequest.IsPrivate;
            product.PopPort = updateAccountListRequest.PopPort;
            product.PopServer = updateAccountListRequest.PopServer;
            product.SmtpPort = updateAccountListRequest.SmtpPort;
            product.SmtpServer = updateAccountListRequest.SmtpServer;
            var updatedAccount = await MailSenderService.UpdateAccountAsync(product);
            return Ok(updatedAccount);
        }
        /// <summary>
        /// Usuwa uzytkownika po id
        /// </summary>
        [AllowAnonymous]
        [HttpDelete(ApiRoutes.MailSender.DeleteAccount)]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid id)
        {
            var deleted = await MailSenderService.DeleteAccountAsync(id);
            if (deleted)
                return NoContent();
            return NotFound();
        }
    }
}