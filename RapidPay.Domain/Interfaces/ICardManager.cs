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
        void CreateCard(Card card);
        void SendPayment(Transaction t);
        Decimal GetBalance(Card card);
    }
}
