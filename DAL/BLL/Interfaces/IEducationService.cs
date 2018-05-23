using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IEducationService
    {
        //IEnumerable<EducationDTO> GetEducations();
        EducationDTO GetEducation(int idEducation);
        StudentDTO GetStudent(int idEducation);
        //IEnumerable<SubjectDTO> GetSubjects(int idStudent);
        GroupDTO GetStudentGroup(int idStudent);
        void Dispose();
    }
}
