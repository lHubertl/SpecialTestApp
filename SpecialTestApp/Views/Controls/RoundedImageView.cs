using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace SpecialTestApp.Views.Controls
{
    internal class RoundedImageView : ImageView
    {
        Bitmap _maskBitmap;
        Paint _maskPaint;

        public RoundedImageView(Context context) : base(context) { Init(); }
        public RoundedImageView(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); }
        public RoundedImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { Init(); }
        protected RoundedImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Init(); }

        public override void Draw(Canvas canvas)
        {
            var offscreenBitmap = Bitmap.CreateBitmap(canvas.Width, canvas.Height, Bitmap.Config.Argb8888, true);
            var offscreenCanvas = new Canvas(offscreenBitmap);

            base.Draw(offscreenCanvas);

            if (_maskBitmap == null)
            {
                _maskBitmap = CreateMask(canvas.Width, canvas.Height);
            }

            var paint = new Paint(PaintFlags.AntiAlias)
            {
                Color = Color.Red
            };

            offscreenCanvas.DrawBitmap(_maskBitmap, 0f, 0f, _maskPaint);
            canvas.DrawBitmap(offscreenBitmap, 0f, 0f, paint);
        }

        private void Init()
        {
            SetScaleType(ScaleType.CenterCrop);
            _maskPaint = new Paint(PaintFlags.AntiAlias | PaintFlags.FilterBitmap);
            _maskPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
        }

        public static Bitmap TintImage(Bitmap bitmap, Color color)
        {
            var paint = new Paint();
            paint.SetColorFilter(new PorterDuffColorFilter(color, PorterDuff.Mode.SrcIn));
            var bitmapResult = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);
            var canvas = new Canvas(bitmapResult);
            canvas.DrawBitmap(bitmap, 0, 0, paint);
            return bitmapResult;
        }

        private Bitmap CreateMask(int width, int height)
        {
            var mask = Bitmap.CreateBitmap(width, height, Bitmap.Config.Alpha8);
            var canvas = new Canvas(mask);

            var paint = new Paint(PaintFlags.AntiAlias) {Color = Color.White};

            canvas.DrawRect(0, 0, width, height, paint);

            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
            canvas.DrawRoundRect(new RectF(0, 0, width, height), rx: width / 2, width / 2, paint);

            return mask;
        }
    }
}