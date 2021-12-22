using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces;
using RapidPay.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace RapidPay.Services
{
    public class CardManager : ICardManager
    {
        private readonly DatabaseContext dbContext;
        private readonly IPaymentFeeManager feeManager;
        private readonly object cardBalanceLock = new object();

        public enum Status { Ok=0, InsuficientBalance=1, NotFound=2, Error=3 };
        public CardManager(DatabaseContext dbContext,IPaymentFeeManager feeManager)
        {
            this.dbContext = dbContext;
            this.feeManager = feeManager;
        }

        public int CreateCard(Card card)
        {
            int status= (int)Status.Ok;
            try
            {
                dbContext.Add(card);
                dbContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                status = (int)Status.Error;
                throw ex;
            }
            return status;
        }

        public Card GetBalance(string cardNumber)
        {
            return dbContext.Cards.Where(x => x.Number == cardNumber).FirstOrDefault();
        }

        public async Task<int> SendPayment(string cardNumber, decimal amount, string description)
        {
            int res = (int)Status.Ok;

            await Task.Run(() => { 
            try
            {
                Transaction transaction = new Transaction() { 
                    Amount=amount,
                    CardNumber=cardNumber, 
                    Date=DateTime.Now, 
                    Description=description,
                    Fee= feeManager.CalculatePaymentFee()
                };
                

                var card = dbContext.Cards.Where(x => x.Number == transaction.CardNumber).FirstOrDefault();
                if (card == null)
                {
                        res = (int)Status.NotFound;
                }
                else
                if (card.Balance < transaction.Amount + transaction.Fee)
                {
                    res = (int)Status.InsuficientBalance;
                    
                }
                else
                {
                        lock (cardBalanceLock) 
                        { 
                            card.Balance -= (transaction.Amount + transaction.Fee);
                            transaction.IdCard = card.Id;
                            dbContext.Transactions.Add(transaction);
                            dbContext.SaveChanges();
                        }
                } 
            }
            catch (Exception)
            {
                res=(int)Status.Error;
            }
            });

            return res;
        }

        public List<Card> ListCards()
        {
            return dbContext.Cards.ToList();
        }

        public List<Transaction> ListTransactions()
        {
            return dbContext.Transactions.ToList();
        }
    }
}
