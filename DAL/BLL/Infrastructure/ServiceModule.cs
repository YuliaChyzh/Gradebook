using Ninject.Modules;
using DAL.Interfaces;
using DAL.Repositories;

namespace BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
/*ServiceModule представляет специальный модуль Ninject, который служит для организации сопоставления зависимостей.
 * В частности, он устанавливает использование EFUnitOfWork в качестве объекта IUnitOfWork.
 * Кроме того, здесь через конструктор передается название подключения, которое в итоге будет
 * определяться в файле web.config проекта, представляющего уровень представления.*/
