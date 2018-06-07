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
        IEducationService educationService;

        public SubjectService(IUnitOfWork uow, IEducationService educationService)
        {
            Database = uow;
            this.educationService = educationService;
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

        public void DeleteSubject(int id)
        {
            Subject subject = Database.SubjectsRepository.FindById(id);
            Database.SubjectsRepository.Remove(subject);
            Database.SaveChanges();
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

        public double GetSubjectAvg(int idSubject)
        {
            Subject subject = Database.SubjectsRepository.FindById(idSubject);
            IEnumerable<Education> education = Database.EducationsRepository.Get().Where(o => o.IdSubject == subject.Id);
            if (education.Count() != 0)
                subject.SubjectAvg = education.Average(num => Convert.ToInt64(num.SubjectResult));

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            SubjectDTO subjectDTO = mapper.Map<Subject, SubjectDTO>(subject);
            EditSubject(subjectDTO);
            return subjectDTO.SubjectAvg;
        }

        public IEnumerable<SubjectDTO> SearchByName(IEnumerable<SubjectDTO> subjectDtos, string searchName)
        {
            subjectDtos = subjectDtos.Where(s => s.Name.ToUpper().Contains(searchName.ToUpper())).OrderBy(s => s.Name);
            return subjectDtos;
        }

        public IEnumerable<SubjectDTO> SearchBySubjectAvg(IEnumerable<SubjectDTO> subjectDtos, string searchSubjectAvg)
        {

            subjectDtos = subjectDtos.Where(s => s.SubjectAvg.ToString("0.00") == searchSubjectAvg).OrderBy(s => s.Name);
            return subjectDtos;
        }

        public IEnumerable<SubjectDTO> SearchByProgress(IEnumerable<SubjectDTO> subjectDtos, string searchProgress)
        {
            List<EducationDTO> educations = educationService.Get().Where(s => s.SubjectResult < 60).Distinct().ToList();
            List<int> idSubjects = new List<int>();
            foreach (var education in educations)
            {
                idSubjects.Add(education.IdSubject);
            }
            idSubjects = idSubjects.Distinct().ToList();

            List<SubjectDTO> subjects = new List<SubjectDTO>();
            foreach (var id in idSubjects)
            {
                subjects.Add(GetSubject(id));
            }
            IEnumerable<SubjectDTO> subjectNoSuccess = subjectDtos, subjectSuccess = subjectDtos;

            foreach (var subject in subjectDtos)
            {
                if (subjects.FirstOrDefault(s => s.Id == subject.Id) == null) subjectNoSuccess = subjectNoSuccess.Where(element => element.Id != subject.Id);
            }
            foreach (var subject in subjectDtos)
            {
                if (subjectNoSuccess.FirstOrDefault(s => s.Id == subject.Id) != null) subjectSuccess = subjectSuccess.Where(s => s.Id != subject.Id);
            }

            if (searchProgress == "Успішні")
            {
                return subjectSuccess;
            }
            else if (searchProgress == "Неуспішні")
            {
                return subjectNoSuccess;
            }
            return subjectDtos;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
