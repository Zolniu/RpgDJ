using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using RpgDJ.Controls;
using RpgDJ.DataModels;
using RpgDJ.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
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
        private SessionPanelViewModel selectedSession;
        private int selectedSessionIndex;
        private ICommand selectSessionCommand;
        private string _saveFileName = "Sessions.json";
        private Visibility topPanelVisibility;

        public MainWindowViewModel()
        {
            Load();

            SelectedSessionIndex = 0;
            SelectedSession = SessionPanels[SelectedSessionIndex];

            SelectSessionCommand = new RelayCommand<int>(index => 
            {
                SelectedSessionIndex = index;
            });

            SaveAllCommand = new RelayCommand(() =>
            {
                SaveAll();
            });

            SaveSessionCommand = new RelayCommand<int>((index) => 
            {
                SessionPanels[index].Save();
            });

            NewSessionCommand = new RelayCommand(() => 
            {
                SessionPanels.Add(PrepareSessionPanelVM($"Session {SessionPanels.Count + 1}", $"session{SessionPanels.Count + 1}.json", SessionPanels.Count));

                SelectedSessionIndex = SessionPanels.Count - 1;
            });
        }

        private void DeleteSession(int index)
        {
            var answer = MessageBox.Show("Delete session?", "Confirm", MessageBoxButton.YesNo);

            if (answer == MessageBoxResult.Yes)
            {
                SessionPanels.RemoveAt(index);

                EnsureSessionExists();

                SelectedSessionIndex = 0;

                SaveSessionEntries();
            }
        }

        private void Load()
        {
            try
            {
                var json = File.ReadAllText(_saveFileName);

                var saved = JsonConvert.DeserializeObject<SaveFileModel>(json);

                var index = 0;
                foreach (var session in saved.Sessions)
                {
                    SessionPanels.Add(PrepareSessionPanelVM(session.Name, session.Path, index++));
                }
            }
            catch (FileNotFoundException ex)
            {

            }

            EnsureSessionExists();
        }

        private void EnsureSessionExists()
        {
            if (SessionPanels.Count == 0)
            {
                SessionPanels.Add(PrepareSessionPanelVM("Session 1", "session1.json", 0));
            }
        }

        private SessionPanelViewModel PrepareSessionPanelVM(string sessionName, string saveFilePath, int index)
        {
            var newSession = new SessionPanelViewModel(Application.Current.MainWindow, saveFilePath)
            {
                SessionName = sessionName,
                SessionIndex = index,
            };

            newSession.DeleteSession += DeleteSession;
            newSession.SaveSession += index => SaveSessionEntries();

            return newSession;
        }

        public void SaveAll()
        {
            var saveModel = new SaveFileModel { Sessions = [] };

            foreach (var session in SessionPanels)
            {
                session.Save();

                saveModel.Sessions.Add(new SessionEntry { Name = session.SessionName, Path = session.SaveFilePath });
            }

            var json = JsonConvert.SerializeObject(saveModel);

            File.WriteAllText(_saveFileName, json);
        }

        public void SaveSessionEntries()
        {
            var saveModel = new SaveFileModel { Sessions = [] };

            foreach (var session in SessionPanels)
            {
                saveModel.Sessions.Add(new SessionEntry { Name = session.SessionName, Path = session.SaveFilePath });
            }

            var json = JsonConvert.SerializeObject(saveModel);

            File.WriteAllText(_saveFileName, json);
        }

        public ICommand SelectSessionCommand 
        { 
            get => selectSessionCommand;
            set
            {
                selectSessionCommand = value;
                OnPropertyChanged(nameof(SelectSessionCommand));
            }
        }

        public Visibility TopPanelVisibility
        {
            get => topPanelVisibility;

            set
            {
                topPanelVisibility = value;
                OnPropertyChanged(nameof(TopPanelVisibility));
            }
        }

        public ICommand DeleteSessionCommand { get; set; }

        public ICommand SaveAllCommand { get; set; }

        public ICommand SaveSessionCommand { get; set; }

        public ICommand NewSessionCommand { get; set; }

        public ObservableCollection<SessionPanelViewModel> SessionPanels { get; set; } = [];

        public bool UnsavedChanges
        {
            get => SessionPanels.Any(s => s.UnsavedChanges);
        }

        public SessionPanelViewModel SelectedSession 
        { 
            get => selectedSession;

            set
            {
                selectedSession = value;
                OnPropertyChanged(nameof(SelectedSession));
            }
        }

        public int SelectedSessionIndex 
        { 
            get => selectedSessionIndex;

            set
            {
                if (SelectedSession is not null)
                {
                    SelectedSession.IsActive = false;
                }

                selectedSessionIndex = value;                
                SelectedSession = SessionPanels[SelectedSessionIndex];

                SelectedSession.IsActive = true;

                OnPropertyChanged(nameof(SelectedSessionIndex));
            }
        }
    }
}
