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
            IEnumerable<GroupDTO> groupDtos = groupService.Get();
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
                return Json(new { Succes = false, Message = "Group isn`t exist" }, JsonRequestBehavior.AllowGet);

            if (studentList.Count() != 0)
                return Json(new { Succes = false, Message = "It group have students yet" }, JsonRequestBehavior.AllowGet);

            return Json(new { Success = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroup(int idGroup)
        {
            GroupDTO groupDTO = groupService.GetGroup(idGroup);

            EditGroupViewModel editgroupVM = new EditGroupViewModel()
            {
                Id = groupDTO.Id,
                Name = groupDTO.Name
            };

            return View("EditGroup", editgroupVM);
        }

        [HttpPost]
        public ActionResult EditProfile(string editVMJSON)
        {
            /*IMapper mapperUser = new MapperConfiguration(cfg => cfg.CreateMap<EditViewModel, UserDTO>()).CreateMapper();
            EditViewModel editVM = JsonConvert.DeserializeObject<EditViewModel>(editVMJSON);

            Validate validate = new Validate();
            if (!(validate.ValidationName(editVM.FirstName) && validate.ValidationName(editVM.LastName) && validate.ValidationPhone(editVM.Phone) && validate.ValidationEmail(editVM.Email)))
                return Json(new { Success = false });

            RoleDTO roleDTO = null;
            if (editVM.IsAdmin)
                roleDTO = roleService.GetAllRoles().Where(r => r.Name == "Admin").First();
            else
                roleDTO = roleService.GetAllRoles().Where(r => r.Name == "User").First();

            UserDTO userDTO = userService.GetUserById(editVM.UserId);

            if (hotelService.GetHotelsForUser(userDTO.UserId).Where(h => h.UserId == userDTO.UserId).ToList().Count != 0)
                return Json(new { Success = false, Message = "Delete all your hotels" });

            userDTO = mapperUser.Map<EditViewModel, UserDTO>(editVM);
            userDTO.RoleDTO = roleDTO;
            userDTO.RoleId = roleDTO.RoleId;

            userService.EditUser(userDTO);

            return Json(new { Success = true });*/
        }

    }
}