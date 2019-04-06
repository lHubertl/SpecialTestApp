using System;
using SpecialTestApp.Core.Models;

namespace SpecialTestApp.Core.Services
{
    public class UserService : IUserService
    {
        public UserModel GetUserModel()
        {
            return GetMockedUserModel();
        }

        private UserModel GetMockedUserModel()
        {
            return new UserModel
            {
                FirstName = "Steven",
                LastName = "Spielberg",
                DateOfBirth = new DateTime(1946, 12, 18),
                FavoritePetImageSource =
                    @"https://s3.amazonaws.com/cdn-origin-etr.akc.org/wp-content/uploads/2017/11/16105011/English-Cocker-Spaniel-Slide03.jpg"
            };
        }
    }
}
