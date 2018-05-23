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

        public StudentDTO GetStudent(int idEducation)
        {
            Education education = Database.EducationsRepository.FindById(idEducation);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<Student, StudentDTO>(education.Student);

        }

        public GroupDTO GetStudentGroup(int idStudent)
        {
            Student student = Database.StudentsRepository.FindById(idStudent);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()).CreateMapper();
            return mapper.Map<Group, GroupDTO>(student.Group);
        }

        public void SetGroupName(int idEducation)
        {
            Education education = Database.EducationsRepository.FindById(idEducation);
            GroupDTO groupDTO = GetStudentGroup(education.IdStudent);
            education.GroupName = groupDTO.Name;
        }

        public void Dispose()
        {
            Database.Dispose();
        }


    }
}
