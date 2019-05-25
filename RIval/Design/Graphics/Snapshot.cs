using Ignite.Core;
using Ignite.Design.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ignite.Design.Graphics
{
    class Snapshot : Singleton<Snapshot>
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public RenderTargetBitmap TakeSnapshot(FrameworkElement ctl)
        {
            var point = ctl.TransformToAncestor(ctl).Transform(new System.Windows.Point(0, 0));

            RenderTargetBitmap render = new RenderTargetBitmap((int)ctl.Width, (int)ctl.Height, point.X, point.Y, PixelFormats.Pbgra32);
            render.Render(ctl);

            return render;
        }

        public BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
    }
}
