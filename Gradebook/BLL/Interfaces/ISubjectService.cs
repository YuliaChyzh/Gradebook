using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ISubjectService
    {
        SubjectDTO GetSubject(int id);
        IEnumerable<SubjectDTO> Get();
        void DeleteSubject(int id);
        void AddSubject(SubjectDTO subjectDTO);
        void EditSubject(SubjectDTO subjectDTO);
        double GetSubjectAvg(int id);
        IEnumerable<SubjectDTO> SearchByName(IEnumerable<SubjectDTO> subjectDtos, string searchName);
        IEnumerable<SubjectDTO> SearchBySubjectAvg(IEnumerable<SubjectDTO> subjectDtos, string searchSubjectAvg);
        IEnumerable<SubjectDTO> SearchByProgress(IEnumerable<SubjectDTO> subjectDtos, string searchProgress);
        void Dispose();
    }
}
