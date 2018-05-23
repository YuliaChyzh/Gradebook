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
        //DbContext context;
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

        /*private EducationContext db;

        public SubjectRepository(EducationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Subject> GetAll()
        {
            return db.Subjects;
        }

        public Subject Get(int id)
        {
            return db.Subjects.Find(id);
        }

        public void Create(Subject subject)
        {
            db.Subjects.Add(subject);
            db.SaveChanges();
        }

        public void Update(Subject subject)
        {
            db.Entry(subject).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Subject FindById(int id)
        {
            return db.Subjects
                .Where(s => s.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Subject> Find(Func<Subject, Boolean> predicate)
        {
            return db.Subjects.Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject != null)
                db.Subjects.Remove(subject);
            db.SaveChanges();
        }
        */
    }
}
