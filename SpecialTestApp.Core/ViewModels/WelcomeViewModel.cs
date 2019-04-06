using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SpecialTestApp.Core.Services;

namespace SpecialTestApp.Core.ViewModels
{
    public class WelcomeViewModel : MvxViewModel
    {
        private readonly IUserService _userService;

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

        public WelcomeViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            var user = _userService.GetUserModel();

            FavoritePetImageSource = user.FavoritePetImageSource;
            WelcomeText = $"Hello world, it is now {DateTime.Now:g}";
        }

        private Task ExecuteOpenRadialMenuCommand()
        {
            return Task.FromResult(true);
        }
    }
}
