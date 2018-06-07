using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class EducationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Choose student")]
        public int IdStudent { get; set; }

        [Required(ErrorMessage = "Choose subject")]
        public int IdSubject { get; set; }

        [Required(ErrorMessage = "Оберіть групу")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Enter subject result")]
        [Range(0, 100,ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [RegularExpression(@"^[0-9]{1,3}$", ErrorMessage = "Subject result is incorrect.")]
        public int SubjectResult { get; set; }
    }
}