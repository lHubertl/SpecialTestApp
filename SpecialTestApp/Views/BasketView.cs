using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using SpecialTestApp.Core.ViewModels;

namespace SpecialTestApp.Views
{
    [Activity(Label = "Basket", MainLauncher = false, Theme = "@style/AppTheme")]
    internal class BasketView : MvxAppCompatActivity<BasketViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BasketView);

            // Show back button
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    ViewModel.BackCommand.Execute(null);
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}