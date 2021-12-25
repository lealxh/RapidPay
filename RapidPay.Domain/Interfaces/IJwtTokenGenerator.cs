using System;
using System.Collections.Generic;
using System.Text;

namespace RapidPay.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string CreateToken(string username);
    }
}
