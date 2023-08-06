using DataAccess.Repositories.Interfaces;
using DataAccess.UnitOfWork.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RapidPay.ConfigurationSections;
using RapidPay.DTO;
using Service.Services.Interfaces;
using System.Text.RegularExpressions;

namespace RapidPay.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IUFEService _uFEService;

        public CreditCardController(IUnitOfWork unitOfWork, IUFEService uFEService)
        {
            _unitOfWork = unitOfWork;
            _uFEService = uFEService;
        }

        [HttpPost]
        public ActionResult<CreditCard> Create(CreateCreditCardRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Number))
            {
                return BadRequest("Empty credit card number");
            }

            if (!Regex.Match(request.Number, "^(?!0)\\d{15}$").Success)
            {
                return BadRequest("Invalid credit card number");
            }

            var existingCreditCard = _unitOfWork.CreditCard.GetByNumber(request.Number);
            if (existingCreditCard != null)
            {
                return BadRequest("Credit card number already in use");
            }

            var creditCard = new CreditCard();
            creditCard.Balance = 0;
            creditCard.Number = request.Number;

            _unitOfWork.CreditCard.AddCreditCard(creditCard);
            _unitOfWork.Complete();
            return Ok(creditCard);
        }

        [Route("balance")]
        [HttpGet]
        public ActionResult<CreditCard> Balance(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return BadRequest("Empty credit card number");
            }

            if (!Regex.Match(number, "^(?!0)\\d{15}$").Success)
            {
                return BadRequest("Invalid credit card number");
            }
            var creditCard = _unitOfWork.CreditCard.GetByNumber(number);
            if (creditCard == null)
            {
                return BadRequest("Invalid credit card number");
            }
            return Ok(creditCard.Balance);
        }

        [Route("pay")]
        [HttpPost]
        public ActionResult<CreditCard> Pay(PayRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.CreditCardNumber))
            {
                return BadRequest("Empty credit card number");
            }

            if (!Regex.Match(request.CreditCardNumber, "^(?!0)\\d{15}$").Success)
            {
                return BadRequest("Invalid credit card number");
            }

            var creditCard = _unitOfWork.CreditCard.GetByNumber(request.CreditCardNumber);
            if (creditCard == null)
            {
                return BadRequest("Invalid credit card number");
            }

            var transactionFee = _uFEService.CalculateTransactionFee(request.Amount);
            var totalAmount = request.Amount + transactionFee;

            var transaction = new Transaction();
            transaction.Amount = request.Amount;
            transaction.Fee = transactionFee;
            transaction.CreditCard = creditCard;
            transaction.Date = DateTime.Now;
            _unitOfWork.Transaction.AddTransaction(transaction);

            creditCard.Balance = creditCard.Balance + totalAmount;

            _unitOfWork.CreditCard.Update(creditCard);

            _unitOfWork.Complete();
            return Ok(transaction);
        }

    }
}
