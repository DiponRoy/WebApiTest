using Db;
using Ninject.Modules;

namespace Web.Api.Ioc
{
    public class NinjectIocModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICmsDb>().To<CmsDb>().InSingletonScope();
        }
    }
}