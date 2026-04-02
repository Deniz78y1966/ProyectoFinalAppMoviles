using ProyectoFinal.Models;
using ProyectoFinal.Service;

namespace ProyectoFinal.Views;

public partial class StatisticsPage : ContentPage
{
    public int TotalBooks { get; set; }
    public int TotalPages { get; set; }
    public string MostReadCategory { get; set; }
    public double AveragePages { get; set; }

#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
    public StatisticsPage()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
	{
		InitializeComponent();
        BindingContext = this;
	}
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadStatistics(); //esto para que vuelva a cargar una vez el sitio sea visitado. 
    }

    void LoadStatistics()
    {
        // TODO: en base a lo guardado, entonces se carga la informacion estadistica. Por ahora, se ponen valores de ejemplo.
        TotalBooks = 0;
        TotalPages = 0;
        MostReadCategory = "N/A";
        AveragePages = 0;

        OnPropertyChanged(nameof(TotalBooks));
        OnPropertyChanged(nameof(TotalPages));
        OnPropertyChanged(nameof(MostReadCategory));
        OnPropertyChanged(nameof(AveragePages));
    }
}