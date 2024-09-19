using RpgDJ.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RpgDJ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AllowDrop = true;
            Drop += MainWindow_Drop;
        }

        private void MainWindow_Drop(object sender, DragEventArgs e)
        {
            MainWindowViewModel?.HandleDrop(e.GetPosition(Application.Current.MainWindow), e.Data);
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MainWindowViewModel?.HandleMouseMove(e);
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel?.HandleMouseUp(e);
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel?.HandleMouseUpOnDeleteButton(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (WindowStyle == WindowStyle.SingleBorderWindow)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else 
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainWindowViewModel is not null && MainWindowViewModel.UnsavedChanges)
            {
                var answer = MessageBox.Show("Save changes?", "Unsaved changes.", MessageBoxButton.YesNoCancel);

                switch (answer)
                {
                    case MessageBoxResult.Yes: MainWindowViewModel.Save(); break;
                    case MessageBoxResult.Cancel: e.Cancel = true; break;
                }
            }
        }

        private MainWindowViewModel? MainWindowViewModel { get => DataContext as MainWindowViewModel; }
    }
}