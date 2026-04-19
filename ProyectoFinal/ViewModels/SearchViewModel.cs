using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProyectoFinal.ViewModels;

public class SearchViewModel : INotifyPropertyChanged
{
    private readonly BookApiService _bookApiService;
    private string _searchQuery;
    private bool _isLoading;
    private bool _noResults;

    public ObservableCollection<BookSearchResult> Results { get; set; } = new();

    public string SearchQuery
    {
        get => _searchQuery;
        set { _searchQuery = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasQuery)); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public bool NoResults
    {
        get => _noResults;
        set { _noResults = value; OnPropertyChanged(); }
    }

    // Calculated properties
    public bool HasQuery => !string.IsNullOrWhiteSpace(SearchQuery);
    public int ResultsCount => Results.Count;
    public string ResultsMessage => ResultsCount > 0
        ? $"{ResultsCount} resultados encontrados"
        : "No se encontraron resultados";

    // Commands
    public ICommand LoadAsyncCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ViewDetailsCommand { get; }

    public SearchViewModel(BookApiService bookApiService)
    {
        _bookApiService = bookApiService;
        LoadAsyncCommand = new RelayCommand(async () => await LoadAsync());
        SearchCommand = new RelayCommand(async () => await SearchBooks());
        ViewDetailsCommand = new RelayCommand<BookSearchResult>(async (book) => await ViewDetails(book));
    }

    // LoadAsync → initial state
    public async Task LoadAsync()
    {
        Results.Clear();
        NoResults = false;
        await Task.CompletedTask;
    }

    async Task SearchBooks()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery)) return;

        IsLoading = true;
        NoResults = false;
        Results.Clear();

        try
        {
            var results = await _bookApiService.SearchBooksAsync(SearchQuery);
            foreach (var book in results)
                Results.Add(book);

            NoResults = Results.Count == 0;
            OnPropertyChanged(nameof(ResultsCount));
            OnPropertyChanged(nameof(ResultsMessage));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo buscar: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    async Task ViewDetails(BookSearchResult book)
    {
        if (book != null)
            await Shell.Current.GoToAsync($"bookdetail?bookId={book.Id}");
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}