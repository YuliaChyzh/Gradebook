using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;
using BLL.Infrastructure;
using BLL.Interfaces;
using AutoMapper;

namespace BLL.Services
{
    public class SubjectService: ISubjectService
    {
        IUnitOfWork Database { get; set; }

        public SubjectService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<SubjectDTO> GetSubjects()
        {         
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Subject>, List<SubjectDTO>>(Database.SubjectsRepository.Get());
        }

        public SubjectDTO GetSubject(int id)
        {
            var subject = Database.SubjectsRepository.FindById(id);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            return mapper.Map<Subject, SubjectDTO>(subject);
            //return new SubjectDTO { Id = subject.Id, Name = subject.Name, SubjectAvg=subject.SubjectAvg };
        }

        public SubjectDTO GetSubjectAvg(int idSubject)
        {
            Subject subject = Database.SubjectsRepository.FindById(idSubject);
            subject.SubjectAvg = Database.EducationsRepository.Get().Where(o => o.Subject == subject).Average(num => Convert.ToInt64(num.SubjectResult));

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            return mapper.Map<Subject, SubjectDTO>(subject);

            //return new SubjectDTO { Id = subject.Id, Name = subject.Name, SubjectAvg = subject.SubjectAvg };
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
