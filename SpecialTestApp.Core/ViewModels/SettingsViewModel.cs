using MvvmCross.Navigation;

namespace SpecialTestApp.Core.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(IMvxNavigationService navigationService) : base(navigationService)
        {
        }

        public override void Prepare(object parameter)
        {

        }
    }
}
