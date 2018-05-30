using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using DAL.Entities;
//using BLL.BusinessModels;
using DAL.Interfaces;
using BLL.Infrastructure;
using BLL.Interfaces;
using AutoMapper;

namespace BLL.Services
{
    public class StudentService: IStudentService
    {
        IUnitOfWork Database { get; set; }

        public StudentService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<StudentDTO> GetStudents()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Student>, List<StudentDTO>>(Database.StudentsRepository.Get());
        }

        public StudentDTO GetStudent(int id)
        {
            var student = Database.StudentsRepository.FindById(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<Student, StudentDTO>(student);
        }

        public void AddStudent(StudentDTO studentDTO)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, Student>()).CreateMapper();
            Database.StudentsRepository.Create(mapper.Map<StudentDTO, Student>(studentDTO));
        }

        public GroupDTO GetGroup(StudentDTO student)
        {
            Group group = Database.GroupsRepository.FindById(student.IdGroup);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()).CreateMapper();
            return mapper.Map<Group, GroupDTO>(group);
        }

        public StudentDTO GetStudentAvg(int idStudent)
        {
            Student student = Database.StudentsRepository.FindById(idStudent);
            student.StudentAvg = Database.EducationsRepository.Get().Where(o => o.IdStudent == idStudent).Average(num => Convert.ToInt64(num.SubjectResult));
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<Student, StudentDTO>(student);
        }

        public IEnumerable<StudentDTO> GetGroupList(int idGroup)
        {
            IEnumerable<Student> students = Database.StudentsRepository.Get().Where(g => g.IdGroup == idGroup);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Student>, IEnumerable<StudentDTO>>(students);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
