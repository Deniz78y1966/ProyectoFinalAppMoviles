using Microsoft.Extensions.Logging;
using ProyectoFinal.Services;


namespace ProyectoFinal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<Views.LibraryPage>();
            builder.Services.AddTransient<Views.SearchPage>();
            builder.Services.AddTransient<Views.StatisticsPage>();
            builder.Services.AddTransient<Views.AddBookPage>();
            builder.Services.AddTransient<Views.BookDetailPage>();
            builder.Services.AddSingleton<Service.BookApiService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
