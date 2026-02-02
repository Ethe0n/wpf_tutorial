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


        public class FilterItem
        {
            public string DisplayName { get; set; }
            public string DBFieldName { get; set; }
        }
        public ObservableCollection<FilterItem> FilterOptions { get; set; }

        [ObservableProperty]
        private string _selectedFilter;

        [ObservableProperty]
        private string _searchText = "";

        public LibraryViewModel()
        {
            FilterOptions = new ObservableCollection<FilterItem>
            {
                new FilterItem {DisplayName = "제목", DBFieldName = "title"},
                new FilterItem { DisplayName = "저자", DBFieldName = "author"},
                new FilterItem { DisplayName = "장르", DBFieldName = "genre"}
            };
            SelectedFilter = FilterOptions[0].DBFieldName;

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
        private async Task Search()
        {
            IsLoading = true;

            try
            {
                BookDataManager manager = new BookDataManager();
                var bookList = await manager.SearchBooks(SelectedFilter, SearchText);

                Books = new ObservableCollection<Book>(bookList);
            }
            finally { IsLoading = false; }
        }
    }
}
