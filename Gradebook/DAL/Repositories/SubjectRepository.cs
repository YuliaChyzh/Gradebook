using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class SubjectRepository: IRepository<Subject>
    {
        EducationContext context;
        DbSet<Subject> subjectSet;

        public SubjectRepository(EducationContext context)
        {
            this.context = context;
            subjectSet = context.Set<Subject>();
        }

        public IEnumerable<Subject> Get()
        {
            return subjectSet
                .ToList();
        }

        public IEnumerable<Subject> Get(Func<Subject, bool> predicate)
        {
            return subjectSet
                .Where(predicate)
                .ToList();
        }
        public Subject FindById(int id)
        {
            return subjectSet
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void Create(Subject item)
        {
            subjectSet.Add(item);
            context.SaveChanges();
        }
        public void Update(Subject item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Remove(Subject item)
        {
            subjectSet.Remove(item);
            context.SaveChanges();
        }
    }
}
