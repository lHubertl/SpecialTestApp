using MvvmCross.Navigation;

namespace SpecialTestApp.Core.ViewModels
{
    public class BasketViewModel : BaseViewModel
    {
        public BasketViewModel(IMvxNavigationService navigationService) : base(navigationService)
        {
        }

        public override void Prepare(object parameter)
        {

        }
    }
}
