using ProyectoFinal.Models;
using ProyectoFinal.Service;

namespace ProyectoFinal.Views;

[QueryProperty(nameof(BookId), "bookId")]
public partial class BookDetailPage : ContentPage
{
    private readonly BookApiService _bookApiService;
    private string _bookId;

    public string BookId
    {
        get => _bookId;
        set
        {
            _bookId = value;
            LoadBookDetail(value); // para que desde que tome el ID, entonces cargue el detalle del libro
        }
    }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public BookDetailPage()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    {
        InitializeComponent();
        _bookApiService = new BookApiService();
    }

    async void LoadBookDetail(string bookId)
    {
        try
        {
            var book = await _bookApiService.GetBookDetailAsync(bookId);
            BindingContext = book; 
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo cargar el libro: {ex.Message}", "OK");
        }
    }

    async void OnBackTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(".."); // para retroceder
    }
}