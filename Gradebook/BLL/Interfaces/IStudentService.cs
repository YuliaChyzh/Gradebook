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
        IEnumerable<StudentDTO> Get();
        void DeleteStudent(int id);
        void AddStudent(StudentDTO studentDTO);
        StudentDTO GetStudentAvg(int id);
        IEnumerable<StudentDTO> GetGroupList(int idGroup);
        void EditStudent(StudentDTO studentDTO);
        void Dispose();
    }
}
