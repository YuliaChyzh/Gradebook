﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using DAL.Entities;
using DAL.Interfaces;
using BLL.Infrastructure;
using BLL.Interfaces;
using AutoMapper;

namespace BLL.Services
{
    public class GroupService : IGroupService
    {
        IUnitOfWork Database { get; set; }

        public GroupService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<GroupDTO> Get()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Group>, List<GroupDTO>>(Database.GroupsRepository.Get());
        }

        public GroupDTO GetGroup(int id)
        {
            var group = Database.GroupsRepository.FindById(id);
            //group.Id = id;
            if (group == null) return null;
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Group, GroupDTO>()).CreateMapper();
            return mapper.Map<Group, GroupDTO>(group);
        }

        public bool DeleteGroup(int id, int countStudents)
        {
            Group group = Database.GroupsRepository.FindById(id);

            if (group != null && countStudents == 0)
                Database.GroupsRepository.Remove(group);
            else
                return false;

            Database.SaveChanges();
            return true;
        }

        public void EditGroup(GroupDTO groupDTO)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<GroupDTO, Group>()).CreateMapper();
            Group group = Database.GroupsRepository.FindById(groupDTO.Id);
            group = mapper.Map<GroupDTO, Group>(groupDTO);

            Database.GroupsRepository.Update(group);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
