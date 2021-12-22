using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Entities
{
    public class Card
    {
        public int Id { get; set; }
        [Required, MaxLength(15)]
        public string Number { get; set; }
        public Decimal Balance { get; set; }
    }
}
