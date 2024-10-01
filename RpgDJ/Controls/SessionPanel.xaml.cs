using RpgDJ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RpgDJ.Controls
{
    /// <summary>
    /// Interaction logic for SessionPanel.xaml
    /// </summary>
    public partial class SessionPanel : UserControl
    {
        public SessionPanel()
        {
            InitializeComponent();

            AllowDrop = true;
            Drop += MainWindow_Drop;
        }

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
            SessionPanelViewModel?.HandleDrop(e.GetPosition(Application.Current.MainWindow), e.Data);
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            SessionPanelViewModel?.HandleMouseMove(e);
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SessionPanelViewModel?.HandleMouseUp(e);
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SessionPanelViewModel?.HandleMouseUpOnDeleteButton(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SessionPanelViewModel is not null && SessionPanelViewModel.UnsavedChanges)
            {
                var answer = MessageBox.Show("Save changes?", "Unsaved changes.", MessageBoxButton.YesNoCancel);

                switch (answer)
                {
                    case MessageBoxResult.Yes: SessionPanelViewModel.Save(); break;
                    case MessageBoxResult.Cancel: e.Cancel = true; break;
                }
            }
        }

        private SessionPanelViewModel? SessionPanelViewModel { get => DataContext as SessionPanelViewModel; }
    }
}
