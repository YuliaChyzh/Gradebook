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

            //return new StudentDTO { Id = student.Id, Name = student.Name, IdGroup=student.IdGroup, StudentAvg = student.StudentAvg };
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
            student.StudentAvg = Database.EducationsRepository.Get().Where(o => o.Student == student).Average(num => Convert.ToInt64(num.SubjectResult));
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<Student, StudentDTO>(student);

           //return new StudentDTO { Id = student.Id, Name = student.Name, IdGroup=student.IdGroup, StudentAvg = student.StudentAvg };
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
