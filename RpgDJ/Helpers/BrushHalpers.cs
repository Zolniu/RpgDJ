using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RpgDJ.Helpers
{
    internal static class BrushHalpers
    {
        private static Dictionary<int, Brush> _mappings = new()
        {
            {0, Brushes.Transparent },
            {1, Brushes.Beige },
            {2, Brushes.Red },
            {3, Brushes.Green },
            {4, Brushes.Blue },
            {5, Brushes.Olive },
            {6, Brushes.Plum },
            {7, Brushes.Orange },
            {8, Brushes.Violet },
            {9, Brushes.Yellow },
            {10, Brushes.RosyBrown },
            {11, Brushes.MediumVioletRed },
            {12, Brushes.LightGray },
            {13, Brushes.Gold },
            {14, Brushes.Fuchsia},
            {15, Brushes.Khaki },
            {16, Brushes.DeepPink },
            {17, Brushes.Turquoise },
            {18, Brushes.Orchid },
            {19, Brushes.Sienna },
            {20, Brushes.WhiteSmoke }
        };

        public static ReadOnlyDictionary<int, Brush> BrushMappings = new(_mappings);

    }
}
