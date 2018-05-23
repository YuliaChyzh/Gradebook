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
        //DbContext context;
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



        /*public IEnumerable<Group> GetAll()
        {
            return db.Groups;
        }

        public Group Get(int id)
        {
            return db.Groups.Find(id);
        }

        public void Create(Group group)
        {
            db.Groups.Add(group);
            db.SaveChanges();
        }

        public void Update(Group group)
        {
            db.Entry(group).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Group FindById(int id)
        {
            return db.Groups.Where(g => g.Id == id).FirstOrDefault();
        }

        public IEnumerable<Group> Find(Func<Group, Boolean> predicate)
        {
            return db.Groups.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Group book = db.Groups.Find(id);
            if (book != null)
                db.Groups.Remove(book);
            db.SaveChanges();
        }*/




    }
}
