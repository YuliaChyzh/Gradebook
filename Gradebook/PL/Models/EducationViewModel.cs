using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PL.Models
{
    public class EducationViewModel
    {
        public int Id { get; set; }
        public int IdStudent { get; set; }
        public int IdSubject { get; set; }
        public int SubjectResult { get; set; }
    }
}