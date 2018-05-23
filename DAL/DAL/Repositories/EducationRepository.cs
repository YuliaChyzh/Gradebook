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
                .ToList();
        }

        public IEnumerable<Education> Get(Func<Education, bool> predicate)
        {
            return eduSet
                .Where(predicate)
                .ToList();
        }
        public Education FindById(int id)
        {
            return eduSet
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
            eduSet.Remove(item);
            context.SaveChanges();
        }



        /*private EducationContext db;

        public EducationRepository(EducationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Education> GetAll()
        {
            return db.Educations.Include(o => o.Student).Include(o => o.Subject);
        }

        public Education Get(int id)
        {
            return db.Educations.Find(id);
        }

        public void Create(Education education)
        {
            db.Educations.Add(education);
            db.SaveChanges();
        }

        public Education FindById(int id)
        {
            return db.Educations
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .Where(e => e.Id == id)
                .FirstOrDefault();
        }

        public void Update(Education education)
        {
            db.Entry(education).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IEnumerable<Education> Find(Func<Education, Boolean> predicate)
        {
            return db.Educations.Include(o => o.Student).Include(o => o.Subject).Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Education education = db.Educations.Find(id);
            if (education != null)
                db.Educations.Remove(education);
            db.SaveChanges();
        }
        */
    }
}
