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
            IEnumerable<SubjectDTO> subjectDtos = subjectService.GetSubjects();
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
    }
}