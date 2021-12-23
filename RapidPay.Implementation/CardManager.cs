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
using System.Text.RegularExpressions;

namespace RapidPay.Services
{
    public class CardManager : ICardManager
    {
        private readonly DatabaseContext dbContext;
        private readonly IPaymentFeeManager feeManager;
        private readonly ILogger logger;
        private readonly object cardBalanceLock = new object();
        
        public enum Status { Ok=0, InsuficientBalance=1, NotFound=2, Error=3, FormatInvalid=4 };
        public CardManager(DatabaseContext dbContext,IPaymentFeeManager feeManager, ILogger<CardManager> logger)
        {
            this.dbContext = dbContext;
            this.feeManager = feeManager;
            this.logger = logger;
        }

        public int CreateCard(Card card)
        {
            int status= (int)Status.Ok;
           
            if (!isCardValid(card.Number))
                return (int)Status.FormatInvalid;

            try
            {
                dbContext.Add(card);
                dbContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                status = (int)Status.Error;
                logger.LogError(ex.Message);
                if(ex.InnerException!=null)
                    logger.LogError(ex.InnerException.Message);
            }
            return status;
        }

        public Card GetBalance(string cardNumber)
        {
          
            try
            {
                return dbContext.Cards.Where(x => x.Number == cardNumber).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                if (ex.InnerException != null)
                    logger.LogError(ex.InnerException.Message);
            }
            return null;
        }

        public async Task<int> SendPayment(string cardNumber, decimal amount, string description)
        {
            int res = (int)Status.Ok;

            if (!isCardValid(cardNumber))
                return (int)Status.FormatInvalid;

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
                logger.LogInformation(String.Format("Starting transaction Description:{0} Card{1} Amount:{2} Fee:{3}", transaction.Description,transaction.CardNumber, transaction.Amount,transaction.Fee));



                var card = dbContext.Cards.Where(x => x.Number == transaction.CardNumber).FirstOrDefault();
                if (card == null)
                {
                        res = (int)Status.NotFound;
                        logger.LogInformation(String.Format("Card {0} not found", cardNumber));
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
            catch (Exception ex)
            {
                res=(int)Status.Error;
                logger.LogError(ex.Message);
                
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

        public bool isCardValid(string s)
        {
            bool isValid = false;
            if (s.All(char.IsDigit) && s.Length == 15)
                isValid = true;

            return isValid;
        }
    }
}
