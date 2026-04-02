using ProyectoFinal.Models;
using ProyectoFinal.Service;

namespace ProyectoFinal.Views;

public partial class AddBookPage : ContentPage
{
	public AddBookPage()
	{
		InitializeComponent();
	}

	async void OnSaveBookTapped(object sender, EventArgs e)
	{
        //se toman valores de los campos
        var book = new BookDetail
        {
            Title = TitleEntry.Text,
            Author = AuthorEntry.Text,
            ISBN = ISBNEntry.Text,
            PublishedYear = int.TryParse(YearEntry.Text, out int year) ? year : 0,
            PageCount = int.TryParse(PagesEntry.Text, out int pages) ? pages : 0,
            Description = DescriptionEntry.Text
        };

        if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
        {
            await DisplayAlertAsync("Error", "Título y autor son obligatorios", "OK");
            return;
        }

        //TODO: Guardar el libro en la base de datos o servicio.
        await DisplayAlertAsync("Libro Guardado", "El libro ha sido guardado exitosamente.", "OK");
		await Shell.Current.GoToAsync(".."); // Regresa a la página anterior
	}
    async void OnCancelTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(".."); // Regresa a la página anterior sin guardar
    }
}   