using ProyectoFinal.Models;
using ProyectoFinal.Services;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Views;

[QueryProperty(nameof(BookId), "bookId")]
public partial class BookDetailPage : ContentPage
{
    private readonly BookDetailViewModel _viewModel;
    private string _bookId;

    public string BookId
    {
        get => _bookId;
        set
        {
            _bookId = value;
            _viewModel.LoadBook(value);
        }
    }

    public BookDetailPage(BookApiService bookApiService, DatabaseService databaseService)
    {
        InitializeComponent();
        _viewModel = new BookDetailViewModel(bookApiService, databaseService);
        BindingContext = _viewModel;
    }

    void OnStarTapped(object sender, EventArgs e)
    {
        var tappedStar = (Button)sender;
        var stars = new[] { Star1, Star2, Star3, Star4, Star5 };
        _viewModel.Rating = Array.IndexOf(stars, tappedStar) + 1;
        for (int i = 0; i < stars.Length; i++)
            stars[i].TextColor = i < _viewModel.Rating ? Colors.Gold : Colors.Gray;
    }
}