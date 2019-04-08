using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Com.Oguzdev.Circularfloatingactionmenu.Library;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using SpecialTestApp.Core.ViewModels;
using SpecialTestApp.Helpers;
using SpecialTestApp.Views.Controls;
using FloatingActionButton = Com.Oguzdev.Circularfloatingactionmenu.Library.FloatingActionButton;

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
            var imageView = InitializeImageView(view);

            var frameLayout = view.FindViewById<FrameLayout>(Resource.Id.radialMenuFrameLayout);

            InitializeMenu(frameLayout);

            return view;
        }

        private ImageView InitializeImageView(View view)
        {
            var imageView = view.FindViewById<RoundedImageView>(Resource.Id.roundedImageView);

            var bitmap = GraphicHelper.GetImageBitmapFromUrl(ViewModel.UserImageSource);
            imageView.SetImageBitmap(bitmap);

            return imageView;
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

        private void InitializeMenu(ViewGroup view)
        {
            // Set up the large red button on the center right side
            // With custom button and content sizes and margins
            int redActionButtonSize = 108;
            int redActionButtonMargin = 12;
            int redActionButtonContentSize = 16;
            int redActionButtonContentMargin = 36;
            int redActionMenuRadius = 96;
            int blueSubActionButtonSize = 48;
            int blueSubActionButtonContentMargin = 16;

            var context = view.Context;

            ImageView fabIconStar = new ImageView(context);


            var starParams = new FrameLayout.LayoutParams(redActionButtonSize, redActionButtonSize);
            starParams.SetMargins(redActionButtonMargin,
                redActionButtonMargin,
                redActionButtonMargin,
                redActionButtonMargin);

            fabIconStar.LayoutParameters = starParams;
            fabIconStar.Resources.GetDrawable(Resource.Drawable.button_action_blue_selector, null);







            var fabIconStarParams = new FrameLayout.LayoutParams(redActionButtonContentSize, redActionButtonContentSize);
            fabIconStarParams.SetMargins(redActionButtonContentMargin,
                redActionButtonContentMargin,
                redActionButtonContentMargin,
                redActionButtonContentMargin);

            var leftCenterButton = new FloatingActionButton.Builder(context)
                .SetContentContainer(view)
                .SetContentView(fabIconStar, fabIconStarParams)
                .SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.button_action_blue_selector, null))
                .SetPosition((int)GravityFlags.Center)
                .SetLayoutParams(starParams)
                .Build();

            var lCSubBuilder = new SubActionButton.Builder(context);
            lCSubBuilder.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.button_action_blue_selector, null));



            FrameLayout.LayoutParams blueContentParams = new FrameLayout.LayoutParams(width: ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            blueContentParams.SetMargins(blueSubActionButtonContentMargin,
                blueSubActionButtonContentMargin,
                blueSubActionButtonContentMargin,
                blueSubActionButtonContentMargin);
            lCSubBuilder.SetLayoutParams(blueContentParams);


            // Set custom layout params
            FrameLayout.LayoutParams blueParams = new FrameLayout.LayoutParams(blueSubActionButtonSize, blueSubActionButtonSize);
            lCSubBuilder.SetLayoutParams(blueParams);




            var lcIcon1 = new ImageView(context);
            var lcIcon2 = new ImageView(context);
            var lcIcon3 = new ImageView(context);
            var lcIcon4 = new ImageView(context);

            lcIcon1.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.outline_favorite_border_white_18, null));
            lcIcon2.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.outline_notifications_white_18, null));
            lcIcon3.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.outline_settings_white_18, null));
            lcIcon4.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.outline_shopping_basket_white_18, null));

            // Build another menu with custom options
            var leftCenterMenu = new FloatingActionMenu.Builder(context)
                .SetContentContainer(view)
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon1, blueContentParams).Build())
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon2, blueContentParams).Build())
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon3, blueContentParams).Build())
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon4, blueContentParams).Build())
                .SetRadius(redActionMenuRadius)
                .SetStartAngle(70)
                .SetEndAngle(-70)
                .AttachTo(leftCenterButton)
                .Build();
        }

    }
}