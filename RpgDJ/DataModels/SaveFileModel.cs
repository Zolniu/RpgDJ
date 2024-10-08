﻿using RpgDJ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RpgDJ.DataModels
{
    public class SaveFileModel 
    {
        public List<SessionEntry> Sessions { get; set; }
    }

    public class SessionEntry
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    internal class SessionSaveFileModel
    {
        public string SessionName { get; set; }
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
            ColorNumber = viewModel.ColorNumber;
            ImagePath = viewModel.ImagePath is null ? viewModel.ImagePath : viewModel.ImagePath.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty);
            AnimatedImagePath = viewModel.AnimatedImagePath is null ? viewModel.AnimatedImagePath : viewModel.AnimatedImagePath.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty);
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
                ColorNumber = ColorNumber,
                ImagePath = (ImagePath is null || ImagePath == SoundButtonViewModel.DefaultImage ||  ImagePath.Contains(':')) ? ImagePath : $"{AppDomain.CurrentDomain.BaseDirectory}{ImagePath}",
                AnimatedImagePath = (AnimatedImagePath is null || AnimatedImagePath.Contains(':')) ? AnimatedImagePath : $"{AppDomain.CurrentDomain.BaseDirectory}{AnimatedImagePath}",
                SoundName = Name,
                IsLooping = IsLooping,
                Volume = Volume
            };
        }

        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Margin { get; set; } = "0, 0, 0, 0";

        public int ColorNumber { get; set; }
        public string? ImagePath { get; set;} = string.Empty;
        public string? AnimatedImagePath { get; set;} = string.Empty;

        public bool IsLooping { get; set; } = false;

        public int WidthPoints { get; set; } = 2;
        public int HeightPoints { get; set; } = 2;

        public float Volume { get; set; } = 1;
    }
}
