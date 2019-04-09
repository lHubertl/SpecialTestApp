using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Oguzdev.Circularfloatingactionmenu.Library;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using SpecialTestApp.Core.ViewModels;
using SpecialTestApp.Helpers;
using SpecialTestApp.Views.Controls;
using Xamarin.Essentials;
using FloatingActionButton = Com.Oguzdev.Circularfloatingactionmenu.Library.FloatingActionButton;

namespace SpecialTestApp.Views
{
    [MvxFragmentPresentation]
    [Register(nameof(RadialMenuView))]
    internal class RadialMenuView : MvxFragment<RadialMenuViewModel>
    {
        private FloatingActionMenu _menu;
        private View _userImageButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.RadialMenuView, null, true);

            SetBlurBackground(view);
            InitializeMenu((ViewGroup) view, InitializeImageView(ViewModel.UserImageSource));

            _userImageButton.Click += UserImageButtonOnClick;

            return view;
        }

        public override void OnStop()
        {
            base.OnStop();

            _userImageButton.Click -= UserImageButtonOnClick;
        }

        private void UserImageButtonOnClick(object sender, EventArgs e)
        {
            ViewModel.ToProfileCommand?.Execute(null);
        }

        public override async void OnStart()
        {
            base.OnStart();

            await SetIsOpen(true);
        }

        /// <summary>
        /// This is a workaround for the library
        /// The reason is that this library was developed only for activities, not for fragments
        /// </summary>
        /// <param name="state"></param>
        private async Task SetIsOpen(bool state)
        {
            await Task.Delay(100);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (state) _menu.Open(true);
                else _menu.Close(true);
            });
        }

        private ImageView InitializeImageView(string imageUri)
        {
            var imageView = new RoundedImageView(Context);

            var bitmap = GraphicHelper.GetImageBitmapFromUrl(imageUri);
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

            var compatActivity = activity as AppCompatActivity; 
            if (compatActivity?.SupportActionBar.IsShowing == true)
            {
                frame.Top += compatActivity.SupportActionBar.Height;
            }

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

        private void InitializeMenu(ViewGroup view, ImageView userImageView)
        {
            var d = (int) Resources.DisplayMetrics.Density;

            // Set up the large red button on the center right side
            // With custom button and content sizes and margins
            var imageButtonSize = 150 * d;
            var imageButtonMargin = 0;
            var imageButtonContentSize = imageButtonSize;
            var imageButtonContentMargin = 0;
            var imageButtonMenuRadius = 110 * d; // Distance between sub buttons and the button
            var subActionButtonSize = 55 * d;
            var subActionButtonContentMargin = 15 * d;

            //Those properties define how to stretch out sub buttons
            // For example if it 0 and 360 then all buttons will be surround the menu
            // If you want it to be at the top, choose 90 and 270 
            const int startAngle = 360;
            const int endAngle = 180;

            var context = view.Context;

            var starParams = new FrameLayout.LayoutParams(imageButtonSize, imageButtonSize);
            starParams.SetMargins(imageButtonMargin,
                imageButtonMargin,
                imageButtonMargin,
                imageButtonMargin);

            userImageView.LayoutParameters = starParams;

            var userImageParams = new FrameLayout.LayoutParams(imageButtonContentSize, imageButtonContentSize);
            userImageParams.SetMargins(imageButtonContentMargin,
                imageButtonContentMargin,
                imageButtonContentMargin,
                imageButtonContentMargin);

            // Use SetBackgroundDrawable to set background drawable for centered button
            _userImageButton = new FloatingActionButton.Builder(context)
                .SetContentContainer(view)
                .SetContentView(userImageView, userImageParams)
                .SetPosition((int) GravityFlags.Center)
                .SetLayoutParams(starParams)
                .Build();

            var lCSubBuilder = new SubActionButton.Builder(context);
            lCSubBuilder.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.sub_button_selector, null));

            var blueContentParams = new FrameLayout.LayoutParams(width: ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);
            blueContentParams.SetMargins(subActionButtonContentMargin,
                subActionButtonContentMargin,
                subActionButtonContentMargin,
                subActionButtonContentMargin);
            lCSubBuilder.SetLayoutParams(blueContentParams);

            // Set custom layout params
            var blueParams = new FrameLayout.LayoutParams(subActionButtonSize, subActionButtonSize);
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
            _menu = new FloatingActionMenu.Builder(context)
                .SetContentContainer(view)
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon1, blueContentParams).Build())
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon2, blueContentParams).Build())
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon3, blueContentParams).Build())
                .AddSubActionView(lCSubBuilder.SetContentView(lcIcon4, blueContentParams).Build())
                .SetRadius(imageButtonMenuRadius)
                .SetStartAngle(startAngle)
                .SetEndAngle(endAngle)
                .AttachTo(_userImageButton)
                .Build();
        }
    }
}