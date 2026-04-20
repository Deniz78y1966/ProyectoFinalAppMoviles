using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProyectoFinal.ViewModels;

public class LibraryViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;
    private string _selectedGenre;
    private string _searchQuery;
    private List<Book> _allBooks = new(); // stores all books for filtering

    public ObservableCollection<Book> Books { get; set; } = new();

    public string SearchQuery
    {
        get => _searchQuery;
        set { _searchQuery = value; OnPropertyChanged(); ApplyFilters(); }
    }

    public string SelectedGenre
    {
        get => _selectedGenre;
        set { _selectedGenre = value; OnPropertyChanged(); }
    }

    // Commands
    public ICommand AddBookCommand { get; set; }
    public ICommand ViewDetailsCommand { get; set; }
    public ICommand MarkAsReadCommand { get; set; }
    public ICommand DeleteBookCommand { get; set; }
    public ICommand FilterByGenreCommand { get; set; }
    public ICommand FilterAllCommand { get; set; }
    public ICommand FilterReadCommand { get; set; }
    public ICommand FilterPendingCommand { get; set; }
    public ICommand LoadAsyncCommand { get; set; }

    // Calculated properties
    public int TotalBooks => Books?.Count ?? 0;
    public int ReadBooks => Books?.Count(b => b.IsRead) ?? 0;
    public int PendingCount => TotalBooks - ReadBooks;
    public string ReadPercentage => TotalBooks > 0
        ? $"{Math.Round((double)ReadBooks / TotalBooks * 100, 1)}% leído"
        : "0% leído";
    public int BooksCount => TotalBooks;

    public LibraryViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;

        AddBookCommand = new RelayCommand(async () => await AddBook());
        ViewDetailsCommand = new RelayCommand<Book>(async (book) => await ViewDetails(book));
        MarkAsReadCommand = new RelayCommand<Book>(async (book) => await MarkAsRead(book));
        DeleteBookCommand = new RelayCommand<Book>(async (book) => await DeleteBook(book));
        FilterByGenreCommand = new RelayCommand<string>(async (genre) => await FilterByGenre(genre));
        FilterAllCommand = new RelayCommand(() => FilterAll());
        FilterReadCommand = new RelayCommand(() => FilterRead());
        FilterPendingCommand = new RelayCommand(() => FilterPending());
        LoadAsyncCommand = new RelayCommand(async () => await LoadAsync());

        _ = LoadAsync();
    }

    public async Task LoadAsync()
    {
        var books = await _databaseService.GetBooksAsync();
        _allBooks = books;
        Books.Clear();
        foreach (var book in books)
            Books.Add(book);

        NotifyCalculatedProperties();
    }

    void ApplyFilters()
    {
        var filtered = _allBooks.AsEnumerable();

        // Apply search query
        if (!string.IsNullOrWhiteSpace(SearchQuery))
            filtered = filtered.Where(b =>
                (b.Title?.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (b.Author?.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ?? false));

        Books.Clear();
        foreach (var book in filtered)
            Books.Add(book);

        NotifyCalculatedProperties();
    }

    void FilterAll()
    {
        Books.Clear();
        foreach (var book in _allBooks)
            Books.Add(book);
        NotifyCalculatedProperties();
    }

    void FilterRead()
    {
        Books.Clear();
        foreach (var book in _allBooks.Where(b => b.IsRead))
            Books.Add(book);
        NotifyCalculatedProperties();
    }

    void FilterPending()
    {
        Books.Clear();
        foreach (var book in _allBooks.Where(b => !b.IsRead))
            Books.Add(book);
        NotifyCalculatedProperties();
    }

    async Task FilterByGenre(string genre)
    {
        Books.Clear();
        var filtered = string.IsNullOrEmpty(genre)
            ? _allBooks
            : _allBooks.Where(b => b.Genre == genre).ToList();
        foreach (var book in filtered)
            Books.Add(book);
        NotifyCalculatedProperties();
        await Task.CompletedTask;
    }

    void NotifyCalculatedProperties()
    {
        OnPropertyChanged(nameof(TotalBooks));
        OnPropertyChanged(nameof(ReadBooks));
        OnPropertyChanged(nameof(PendingCount));
        OnPropertyChanged(nameof(BooksCount));
        OnPropertyChanged(nameof(ReadPercentage));
    }

    async Task AddBook() => await Shell.Current.GoToAsync("addbook");

    async Task ViewDetails(Book book)
    {
        if (book != null)
            await Shell.Current.GoToAsync($"bookdetail?bookId={book.ISBN}");
    }

    async Task MarkAsRead(Book book)
    {
        if (book != null)
        {
            book.IsRead = true;
            await _databaseService.SaveBookAsync(book);
            await LoadAsync();
            await Shell.Current.DisplayAlert("Éxito", $"'{book.Title}' marcado como leído!", "OK");
        }
    }

    async Task DeleteBook(Book book)
    {
        if (book != null)
        {
            bool confirm = await Shell.Current.DisplayAlert("Eliminar",
                $"¿Eliminar '{book.Title}'?", "Sí", "No");
            if (confirm)
            {
                await _databaseService.DeleteBookAsync(book);
                await LoadAsync();
            }
        }
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}