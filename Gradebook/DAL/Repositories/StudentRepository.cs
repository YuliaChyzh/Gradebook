using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class StudentRepository: IRepository<Student>
    {
        private EducationContext context;
        //DbContext context;
        DbSet<Student> studentSet;

        public StudentRepository(EducationContext context)
        {
            this.context = context;
            studentSet = context.Set<Student>();
        }

        public IEnumerable<Student> Get()
        {
            return studentSet
                .ToList();
        }

        public IEnumerable<Student> Get(Func<Student, bool> predicate)
        {
            return studentSet
                .Where(predicate)
                .ToList();
        }
        public Student FindById(int id)
        {
            return studentSet
                .Include(p => p.Group)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void Create(Student item)
        {
            studentSet.Add(item);
            context.SaveChanges();
        }
        public void Update(Student item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Remove(Student item)
        {
            studentSet.Remove(item);
            context.SaveChanges();
        }

        /*private EducationContext db;

        public StudentRepository(EducationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Student> GetAll()
        {
            return db.Students.Include(o => o.Group);
        }

        public Student Get(int id)
        {
            return db.Students.Find(id);
        }

        public void Create(Student student)
        {
            db.Students.Add(student);
            db.SaveChanges();
        }

        public void Update(Student student)
        {
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Student FindById(int id)
        {
            return db.Students
                .Include(s => s.Group)
                .Where(s => s.Id == id)
                .FirstOrDefault();
        }


        public IEnumerable<Student> Find(Func<Student, Boolean> predicate)
        {
            return db.Students.Include(o => o.Group).Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Student student = db.Students.Find(id);
            if (student != null)
                db.Students.Remove(student);
            db.SaveChanges();
        }
        */
    }
}
