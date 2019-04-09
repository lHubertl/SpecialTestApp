using MvvmCross.Navigation;
using SpecialTestApp.Core.Services;

namespace SpecialTestApp.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserService _userService;

        public ProfileViewModel(IMvxNavigationService navigationService, IUserService userService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _userService = userService;
        }

        public override void Prepare(object parameter)
        {
            
        }
    }
}
