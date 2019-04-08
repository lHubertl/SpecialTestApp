using System;
using Android.Content;
using Android.Graphics;
using Android.Renderscripts;

namespace SpecialTestApp.Helpers
{
    internal class RenderScriptBlur : IDisposable
    {
        private readonly RenderScript _renderScript;
        private readonly ScriptIntrinsicBlur _blurScript;

        private int _lastBitmapWidth = -1;
        private int _lastBitmapHeight = -1;

        private Allocation _outAllocation;

        public RenderScriptBlur(Context context)
        {
            _renderScript = RenderScript.Create(context);
            _blurScript = ScriptIntrinsicBlur.Create(_renderScript, Element.U8_4(_renderScript));
        }

        /// <summary>
        /// Blur bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap to be blurred</param>
        /// <param name="blurRadius">from 1 and up to 25</param>
        /// <returns></returns>
        public Bitmap Blur(Bitmap bitmap, float blurRadius)
        {
            if (blurRadius < 1f && blurRadius > 25f)
            {
                throw new ArgumentOutOfRangeException(nameof(blurRadius), "Blur radius must be in range 1..25");
            }

            //Allocation will use the same backing array of pixels as bitmap if created with USAGE_SHARED flag
            var inAllocation = Allocation.CreateFromBitmap(_renderScript, bitmap);

            if (!CanReuseAllocation(bitmap))
            {
                _outAllocation?.Destroy();

                _outAllocation = Allocation.CreateTyped(_renderScript, inAllocation.Type);
                _lastBitmapWidth = bitmap.Width;
                _lastBitmapHeight = bitmap.Height;
            }

            _blurScript.SetRadius(blurRadius);
            _blurScript.SetInput(inAllocation);

            //do not use inAllocation in forEach. it will cause visual artifacts on blurred Bitmap
            _blurScript.ForEach(_outAllocation);
            _outAllocation.CopyTo(bitmap);

            inAllocation.Destroy();
            return bitmap;
        }

        private bool CanReuseAllocation(Bitmap bitmap)
        {
            return bitmap.Height == _lastBitmapHeight && bitmap.Width == _lastBitmapWidth;
        }

        public void Dispose()
        {
            _blurScript.Destroy();
            _renderScript.Destroy();
            _outAllocation?.Destroy();
        }
    }
}