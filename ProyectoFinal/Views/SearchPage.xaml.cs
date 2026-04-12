using ProyectoFinal.Models;
using ProyectoFinal.Services;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(BookApiService bookApiService)
    {
        InitializeComponent();
        BindingContext = new SearchViewModel(bookApiService);
    }
}