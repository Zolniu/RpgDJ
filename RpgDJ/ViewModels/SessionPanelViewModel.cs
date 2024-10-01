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
    internal class SessionPanelViewModel : ViewModelBase
    {
        public SessionPanelViewModel(Window mainWindow, string saveFilePath)
        {
            IsActive = false;
            AdditionalButtonsVisibility = Visibility.Collapsed;

            SaveFilePath = saveFilePath;

            if (!string.IsNullOrEmpty(SaveFilePath))
            {
                Load();
            }

            MuteAllCommand = new RelayCommand(() => 
            {
                foreach (var sound in SoundButtons)
                    sound.Stop();
            });

            FullScreenCommand = new RelayCommand(() => 
            {
                if (mainWindow.WindowStyle == WindowStyle.SingleBorderWindow)
                {
                    mainWindow.WindowStyle = WindowStyle.None;
                    mainWindow.WindowState = WindowState.Maximized;

                    (mainWindow.DataContext as MainWindowViewModel).TopPanelVisibility = Visibility.Collapsed;
                }
                else
                {
                    mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    mainWindow.WindowState = WindowState.Normal;

                    (mainWindow.DataContext as MainWindowViewModel).TopPanelVisibility = Visibility.Visible;
                }
            });

            DeleteSessionCommand = new RelayCommand(Delete);

            SaveSessionCommand = new RelayCommand(Save);
        }

        public event Action<int> DeleteSession;
        public event Action<int> SaveSession;

        public string SessionName { get; set; }

        public string SaveFilePath { get; set; }

        public string DisplayName { get => $"{SessionName}{(UnsavedChanges ? "*" : string.Empty) }"; }

        public int SessionIndex { get; set; }

        public bool IsActive 
        { 
            get => isActive;
            set
            {
                isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public ICommand MuteAllCommand { get; set; }

        public ICommand FullScreenCommand { get; set; }

        public ICommand SaveSessionCommand { get; set; }

        public ICommand DeleteSessionCommand { get; set; }

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

                UnsavedChanges = true;
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

                _lastDropPosition.X += Parameters.GridSize * 3;
                _lastDropPosition = _lastDropPosition.SnapToGrid();
            }

            UnsavedChanges = true;
        }

        private void SoundButtonVM_ButtonDragged(object? sender, ButtonDraggedEventArgs e)
        {
            _draggedButton = sender as SoundButtonViewModel;

            AdditionalButtonsVisibility = e.IsBeingDragged ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SoundButtonVM_ApperanceChanged(object? sender, EventArgs e)
        {
            UnsavedChanges = true;
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
                var json = File.ReadAllText(SaveFilePath);

                var saved = JsonConvert.DeserializeObject<SessionSaveFileModel>(json);

                foreach (var sound in saved.Sounds)
                {
                    var soundButtonVM = sound.ToViewModel();

                    soundButtonVM.ApperanceChanged += SoundButtonVM_ApperanceChanged;
                    soundButtonVM.ButtonDragged += SoundButtonVM_ButtonDragged;

                    SoundButtons.Add(soundButtonVM);
                }

                _lastDropPosition = saved.LastDropPosition;
            }
            catch 
            {
            }
        }

        public void Save()
        {
            var save = new SessionSaveFileModel
            {
                LastDropPosition = _lastDropPosition,
                Sounds = SoundButtons.Select(s => new SoundFileModel(s)).ToList()
            };

            var json = JsonConvert.SerializeObject(save);

            File.WriteAllText(SaveFilePath, json);

            UnsavedChanges = false;

            SaveSession?.Invoke(SessionIndex);
        }

        public void Delete()
        {
            File.Delete(SaveFilePath);

            DeleteSession?.Invoke(SessionIndex);
        }

        public bool UnsavedChanges 
        { 
            get => unsavedChanges;

            set
            {
                unsavedChanges = value;
                OnPropertyChanged(nameof(UnsavedChanges));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private Point _lastDropPosition = new (0, 0);
        private Visibility additionalButtonsVisibility;

        private SoundButtonViewModel? _draggedButton;
        private bool unsavedChanges = false;
        private bool isActive;
    }
}
