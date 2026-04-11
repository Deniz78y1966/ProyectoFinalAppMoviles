using ProyectoFinal.Models;
using ProyectoFinal.Services;

namespace ProyectoFinal.Views;

[QueryProperty(nameof(BookId), "bookId")]
public partial class BookDetailPage : ContentPage
{
    private readonly BookApiService _bookApiService;
    private readonly DatabaseService _databaseService;
    private BookDetail _currentBook;
    private string _bookId;
    private int _rating = 0;

    public string BookId
    {
        get => _bookId;
        set
        {
            _bookId = value;
            LoadBookDetail(value); // para que desde que tome el ID, entonces cargue el detalle del libro
        }
    }

#pragma warning disable CS8618
    public BookDetailPage(BookApiService bookApiService, DatabaseService databaseService)
    {
        InitializeComponent();
        _bookApiService = bookApiService;   
        _databaseService = databaseService; 
    }
#pragma warning restore CS8618
    async void LoadBookDetail(string bookId)
    {
        try
        {
            _currentBook = await _bookApiService.GetBookDetailAsync(bookId);
            BindingContext = _currentBook;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cargar el libro: {ex.Message}", "OK");
        }
    }

    void OnStarTapped(object sender, EventArgs e)
    {
        var tappedStar = (Button)sender;
        var stars = new[] { Star1, Star2, Star3, Star4, Star5 };

        _rating = Array.IndexOf(stars, tappedStar) + 1;

        for (int i = 0; i < stars.Length; i++)
            stars[i].TextColor = i < _rating ? Colors.Gold : Colors.Gray;
    }

    async void OnSaveBookTapped(object sender, EventArgs e)
    {
        if (_currentBook == null) return; //si esta vacio simplemente retorna y evita cualquier choque.

        var book = new Book
        {
            Title = _currentBook.Title,
            Author = _currentBook.Author,
            ISBN = _currentBook.ISBN,
            Year = _currentBook.PublishedYear,
            Pages = _currentBook.PageCount,
            Description = _currentBook.Description,
            CoverUrl = _currentBook.CoverUrl,
            Genre = _currentBook.Categories?.FirstOrDefault() ?? "No tiene categoria", //en caso de no pertenecer a ninguna categoria de la lista
            IsRead = false,
            Rating = _rating,
            Notes = "",
            DateAdded = DateTime.Now
        };

        await _databaseService.SaveBookAsync(book);
        await DisplayAlert("Éxito", $"'{book.Title}' se ha guardado", "OK");
        await Shell.Current.GoToAsync("..");
    }

    async void OnDeleteBookTapped(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Eliminar",
            "¿Estás seguro que quieres eliminar este libro?", "Sí", "No");
        if (confirm)
        {
            // TODO: delete from local storage
            await Shell.Current.GoToAsync("..");
        }
    }

    async void OnBackTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(".."); 
    }
}