using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using RpgDJ.DataModels;
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
            Load();
        }

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
                foreach (var button in SoundButtons)
                {
                    button.StopDragging();
                }
            }
        }

        public void HandleDrop(IDataObject dataObject)
        {
            if (dataObject is null) return;

            if (dataObject.GetData(DataFormats.FileDrop) is not IEnumerable<string> files) return;

            foreach ( var file in files) 
            {
                var soundButtonVM = new SoundButtonViewModel(file)
                {
                    Margin = $"{_lastDropPosition.X}, 0, 0, 0"
                };

                soundButtonVM.PositionChanged += SoundButtonVM_PositionChanged;

                SoundButtons.Add(soundButtonVM);

                _lastDropPosition.X += Parameters.GridSize;
            }

            Save();
        }

        private void SoundButtonVM_PositionChanged(object? sender, EventArgs e)
        {
            Save();
        }

        public ObservableCollection<SoundButtonViewModel> SoundButtons { get; set; } = 
            [ 
            ];

        private void Load() 
        {
            try
            {
                var json = File.ReadAllText(_saveFileName);

                var saved = JsonConvert.DeserializeObject<SaveFileModel>(json);

                foreach (var sound in saved.Sounds)
                {
                    var soundButtonVM = sound.ToViewModel();

                    soundButtonVM.PositionChanged += SoundButtonVM_PositionChanged;
                    
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
    }
}
