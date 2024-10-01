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
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainWindowViewModel is not null && MainWindowViewModel.UnsavedChanges)
            {
                var answer = MessageBox.Show("Save changes?", "Unsaved changes.", MessageBoxButton.YesNoCancel);

                switch (answer)
                {
                    case MessageBoxResult.Yes: MainWindowViewModel.SaveAll(); break;
                    case MessageBoxResult.Cancel: e.Cancel = true; break;
                }
            }
        }

        private MainWindowViewModel? MainWindowViewModel { get => DataContext as MainWindowViewModel; }
    }
}