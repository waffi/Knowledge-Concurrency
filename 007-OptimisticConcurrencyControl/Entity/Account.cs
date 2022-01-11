using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _007_OptimisticConcurrencyControl.Entity
{
    public class Account
    {
        public int Id { get; set; }
        [ConcurrencyCheck]
        public double Balance { get; set; }
    }
}
