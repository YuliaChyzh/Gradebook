using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces;
using BLL.DTO;
using PL.Models;
using AutoMapper;
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

        // Student/ShowStudent
        [HttpGet]
        public ActionResult ShowStudent(string searchName, string searchGroup, int? searchStudentAvg, string searchProgress, string searchSubject )
        {
            var groups = groupService.Get();
            ViewBag.groups = groups;
            var subjects = subjectService.Get();
            ViewBag.subjects = subjects;

            IEnumerable<StudentDTO> studentDtos = studentService.Get().OrderBy(s => s.Name);

            if (!String.IsNullOrEmpty(searchName))
            {
                studentDtos = studentService.SearchByName(studentDtos, searchName);
            }
            if (!String.IsNullOrEmpty(searchGroup))
            {
                int groupId = groupService.Get().Where(g => g.Name == searchGroup).FirstOrDefault().Id;
                studentDtos = studentService.SearchByGroup(studentDtos, groupId);
            }
            if (!String.IsNullOrEmpty(searchStudentAvg.ToString()))
            {
                studentDtos = studentService.SearchByStudentAvg(studentDtos, (int)searchStudentAvg);
            }
            if (!String.IsNullOrEmpty(searchProgress))
            {
                studentDtos = studentService.SearchByProgress(studentDtos, searchProgress);
            }
            if (!String.IsNullOrEmpty(searchSubject))
            {

                int subjectId = subjectService.Get().Where(s => s.Name == searchSubject).FirstOrDefault().Id;
                IEnumerable<EducationDTO> educationDtos = educationService.Get().Where(s => s.IdSubject == subjectId);
                studentDtos = studentService.SearchBySubject(studentDtos, educationDtos);
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

            if (studentGroup.Count() == 0) ViewBag.message = "Пошук не дав результатів";
            return View(studentGroup);
        }

        // Student/StudentDetails
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
                if (!(validate.ValidationStudentName(studentVM.Name)))
                {
                    ViewBag.message = "Введіть ім'я студента ще раз";
                    return View("Report");
                }

                if(studentVM.IdGroup==0)
                {
                    ViewBag.message = "Введіть групу ім'я студента";
                    return View("Report");
                }
                StudentDTO studentDTO = studentService.GetStudent(studentVM.Id);

                if (studentService.Get().ToList().Contains(studentService.Get().Where(s => s.Name == studentVM.Name).FirstOrDefault()))
                {
                    ViewBag.message = "Студент з таким ім'ям вже існує";
                    return View("Report");
                }

                int groupId = groupService.Get().Where(g => g.Name == studentVM.GroupName).FirstOrDefault().Id;
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentViewModel, StudentDTO>()).CreateMapper();
                StudentDTO student = mapper.Map<StudentViewModel, StudentDTO>(studentVM);
                student.IdGroup = groupId;

                studentService.AddStudent(student);
                ViewBag.message = "Студента успішно створено";
                return View("Report");
            }
            return View();
        }

        // Student/DeleteStudent
        public ActionResult DeleteStudent(int idStudent)
        {
            IEnumerable<EducationDTO> eduList = educationService.GetStudentList(idStudent);

            if (eduList.Count() == 0)
            {
                studentService.DeleteStudent(idStudent);
                ViewBag.message = "Студента успішно видалено";
                return View("Report");
            }
                
            if (studentService.Get().FirstOrDefault(s => s.Id == idStudent) == null)
            {
                ViewBag.message = "Студент не існує";
                return View("Report");
            }

            if (eduList.Count() != 0)
            {
                ViewBag.message = "Студент ще має предмети вивчення";
                return View("Report");
            }

            ViewBag.message = "Помилка при видаленні";
            return View("Report");
        }

        // Student/EditStudent
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
            else
            {
                ViewBag.message = "Оберіть групу";
                return View("Report");
            }

            Validate validate = new Validate();
            if (!(validate.ValidationStudentName(editStudentVM.Name)))
            {
                ViewBag.message = "Введіть ім'я студента ще раз";
                return View("Report");
            }

            StudentDTO studentDTO = studentService.GetStudent(editStudentVM.Id);

            if (studentService.Get().ToList().Contains(studentService.Get().Where(s => s.Name == editStudentVM.Name).FirstOrDefault()))
            {
                ViewBag.message = "Студент з таким ім'ям вже існує";
                return View("Report");
            }

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditStudentViewModel, StudentDTO>()).CreateMapper();
            studentDTO = mapper.Map<EditStudentViewModel, StudentDTO>(editStudentVM);

            studentService.EditStudent(studentDTO);
            ViewBag.message = "Дані студента оновлено";
            return View("Report");
        }

        // Student/AddSubject
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
                if (!(validate.ValidationSubjectRes(educationVM.SubjectResult))) {
                    ViewBag.message = "Введіть оцінку ще раз";
                    return View("Report");
                }

                int subjectId = subjectService.Get().Where(s => s.Name == educationVM.SubjectName).FirstOrDefault().Id;
                educationVM.IdSubject = subjectId;

                if (educationService.Get().ToList().Contains(educationService.Get().Where(s => s.IdStudent == educationVM.IdStudent).Where(s => s.IdSubject == educationVM.IdSubject).FirstOrDefault()))
                {
                    ViewBag.message = "Студент вже має такий предмет";
                    return View("Report");
                }

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EducationViewModel, EducationDTO>()).CreateMapper();
                EducationDTO educationDTO = mapper.Map<EducationViewModel, EducationDTO>(educationVM);

                educationService.AddSubject(educationDTO);
                ViewBag.message = "Предмет додано до даних студента";
                return View("Report");
            }
            return View();
        }

        // Student/DeleteEducation
        public ActionResult DeleteEducation(int idEducation)
        {
            educationService.DeleteEducation(idEducation);
            ViewBag.message = "Видалення успішне";
            return View("Report");

        }

        // Student/EditEducation
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
            ViewBag.message = "Дані про предмет оновлено";
            return View("Report");
        }

        // Student/Report
        public ActionResult Report()
        {
            return View();
        }

    }
}