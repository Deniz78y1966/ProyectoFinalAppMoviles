using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProyectoFinal.ViewModels;

public class BookDetailViewModel : INotifyPropertyChanged
{
    private readonly BookApiService _bookApiService;
    private readonly DatabaseService _databaseService;
    private BookDetail _currentBook;
    private int _rating;

    public BookDetail CurrentBook
    {
        get => _currentBook;
        set { _currentBook = value; OnPropertyChanged(); }
    }

    public int Rating
    {
        get => _rating;
        set { _rating = value; OnPropertyChanged(); }
    }

    // Commands
    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand BackCommand { get; }

    public BookDetailViewModel(BookApiService bookApiService, DatabaseService databaseService)
    {
        _bookApiService = bookApiService;
        _databaseService = databaseService;
        SaveCommand = new RelayCommand(async () => await SaveBook());
        DeleteCommand = new RelayCommand(async () => await DeleteBook());
        BackCommand = new RelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }

    public async Task LoadBook(string bookId)
    {
        try
        {
            CurrentBook = await _bookApiService.GetBookDetailAsync(bookId);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo cargar el libro: {ex.Message}", "OK");
        }
    }

    async Task SaveBook()
    {
        if (CurrentBook == null) return;

        var book = new Book
        {
            Title = CurrentBook.Title ?? "Sin título",
            Author = CurrentBook.Author ?? "Sin autor",
            ISBN = CurrentBook.ISBN ?? "",
            Year = CurrentBook.PublishedYear,
            Pages = CurrentBook.PageCount,
            Description = CurrentBook.Description ?? "",
            CoverUrl = CurrentBook.CoverUrl ?? "",
            Genre = CurrentBook.Categories?.FirstOrDefault() ?? "Sin categoría",
            Rating = _rating,
            IsRead = false,
            Notes = "",
            DateAdded = DateTime.Now
        };

        await _databaseService.SaveBookAsync(book);
        await Shell.Current.DisplayAlert("Éxito", $"'{book.Title}' agregado a tu biblioteca!", "OK");
        await Shell.Current.GoToAsync("..");
    }

    async Task DeleteBook()
    {
        bool confirm = await Shell.Current.DisplayAlert("Eliminar",
            "¿Estás seguro que quieres eliminar este libro?", "Sí", "No");
        if (confirm)
            await Shell.Current.GoToAsync("..");
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}