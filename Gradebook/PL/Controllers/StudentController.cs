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

        [HttpGet]
        public ActionResult ShowStudent(string searchName, string searchGroup, int? searchStudentAvg, string searchProgress, string searchSubject )
        {
            var groups = groupService.Get();
            ViewBag.groups = groups;
            var subjects = subjectService.Get();
            ViewBag.subjects = subjects;
            foreach (var student in studentService.Get())
            {
                student.StudentAvg=studentService.GetStudentAvg(student.Id);
            }

            IEnumerable<StudentDTO> studentDtos = studentService.Get().OrderBy(s => s.Name);

            if (!String.IsNullOrEmpty(searchName))
            {
                studentDtos = studentDtos.Where(s=>s.Name.ToUpper().Contains(searchName.ToUpper())).OrderBy(s => s.Name);
            }

            if (!String.IsNullOrEmpty(searchGroup))
            {
                int groupId = groupService.Get().Where(g => g.Name == searchGroup).FirstOrDefault().Id;
                studentDtos = studentDtos.Where(s => s.IdGroup == groupId).OrderBy(s => s.Name);
            }

            if (!String.IsNullOrEmpty(searchStudentAvg.ToString()))
            {
                studentDtos = studentDtos.Where(s => s.StudentAvg==searchStudentAvg).OrderBy(s => s.Name);
            }

            if (!String.IsNullOrEmpty(searchProgress))
            {
                if (searchProgress == "Успішні")
                    {
                        studentDtos = studentDtos.Where(s => s.StudentAvg > 60).OrderBy(s => s.Name);
                    }
                    else if (searchProgress == "Неуспішні")
                {
                        studentDtos = studentDtos.Where(s => s.StudentAvg < 60).OrderBy(s => s.Name);
                    }
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>()).CreateMapper();
            var students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentDtos);
            List<GroupDTO> groupsList = new List<GroupDTO>();
            foreach (var item in studentDtos)
            {
                groupsList.Add(groupService.GetGroup(item.IdGroup));
            }
            Dictionary<StudentViewModel, GroupDTO> studentGroup = new Dictionary<StudentViewModel, GroupDTO>();
            foreach (StudentViewModel studentVM in students)
            {
                studentGroup.Add(studentVM, groupsList.FirstOrDefault(g => g.Id == studentVM.IdGroup));
            }

            if (!String.IsNullOrEmpty(searchSubject))
            {
                studentGroup.Clear();
                    int subjectId = subjectService.Get().Where(s => s.Name == searchSubject).FirstOrDefault().Id;
                    IEnumerable<EducationDTO> educationDtos = educationService.Get().Where(s => s.IdSubject == subjectId);
                    List<int> idStudents = new List<int>();
                    List<StudentDTO> studDtos = new List<StudentDTO>();
                    foreach (var id in idStudents)
                    {
                        studDtos.Add(studentService.GetStudent(id));
                    }
                    students = mapper.Map<List<StudentDTO>, List<StudentViewModel>>(studDtos);
                    foreach (var item in studentDtos)
                    {
                        groupsList.Add(groupService.GetGroup(item.IdGroup));
                    }
                    foreach (StudentViewModel studentVM in students)
                    {
                        if (!studentGroup.ContainsKey(studentVM))
                        studentGroup.Add(studentVM, groupsList.FirstOrDefault(g => g.Id == studentVM.IdGroup));
                    }
            }
            if (studentGroup.Count() == 0) ViewBag.message = "Пошук не дав результатів";
            return View(studentGroup);
        }

        public ActionResult StudentDetails(int idStudent)
        {
            StudentDTO studentDTO = studentService.GetStudent(idStudent);
            studentDTO.StudentAvg = studentService.GetStudentAvg(idStudent);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>()).CreateMapper();
            var studentVM = mapper.Map<StudentDTO, StudentViewModel>(studentDTO);

            ViewBag.studentName = studentVM.Name;
            ViewBag.studentGroup = educationService.GetStudentGroup(idStudent).Name;
            ViewBag.studentAvg = studentVM.StudentAvg;
            ViewBag.studentId = studentVM.Id;

            IEnumerable<EducationDTO> eduDTOs = educationService.Get().Where(e => e.IdStudent == idStudent);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<EducationDTO, EducationViewModel>()).CreateMapper();
            var educationVMs = mapper.Map<IEnumerable<EducationDTO>,IEnumerable<EducationViewModel>>(eduDTOs);

            foreach (var e in educationVMs)
            {
                e.SubjectName = educationService.SetSubjectName(e.Id);
            }

            return View(educationVMs);
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
                Validate validate = new Validate();
                if (!(validate.ValidationStudentName(studentVM.Name))) return Json(new { Success = false, Message = "Input student name again" });

                StudentDTO studentDTO = studentService.GetStudent(studentVM.Id);

                if (studentService.Get().ToList().Contains(studentService.Get().Where(s => s.Name == studentVM.Name).FirstOrDefault()))
                    return Json(new { Success = false, Message = "Student with that name already exists" });

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
            {
                studentService.DeleteStudent(idStudent);
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
                
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

        [HttpGet]
        public ActionResult AddSubject(int idStudent)
        {
            EducationViewModel educationVM = new EducationViewModel();
            educationVM.IdStudent = idStudent;

            var subjects = subjectService.Get();
            ViewBag.subjects = subjects;

            return View("AddSubject", educationVM);
        }
        [HttpPost]
        public ActionResult AddSubject(EducationViewModel educationVM)
        {
            if (ModelState.IsValid)
            {
                Validate validate = new Validate();
                if (!(validate.ValidationSubjectRes(educationVM.SubjectResult))) return Json(new { Success = false, Message = "Input subject result again" });

                int subjectId = subjectService.Get().Where(s => s.Name == educationVM.SubjectName).FirstOrDefault().Id;
                educationVM.IdSubject = subjectId;

                if (educationService.Get().ToList().Contains(educationService.Get().Where(s => s.IdStudent == educationVM.IdStudent).Where(s => s.IdSubject == educationVM.IdSubject).FirstOrDefault()))
                    return Json(new { Success = false, Message = "Student already has this subject" });

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EducationViewModel, EducationDTO>()).CreateMapper();
                EducationDTO educationDTO = mapper.Map<EducationViewModel, EducationDTO>(educationVM);

                educationService.AddSubject(educationDTO);
                return Json(new { Success = true, Message = "Предмет додано до даних студента" });
            }
            return View();
        }

        public ActionResult DeleteEducation(int idEducation)
        {
            educationService.DeleteEducation(idEducation);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult EditEducation(int idEducation)
        {
            EducationDTO educationDTO = educationService.GetEducation(idEducation);
            
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EducationDTO, EducationViewModel>()).CreateMapper();
            EducationViewModel educationVM = mapper.Map<EducationDTO, EducationViewModel>(educationDTO);
            educationVM.SubjectName=educationService.SetSubjectName(educationVM.Id);
            return View("EditEducation", educationVM);
        }

        [HttpPost]
        public ActionResult EditEducation(EducationViewModel educationVM)
        {
            educationVM.IdSubject= subjectService.Get().Where(s => s.Name == educationVM.SubjectName).FirstOrDefault().Id;
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EducationViewModel,EducationDTO>()).CreateMapper();
            EducationDTO educationDTO = mapper.Map<EducationViewModel, EducationDTO>(educationVM);


            educationService.EditEducation(educationDTO);
            return Json(new { Success = true, Message = "Дані про предмет оновлено" });
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