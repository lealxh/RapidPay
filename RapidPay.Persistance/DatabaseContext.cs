using Microsoft.EntityFrameworkCore;
using RapidPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Persistance
{
    public class DatabaseContext:DbContext
    { 
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }

      
    }
}
