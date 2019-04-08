using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SpecialTestApp.Core.Models;
using SpecialTestApp.Core.Services;

namespace SpecialTestApp.Core.ViewModels
{
    public class WelcomeViewModel : MvxViewModel
    {
        private readonly IUserService _userService;
        private readonly IMvxNavigationService _navigationService;
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

        public WelcomeViewModel(IUserService userService, IMvxNavigationService navigationService)
        {
            _userService = userService;
            _navigationService = navigationService;
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
            return _navigationService.Navigate<RadialMenuViewModel, string>(userImageUrl);
        }
    }
}
