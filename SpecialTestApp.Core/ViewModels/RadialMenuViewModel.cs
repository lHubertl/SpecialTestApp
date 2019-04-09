using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace SpecialTestApp.Core.ViewModels
{
    public class RadialMenuViewModel : BaseViewModel<string> 
    {
        private string _userImageSource;
        public string UserImageSource
        {
            get => _userImageSource;
            set => SetProperty(ref _userImageSource, value);
        }

        public ICommand ToProfileCommand => new MvxAsyncCommand(ExecuteToProfileCommand);

        public RadialMenuViewModel(IMvxNavigationService navigationService)
            : base(navigationService)
        {

        }

        private Task ExecuteBackCommand()
        {
            return NavigationService.Close(this);
        }

        public override void Prepare(string parameter)
        {
            UserImageSource = parameter;
        }

        private async Task ExecuteToProfileCommand()
        {
            await NavigationService.Navigate<ProfileViewModel>();
            await NavigationService.Close(this);
        }
    }
}
