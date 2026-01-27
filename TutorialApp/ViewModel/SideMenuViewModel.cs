using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TutorialApp.ViewModel
{
    public partial class SideMenuViewModel
    {
        [RelayCommand]
        private void CloseWindow()
        {
            Application.Current.MainWindow?.Close();
        }
    }
}
