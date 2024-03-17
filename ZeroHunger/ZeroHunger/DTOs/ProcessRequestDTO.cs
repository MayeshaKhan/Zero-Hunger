using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class ProcessRequestDTO
    {
        [Required]
        public System.DateTime Expiry { get; set; }
    }
}