using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TutorialApp.ViewModel
{
    public partial class SideMenuViewModel : ObservableObject
    {
        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.MainWindow?.Close();
        }
    }
}
