using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TutorialApp.Service;
using TutorialApp.Model;
using CommunityToolkit.Mvvm.Input;

namespace TutorialApp.ViewModel
{
    public partial class LibraryViewModel : ObservableObject
    {
        private ObservableCollection<Book> _books { get; set; }
        public ObservableCollection<Book> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        [ObservableProperty]
        private bool _isLoading;

        public List<string> SearchOptions { get; } = new List<string>
        {
            "제목", "저자", "장르"
        };
        [ObservableProperty]
        private string _selectedSearchOption = "제목";

        [ObservableProperty]
        private string _searchText = "";

        public LibraryViewModel()
        {
            LoadAllBooks();
        }

        private async void LoadAllBooks()
        {
            IsLoading = true;
            try
            {
                BookDataManager manager = new BookDataManager();
                var bookList = await manager.LoadAllBooks();

                Books = new ObservableCollection<Book>(bookList);
            }
            finally { IsLoading = false; }
        }

        [RelayCommand]
        private async void Search()
        {
            IsLoading = true;

            try
            {
                BookDataManager manager = new BookDataManager();
                await manager.AsyncTest();
            }
            finally { IsLoading = false; }
        }
    }
}
