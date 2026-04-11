namespace ProyectoFinal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Registro de rutas para navegación entre páginas principales.
#pragma warning disable CA1416
            Routing.RegisterRoute("bookdetail", typeof(Views.BookDetailPage));
            Routing.RegisterRoute("addbook", typeof(Views.AddBookPage));
#pragma warning restore CA1416
        }
    }
}
