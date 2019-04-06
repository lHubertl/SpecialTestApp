using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace SpecialTestApp.Core.ViewModels
{
    public class RadialMenuViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public ICommand BackCommand => new MvxAsyncCommand(ExecuteBackCommand);

        public RadialMenuViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private Task ExecuteBackCommand()
        {
            return _navigationService.Close(this);
        }
    }
}
