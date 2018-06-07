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
    public class StudentService: IStudentService
    {
        IUnitOfWork Database { get; set; }
        IEducationService educationService;

        public StudentService(IUnitOfWork uow, IEducationService educationService)
        {
            Database = uow;
            this.educationService = educationService;
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
            StudentDTO studentDTO = mapper.Map<Student, StudentDTO>(student);
            EditStudent(studentDTO);

            return studentDTO.StudentAvg;
        }

        public IEnumerable<StudentDTO> GetGroupList(int idGroup)
        {
            IEnumerable<Student> students = Database.StudentsRepository.Get().Where(g => g.IdGroup == idGroup);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Student>, IEnumerable<StudentDTO>>(students);
        }

        public IEnumerable<StudentDTO> SearchByName(IEnumerable<StudentDTO> studentDtos, string searchName)
        {
            studentDtos = studentDtos.Where(s => s.Name.ToUpper().Contains(searchName.ToUpper())).OrderBy(s => s.Name);
            return studentDtos;
        }

        public IEnumerable<StudentDTO> SearchByGroup(IEnumerable<StudentDTO> studentDtos, int idGroup)
        {
            studentDtos = studentDtos.Where(s => s.IdGroup == idGroup).OrderBy(s => s.Name);
            return studentDtos;
        }

        public IEnumerable<StudentDTO> SearchByStudentAvg(IEnumerable<StudentDTO> studentDtos, string searchStudentAvg)
        {

            studentDtos = studentDtos.Where(s => s.StudentAvg.ToString("0.00") == searchStudentAvg).OrderBy(s => s.Name);
            return studentDtos;
        }

        public IEnumerable<StudentDTO> SearchByProgress(IEnumerable<StudentDTO> studentDtos, string searchProgress)
        {
            List<EducationDTO> educations = educationService.Get().Where(s => s.SubjectResult < 60).Distinct().ToList();
            List<int> idStudents = new List<int>();
            foreach (var education in educations)
            {
                idStudents.Add(education.IdStudent);
            }
            idStudents = idStudents.Distinct().ToList();

            List<StudentDTO> students = new List<StudentDTO>();
            foreach (var id in idStudents)
            {
                students.Add(GetStudent(id));
            }
            IEnumerable<StudentDTO> studentNoSuccess = studentDtos, studentSuccess = studentDtos;
            foreach (var student in studentDtos)
            {
                if (students.FirstOrDefault(s => s.Id == student.Id) == null) studentNoSuccess = studentNoSuccess.Where(element => element.Id != student.Id);
            }
            foreach (var student in studentDtos)
            {
                if (studentNoSuccess.FirstOrDefault(s => s.Id == student.Id) != null) studentSuccess = studentSuccess.Where(s => s.Id != student.Id);
            }

            if (searchProgress == "Успішні")
            {
                return studentSuccess;
            }
            else if (searchProgress == "Неуспішні")
            {
                return studentNoSuccess;
            }
            return studentDtos;
        }

        public IEnumerable<StudentDTO> SearchBySubject(IEnumerable<StudentDTO> studentDtos, IEnumerable<EducationDTO> educationDtos)
        {
            List<int> idStudents = new List<int>();
            foreach (var education in educationDtos)
            {
                idStudents.Add(education.IdStudent);
            }
            List<StudentDTO> studDtos = new List<StudentDTO>();
            foreach (var id in idStudents)
            {
                studDtos.Add(GetStudent(id));
            }
            List<StudentDTO> stud = new List<StudentDTO>();
            foreach (var student in studDtos)
            {
                if (studentDtos.Contains(studentDtos.Where(s => s.Id == student.Id).FirstOrDefault())) stud.Add(student);
            }
            return stud;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
