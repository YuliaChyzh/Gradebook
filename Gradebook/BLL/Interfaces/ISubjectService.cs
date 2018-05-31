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
        void AddSubject(SubjectDTO subjectDTO);
        void EditSubject(SubjectDTO subjectDTO);
        SubjectDTO GetSubjectAvg(int id);
        void Dispose();
    }
}
