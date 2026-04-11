using ProyectoFinal.Models;
using ProyectoFinal.Services;


namespace ProyectoFinal.Views;

public partial class AddBookPage : ContentPage
{
	private readonly DatabaseService _databaseService;

	public AddBookPage(DatabaseService databaseService)
	{
		InitializeComponent();
		_databaseService = databaseService;
	}

	async void OnSaveBookTapped(object sender, EventArgs e)
	{
		//se toman valores de los campos
		var book = new Book
		{
#pragma warning disable CA1416, CS0618
            Title = TitleEntry.Text,
			Author = AuthorEntry.Text,
			ISBN = ISBNEntry.Text,
			Year = int.TryParse(YearEntry.Text, out int year) ? year : 0,
			Pages = int.TryParse(PagesEntry.Text, out int pages) ? pages : 0,
			Description = DescriptionEntry.Text,
			DateAdded = DateTime.Now
        };

		if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
		{
            await DisplayAlert("Error", "Título y autor son obligatorios", "OK");
			return;
		}
        await _databaseService.SaveBookAsync(book);
		await DisplayAlert("Libro Guardado", "El libro ha sido guardado exitosamente.", "OK");
		await Shell.Current.GoToAsync(".."); // Regresa a la página anterior
	}
	async void OnCancelTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(".."); // Regresa a la página anterior sin guardar
	}
#pragma warning restore CA1416, CS0618
}