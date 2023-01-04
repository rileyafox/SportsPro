using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SportsPro.Models
{
    public class Registration
    {
        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public Customer Customer { get; set; }

        public Product Product { get; set; }
    }
}
