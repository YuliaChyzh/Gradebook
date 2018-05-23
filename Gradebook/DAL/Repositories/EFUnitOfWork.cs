using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        /*private EducationContext db;
        private GroupRepository groupRepository;
        private SubjectRepository subjectRepository;
        private StudentRepository studentRepository;
        private EducationRepository educationRepository;*/

        private readonly Lazy<GroupRepository> groupRepository;
        private readonly Lazy<SubjectRepository> subjectRepository;
        private readonly Lazy<StudentRepository> studentRepository;
        private readonly Lazy<EducationRepository> educationRepository;

        public IRepository<Group> GroupsRepository => groupRepository.Value;
        public IRepository<Subject> SubjectsRepository => subjectRepository.Value;
        public IRepository<Student> StudentsRepository => studentRepository.Value;
        public IRepository<Education> EducationsRepository => educationRepository.Value;

        private readonly EducationContext db;

        public EFUnitOfWork(EducationContext db)
        {
            this.db = db;

            groupRepository = new Lazy<GroupRepository>(() => new GroupRepository(db));
            subjectRepository = new Lazy<SubjectRepository>(() => new SubjectRepository(db));
            studentRepository = new Lazy<StudentRepository>(()=>new StudentRepository(db));
            educationRepository = new Lazy<EducationRepository>(()=>new EducationRepository(db));
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
