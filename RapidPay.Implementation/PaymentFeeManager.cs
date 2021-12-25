using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces;
using RapidPay.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RapidPay.Services
{
    /// <summary>
    /// Service for Payment Fee Calculation
    /// </summary>
    public class PaymentFeeManager : IPaymentFeeManager
    {
        private decimal _randomFactor { get; set; }
        private decimal _lastFee { get; set; }
        private Random r;
        private readonly object lastFeeLock = new object();

        public PaymentFeeManager()
        {

            this._lastFee = 1;
            r = new Random();
            Timer t = new Timer((e)=>
            {
                _randomFactor = Convert.ToDecimal(r.NextDouble()) *2;
            },
            null, 0, 1000*3600);
          
        }
        
        public decimal CalculatePaymentFee()
        {

            lock (lastFeeLock) {
                _lastFee = _lastFee * _randomFactor;
            }
            return this._lastFee;
        }
    }
}
