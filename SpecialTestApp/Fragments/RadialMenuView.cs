using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using SpecialTestApp.Core.ViewModels;

namespace SpecialTestApp.Fragments
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(RadialMenuView))]
    internal class RadialMenuView : MvxDialogFragment<RadialMenuViewModel>
    {
        public RadialMenuView()
        {
            
        }

        protected RadialMenuView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.RadialMenuView, null);

            return view;
        }
    }
}