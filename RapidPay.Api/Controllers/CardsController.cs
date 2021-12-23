using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidPay.Api.DTO;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidPay.WebApi.Controllers
{
    [Authorize]
    [Route("/")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardManager cardManager;

        public CardsController(ICardManager cardManager)
        {
            this.cardManager = cardManager;
        }
        [HttpGet("GetBalance/{cardNumber}")]
        public IActionResult GetBalance(string cardNumber)
        {
            if (!cardManager.isCardValid(cardNumber))
                return BadRequest("Card Format incorrect");

            Card card = cardManager.GetBalance(cardNumber);
            if(card==null)
                return NotFound("Card not Found");
            else
            return Ok(card.Balance);
        }

        [HttpGet("GetCards")]
        public IActionResult GetCards()
        {
            return Ok(cardManager.ListCards());
        }

        [HttpGet("GetTransactions")]
        public IActionResult GetTransactions()
        {
            return Ok(cardManager.ListTransactions());
        }

        [HttpPost("CreateCard")]
        public IActionResult CreateCard([FromBody]Card card)
        {
             

            int result=cardManager.CreateCard(card);

            ICardManager.Status status = (ICardManager.Status)result;
            switch (status)
            {
                case ICardManager.Status.Ok:
                    return Ok(card);
                case ICardManager.Status.Error:
                    return StatusCode(500);
                case ICardManager.Status.FormatInvalid:
                    return BadRequest("Card Format incorrect");
            }
            return Ok();
        }

        [HttpPost("Pay")]
        public async Task<IActionResult> Pay([FromBody]PaymentDTO dto)
        {
    

            int result= await cardManager.SendPayment( dto.CardNumber, dto.Amount, dto.Description);
            ICardManager.Status status = (ICardManager.Status)result;
            switch (status)
            {
                case ICardManager.Status.Ok:
                    return Ok();
                case ICardManager.Status.InsuficientBalance:
                    return BadRequest("Balance insufficient");
                case ICardManager.Status.NotFound:
                    return NotFound("Card not Found");
                case ICardManager.Status.Error:
                    return StatusCode(500);
                case ICardManager.Status.FormatInvalid:
                    return BadRequest("Card Format incorrect");
            }
            return Ok();
        }


    }
}
