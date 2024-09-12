using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RpgDJ.Helpers
{
    internal static class MathHelper
    {
        public static Point SnapToGrid(this Point input)
        {
            return new Point((input.X / Parameters.GridSize) * Parameters.GridSize, (input.Y / Parameters.GridSize) * Parameters.GridSize);
            ;
        }

        public static int SnapToGrid(this int input) 
        {
            return (int)(input / Parameters.GridSize) * Parameters.GridSize;
        }

        public static int SnapToGrid(this float input)
        {
            return (int)(input / Parameters.GridSize) * Parameters.GridSize;
        }

        public static int SnapToGrid(this double input)
        {
            return (int)(input / Parameters.GridSize) * Parameters.GridSize;
        }
    }
}
