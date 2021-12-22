using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        [ MaxLength(50)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Fee { get; set; }
        public DateTime Date { get; set; }
        public int IdCard { get; set; }
        [Required, MaxLength(15)]
        public string  CardNumber { get; set; }
    }
}
