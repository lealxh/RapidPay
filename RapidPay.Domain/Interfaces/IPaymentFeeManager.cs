using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Interfaces
{
    public interface IPaymentFeeManager
    {
        decimal CalculatePaymentFee();

    }
}
