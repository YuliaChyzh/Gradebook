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
    public class SubjectController : Controller
    {
        IStudentService studentService;
        ISubjectService subjectService;
        IGroupService groupService;
        IEducationService educationService;

        public SubjectController() { }

        public SubjectController(IStudentService studentService, ISubjectService subjectService, IGroupService groupService, IEducationService educationService)
        {
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.groupService = groupService;
            this.educationService = educationService;
        }

        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowSubject()
        {
            IEnumerable<SubjectDTO> subjectDtos = subjectService.Get().OrderBy(s=>s.Name);
            List<SubjectDTO> subjectList = new List<SubjectDTO>();
            foreach (var item in subjectDtos)
            {
                subjectList.Add(subjectService.GetSubjectAvg(item.Id));
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectDTO, SubjectViewModel>()).CreateMapper();
            var students = mapper.Map<List<SubjectDTO>, List<SubjectViewModel>>(subjectList);

            return View(students);
        }

        public ActionResult SubjectDetails(int idSubject)
        {
            SubjectDTO subjectDTO = subjectService.GetSubject(idSubject);
            subjectDTO = subjectService.GetSubjectAvg(idSubject);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectDTO, SubjectViewModel>()).CreateMapper();
            var subjectVM = mapper.Map<SubjectDTO, SubjectViewModel>(subjectDTO);

            ViewBag.subjectName = subjectVM.Name;
            ViewBag.subjectAvg = subjectVM.SubjectAvg;

            Dictionary<string, int> subjectResult = educationService.GetSubjectDetail(idSubject);
            return View(subjectResult);
        }

        public ActionResult DeleteSubject(int idSubject)
        {
            IEnumerable<EducationDTO> eduList = educationService.GetSubbjectList(idSubject);

            if (eduList.Count()==0)
            {
                subjectService.DeleteSubject(idSubject);
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
                
            if (subjectService.Get().FirstOrDefault(s=>s.Id==idSubject)==null)
                return Json(new { Succes = false, Message = "Subject doesn`t exist" }, JsonRequestBehavior.AllowGet);

            if (eduList.Count() != 0)
                return Json(new { Succes = false, Message = "Students have this subject yet" }, JsonRequestBehavior.AllowGet);

            return Json(new { Success = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSubject(int idSubject)
        {
            SubjectDTO subjectDTO = subjectService.GetSubject(idSubject);

            EditSubjectViewModel editSubjectVM = new EditSubjectViewModel()
            {
                Id = subjectDTO.Id,
                Name = subjectDTO.Name,
                SubjectAvg= subjectDTO.SubjectAvg
            };

            return View("EditSubject", editSubjectVM);
        }

        [HttpPost]
        public ActionResult EditSubject(EditSubjectViewModel editSubjectVM)
        {
            Validate validate = new Validate();
            if (!(validate.ValidationSubjectName(editSubjectVM.Name))) return Json(new { Success = false, Message = "Input subject name again" });

            SubjectDTO subjectDTO = subjectService.GetSubject(editSubjectVM.Id);

            if (subjectService.Get().ToList().Contains(subjectService.Get().Where(s => s.Name == editSubjectVM.Name).FirstOrDefault()))
                return Json(new { Success = false, Message = "Subject with that name already exists" });

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditSubjectViewModel, SubjectDTO>()).CreateMapper();
            subjectDTO = mapper.Map<EditSubjectViewModel, SubjectDTO>(editSubjectVM);

            subjectService.EditSubject(subjectDTO);

            return Json(new { Success = true, Message = "Предмет оновлено" });
        }

        [HttpGet]
        public ActionResult CreateSubject()
        {
            SubjectViewModel subjectVM = new SubjectViewModel();
            return View(subjectVM);
        }
        [HttpPost]
        public ActionResult CreateSubject(SubjectViewModel subjectVM)
        {
            if (ModelState.IsValid)
            {
                Validate validate = new Validate();
                if (!(validate.ValidationSubjectName(subjectVM.Name))) return Json(new { Success = false, Message = "Input subject name again" });

                SubjectDTO subjectDTO = subjectService.GetSubject(subjectVM.Id);

                if (subjectService.Get().ToList().Contains(subjectService.Get().Where(s => s.Name == subjectVM.Name).FirstOrDefault()))
                    return Json(new { Success = false, Message = "Subject with that name already exists" });

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectViewModel, SubjectDTO>()).CreateMapper();
                subjectDTO = mapper.Map<SubjectViewModel, SubjectDTO>(subjectVM);
                subjectService.AddSubject(subjectDTO);

                return Json(new { Success = true, Message = "Предмет додано" });
            }
            return View();
        }
    }
}