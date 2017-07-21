using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transpng
{
    class Program
    {
        static void Main(string[] args)
        {



        }
        //透過で上書きする
        static void TransparentImage(string imgpath)
        {
            using (ImageMagick.MagickImage img = new ImageMagick.MagickImage(imgpath))
            {
                img.Format = ImageMagick.MagickFormat.Png8;
                img.Write(imgpath);
            }
            using (ImageMagick.MagickImage img = new ImageMagick.MagickImage(imgpath))
            {
                img.Format = ImageMagick.MagickFormat.Png8;
                img.Transparent(new ImageMagick.MagickColor(255, 255, 255));    //White
                img.ColorFuzz = new ImageMagick.Percentage(10.0);
                img.Write(imgpath);
            }
        }

    }
}
