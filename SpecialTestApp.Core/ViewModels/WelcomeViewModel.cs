using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using SpecialTestApp.Core.Models;
using SpecialTestApp.Core.Services;

namespace SpecialTestApp.Core.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private UserModel _userModel;

        private string _favoritePetImageSource;
        public string FavoritePetImageSource
        {
            get => _favoritePetImageSource;
            set => SetProperty(ref _favoritePetImageSource, value);
        }

        private string _welcomeText;
        public string WelcomeText
        {
            get => _welcomeText;
            set => SetProperty(ref _welcomeText, value);
        }

        public ICommand OpenRadialMenuCommand => new MvxAsyncCommand(ExecuteOpenRadialMenuCommand);

        public WelcomeViewModel(IMvxNavigationService navigationService,
            IUserService userService) : base(navigationService)
        {
            _userService = userService;
        }

        public override void Prepare(object parameter)
        {

        }

        public override async Task Initialize()
        {
            await base.Initialize();

            _userModel = _userService.GetUserModel();

            FavoritePetImageSource = _userModel.FavoritePetImageSource;
            WelcomeText = $"Hello world, it is now {DateTime.Now:g}";
        }

        private Task ExecuteOpenRadialMenuCommand()
        {
            var userImageUrl = _userModel.ImageSource;
            return NavigationService.Navigate<RadialMenuViewModel, string>(userImageUrl);
        }
    }
}
