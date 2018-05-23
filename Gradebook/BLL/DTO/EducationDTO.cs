using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL.DTO
{
    public class EducationDTO
    {
        public int Id { get; set; }
        public int IdStudent { get; set; }
        public int IdSubject { get; set; }
        public int SubjectResult { get; set; }
    }
}
