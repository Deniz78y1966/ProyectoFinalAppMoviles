using Microsoft.Extensions.Logging;
using ProyectoFinal.Views;
using ProyectoFinal.Services;


namespace ProyectoFinal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
#pragma warning disable CA1416
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<Services.DatabaseService>();
            builder.Services.AddTransient<Views.LibraryPage>();
            builder.Services.AddTransient<Views.SearchPage>();
            builder.Services.AddTransient<Views.StatisticsPage>();
            builder.Services.AddTransient<Views.AddBookPage>();
            builder.Services.AddTransient<Views.BookDetailPage>();
            builder.Services.AddSingleton<Services.BookApiService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
#pragma warning restore CA1416
    }
}
