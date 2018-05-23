using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using DAL.Entities;

namespace DAL.EF
{
    public class EducationContext : DbContext
    {
        public EducationContext()
            : base("name=EducationContext")
        {
            Database.SetInitializer<EducationContext>(new DataInitializer());
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Education> Educations { get; set; }
    }

    class DataInitializer : DropCreateDatabaseAlways<EducationContext>
    {
        protected override void Seed(EducationContext context)
        {
            base.Seed(context);


                context.Groups.Add(new Group { Name = "ІП-51" });
                context.Groups.Add(new Group { Name = "ІП-52" });
                context.Groups.Add(new Group { Name = "ІП-53" });
                context.Groups.Add(new Group { Name = "ІП-54" });

                context.Students.Add(new Student { Name = "Кравець Анастасія", IdGroup = 1 });
                context.Students.Add(new Student { Name = "Литвинюк Дмитро", IdGroup = 2 });
                context.Students.Add(new Student { Name = "Макаренко Антон", IdGroup = 3 });
                context.Students.Add(new Student { Name = "Сеніва Катерина", IdGroup = 4 });
                context.Students.Add(new Student { Name = "Чиж Юлія", IdGroup = 4 });

                context.Subjects.Add(new Subject { Name = "Технології програмування" });
                context.Subjects.Add(new Subject { Name = "Бази даних" });
                context.Subjects.Add(new Subject { Name = "Компоненти програмної інженерії" });
                context.Subjects.Add(new Subject { Name = "Паралельне програмування" });
                context.Subjects.Add(new Subject { Name = "Архітектура комп'ютера" });

                context.Educations.Add(new Education { IdStudent = 1, IdSubject = 1, SubjectResult = 95 });
                context.Educations.Add(new Education { IdStudent = 2, IdSubject = 2, SubjectResult = 98 });
                context.Educations.Add(new Education { IdStudent = 3, IdSubject = 3, SubjectResult = 97 });
                context.Educations.Add(new Education { IdStudent = 4, IdSubject = 4, SubjectResult = 96 });
                context.Educations.Add(new Education { IdStudent = 5, IdSubject = 5, SubjectResult = 99 });

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

    }
}