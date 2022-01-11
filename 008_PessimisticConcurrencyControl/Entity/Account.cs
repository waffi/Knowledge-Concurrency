using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _008_PessimisticConcurrencyControl.Entity
{
    public class Account
    {
        public int Id { get; set; }
        public double Balance { get; set; }
    }
}
