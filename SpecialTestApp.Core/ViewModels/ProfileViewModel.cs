using System.Threading.Tasks;
using MvvmCross.Navigation;
using SpecialTestApp.Core.Services;

namespace SpecialTestApp.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        private string _username;
        public string UserName
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _userImageUri;
        public string UserImageUri
        {
            get => _userImageUri;
            set => SetProperty(ref _userImageUri, value);
        }

        private string _dateOfBirth;
        public string DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }

        public ProfileViewModel(IMvxNavigationService navigationService, IUserService userService)
            : base(navigationService)
        {
            _userService = userService;
        }

        public override Task Initialize()
        {
            var userModel = _userService.GetUserModel();

            UserName = $"{userModel.FirstName} {userModel.LastName}";
            UserImageUri = userModel.ImageSource;
            DateOfBirth = userModel.DateOfBirth.ToString("yyyy MMMM dd");

            return base.Initialize();
        }

        public override void Prepare(object parameter)
        {
            
        }
    }
}
