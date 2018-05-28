using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IStudentService
    {
        StudentDTO GetStudent(int id);
        IEnumerable<StudentDTO> GetStudents();
        void AddStudent(StudentDTO studentDTO);
        StudentDTO GetStudentAvg(int id);
        void Dispose();
    }
}
