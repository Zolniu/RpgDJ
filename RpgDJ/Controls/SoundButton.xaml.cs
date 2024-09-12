using RpgDJ.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for SoundButton.xaml
    /// </summary>
    public partial class SoundButton : UserControl
    {
        public SoundButton()
        {
            InitializeComponent();

            Drop += SoundButton_Drop;
        }

        private void SoundButton_Drop(object sender, DragEventArgs e)
        {
            e.Handled = (DataContext as SoundButtonViewModel)!.HandleDrop(e.Data);
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed) 
            {
                (DataContext as SoundButtonViewModel)!.StartDragging();
            }
        }
        
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                (DataContext as SoundButtonViewModel)!.StartResizing();
            }
        }

        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton != MouseButtonState.Pressed) 
            {
                (DataContext as SoundButtonViewModel)!.StopDragging();
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            (DataContext as SoundButtonViewModel)!.MouseMove(e.GetPosition(Application.Current.MainWindow));
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
            {
                (sender as UIElement)!.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            (DataContext as SoundButtonViewModel)!.AdditionalButtonsVisibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            var vm = DataContext as SoundButtonViewModel;

            if (vm is not null)
            {
                vm.AdditionalButtonsVisibility = Visibility.Collapsed;
            }
        }
    }
}
