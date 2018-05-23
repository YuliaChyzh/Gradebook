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
    }
}
