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
using Newtonsoft.Json;
using PL.Models.EditModels;
using PL.Validation;

namespace PL.Controllers
{
    public class GroupController : Controller
    {
        IStudentService studentService;
        ISubjectService subjectService;
        IGroupService groupService;
        IEducationService educationService;

        public GroupController() { }

        public GroupController(IStudentService studentService, ISubjectService subjectService, IGroupService groupService, IEducationService educationService)
        {
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.groupService = groupService;
            this.educationService = educationService;
        }

        // GET: Group
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowGroups()
        {
            IEnumerable<GroupDTO> groupDtos = groupService.Get().OrderBy(g=>g.Name);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, GroupViewModel>()).CreateMapper();
            var groups = mapper.Map<IEnumerable<GroupDTO>, List<GroupViewModel>>(groupDtos);

            return View(groups);
        }

        public ActionResult GroupDetails(int idGroup)
        {
            IEnumerable<StudentDTO> studentDtos = studentService.GetGroupList(idGroup);
            List<StudentDTO> studentList = new List<StudentDTO>();

            foreach (StudentDTO student in studentDtos)
            {
                studentList.Add(studentService.GetStudentAvg(student.Id));
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, StudentViewModel>()).CreateMapper();
            var students = mapper.Map<List<StudentDTO>, List<StudentViewModel>>(studentList);

            ViewBag.groupName = groupService.GetGroup(idGroup).Name;
            return View(students);
        }

        public ActionResult DeleteGroup(int idGroup)
        {
            IEnumerable<StudentDTO> studentList = studentService.GetGroupList(idGroup);

            if (groupService.DeleteGroup(idGroup, studentList.Count()))
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            if (groupService.Get().FirstOrDefault(r => r.Id == idGroup) == null)
                return Json(new { Succes = false, Message = "Group doesn`t exist" }, JsonRequestBehavior.AllowGet);

            if (studentList.Count() != 0)
                return Json(new { Succes = false, Message = "It group have students yet" }, JsonRequestBehavior.AllowGet);

            return Json(new { Success = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroup(int idGroup)
        {
            GroupDTO groupDTO = groupService.GetGroup(idGroup);

            EditGroupViewModel editGroupVM = new EditGroupViewModel()
            {
                Id = groupDTO.Id,
                Name = groupDTO.Name
            };

            return View("EditGroup", editGroupVM);
        }

        [HttpPost]
        public ActionResult EditGroup(EditGroupViewModel editGroupVM)
        {
            Validate validate = new Validate();
            if (!(validate.ValidationGroupName(editGroupVM.Name))) return Json(new { Success = false });

            GroupDTO groupDTO = groupService.GetGroup(editGroupVM.Id);

            if (groupService.Get().ToList().Contains(groupService.Get().Where(g => g.Name == editGroupVM.Name).FirstOrDefault()))
                return Json(new { Success = false, Message = "Group with that name already exists" });

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditGroupViewModel, GroupDTO>()).CreateMapper();
            groupDTO = mapper.Map<EditGroupViewModel, GroupDTO>(editGroupVM);

            groupService.EditGroup(groupDTO);

            IEnumerable<GroupDTO> groupDtos = groupService.Get();
            var mapperGroups = new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, GroupViewModel>()).CreateMapper();
            var groups = mapperGroups.Map<IEnumerable<GroupDTO>, List<GroupViewModel>>(groupDtos);
            return View("ShowGroups", groups);
        }

        /*public ActionResult RecordGroup(GroupDTO groupDTO)
        {
            return View(groupDTO);
        }*/


        [HttpGet]
        public ActionResult CreateGroup()
        {
            GroupViewModel groupVM = new GroupViewModel();
            return View(groupVM);
        }
        [HttpPost]
        public ActionResult CreateGroup(GroupViewModel groupVM)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<GroupViewModel, GroupDTO>()).CreateMapper();
                GroupDTO groupDTO = mapper.Map<GroupViewModel, GroupDTO>(groupVM);
                groupService.AddGroup(groupDTO);

                IEnumerable<GroupDTO> groupDtos = groupService.Get();
                var mapperGroups = new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, GroupViewModel>()).CreateMapper();
                var groups = mapperGroups.Map<IEnumerable<GroupDTO>, List<GroupViewModel>>(groupDtos);
                return View("ShowGroups", groups);
            }
            return View();
        }

        
    }
}