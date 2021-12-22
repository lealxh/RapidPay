using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidPay.Api.DTO
{
    public class PaymentDTO
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

    }
}
