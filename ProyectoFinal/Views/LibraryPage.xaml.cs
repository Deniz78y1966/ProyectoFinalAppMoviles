using ProyectoFinal.Service;

namespace ProyectoFinal.Views;

public partial class LibraryPage : ContentPage
{
	public LibraryPage()
	{
		InitializeComponent();
	}
    async void OnBookTapped(object sender, EventArgs e)
    {
        var book = (sender as BindableObject)?.BindingContext as BookSearchResult;
        if (book != null)
        {
            await Shell.Current.GoToAsync($"bookdetail?bookId={book.Id}");
        }
    }

}