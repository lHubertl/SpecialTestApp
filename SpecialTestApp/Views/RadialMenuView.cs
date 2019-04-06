using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using SpecialTestApp.Core.ViewModels;
using SpecialTestApp.Helpers;

namespace SpecialTestApp.Views
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(RadialMenuView))]
    internal sealed class RadialMenuView : MvxDialogFragment<RadialMenuViewModel>
    {
        public RadialMenuView()
        {
            SetStyle(DialogFragmentStyle.NoFrame, Resource.Style.RadialFragmentDialogStyle);
        }

        private RadialMenuView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            SetStyle(DialogFragmentStyle.NoFrame, Resource.Style.RadialFragmentDialogStyle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.RadialMenuView, null, true);
            SetBlurBackground(view);
            
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            View.Click += ViewOnClick;
        }

        public override void OnStop()
        {
            base.OnStop();
            View.Click -= ViewOnClick;
        }

        private void ViewOnClick(object sender, EventArgs e)
        {
            ViewModel.BackCommand?.Execute(null);
        }

        private void SetBlurBackground(View view)
        {
            var screenShot = BlurImageHelper.TakeScreenShot(Activity);
            var blur = BlurImageHelper.FastBlur(screenShot, 10);
            var drawable = new BitmapDrawable(Resources, blur);
            view.Background = drawable;
        }
    }
}