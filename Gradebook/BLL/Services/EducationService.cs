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
    public class EducationService : IEducationService
    {
        IUnitOfWork Database { get; set; }

        public EducationService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public EducationDTO GetEducation(int id)
        {
            var education = Database.EducationsRepository.FindById(id);         
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Education, EducationDTO>()).CreateMapper();
            return mapper.Map<Education, EducationDTO>(education);
        }

        public EducationDTO AddStudent(int idStudent, int idSubject, int subjectResult)
        {
            Education education = new Education();
            Student student= Database.StudentsRepository.FindById(idStudent);
            education.IdStudent = idStudent;
            education.IdSubject = idSubject;
            SetGroupName(education.Id);
            education.SubjectResult = subjectResult;
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Education, EducationDTO>()).CreateMapper();
            return mapper.Map<Education, EducationDTO>(education);
        }

        public string GetStudentName(int idStudent)
        {
            Student student= Database.StudentsRepository.FindById(idStudent);
            return student.Name;
        }

        public GroupDTO GetStudentGroup(int idStudent)
        {
            Student student = Database.StudentsRepository.FindById(idStudent);
            Group group = Database.GroupsRepository.Get().Where(o => o.Id == student.IdGroup).FirstOrDefault();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()).CreateMapper();
            return mapper.Map<Group, GroupDTO>(group);
        }

        public void SetGroupName(int idEducation)
        {
            Education education = Database.EducationsRepository.FindById(idEducation);
            GroupDTO groupDTO = GetStudentGroup(education.IdStudent);
            education.GroupName = groupDTO.Name;
        }

        public SubjectDTO GetSubject(int idSubject)
        {
            var subject = Database.SubjectsRepository.FindById(idSubject);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Subject, SubjectDTO>()).CreateMapper();
            return mapper.Map<Subject, SubjectDTO>(subject);
        }

        public Dictionary<string, int> GetStudentDetail(int idStudent)
        {
            IEnumerable<Education> educations = Database.EducationsRepository.Get().Where(o => o.IdStudent == idStudent);
            Dictionary<string, int> subjectResult = new Dictionary<string, int>();  //name of subject + subject result

            foreach (Education education in educations)
            {
                subjectResult.Add(GetSubject(education.IdSubject).Name, education.SubjectResult);
            }

            return subjectResult;
        }

        public Dictionary<string,int> GetSubjectDetail(int idSubject)
        {
            IEnumerable<Education> educations = Database.EducationsRepository.Get().Where(o => o.IdSubject == idSubject);
            Dictionary<string, int> subjectResult = new Dictionary<string, int>(); //name of student, which has this subject + subject result

            foreach (Education education in educations)
            {
                subjectResult.Add(GetStudentName(education.IdStudent), education.SubjectResult);
            }

            return subjectResult;
        }

        public IEnumerable<EducationDTO> GetSubbjectList(int idSubject)
        {
            IEnumerable<Education> educations = Database.EducationsRepository.Get().Where(s => s.IdSubject == idSubject);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Education, EducationDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(educations);
        }

        public IEnumerable<EducationDTO> GetStudentList(int idStudent)
        {
            IEnumerable<Education> educations = Database.EducationsRepository.Get().Where(s => s.IdStudent == idStudent);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Education, EducationDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Education>, IEnumerable<EducationDTO>>(educations);
        }

        public void Dispose()
        {
            Database.Dispose();
        }


    }
}
