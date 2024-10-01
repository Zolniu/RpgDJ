using CommunityToolkit.Mvvm.Input;
using RpgDJ.Controls;
using RpgDJ.DataModels;
using RpgDJ.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RpgDJ.ViewModels
{
    internal class SoundButtonViewModel : ViewModelBase
    {
        SoundButtonViewModel()
        {
            SoundName = "No sound";
            ImagePath = _defaultImage;
        }

        public SoundButtonViewModel(string path)
        {
            HandleSoundPath(path);

            WidthPoints = 3;
            HeightPoints = 3;

            ImagePath = _defaultImage;

            ColorNumber = (int)Random.Shared.NextInt64(20);
           
            IsLooping = false;

            ImageVisibility = Visibility.Visible;
            PlayIconVisibility = Visibility.Collapsed;
            AdditionalButtonsVisibility = Visibility.Collapsed;

            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;

            ClickCommand = new RelayCommand(() =>
            {
                if (!_isPlaying)
                {
                    Play();
                }
                else
                {
                    Stop();
                }
            });
        }

        private void HandleSoundPath(string path)
        {
            path = path.Replace(AppDomain.CurrentDomain.BaseDirectory, @"");

            var pathUri = path.Contains(':') ? new Uri(path) : new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), path);

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Open(pathUri);

            Path = path;
            SoundName = path.Split('\\', '/').Last();
        }

        private void _mediaPlayer_MediaEnded(object? sender, EventArgs e)
        {
            if(IsLooping)
            {
                _mediaPlayer.Stop();
                Play();
            }
            else
            {
                Stop();
            }
        }

        public static string DefaultImage { get => _defaultImage; }

        public void Play()
        {
            _mediaPlayer.Play();
            _isPlaying = true;
            PlayIconVisibility = Visibility.Visible;
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
            _isPlaying = false;
            PlayIconVisibility = Visibility.Collapsed;
        }

        public void StartDragging()
        {
            if (!_isBeingResized)
            {
                _isBeingDragged = true;
                _positionChanged = false;

                ButtonDragged?.Invoke(this, new ButtonDraggedEventArgs { IsBeingDragged = true });
            }
        }

        public void StartResizing()
        {
            _isBeingDragged = false;
            _isBeingResized = true;
            _positionChanged = false;
        }

        public void StopDragging() 
        {
            _isBeingDragged = false;
            _isBeingResized = false;

            ButtonDragged?.Invoke(this, new ButtonDraggedEventArgs { IsBeingDragged = false });

            if (_positionChanged)
            {
                ApperanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void MouseMove(Point mousePosition) 
        {
            if (_isBeingResized)
            {
                var x = ((int)mousePosition.X - _marginLeft);
                var y = ((int)mousePosition.Y - _marginTop); 

                WidthPoints = (x / Parameters.GridSize) + 1;
                HeightPoints = (y / Parameters.GridSize) + 1;
            }
            if (_isBeingDragged)
            {
                _marginLeft = (((int)mousePosition.X - _mouseOffsetX) / Parameters.GridSize) * Parameters.GridSize;
                _marginTop = (((int)mousePosition.Y - _mouseOffsetY) / Parameters.GridSize) * Parameters.GridSize;

                Margin = $"{_marginLeft}, {_marginTop}, 0, 0";
            }
            else
            {
                _mouseOffsetX = (int)mousePosition.X - _marginLeft;
                _mouseOffsetY = (int)mousePosition.Y - _marginTop;
            }
        }

        public bool HandleDrop(IDataObject dropData)
        {
            if (dropData is null)
                return false;

            if (dropData.GetData(DataFormats.FileDrop) is not IEnumerable<string> files)
                return false;

            if (files.Count() > 1)
                return false;

            var file = files.First();

            if (!FilesHelper.IsImageFile(file))
                return false;

            if (FilesHelper.IsAnimatedImageFile(file))
            {
                ImagePath = null;
                AnimatedImagePath = file;
            }
            else
            {
                AnimatedImagePath = null;
                ImagePath = file;
            }

            ApperanceChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public ICommand ClickCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public event EventHandler ApperanceChanged;

        public event EventHandler<ButtonDraggedEventArgs> ButtonDragged;

        public int ColorNumber 
        { 
            get => colorNumber;
            set
            {
                colorNumber = value;
                ImageTintBrush = BrushHalpers.BrushMappings[ColorNumber];
            }
        }

        public string SoundName 
        { 
            get => soundName;
            set 
            {
                soundName = value;
                OnPropertyChanged(nameof(SoundName));

                ApperanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string? ImagePath 
        { 
            get => imagePath;

            set
            {


                imagePath = value;
                OnPropertyChanged(nameof(ImagePath));

                ImageVisibility = imagePath == _defaultImage ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string? AnimatedImagePath
        {
            get => animatedImagePath;

            set
            {
                animatedImagePath = value;
                OnPropertyChanged(nameof(AnimatedImagePath));

                if(value is not null)
                    ImageVisibility = Visibility.Collapsed;
            }
        }

        public Visibility ImageVisibility
        {
            get => imageVisibility;

            set
            {
                imageVisibility = value;
                OnPropertyChanged(nameof(ImageVisibility));
            }
        }

        public Visibility PlayIconVisibility
        {
            get => playIconVisibility;

            set
            {
                playIconVisibility = value;
                OnPropertyChanged(nameof(PlayIconVisibility));
            }
        }

        public Brush ImageTintBrush 
        { 
            get => imageTintBrush;
            set
            {
                imageTintBrush = value;
                OnPropertyChanged(nameof(ImageTintBrush));
            }
        }

        public string Path { get; set; }

        public string Margin 
        { 
            get => margin;

            set
            {

                margin = value;

                var parts = margin.Split(", ");

                _marginLeft = int.Parse(parts[0]);
                _marginTop = int.Parse(parts[1]);

                _positionChanged = true;

                OnPropertyChanged(nameof(Margin));
            }
        }

        public int WidthPoints 
        { 
            get => widthPoints; 
            
            set 
            {
                if (value > 0)
                {
                    widthPoints = value;
                    Width = value * Parameters.GridSize;

                    OnPropertyChanged(nameof(WidthPoints));
                }
            } 
        }

        public int HeightPoints
        {
            get => heightPoints;

            set
            {
                if (value > 0)
                {
                    heightPoints = value;
                    Height = value * Parameters.GridSize;

                    OnPropertyChanged(nameof(HeightPoints));
                }
            }
        }

        public int Width 
        { 
            get => width;
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public int Height 
        { 
            get => height;
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public Visibility AdditionalButtonsVisibility 
        { 
            get => additionalButtonsVisibility;
            set
            {
                additionalButtonsVisibility = value;
                OnPropertyChanged(nameof(AdditionalButtonsVisibility));
            }
        }

        public bool IsLooping 
        { 
            get => isLooping;

            set
            {
                isLooping = value;
                OnPropertyChanged(nameof(IsLooping));

                ApperanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public double Volume 
        { 
            get => _mediaPlayer.Volume;

            set
            {
                _mediaPlayer.Volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }

        private MediaPlayer _mediaPlayer;
        private bool _isPlaying;
        private string soundName;
        private int widthPoints;
        private int heightPoints;
        private int width;
        private int height;
        private string margin = "0, 0, 0, 0";
        private bool _isBeingDragged = false;
        private bool _isBeingResized = false;
        private bool _positionChanged = false;

        private int _marginLeft = 0;
        private int _marginTop = 0;
        private int _mouseOffsetX = 0;
        private int _mouseOffsetY = 0;
        private string? imagePath;
        private string? animatedImagePath;
        private Visibility imageVisibility;
        private Visibility playIconVisibility;
        private Visibility additionalButtonsVisibility;
        private bool isLooping;

        private static string _defaultImage = @"/Images/defaultButtonImage.png";
        private Brush imageTintBrush;
        private double volume;
        private int colorNumber;
    }
}
