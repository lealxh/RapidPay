using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Interfaces
{
    public interface ICardManager
    {
        public enum Status { Ok = 0, InsuficientBalance = 1, NotFound = 2, Error = 3 };
        int CreateCard(Card card);
        Task<int> SendPayment(string cardNumber, decimal amount, string description);
        Card GetBalance(string cardNumber);
        List<Card> ListCards();
        List<Transaction> ListTransactions();

        
    }
}
