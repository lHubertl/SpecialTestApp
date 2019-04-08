using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Java.Lang;


namespace SpecialTestApp.Views
{
    public sealed class FloatingActionButton : FrameLayout
    {
        private readonly View _viewToAttach;
        private readonly bool _systemOverlay;

        /// <summary>
        /// Constructor that takes parameters collected using FloatingActionMenu.Builder
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewToAttach">Container that will store all content</param>
        /// <param name="layoutParams"></param>
        /// <param name="theme"></param>
        /// <param name="backgroundDrawable"></param>
        /// <param name="position"></param>
        /// <param name="contentView"></param>
        /// <param name="contentParams"></param>
        /// <param name="systemOverlay"></param>
        public FloatingActionButton(
            Context context, 
            View viewToAttach, 
            ViewGroup.LayoutParams layoutParams,
            Theme theme,
            Drawable backgroundDrawable,
            GravityFlags position,
            View contentView,
            LayoutParams contentParams,
            bool systemOverlay) : base(context)
        {
            _systemOverlay = systemOverlay;
            _viewToAttach = viewToAttach;
            
            SetPosition(position, layoutParams);

            // If no custom backgroundDrawable is specified, use the background drawable of the theme.
            if (backgroundDrawable == null)
            {
                backgroundDrawable = context.Resources.GetDrawable(theme == Theme.Light
                    ? Resource.Drawable.button_action_selector 
                    : Resource.Drawable.button_action_dark_selector, null);
            }

            SetBackgroundResource(backgroundDrawable);
            if (contentView != null)
            {
                SetContentView(contentView, contentParams);
            }

            Clickable = true;

            Attach(layoutParams);
        }

        /// <summary>
        ///  Sets the position of the button by calculating its Gravity from the position parameter
        /// </summary>
        /// <param name="position">one of 8 specified positions</param>
        /// <param name="layoutParams">should be either FrameLayout.LayoutParams or WindowManagerLayoutParams</param>
        public void SetPosition(GravityFlags position, ViewGroup.LayoutParams layoutParams)
        {
            if (!_systemOverlay)
            {
                try
                {
                    LayoutParams lp = (LayoutParams) layoutParams;
                    lp.Gravity = position;
                    LayoutParameters = lp;
                }
                catch (ClassCastException)
                {
                    throw new ClassCastException("layoutParams must be an instance of " +
                                                 "FrameLayout.LayoutParams, since this FAB is not a systemOverlay");
                }
            }
            else
            {
                try
                {
                    var lp = (WindowManagerLayoutParams) layoutParams;
                    lp.Gravity = position;
                   
                    LayoutParameters = lp;
                }
                catch (ClassCastException)
                {
                    throw new ClassCastException("layoutParams must be an instance of " +
                                                 "WindowManagerLayoutParams, since this FAB is a systemOverlay");
                }
            }
        }

        /// <summary>
        /// Sets a content view that will be displayed inside this FloatingActionButton.
        /// </summary>
        /// <param name="contentView"></param>
        /// <param name="contentParams"></param>
        public void SetContentView(View contentView, LayoutParams contentParams)
        {
            LayoutParams parameters;
            if (contentParams == null)
            {
                parameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent, GravityFlags.Center);
                var margin = Resources.GetDimensionPixelSize(Resource.Dimension.action_button_content_margin);
                parameters.SetMargins(margin, margin, margin, margin);
            }
            else
            {
                parameters = contentParams;
            }

            parameters.Gravity = GravityFlags.Center;

            contentView.Clickable = false;
            AddView(contentView, parameters);
        }

        /// <summary>
        ///  Attaches it to the content view with specified LayoutParams.
        /// </summary>
        /// <param name="layoutParams"></param>
        public void Attach(ViewGroup.LayoutParams layoutParams)
        {
            if (_systemOverlay)
            {
                try
                {
                    GetWindowManager().AddView(this, layoutParams);
                }
                catch (SecurityException)
                {
                    throw new SecurityException("Your application must have SYSTEM_ALERT_WINDOW " +
                                                "permission to create a system window.");
                }
            }
            else
            {
                ((ViewGroup)_viewToAttach).AddView(this, layoutParams);
            }
        }

        /// <summary>
        /// Detaches it from the container view.
        /// </summary>
        public void Detach()
        {
            if (_systemOverlay)
            {
                GetWindowManager().RemoveView(this);
            }
            else
            {
                ((ViewGroup) _viewToAttach).RemoveView(this);
            }
        }
        
        public IWindowManager GetWindowManager()
        {
            return (IWindowManager) Context.GetSystemService(Context.WindowService);
        }

        private void SetBackgroundResource(Drawable drawable)
        {
            Background = drawable;

        }

        /// <summary>
        /// A builder for FloatingActionButton in conventional Java Builder format
        /// </summary>
        public class Builder
        {
            private readonly Context _context;
            private ViewGroup.LayoutParams _layoutParams;
            private Theme _theme;
            private Drawable _backgroundDrawable;
            private GravityFlags _position;
            private View _contentView;
            private LayoutParams _contentParams;
            private bool _systemOverlay;

            public Builder(Context context)
            {
                _context = context;

                // Default FloatingActionButton settings
                var size = context.Resources.GetDimensionPixelSize(Resource.Dimension.action_button_size);
                var margin = context.Resources.GetDimensionPixelSize(Resource.Dimension.action_button_margin);
                LayoutParams layoutParams =
                    new LayoutParams(size, size, GravityFlags.Bottom | GravityFlags.Right);
                layoutParams.SetMargins(margin, margin, margin, margin);
                SetLayoutParams(layoutParams);
                SetTheme(Theme.Light);
                SetPosition(GravityFlags.Right | GravityFlags.Bottom);
                SetSystemOverlay(false);
            }

            public Builder SetLayoutParams(ViewGroup.LayoutParams parameters)
            {
                _layoutParams = parameters;
                return this;
            }

            public Builder SetTheme(Theme theme)
            {
                _theme = theme;
                return this;
            }

            public Builder SetBackgroundDrawable(Drawable backgroundDrawable)
            {
                _backgroundDrawable = backgroundDrawable;
                return this;
            }

            public Builder SetBackgroundDrawable(int drawableId)
            {
                return SetBackgroundDrawable(_context.Resources.GetDrawable(drawableId, null));
            }

            public Builder SetPosition(GravityFlags position)
            {
                _position = position;
                return this;
            }

            public Builder SetContentView(View contentView)
            {
                return SetContentView(contentView, null);
            }

            public Builder SetContentView(View contentView, LayoutParams contentParams)
            {
                _contentView = contentView;
                _contentParams = contentParams;
                return this;
            }

            public Builder SetSystemOverlay(bool systemOverlay)
            {
                _systemOverlay = systemOverlay;
                return this;
            }

            public FloatingActionButton Build(View view)
            {
                return new FloatingActionButton(_context,
                    view,
                    _layoutParams,
                    _theme,
                    _backgroundDrawable,
                    _position,
                    _contentView,
                    _contentParams,
                    _systemOverlay);
            }
        }

        public enum Theme
        {
            Light,
            Dark
        }
    }
}