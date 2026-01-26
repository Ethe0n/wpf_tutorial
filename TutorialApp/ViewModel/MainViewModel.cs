using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TutorialApp.Helper;

namespace TutorialApp.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private string _title = "Tutorial application";
        private ICommand? _clickCommand;

        public string Title 
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public ICommand ClickCommand => _clickCommand ??= new RelayCommand(obj =>
        {
            Title = "Link Success!";
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
