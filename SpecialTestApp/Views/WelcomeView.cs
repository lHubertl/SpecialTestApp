using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using SpecialTestApp.Core.ViewModels;
using SpecialTestApp.Helpers;

namespace SpecialTestApp.Views
{
    [Activity(Label = "Welcome", MainLauncher = true)]
    internal class WelcomeView : MvxActivity<WelcomeViewModel>
    {
        private ImageView _favoritePetImageView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.WelcomeView);

            _favoritePetImageView = FindViewById<ImageView>(Resource.Id.favoritePetImageView);

            InitializeImageView();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _favoritePetImageView.Click += FavoritePetImageViewOnClick;
        }

        protected override void OnStop()
        {
            base.OnStop();

            _favoritePetImageView.Click -= FavoritePetImageViewOnClick;
        }

        private void InitializeImageView()
        {
            var bitmap = GraphicHelper.GetImageBitmapFromUrl(ViewModel.FavoritePetImageSource);
            _favoritePetImageView.SetImageBitmap(bitmap);
        }

        private void FavoritePetImageViewOnClick(object sender, EventArgs e)
        {
            MoveTaskToBack(true);
        }
    }
}