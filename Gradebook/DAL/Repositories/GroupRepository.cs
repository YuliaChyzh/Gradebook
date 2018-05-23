﻿using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repositories
{
    class GroupRepository : IRepository<Group>
    {
        private EducationContext context;
        DbSet<Group> groupSet;

        public GroupRepository(EducationContext context)
        {
            this.context = context;
            groupSet = context.Set<Group>();
        }

        public IEnumerable<Group> Get()
        {
            return groupSet
                .ToList();
        }

        public IEnumerable<Group> Get(Func<Group, bool> predicate)
        {
            return groupSet
                .Where(predicate)
                .ToList();
        }
        public Group FindById(int id)
        {
            return groupSet
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void Create(Group item)
        {
            groupSet.Add(item);
            context.SaveChanges();
        }
        public void Update(Group item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Remove(Group item)
        {
            groupSet.Remove(item);
            context.SaveChanges();
        }
    }
}
