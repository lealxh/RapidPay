using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RapidPay.Services
{
    public class PaymentFeeManager : IPaymentFeeManager
    {
        private double _randomFactor { get; set; }
        private Random r;
        public PaymentFeeManager()
        {
            r = new Random();
            Timer t = new Timer((e)=>
            {
                _randomFactor = r.NextDouble()*2;
            },
            null, 0, 1000*3600);

        }
        
        public decimal CalculatePaymentFee()
        {
            throw new NotImplementedException();
        }
    }
}
