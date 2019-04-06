using System;
using System.Net;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using SpecialTestApp.Core.ViewModels;

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
            var bitmap = GetImageBitmapFromUrl(ViewModel.FavoritePetImageSource);
            _favoritePetImageView.SetImageBitmap(bitmap);
        }

        private void FavoritePetImageViewOnClick(object sender, EventArgs e)
        {
            MoveTaskToBack(true);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

    }
}