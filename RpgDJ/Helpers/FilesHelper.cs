using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgDJ.Helpers
{
    internal class FilesHelper
    {
        public static bool IsImageFile(string path)
        {
            return 
                path.EndsWith(".jpg") ||
                path.EndsWith(".jpeg") ||
                path.EndsWith(".bmp") ||
                path.EndsWith(".gif") ||
                path.EndsWith(".png") ||
                path.EndsWith(".webp");
        }

        public static bool IsAnimatedImageFile(string path)
        {
            return
                path.EndsWith(".gif");
        }
    }
}
