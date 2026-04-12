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
        set { _searchQuery = value; OnPropertyChanged(); }
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

    // Commands
    public ICommand SearchCommand { get; }
    public ICommand ViewDetailsCommand { get; }

#pragma warning disable CS8618
    public SearchViewModel(BookApiService bookApiService)
    {
        _bookApiService = bookApiService;
        SearchCommand = new RelayCommand(async () => await SearchBooks());
        ViewDetailsCommand = new RelayCommand<BookSearchResult>(async (book) => await ViewDetails(book));
    }
#pragma warning restore CS8618

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
        }
#pragma warning disable CS0618
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo buscar: {ex.Message}", "OK");
        }
#pragma warning restore CS0618
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
#pragma warning disable CS8625, 8612
    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#pragma warning restore CS8625, 8612
}
