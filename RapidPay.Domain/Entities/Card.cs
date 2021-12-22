using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal Balance { get; set; }
    }
}
