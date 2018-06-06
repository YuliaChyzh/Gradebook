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
        double GetStudentAvg(int id);
        IEnumerable<StudentDTO> GetGroupList(int idGroup);
        void EditStudent(StudentDTO studentDTO);
        IEnumerable<StudentDTO> SearchByName(IEnumerable<StudentDTO> studentDtos, string searchName);
        IEnumerable<StudentDTO> SearchByGroup(IEnumerable<StudentDTO> studentDtos, int idGroup);
        IEnumerable<StudentDTO> SearchByStudentAvg(IEnumerable<StudentDTO> studentDtos, int searchStudentAvg);
        IEnumerable<StudentDTO> SearchByProgress(IEnumerable<StudentDTO> studentDtos, string searchProgress);
        IEnumerable<StudentDTO> SearchBySubject(IEnumerable<StudentDTO> studentDtos, IEnumerable<EducationDTO> educationDtos);
        void Dispose();
    }
}
