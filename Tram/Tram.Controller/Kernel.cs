using Ninject;
using Tram.Controller.Controllers;
using Tram.Controller.Repositories;

namespace Tram.Controller
{
    public static class Kernel
    {
        private static IKernel ninjectKernel;

        static Kernel()
        {
            ninjectKernel = new StandardKernel();

            ninjectKernel.Bind<DirectxController>().ToSelf().InSingletonScope();
            ninjectKernel.Bind<VehiclesController>().ToSelf().InSingletonScope();
            ninjectKernel.Bind<MainController>().ToSelf().InSingletonScope();
            ninjectKernel.Bind<CapacityController>().ToSelf().InSingletonScope();


            ninjectKernel.Bind<IRepository>().To<FileRepository>().InSingletonScope();
        }

        public static T Get<T>()
        {
            return ninjectKernel.Get<T>();
        }
    }
}
