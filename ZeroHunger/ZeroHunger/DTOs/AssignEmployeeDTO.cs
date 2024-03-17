using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class AssignEmployeeDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Emp_Id { get; set; }
    }
}