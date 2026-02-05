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

        private int _lastID { get; set; }

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

        [ObservableProperty]
        private ObservableCollection<int> _pageNumbers = new();
        private int _startPageIndex;

        [ObservableProperty]
        private int _currentPageNumber;

        [ObservableProperty]
        private bool _hasNextPage;

        [ObservableProperty]
        private bool _hasPrevPage;
        
        private int _itemNumberPerPage { get; set; }
        private int _pageNumberPerBlock { get; set; }

        public LibraryViewModel()
        {
            FilterOptions = new ObservableCollection<FilterItem>
            {
                new FilterItem {DisplayName = "제목", DBFieldName = "title"},
                new FilterItem { DisplayName = "저자", DBFieldName = "author"},
                new FilterItem { DisplayName = "장르", DBFieldName = "genre"}
            };
            SelectedFilter = FilterOptions[0].DBFieldName;
            HasNextPage = false;
            _hasPrevPage = false;

            _itemNumberPerPage = 10;
            _pageNumberPerBlock = 10;
            _startPageIndex = 1;
            CurrentPageNumber = 1;
        }

        private async void LoadAllBooks()
        {
            IsLoading = true;
            try
            {
                BookDataManager manager = new BookDataManager();
                var bookList = await manager.LoadAllBooks(_itemNumberPerPage);

                Books = new ObservableCollection<Book>(bookList);
            }
            finally { IsLoading = false; }
        }

        // 다음 페이지가 있는지 확인하기 위해서 
        // limit + 1개를 받아서 확인함 -> return: 총 페이지 개수 + 1
        private async Task<int> LoadPageCount(string field, string value, int blockIndex)
        {
            IsLoading = true;
            try
            {
                BookDataManager manager = new BookDataManager();
                int limit = _itemNumberPerPage * _pageNumberPerBlock + 1;
                int offset = blockIndex * limit;
                int itemCount = await manager.GetCountInRange(field, value, limit, offset);

                return (int)Math.Ceiling((double)itemCount / 10.0f);
            }
            finally { IsLoading = false; }
        }

        private void FillPageNumbers(int pageCount)
        {
            int startPageNumber = _startPageIndex;
            int endPageNumber = startPageNumber + pageCount;

            PageNumbers.Clear();
            for (int i = startPageNumber; i < endPageNumber; i++)
            {
                PageNumbers.Add(i);
            }
        }

        [RelayCommand]
        private async Task Search()
        {
            IsLoading = true;
            _startPageIndex = 1;

            try
            {
                BookDataManager manager = new BookDataManager();
                var bookList = await manager.SearchBooks(SelectedFilter, SearchText, _itemNumberPerPage, 0);
                int pageCount = await LoadPageCount(SelectedFilter, SearchText, 0);
                
                HasNextPage = (pageCount > _pageNumberPerBlock);
                HasPrevPage = false;

                pageCount = Math.Clamp(pageCount, 1, _pageNumberPerBlock);
                FillPageNumbers(pageCount);

                Books = new ObservableCollection<Book>(bookList);
            }
            finally { IsLoading = false; }
        }

        [RelayCommand]
        private async Task MovePage(string strMoveNext)
        {
            bool moveNext = Convert.ToBoolean(strMoveNext);
            IsLoading = true;
            _startPageIndex = moveNext ? _startPageIndex + _pageNumberPerBlock : _startPageIndex - _pageNumberPerBlock;

            try
            {
                BookDataManager manager = new BookDataManager();
                int offset = (_startPageIndex - 1) * _itemNumberPerPage;
                var bookList = await manager.SearchBooks(SelectedFilter, SearchText, _itemNumberPerPage, offset);

                int blockIdx = (_startPageIndex / _pageNumberPerBlock);
                int pageCount = await LoadPageCount(SelectedFilter, SearchText, blockIdx);

                HasNextPage = (pageCount > _pageNumberPerBlock);
                HasPrevPage = (_startPageIndex > _pageNumberPerBlock);

                pageCount = Math.Clamp(pageCount, 1, _pageNumberPerBlock);
                FillPageNumbers(pageCount);

                Books = new ObservableCollection<Book>(bookList);
            }
            finally { IsLoading = false; }
        }

        [RelayCommand]
        private async Task SearchPage(int pageIndex)
        {
            IsLoading = true;
            
            try
            {
                BookDataManager manager = new BookDataManager();
                int offset = (pageIndex - 1) * _itemNumberPerPage;
                var bookList = await manager.SearchBooks(SelectedFilter, SearchText, _itemNumberPerPage, offset);

                Books = new ObservableCollection<Book>(bookList);
            }
            finally { IsLoading = false; }
        }
    }
}
