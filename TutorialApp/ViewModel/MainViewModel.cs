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

using TutorialApp.ViewModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TutorialApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private object? _currentView;
        private readonly LibraryViewModel _libraryViewModel = new();
        private readonly HomeViewModel _homeViewModel = new();
        private readonly VideoViewModel _videoViewModel = new();

        [ObservableProperty]
        private string _title = "Tutorial application";

        [ObservableProperty]
        private GridLength _menuWidth = new GridLength(0);
        private readonly GridLength _menuWidthOpened = new GridLength(100);
        private readonly GridLength _menuWidthClosed = new GridLength(0);

        [RelayCommand]
        private void ChangeView(string target)
        {
            if (target == "Main")
            {
                CurrentView = _homeViewModel;
            }
            else if (target == "Library")
            {
                CurrentView = _libraryViewModel;
            }
            else if (target == "Video")
            {
                CurrentView = _videoViewModel;
            }
        }

        [RelayCommand]
        private void SideMenu()
        {
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
