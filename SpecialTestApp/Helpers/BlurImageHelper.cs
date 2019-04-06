using Android.App;
using Android.Graphics;
using Java.Lang;

namespace SpecialTestApp.Helpers
{
    /// <summary>
    /// This is a helper to get background image and blur it
    /// </summary>
    internal static class BlurImageHelper
    {
        /// <summary>
        /// Take screen shot of activity
        /// </summary>
        /// <param name="activity">Target</param>
        /// <returns></returns>
        public static Bitmap TakeScreenShot(Activity activity)
        {
            var view = activity.Window.DecorView;
            view.DrawingCacheEnabled = true;
            view.BuildDrawingCache();

            var cachedBitmap = view.DrawingCache;
            var frame = new Rect();
            activity.Window.DecorView.GetWindowVisibleDisplayFrame(frame);
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

        public static Bitmap FastBlur(Bitmap sentBitmap, int radius)
        {
            var bitmap = sentBitmap.Copy(sentBitmap.GetConfig(), true);
            if (radius < 1)
            {
                return null;
            }

            var w = bitmap.Width;
            var h = bitmap.Height;
            var pix = new int[w * h];
            //Log.e("pix", w + " " + h + " " + pix.length);
            bitmap.GetPixels(pix, 0, w, 0, 0, w, h);
            var wm = w - 1;
            var hm = h - 1;
            var wh = w * h;
            var div = radius + radius + 1;
            var r = new int[wh];
            var g = new int[wh];
            var b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p, yp, yi, yw;
            var vmin = new int[Math.Max(w, h)];
            var divsum = (div + 1) >> 1;
            divsum *= divsum;
            var dv = new int[256 * divsum];
            for (i = 0; i < 256 * divsum; i++)
            {
                dv[i] = i / divsum;
            }
            yw = yi = 0;
            int[][] stack = new int[div][];
            int stackpointer;
            int stackstart;
            int[] sir;
            int rbs;
            int r1 = radius + 1;
            int routsum, goutsum, boutsum;
            int rinsum, ginsum, binsum;
            for (y = 0; y < h; y++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    p = pix[yi + Math.Min(wm, Math.Max(i, 0))];
                    sir = stack[i + radius];

                    if (sir is null)
                    {
                        sir = new int[3];
                        stack[i + radius] = sir;
                    }

                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = p & 0x0000ff;
                    rbs = r1 - Math.Abs(i);
                    rsum += sir[0] * rbs;
                    gsum += sir[1] * rbs;
                    bsum += sir[2] * rbs;
                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }
                }
                stackpointer = radius;
                for (x = 0; x < w; x++)
                {
                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];
                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;
                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];
                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];
                    if (y == 0)
                    {
                        vmin[x] = Math.Min(x + radius + 1, wm);
                    }
                    p = pix[yw + vmin[x]];
                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = (p & 0x0000ff);
                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];
                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;
                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[stackpointer % div];
                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];
                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];
                    yi++;
                }
                yw += w;
            }
            for (x = 0; x < w; x++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = Math.Max(0, yp) + x;
                    sir = stack[i + radius];
                    sir[0] = r[yi];
                    sir[1] = g[yi];
                    sir[2] = b[yi];
                    rbs = r1 - Math.Abs(i);
                    rsum += r[yi] * rbs;
                    gsum += g[yi] * rbs;
                    bsum += b[yi] * rbs;
                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }
                    if (i < hm)
                    {
                        yp += w;
                    }
                }
                yi = x;
                stackpointer = radius;
                for (y = 0; y < h; y++)
                {
                    // Preserve alpha channel: ( 0xff000000 & pix[yi] )  
                    pix[yi] = (int) ((0xff000000 & pix[yi]) | (dv[rsum] << 16) | (dv[gsum] << 8) | dv[bsum]);
                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;
                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];
                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];
                    if (x == 0)
                    {
                        vmin[y] = Math.Min(y + r1, hm) * w;
                    }
                    p = x + vmin[y];
                    sir[0] = r[p];
                    sir[1] = g[p];
                    sir[2] = b[p];
                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];
                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;
                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[stackpointer];
                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];
                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];
                    yi += w;
                }
            }
            //Log.e("pix", w + " " + h + " " + pix.length);
            bitmap.SetPixels(pix, 0, w, 0, 0, w, h);
            return bitmap;
        }
    }
}