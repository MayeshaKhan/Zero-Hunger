using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class FoodDTO
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}