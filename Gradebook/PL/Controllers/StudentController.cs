using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using BLL.DTO;
using PL.Models;
using AutoMapper;
using BLL.Infrastructure;

namespace PL.Controllers
{
    public class StudentController : Controller
    {
        IStudentService studentService;
        ISubjectService subjectService;
        IGroupService groupService;
        IEducationService educationService;

        public StudentController() { }

        public StudentController(IStudentService studentService, ISubjectService subjectService, IGroupService groupService, IEducationService educationService)
        {
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.groupService = groupService;
            this.educationService = educationService;
        }

        public ActionResult ShowStudent()
        {
            IEnumerable<StudentDTO> studentDtos = studentService.GetStudents();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>()).CreateMapper();
            var students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentDtos);

            List<GroupDTO> groups = new List<GroupDTO>();
            foreach (var item in studentDtos)
            {
                groups.Add(groupService.GetGroup(item.IdGroup));
            }
            Dictionary<StudentViewModel, GroupDTO> studentGroup = new Dictionary<StudentViewModel, GroupDTO>();

            foreach (StudentViewModel studentVM in students)
            {
                studentGroup.Add(studentVM, groups.FirstOrDefault(g => g.Id == studentVM.IdGroup));
            }
            ViewBag.groups = groups;
            return View(studentGroup);
        }

        public ActionResult StudentReport(int idStudent)
        {
            StudentDTO studentDTO = studentService.GetStudent(idStudent);
            studentDTO = studentService.GetStudentAvg(idStudent);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>()).CreateMapper();
            var studentVM = mapper.Map<StudentDTO, StudentViewModel>(studentDTO);

            ViewBag.studentName = studentVM.Name;
            ViewBag.studentGroup = educationService.GetStudentGroup(idStudent).Name;
            ViewBag.studentAvg = studentVM.StudentAvg;

            Dictionary<string, int> subjectResult = educationService.GetStudentReport(idStudent);
            return View(subjectResult);
        }

        // Student/CreateStudent
        [HttpGet]
        public ActionResult CreateStudent()
        {
            StudentViewModel studentVM = new StudentViewModel();

            var groups = groupService.Get();
            ViewBag.groups = groups;

            return View(studentVM);
        }
        [HttpPost]
        public ActionResult CreateStudent(StudentViewModel studentVM)
        {
            if (ModelState.IsValid)
            {
                int groupId = groupService.Get().Where(g => g.Name == studentVM.GroupName).FirstOrDefault().Id;
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentViewModel, StudentDTO>()).CreateMapper();
                StudentDTO student = mapper.Map<StudentViewModel, StudentDTO>(studentVM);
                student.IdGroup = groupId;

                studentService.AddStudent(student);
                return RedirectToAction("Index","Home");
            }
            return View("StudentRecord", studentVM);
        }

        public ActionResult StudentRecord(StudentViewModel studentVM)
        {
            ViewBag.studentGroup = educationService.GetStudentGroup(studentVM.Id).Name;
            return View(studentVM);
        }       

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
    }
}