using System;
using Android.App;
using Android.Graphics;
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
    [MvxFragmentPresentation]
    [Register(nameof(RadialMenuView))]
    internal class RadialMenuView : MvxFragment<RadialMenuViewModel>
    {
        public RadialMenuView()
        {

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
            // Take screen shot of fragment background
            var internalBitmap = TakeScreenShot(Activity);

            // Create workbench
            var internalCanvas = new Canvas(internalBitmap);

            //Reduce the bitmap in 8 times, to improve faster blurring
            SetupInternalCanvasMatrix(Activity.Window.DecorView, internalCanvas);

            using (var renderScriptBlur = new RenderScriptBlur(Context))
            {
                // Blur the bitmap
                renderScriptBlur.Blur(internalBitmap, 25);

                // Setup the view background
                view.Background = new BitmapDrawable(Resources, internalBitmap);
            }
        }

        /// <summary>
        /// Reduce the bitmap in 8 times, to improve faster blurring
        /// </summary>
        private void SetupInternalCanvasMatrix(View blurView, Canvas internalCanvas)
        {
            var scaleFactor = 8f;
            
            var frame = new Rect();
            Activity.Window.DecorView.GetWindowVisibleDisplayFrame(frame);

            var scaledLeftPosition = -blurView.Left / scaleFactor;
            var scaledTopPosition = -blurView.Top / scaleFactor;

            var scaledTranslationX = blurView.TranslationX / scaleFactor;
            var scaledTranslationY = blurView.TranslationY / scaleFactor;

            internalCanvas.Translate(scaledLeftPosition - scaledTranslationX, scaledTopPosition - scaledTranslationY);
            var scaleX = blurView.ScaleX / scaleFactor;
            var scaleY = blurView.ScaleY / scaleFactor;
            internalCanvas.Scale(scaleX, scaleY);
        }

        /// <summary>
        /// Take screen shot of activity with removed toolbar
        /// </summary>
        /// <param name="activity">Target</param>
        /// <returns></returns>
        public Bitmap TakeScreenShot(Activity activity)
        {
            var view = activity.Window.DecorView;
            view.DrawingCacheEnabled = true;
            view.BuildDrawingCache();

            var frame = new Rect();
            activity.Window.DecorView.GetWindowVisibleDisplayFrame(frame);

            var cachedBitmap = view.DrawingCache;
            var statusBarHeight = frame.Top;

            var display = activity.WindowManager.DefaultDisplay;
            var size = new Point();
            display.GetSize(size);
            var width = size.X;
            var height = size.Y;

            var bitmap = Bitmap.CreateBitmap(cachedBitmap, 0, statusBarHeight, width, height - statusBarHeight);
            view.DestroyDrawingCache();
            return bitmap;
        }
    }
}