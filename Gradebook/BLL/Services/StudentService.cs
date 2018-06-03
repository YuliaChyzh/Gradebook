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

        public IEnumerable<StudentDTO> Get()
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

        public void DeleteStudent(int id)
        {
            Student student = Database.StudentsRepository.FindById(id);
            Database.StudentsRepository.Remove(student);
            Database.SaveChanges();
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

        public void EditStudent(StudentDTO studentDTO)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, Student>()).CreateMapper();
            Student student = Database.StudentsRepository.FindById(studentDTO.Id);
            student = mapper.Map<StudentDTO, Student>(studentDTO);

            Database.StudentsRepository.Update(student);
            Database.SaveChanges();
        }

        public double GetStudentAvg(int idStudent)
        {
            Student student = Database.StudentsRepository.FindById(idStudent);
            IEnumerable<Education> education = Database.EducationsRepository.Get().Where(o => o.IdStudent == idStudent);
            if (education.Count() != 0)
                student.StudentAvg = education.Average(num => Convert.ToInt64(num.SubjectResult));

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<Student, StudentDTO>(student).StudentAvg;
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
