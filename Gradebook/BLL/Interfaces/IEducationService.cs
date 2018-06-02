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
        IEnumerable<EducationDTO> Get();
        EducationDTO GetEducation(int idEducation);
        void DeleteEducation(int id);
        void AddSubject(EducationDTO educationDTO);
        void EditEducation(EducationDTO educationDTO);
        string GetStudentName(int idStudent);
        GroupDTO GetStudentGroup(int idStudent);
        SubjectDTO GetStudentSubject(int idEducation);
        string SetSubjectName(int idEducation);
        SubjectDTO GetSubject(int idSubject);
        Dictionary<string, int> GetStudentDetail(int idStudent);
        Dictionary<string, int> GetSubjectDetail(int idSubject);
        void SetGroupName(int idEducation);
        IEnumerable<EducationDTO> GetSubbjectList(int idSubject);
        IEnumerable<EducationDTO> GetStudentList(int idStudent);
        void Dispose();
    }
}
