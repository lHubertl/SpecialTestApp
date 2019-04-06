using MvvmCross;
using MvvmCross.ViewModels;
using SpecialTestApp.Core.Services;
using SpecialTestApp.Core.ViewModels;

namespace SpecialTestApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<IUserService, UserService>();

            RegisterAppStart<WelcomeViewModel>();
        }
    }
}
