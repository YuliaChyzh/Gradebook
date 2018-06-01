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
using PL.Models.EditModels;
using PL.Validation;

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
            IEnumerable<StudentDTO> studentDtos = studentService.Get().OrderBy(s=>s.Name);
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

        public ActionResult StudentDetails(int idStudent)
        {
            StudentDTO studentDTO = studentService.GetStudent(idStudent);
            studentDTO = studentService.GetStudentAvg(idStudent);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>()).CreateMapper();
            var studentVM = mapper.Map<StudentDTO, StudentViewModel>(studentDTO);

            ViewBag.studentName = studentVM.Name;
            ViewBag.studentGroup = educationService.GetStudentGroup(idStudent).Name;
            ViewBag.studentAvg = studentVM.StudentAvg;

            Dictionary<string, int> subjectResult = educationService.GetStudentDetail(idStudent);
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
                //return RedirectToAction("Index","Home");
                return View("StudentRecord", studentVM);
            }
            return View();
        }

        public ActionResult DeleteStudent(int idStudent)
        {
            IEnumerable<EducationDTO> eduList = educationService.GetStudentList(idStudent);

            if (eduList.Count() == 0)
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            if (studentService.Get().FirstOrDefault(s => s.Id == idStudent) == null)
                return Json(new { Succes = false, Message = "Student doesn`t exist" }, JsonRequestBehavior.AllowGet);

            if (eduList.Count() != 0)
                return Json(new { Succes = false, Message = "Student has some subjects yet" }, JsonRequestBehavior.AllowGet);

            return Json(new { Success = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditStudent(int idStudent)
        {
            StudentDTO studentDTO = studentService.GetStudent(idStudent);

            EditStudentViewModel editStudentVM = new EditStudentViewModel()
            {
                Id = studentDTO.Id,
                Name = studentDTO.Name,
                StudentAvg = studentDTO.StudentAvg
            };

            educationService.SetGroupName(editStudentVM.Id);
            var groups = groupService.Get();
            ViewBag.groups = groups;

            return View("EditStudent", editStudentVM);
        }

        [HttpPost]
        public ActionResult EditStudent(EditStudentViewModel editStudentVM)
        {
            int groupId = groupService.Get().Where(g => g.Name == editStudentVM.GroupName).FirstOrDefault().Id;
            if ((groupId!=0)) editStudentVM.IdGroup = groupId;
            else return Json(new { Success = false, Message = "Input student group again" });

            Validate validate = new Validate();
            if (!(validate.ValidationStudentName(editStudentVM.Name))) return Json(new { Success = false, Message = "Input student name again" });

            StudentDTO studentDTO = studentService.GetStudent(editStudentVM.Id);

            if (studentService.Get().ToList().Contains(studentService.Get().Where(s => s.Name == editStudentVM.Name).FirstOrDefault()))
                return Json(new { Success = false, Message = "Student with that name already exists" });

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditStudentViewModel, StudentDTO>()).CreateMapper();
            studentDTO = mapper.Map<EditStudentViewModel, StudentDTO>(editStudentVM);

            studentService.EditStudent(studentDTO);

            return Json(new { Success = true, Message = "Дані студента оновлено" });
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