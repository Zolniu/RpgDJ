using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using RpgDJ.DataModels;
using RpgDJ.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RpgDJ.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            AdditionalButtonsVisibility = Visibility.Collapsed;

            Load();

            MuteAllCommand = new RelayCommand(() => 
            {
                foreach (var sound in SoundButtons)
                    sound.Stop();
            });
        }

        public ICommand MuteAllCommand { get; set; }

        public void HandleMouseMove(MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                foreach (var button in SoundButtons)
                {
                    button.MouseMove(e.GetPosition(Application.Current.MainWindow));
                }
            }
        }

        public void HandleMouseUp(MouseEventArgs e) 
        {
            if (e.RightButton != MouseButtonState.Pressed)
            {
                _draggedButton?.StopDragging();
            }
        }

        public void HandleMouseUpOnDeleteButton(MouseEventArgs e)
        {
            if (_draggedButton is not null)
            {
                SoundButtons.Remove(_draggedButton);

                Save();
            }
        }

        public void HandleDrop(Point position, IDataObject dataObject)
        {
            if (dataObject is null) return;

            if (dataObject.GetData(DataFormats.FileDrop) is not IEnumerable<string> files) return;

            _lastDropPosition = position.SnapToGrid();

            foreach ( var file in files) 
            {
                var soundButtonVM = new SoundButtonViewModel(file)
                {
                    Margin = $"{_lastDropPosition.X}, {_lastDropPosition.Y}, 0, 0"
                };

                soundButtonVM.ApperanceChanged += SoundButtonVM_ApperanceChanged;
                soundButtonVM.ButtonDragged += SoundButtonVM_ButtonDragged;

                SoundButtons.Add(soundButtonVM);

                _lastDropPosition.X += Parameters.GridSize * 2;
            }

            Save();
        }

        private void SoundButtonVM_ButtonDragged(object? sender, ButtonDraggedEventArgs e)
        {
            _draggedButton = sender as SoundButtonViewModel;

            AdditionalButtonsVisibility = e.IsBeingDragged ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SoundButtonVM_ApperanceChanged(object? sender, EventArgs e)
        {
            Save();
        }

        public ObservableCollection<SoundButtonViewModel> SoundButtons { get; set; } = 
            [ 
            ];

        public Visibility AdditionalButtonsVisibility 
        { 
            get => additionalButtonsVisibility; 
            
            set 
            {
                additionalButtonsVisibility = value;

                OnPropertyChanged(nameof(AdditionalButtonsVisibility));
            } 
        }

        private void Load() 
        {
            try
            {
                var json = File.ReadAllText(_saveFileName);

                var saved = JsonConvert.DeserializeObject<SaveFileModel>(json);

                foreach (var sound in saved.Sounds)
                {
                    var soundButtonVM = sound.ToViewModel();

                    soundButtonVM.ApperanceChanged += SoundButtonVM_ApperanceChanged;
                    soundButtonVM.ButtonDragged += SoundButtonVM_ButtonDragged;

                    SoundButtons.Add(soundButtonVM);
                }

                _lastDropPosition = saved.LastDropPosition;
            }
            catch { }
        }

        private void Save()
        {
            var save = new SaveFileModel
            {
                LastDropPosition = _lastDropPosition,
                Sounds = SoundButtons.Select(s => new SoundFileModel(s)).ToList()
            };

            var json = JsonConvert.SerializeObject(save);

            File.WriteAllText(_saveFileName, json);
        }

        private string _saveFileName = "Save.json";

        private Point _lastDropPosition = new (0, 0);
        private Visibility additionalButtonsVisibility;

        private SoundButtonViewModel? _draggedButton;
    }
}
