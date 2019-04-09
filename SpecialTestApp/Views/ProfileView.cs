using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using SpecialTestApp.Core.ViewModels;

namespace SpecialTestApp.Views
{
    [Activity(Label = "Profile", MainLauncher = false, Theme = "@style/AppTheme")]
    internal class ProfileView : MvxAppCompatActivity<ProfileViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ProfileView);

            // Show back button
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
        }

        
    }
}