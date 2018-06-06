using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class EducationRepository:IRepository<Education>
    {
        //DbContext context;
        EducationContext context;
        DbSet<Education> eduSet;

        public EducationRepository(EducationContext context)
        {
            this.context = context;
            eduSet = context.Set<Education>();
        }

        public IEnumerable<Education> Get()
        {
            return eduSet
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Education> Get(Func<Education, bool> predicate)
        {
            return eduSet
                .AsNoTracking()
                .Where(predicate)
                .ToList();
        }
        public Education FindById(int id)
        {
            return eduSet
                .AsNoTracking()
                .Include(p => p.Student)
                .Include(p=>p.Subject)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void Create(Education item)
        {
            eduSet.Add(item);
            context.SaveChanges();
        }
        public void Update(Education item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Remove(Education item)
        { 
            eduSet.Remove(eduSet.Where(e=>e.Id==item.Id).FirstOrDefault());
            context.SaveChanges();
        }
    }
}
