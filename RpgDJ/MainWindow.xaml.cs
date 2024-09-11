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
            (DataContext as MainWindowViewModel)?.HandleDrop(e.Data);
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.HandleMouseMove(e);
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MainWindowViewModel)?.HandleMouseUp(e);
        }
    }
}