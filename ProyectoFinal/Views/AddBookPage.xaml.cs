using ProyectoFinal.Services;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Views;

public partial class AddBookPage : ContentPage
{
    private readonly AddBookViewModel _viewModel;

    public AddBookPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _viewModel = new AddBookViewModel(databaseService);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}