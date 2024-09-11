using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RpgDJ.ViewModels
{
    internal class SoundButtonViewModel : ViewModelBase
    {
        public SoundButtonViewModel(string path)
        {
            var pathUri = new Uri(path);
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Open(pathUri);

            WidthPoints = 2;
            HeightPoints = 2;

            Path = path;
            SoundName = path.Split('\\', '/').Last();

            ClickCommand = new RelayCommand(() =>
            {
                if (!_isPlaying)
                {
                    _mediaPlayer.Play();
                    _isPlaying = true;
                }
                else
                {
                    _mediaPlayer.Stop();
                    _isPlaying = false;
                }
            });
        }

        public void StartDragging()
        {
            if (!_isBeingResized)
            {
                _isBeingDragged = true;
                _positionChanged = false;
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

            if (_positionChanged)
            {
                PositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void MouseMove(Point mousePosition) 
        {
            if (_isBeingResized)
            {
                var x = ((int)mousePosition.X - _marginLeft);
                var y = ((int)mousePosition.Y - _marginTop); 

                Application.Current.MainWindow.Title = $"Resizing {x}, {y}";

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

        public ICommand ClickCommand { get; set; }

        public event EventHandler PositionChanged;

        public string SoundName 
        { 
            get => soundName;
            set 
            {
                OnPropertyChanged(nameof(SoundName));
                soundName = value;
            }
        }

        public string Path { get; set; }

        public string Margin 
        { 
            get => margin;

            set
            {

                margin = value;

                var parts = margin.Split(",");

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

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

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
    }
}
