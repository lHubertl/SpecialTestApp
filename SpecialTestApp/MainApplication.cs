using System;
using Android.App;
using Android.Runtime;
using SpecialTestApp.Core;

namespace SpecialTestApp
{
    [Application(Theme = "@style/AppTheme", SupportsRtl = true, AllowBackup = true)]
    internal class MainApplication : MvvmCross.Platforms.Android.Views.MvxAndroidApplication<Setup, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }
    }
}