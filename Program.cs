using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ImageMagickObject;

namespace Foobalator.SizeAndSync
{
    public static class MainThread
    {
        private static readonly MagickImageClass Magick = new MagickImageClass();

        public static void Main(string[] args)
        {
            Sync(args[0], args[1]);
        }

        private static void Sync(string dir, string dest)
        {
            Directory.CreateDirectory(dest);

            foreach (string file in Directory.GetFiles(dir, "*.jpg"))
            {
                if (file.Contains(".small."))
                    continue;

                if (file.Contains(".thumb."))
                    continue;

                string name = Path.GetFileName(file);
                string subdest = Path.Combine(dest, name);

                if (File.Exists(subdest))
                    continue;

                string dimensions = string.Format("{0}x{1}", 800, 600);
                try
                {
                    Convert("-size", dimensions, file, "-resize", dimensions, "+profile", "*", subdest);
                    Console.WriteLine(subdest);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} failed: {1}", subdest, e.Message);
                }
            }

            foreach (string subdir in Directory.GetDirectories(dir))
            {
                string name = Path.GetFileName(subdir);
                string subdest = Path.Combine(dest, name);

                Sync(subdir, subdest);
            }
        }

        public static object Convert(params object[] parms)
        {
            return Magick.Convert(ref parms);
        }
    }
}
