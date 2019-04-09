using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Views;
using SpecialTestApp.Core.ViewModels;

namespace SpecialTestApp.Views
{
    [Activity(Label = "Profile")]
    internal class ProfileView : MvxActivity<ProfileViewModel>
    {
        public ProfileView()
        {
            
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ProfileView);
        }
    }
}