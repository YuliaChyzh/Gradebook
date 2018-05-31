using System;
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
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Group> Get(Func<Group, bool> predicate)
        {
            return groupSet
                .AsNoTracking()
                .Where(predicate)
                .ToList();
        }
        public Group FindById(int id)
        {
            return groupSet
                .AsNoTracking()
                .Where(p => p.Id == id)
                .AsNoTracking()
                .FirstOrDefault();
        }
        public void Create(Group item)
        {
            groupSet.Add(item);
            context.SaveChanges();
        }
        public void Update(Group item)
        {
            //var c = groupSet.Local.FirstOrDefault(g => g.Id == item.Id);
               
            //if (c != null)
            //{
            //    context.Entry(c).State = EntityState.Detached;
            //    //context.Entry(c).Property("prop").IsModified = true;
            //}
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
