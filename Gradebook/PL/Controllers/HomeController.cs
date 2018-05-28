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
    public class HomeController : Controller
    {
        IStudentService studentService;
        ISubjectService subjectService;
        IGroupService groupService;
        IEducationService educationService;

        public HomeController() { }

        public HomeController(IStudentService studentService, ISubjectService subjectService, IGroupService groupService, IEducationService educationService)
        {
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.groupService = groupService;
            this.educationService = educationService;
        }

        public ActionResult Index()
        {

            return View();
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
            var studentVM = mapper.Map<StudentDTO,StudentViewModel>(studentDTO);

            ViewBag.studentName = studentVM.Name;
            ViewBag.studentGroup = educationService.GetStudentGroup(idStudent).Name;                       
            ViewBag.studentAvg = studentVM.StudentAvg;

            Dictionary<string, int> subjectResult = educationService.GetStudentReport(idStudent);
            return View(subjectResult);
        } 

        // Home/CreateStudent
        [HttpGet]
        public ActionResult CreateStudent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateStudent(StudentViewModel studentVM)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentViewModel, StudentDTO>()).CreateMapper();
                StudentDTO student = mapper.Map<StudentViewModel, StudentDTO>(studentVM);

                studentService.AddStudent(student);
                return RedirectToAction("Index");
            }

            return View(studentVM);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}