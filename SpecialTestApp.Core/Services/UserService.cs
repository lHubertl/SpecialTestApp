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
                ImageSource = @"https://i2.wp.com/talkiesnetwork.com/wp-content/uploads/2019/03/steven-spielberg.jpg?fit=1000%2C562&ssl=1",
                FavoritePetImageSource =
                    @"https://s3.amazonaws.com/cdn-origin-etr.akc.org/wp-content/uploads/2017/11/16105011/English-Cocker-Spaniel-Slide03.jpg"
            };
        }
    }
}
