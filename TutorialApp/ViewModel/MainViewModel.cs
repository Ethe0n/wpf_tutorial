using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TutorialApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "Tutorial application";

        [ObservableProperty]
        private GridLength _menuWidth = new GridLength(0);
        private readonly GridLength _menuWidthOpened = new GridLength(100);
        private readonly GridLength _menuWidthClosed = new GridLength(0);

        [RelayCommand]
        private void Click()
        {
            Title = "LInk success!";
        }

        [RelayCommand]
        private void SideMenu()
        {
            Title = "Open side menu";
            MenuWidth = (MenuWidth.Value <= 0 ?
                _menuWidthOpened : 
                _menuWidthClosed);
        }

        [RelayCommand]
        private void ExitApp()
        {
            Application.Current.MainWindow?.Close();
        }
    }
}
