using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using BLL.Services;
using BLL.Interfaces;

namespace PL.Util
{
    public class EducationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEducationService>().To<EducationService>();
            Bind<IStudentService>().To<StudentService>();
            Bind<ISubjectService>().To<SubjectService>();
            Bind<IGroupService>().To<GroupService>();

        }
    }
}