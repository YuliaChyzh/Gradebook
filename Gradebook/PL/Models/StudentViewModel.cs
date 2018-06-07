using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PL.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть ім'я студента")]
        [RegularExpression(@"^[\u0410-\u044F\u0406\u0407\u0490\u0404\u0456\u0457\u0491\u0454a-zA-Z ]{1,40}$", ErrorMessage = "Неправильне ім'я студента")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Choose group")]
        public int IdGroup { get; set; }

        [Required(ErrorMessage = "Оберіть групу")]
        public string GroupName { get; set; }
        public double StudentAvg { get; set; }
    }
}