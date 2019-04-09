using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace SpecialTestApp.Core.ViewModels
{
    public class RadialMenuViewModel : MvxViewModel<string> 
    {
        private readonly IMvxNavigationService _navigationService;

        private string _userImageSource;
        public string UserImageSource
        {
            get => _userImageSource;
            set => SetProperty(ref _userImageSource, value);
        }

        public ICommand BackCommand => new MvxAsyncCommand(ExecuteBackCommand);

        public ICommand ToProfileCommand => new MvxAsyncCommand(ExecuteToProfileCommand);

        public RadialMenuViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private Task ExecuteBackCommand()
        {
            return _navigationService.Close(this);
        }

        public override void Prepare(string parameter)
        {
            UserImageSource = parameter;
        }

        private async Task ExecuteToProfileCommand()
        {
            await _navigationService.Close(this);
            await _navigationService.Navigate<ProfileViewModel>();
        }
    }
}
