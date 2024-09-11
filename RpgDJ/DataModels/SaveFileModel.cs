using RpgDJ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RpgDJ.DataModels
{
    internal class SaveFileModel
    {
        public Point LastDropPosition { get; set; } = new Point();
        public List<SoundFileModel> Sounds { get; set; } = [];
    }

    internal class SoundFileModel
    {
        public SoundFileModel() { }

        public SoundFileModel(SoundButtonViewModel viewModel)
        {
            Name = viewModel.SoundName;
            Path = viewModel.Path;
            Margin = viewModel.Margin;
            WidthPoints = viewModel.WidthPoints;
            HeightPoints = viewModel.HeightPoints;
        }

        public SoundButtonViewModel ToViewModel()
        {
            return new SoundButtonViewModel(Path)
            {
                Margin = Margin,
                WidthPoints = WidthPoints,
                HeightPoints = HeightPoints
            };
        }

        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Margin { get; set; } = "0, 0, 0, 0";

        public int WidthPoints { get; set; } = 2;
        public int HeightPoints { get; set; } = 2;
    }
}
