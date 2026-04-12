using ProyectoFinal.Services;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsViewModel _viewModel;

    public StatisticsPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _viewModel = new StatisticsViewModel(databaseService);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadStatistics();
        StatsChart.Invalidate();
    }
}