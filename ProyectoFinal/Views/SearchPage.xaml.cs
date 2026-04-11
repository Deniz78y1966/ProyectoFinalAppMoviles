using ProyectoFinal.Models;
using ProyectoFinal.Services;

namespace ProyectoFinal.Views;

public partial class SearchPage : ContentPage
{
    private readonly BookApiService _bookApiService;
    public List<BookSearchResult> Results { get; set; } = new();

    public SearchPage(BookApiService bookApiService)
    {
        InitializeComponent();
        _bookApiService = bookApiService;
        BindingContext = this;
    }

    async void OnSearchTapped(object sender, EventArgs e)
    {
        var query = SearchEntry.Text;

        if (string.IsNullOrWhiteSpace(query))
        {
            await DisplayAlert("Error", "Escribe algo para buscar", "OK");
            return;
        }

        // Show loading
        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;
        NoResultsLabel.IsVisible = false;

        try
        {
            Results = await _bookApiService.SearchBooksAsync(query);
            ResultsCollection.ItemsSource = Results;

            NoResultsLabel.IsVisible = Results.Count == 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo buscar: {ex.Message}", "OK");
        }
        finally
        {
            // Hide loading whether it succeeded or failed
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }

    async void OnBookTapped(object sender, EventArgs e)
    {
        var book = (sender as BindableObject)?.BindingContext as BookSearchResult;
        if (book != null)
            await Shell.Current.GoToAsync($"bookdetail?bookId={book.Id}");
    }
}