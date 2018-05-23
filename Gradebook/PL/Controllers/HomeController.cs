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

        public HomeController() {}

        public HomeController(IStudentService studentService, ISubjectService subjectService, IGroupService groupService)
        {
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.groupService = groupService;
         }

        public ActionResult Index()
        {        
            //IEnumerable<GroupDTO> groupDtos = groupService.GetGroups();
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, GroupViewModel>()).CreateMapper();
            //var groups = mapper.Map<IEnumerable<GroupDTO>, List<GroupViewModel>>(groupDtos);
            //ViewBag.groups = groups;

            IEnumerable<StudentDTO> studentDtos = studentService.GetStudents();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>()).CreateMapper();
            var students = mapper.Map<IEnumerable<StudentDTO>, List<StudentViewModel>>(studentDtos);

            List < GroupDTO > groups= new List<GroupDTO>();
            foreach (var item in studentDtos)
            {
                groups.Add(groupService.GetGroup(item.IdGroup));
            }
            ViewBag.groups = groups;
            return View(students);
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