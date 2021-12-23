using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace RapidPay.Domain.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<User> Authenticate(string username, string password);
    }
}
