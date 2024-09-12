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
            ImagePath = viewModel.ImagePath;
            AnimatedImagePath = viewModel.AnimatedImagePath;
            IsLooping = viewModel.IsLooping;
            Volume = (float)viewModel.Volume;
        }

        public SoundButtonViewModel ToViewModel()
        {
            return new SoundButtonViewModel(Path)
            {
                Margin = Margin,
                WidthPoints = WidthPoints,
                HeightPoints = HeightPoints,
                ImagePath = ImagePath,
                AnimatedImagePath = AnimatedImagePath,
                SoundName = Name,
                IsLooping = IsLooping,
                Volume = Volume
            };
        }

        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Margin { get; set; } = "0, 0, 0, 0";

        public string ImagePath { get; set;} = string.Empty;
        public string AnimatedImagePath { get; set;} = string.Empty;

        public bool IsLooping { get; set; } = false;

        public int WidthPoints { get; set; } = 2;
        public int HeightPoints { get; set; } = 2;

        public float Volume { get; set; } = 1;
    }
}
