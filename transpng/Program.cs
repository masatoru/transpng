using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace transpng
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                if (args.Length == 0)
                {
                    var appName = System.IO.Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
                    Console.WriteLine($"{appName}.exe: PNGファイルの透過処理");
                    Console.WriteLine($"- 使い方: transpng.exe [入力ディレクトリ] [出力ディレクトリ]");
                    Console.WriteLine($"- 拡張子が.pngが処理の対象になります");
                    Console.WriteLine($"- [出力ディレクトリ]を指定しない場合は[入力ディレクトリ]に上書きします");
                    Console.WriteLine($"- 指定した[出力ディレクトリ]がない場合は作成されます");
                    break;
                }
                if (args.Length > 2)
                {
                    Console.WriteLine($"引数が多すぎます");
                    break;
                }
                // 透過処理を実行する
                Run(args[0], args.Length == 1 ? args[0] : args[1]);
            } while (false);

            Console.Write("終了しました(Push Any Key)");
            Console.ReadKey();
        }
        static void Run(string srcdir, string outdir)
        {
            if (!System.IO.Directory.Exists(srcdir))
                throw new Exception($"入力ディレクトリがありません DIR={srcdir}");

            var pnglst = System.IO.Directory.GetFiles(srcdir, "*.png");
            if(pnglst.Length==0)
                throw new Exception($"入力ディレクトリにPNGファイルがありません DIR={srcdir}");

            //ディレクトリが異なればコピーする
            bool isNeedCopy = srcdir != outdir;

            //出力ディレクトリをなければ作成
            if (!System.IO.Directory.Exists(outdir))
                System.IO.Directory.CreateDirectory(outdir);

            foreach (var srcpath in pnglst.Select((v, i) => new { v, i }))
            {
                var outpath = System.IO.Path.Combine(outdir, System.IO.Path.GetFileName(srcpath.v));
                Console.WriteLine($"==>{srcpath.i + 1}:{pnglst.Length}/{outpath}");
                if (isNeedCopy)
                {
                    System.IO.File.Copy(srcpath.v, outpath, true);
                }
                TransparentImage(outpath);
            }
        }

        // 透過処理をする
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
