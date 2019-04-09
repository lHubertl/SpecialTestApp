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

        public ICommand ToScreenCommand => new MvxAsyncCommand<string>(ExecuteToScreenCommand);

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

            // This is a workaround to hide fragment page
            await NavigationService.Close(this);
        }

        private async Task ExecuteToScreenCommand(string navigationViewModel)
        {
            Task task = null;

            switch (navigationViewModel)
            {
                case nameof(FavoritesViewModel):
                    task = NavigationService.Navigate<FavoritesViewModel>();
                    break;
                case nameof(NotificationsViewModel):
                    task = NavigationService.Navigate<NotificationsViewModel>();
                    break;
                case nameof(SettingsViewModel):
                    task = NavigationService.Navigate<SettingsViewModel>();
                    break;
                case nameof(BasketViewModel):
                    task = NavigationService.Navigate<BasketViewModel>();
                    break;
            }

            if (task != null)
            {
                await task;

                // This is a workaround to hide fragment page
                await NavigationService.Close(this);
            }
        }
    }
}
