using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter subject name")]
        [RegularExpression(@"^[\u0410-\u044F\u0406\u0407\u0490\u0404\u0456\u0457\u0491\u0454a-zA-Z ]{1,40}$", ErrorMessage = "Subject name is incorrect.")]
        public string Name { get; set; }

        public double SubjectAvg { get; set; }
    }
}