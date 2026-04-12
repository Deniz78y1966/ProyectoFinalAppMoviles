using ProyectoFinal.Services;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Views;

public partial class AddBookPage : ContentPage
{
    public AddBookPage(DatabaseService databaseService)
    {
        InitializeComponent();
        BindingContext = new AddBookViewModel(databaseService);
    }
}