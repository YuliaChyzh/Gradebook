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
                ViewBag.message = "Предмет успішно видалено";
                return View("Report");
            }
                
            if (subjectService.Get().FirstOrDefault(s=>s.Id==idSubject)==null)
            {
                ViewBag.message = "Предмет не існує";
                return View("Report");
            }

            if (eduList.Count() != 0)
            {
                ViewBag.message = "Студенти ще мають цей предмет";
                return View("Report");
            }

            ViewBag.message = "Помилка при видаленні";
            return View("Report");
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
            if (!(validate.ValidationSubjectName(editSubjectVM.Name)))
            {
                ViewBag.message = "Введіть назву предмета ще раз";
                return View("Report");
            }

            SubjectDTO subjectDTO = subjectService.GetSubject(editSubjectVM.Id);

            if (subjectService.Get().ToList().Contains(subjectService.Get().Where(s => s.Name == editSubjectVM.Name).FirstOrDefault()))
            {
                ViewBag.message = "Предмет з такою назвою вже існує";
                return View("Report");
            }
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditSubjectViewModel, SubjectDTO>()).CreateMapper();
            subjectDTO = mapper.Map<EditSubjectViewModel, SubjectDTO>(editSubjectVM);

            subjectService.EditSubject(subjectDTO);
            ViewBag.message = "Предмет оновлено";
            return View("Report");
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
                if (!(validate.ValidationSubjectName(subjectVM.Name))) {
                    ViewBag.message = "Введіть назву предмета ще раз";
                    return View("Report");
                }

                SubjectDTO subjectDTO = subjectService.GetSubject(subjectVM.Id);

                if (subjectService.Get().ToList().Contains(subjectService.Get().Where(s => s.Name == subjectVM.Name).FirstOrDefault()))
                {
                    ViewBag.message = "Предмет з такою назвою вже існує";
                    return View("Report");
                }

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectViewModel, SubjectDTO>()).CreateMapper();
                subjectDTO = mapper.Map<SubjectViewModel, SubjectDTO>(subjectVM);
                subjectService.AddSubject(subjectDTO);

                ViewBag.message = "Предмет додано";
                return View("Report");
            }
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }
    }
}