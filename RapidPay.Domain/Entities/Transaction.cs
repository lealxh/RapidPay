using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Fee { get; set; }
        public DateTime Date { get; set; }
        public Card Card { get; set; }
    }
}
