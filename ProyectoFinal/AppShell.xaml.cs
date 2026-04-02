namespace ProyectoFinal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Registro de rutas para navegación entre páginas principales.
            Routing.RegisterRoute("bookdetail", typeof(Views.BookDetailPage));
            Routing.RegisterRoute("addbook", typeof(Views.AddBookPage));
        }
    }
}
