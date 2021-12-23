using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RapidPay.Domain.Entities;
using RapidPay.Domain.Interfaces;
using RapidPay.Persistance;
using System.Linq;
using System.Linq.Expressions;

namespace RapidPay.Services
{
    /// <summary>
    /// Service for user and password validation
    /// </summary>
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly DatabaseContext dbContext;

        public AuthenticationManager(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => dbContext.Users.SingleOrDefault(x => x.Username == username && x.Password == password));
            return user;

        }
    }
}
