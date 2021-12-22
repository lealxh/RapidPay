using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces;
using RapidPay.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Linq.Expressions;

namespace RapidPay.Services
{
    public class CardManager : ICardManager
    {
        public CardManager(DatabaseContext DatabaseContext,IPaymentFeeManager FeeManager)
        {
            this.DatabaseContext = DatabaseContext;
            this.FeeManager = FeeManager;
        }

        public DatabaseContext DatabaseContext { get; }
        public IPaymentFeeManager FeeManager { get; }

        public void CreateCard(Card card)
        {
            DatabaseContext.Add(card);
            DatabaseContext.SaveChanges();
        }

        public decimal GetBalance(Card card)
        {
            Card c = DatabaseContext.Cards.Where(x => x.Number == card.Number).FirstOrDefault();

            return c.Balance;
        }

        public void SendPayment(Transaction transaction)
        {
            transaction.Fee = FeeManager.CalculatePaymentFee();
            DatabaseContext.Transactions.Add(transaction);
            DatabaseContext.SaveChanges();
        }
    }
}
