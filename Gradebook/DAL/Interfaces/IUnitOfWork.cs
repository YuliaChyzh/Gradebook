using System;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Student> StudentsRepository { get; }
        IRepository<Subject> SubjectsRepository { get; }
        IRepository<Group> GroupsRepository { get; }
        IRepository<Education> EducationsRepository { get; }
        void SaveChanges();
     }
}
