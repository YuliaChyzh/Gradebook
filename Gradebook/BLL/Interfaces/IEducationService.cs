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
        EducationDTO GetEducation(int idEducation);
        EducationDTO AddStudent(int idStudent, int idSubject, int subjectResult);
        StudentDTO GetStudent(int idEducation);
        GroupDTO GetStudentGroup(int idStudent);
        SubjectDTO GetSubject(int idSubject);
        Dictionary<string, int> GetStudentReport(int idStudent);
        void SetGroupName(int idEducation);
        void Dispose();
    }
}
