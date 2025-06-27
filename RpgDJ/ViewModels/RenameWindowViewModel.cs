using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RpgDJ.ViewModels
{
    class RenameWindowViewModel : ViewModelBase
    {
        public RenameWindowViewModel()
        {
            RenameWindowVisibility = Visibility.Collapsed;

            SaveCommand = new RelayCommand(()=>
            {
                RenameWindowVisibility = Visibility.Collapsed;
                SessionPanel.UnsavedChanges = true;
            });

            CancelCommand = new RelayCommand(() =>
            {
                RenameWindowVisibility = Visibility.Collapsed;
                SessionName = _prevoiusName;
            });
        }

        public SessionPanelViewModel SessionPanel { get; set; }

        public string SessionName 
        {
            get => SessionPanel?.SessionName ?? string.Empty;

            set => SessionPanel.SessionName = value;
        }

        public Visibility RenameWindowVisibility
        {
            get => renameWindowVisibility;

            set
            {
                if (value != Visibility.Collapsed)
                {
                    _prevoiusName = SessionName;
                }

                renameWindowVisibility = value;
                OnPropertyChanged(nameof(RenameWindowVisibility));
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private Visibility renameWindowVisibility;

        private string _prevoiusName;
    }
}
