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

        public IEnumerable<SubjectDTO> Get()
        {         
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Subject>, List<SubjectDTO>>(Database.SubjectsRepository.Get());
        }

        public SubjectDTO GetSubject(int id)
        {
            var subject = Database.SubjectsRepository.FindById(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            return mapper.Map<Subject, SubjectDTO>(subject);
        }

        public void AddSubject(SubjectDTO subjectDTO)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectDTO, Subject>()).CreateMapper();
            Database.SubjectsRepository.Create(mapper.Map<SubjectDTO, Subject>(subjectDTO));
            Database.SaveChanges();
        }

        public void EditSubject(SubjectDTO subjectDTO)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectDTO, Subject>()).CreateMapper();
            Subject subject = Database.SubjectsRepository.FindById(subjectDTO.Id);
            subject = mapper.Map<SubjectDTO,Subject>(subjectDTO);

            Database.SubjectsRepository.Update(subject);
            Database.SaveChanges();
        }

        public SubjectDTO GetSubjectAvg(int idSubject)
        {
            Subject subject = Database.SubjectsRepository.FindById(idSubject);
            IEnumerable<Education> education = Database.EducationsRepository.Get().Where(o => o.IdSubject == subject.Id);
            if (education.Count() != 0)
                subject.SubjectAvg = education.Average(num => Convert.ToInt64(num.SubjectResult));

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            return mapper.Map<Subject, SubjectDTO>(subject);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
