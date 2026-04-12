using ProyectoFinal.Services;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Views;

public partial class LibraryPage : ContentPage
{
    private readonly LibraryViewModel _viewModel;

    public LibraryPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _viewModel = new LibraryViewModel(databaseService);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadBooks();
    }
}