using ProyectoFinal.Models;
using ProyectoFinal.Services;

namespace ProyectoFinal.Views;

public partial class LibraryPage : ContentPage
{
    private readonly DatabaseService _databaseService; // ← fixed naming to _databaseService
    public List<Book> Books { get; set; } = new();

    public LibraryPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        BindingContext = this;
    }

    protected override async void OnAppearing() // ← added async
    {
        base.OnAppearing();
        await LoadBooks();
    }

    async Task LoadBooks() // ← fixed syntax (was "void Task")
    {
        Books = await _databaseService.GetBooksAsync();
        BooksCollection.ItemsSource = Books;
    }

    async void OnBookTapped(object sender, EventArgs e)
    {
        var book = (sender as BindableObject)?.BindingContext as Book;
        if (book != null)
            await Shell.Current.GoToAsync($"bookdetail?bookId={book.ISBN}");
    }

    async void OnMarkAsReadTapped(object sender, EventArgs e)
    {
        var book = (sender as BindableObject)?.BindingContext as Book;
        if (book != null)
        {
            book.IsRead = true;
            await _databaseService.SaveBookAsync(book);
            await LoadBooks();
            await DisplayAlert("Éxito", $"'{book.Title}' marcado como leído!", "OK");
        }
    }

    async void OnAddBookTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("addbook");
    }
}