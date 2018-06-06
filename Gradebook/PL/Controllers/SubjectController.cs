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
    public class SubjectController : Controller
    {
        ISubjectService subjectService;
        IEducationService educationService;

        public SubjectController() { }

        public SubjectController(ISubjectService subjectService, IEducationService educationService)
        {
            this.subjectService = subjectService;
            this.educationService = educationService;
        }

        // Subject/ShowSubject
        [HttpGet]
        public ActionResult ShowSubject(string searchName, string searchSubjectAvg, string searchProgress)
        {
            ViewBag.subjects = subjectService.Get().OrderBy(s => s.SubjectAvg);

            IEnumerable<SubjectDTO> subjectDtos = subjectService.Get().OrderBy(s=>s.Name);

            if (!String.IsNullOrEmpty(searchName))
            {
                subjectDtos = subjectService.SearchByName(subjectDtos, searchName);
            }
            if (!String.IsNullOrEmpty(searchSubjectAvg) && (searchSubjectAvg != "Всі"))
            {
                subjectDtos = subjectService.SearchBySubjectAvg(subjectDtos, searchSubjectAvg);
            }
            if (!String.IsNullOrEmpty(searchProgress))
            {
                subjectDtos = subjectService.SearchByProgress(subjectDtos, searchProgress);
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectDTO, SubjectViewModel>()).CreateMapper();
            var students = mapper.Map<IEnumerable<SubjectDTO>, IEnumerable<SubjectViewModel>>(subjectDtos);

            return View(students);
        }

        // Subject/SubjectDetails
        public ActionResult SubjectDetails(int idSubject)
        {
            SubjectDTO subjectDTO = subjectService.GetSubject(idSubject);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectDTO, SubjectViewModel>()).CreateMapper();
            var subjectVM = mapper.Map<SubjectDTO, SubjectViewModel>(subjectDTO);

            ViewBag.subjectName = subjectVM.Name;
            ViewBag.subjectAvg = subjectService.GetSubjectAvg(idSubject).ToString("0.00");

            Dictionary<string, int> subjectResult = educationService.GetSubjectDetail(idSubject);
            return View(subjectResult);
        }

        // Subject/DeleteSubject
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
                ViewBag.message = "Помилка видалення: студенти ще мають оцінки з даного предмету";
                return View("Report");
            }

            ViewBag.message = "Помилка при видаленні";
            return View("Report");
        }

        // Subject/EditSubject
        [HttpGet]
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

            SubjectDTO subjectDTO1 = subjectService.Get().Where(s => s.Name == editSubjectVM.Name).FirstOrDefault();

            if (subjectDTO1 != null)
            {
                ViewBag.message = "Предмет з такою назвою вже існує";
                return View("Report");
            }
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditSubjectViewModel, SubjectDTO>()).CreateMapper();
            SubjectDTO subjectDTO = mapper.Map<EditSubjectViewModel, SubjectDTO>(editSubjectVM);

            subjectService.EditSubject(subjectDTO);
            ViewBag.message = "Предмет оновлено";
            return View("Report");
        }

        // Subject/CreateSubject
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

                SubjectDTO subjectDTO1 = subjectService.Get().Where(s => s.Name == subjectVM.Name).FirstOrDefault();

                if(subjectDTO1!=null)
                {
                    ViewBag.message = "Предмет з такою назвою вже існує";
                    return View("Report");
                }

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SubjectViewModel, SubjectDTO>()).CreateMapper();
                SubjectDTO subjectDTO = mapper.Map<SubjectViewModel, SubjectDTO>(subjectVM);
                subjectService.AddSubject(subjectDTO);

                ViewBag.message = "Предмет додано";
                return View("Report");
            }
            return View();
        }

        // Subject/Report
        public ActionResult Report()
        {
            return View();
        }
    }
}