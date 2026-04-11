using ProyectoFinal.Models;
using ProyectoFinal.Services;

namespace ProyectoFinal.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly DatabaseService _databaseService;
    public int TotalBooks { get; set; }
    public int TotalPages { get; set; }
    public string MostReadCategory { get; set; }
    public double AveragePages { get; set; }
    public int BooksRead {  get; set; }
    public int BooksPending { get; set; }
    public BooksChartDrawable ChartDrawable { get; set; } = new();

#pragma warning disable CS8618
    public StatisticsPage(DatabaseService databaseService)
	{
		InitializeComponent();
        _databaseService = databaseService;
        BindingContext = this;
	}
#pragma warning restore CS8618    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadStatistics(); //esto para que vuelva a cargar una vez el sitio sea visitado. 
    }

    async Task LoadStatistics()
    {
        var (total, read, pending, totalPages) = await _databaseService.GetStatsAsync();

        TotalBooks = total;
        BooksRead = read;
        BooksPending = pending;
        TotalPages = totalPages;
        AveragePages = total > 0 ? Math.Round((double)totalPages / total, 1) : 0;

        //Mayor lectura
        var books = await _databaseService.GetBooksAsync();
        MostReadCategory = books
            .Where(b => b.IsRead && !string.IsNullOrEmpty(b.Genre))
            .GroupBy(b => b.Genre)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault() ?? "N/A";

        //para actualizar el chart
        ChartDrawable.BooksRead = BooksRead;
        ChartDrawable.BooksPending = BooksPending;

        OnPropertyChanged(nameof(TotalBooks));
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(MostReadCategory));
        OnPropertyChanged(nameof(AveragePages));
        OnPropertyChanged(nameof(BooksRead));
        OnPropertyChanged(nameof(BooksPending));

        StatsChart.Invalidate();
    }
}