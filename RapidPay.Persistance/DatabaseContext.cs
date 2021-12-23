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
        /// <summary>
        /// Database context definition for Entity framework
        /// </summary>
        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }

        //Adding the test user to the model creation
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User() {Id=1,Username = "test", Password = "test" }
              );
        }


    }
}
