using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RapidPay.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
