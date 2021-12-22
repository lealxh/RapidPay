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
        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User() {Id=1,Username = "test", Password = "test" }
              );
        }


    }
}
