﻿using Android.App;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using SpecialTestApp.Core.ViewModels;
using SpecialTestApp.Helpers;
using SpecialTestApp.Views.Controls;

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

            var roundedImageView = FindViewById<RoundedImageView>(Resource.Id.roundedUserImageView);
            var bitmap = GraphicHelper.GetImageBitmapFromUrl(ViewModel.UserImageUri);
            roundedImageView.SetImageBitmap(bitmap);
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