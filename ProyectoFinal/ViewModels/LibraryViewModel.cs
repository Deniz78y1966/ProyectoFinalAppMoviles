using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProyectoFinal.ViewModels;

public class LibraryViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<Book> Books { get; set; } = new();

    // Calculated properties
    public int BooksCount => Books.Count;
    public int ReadCount => Books.Count(b => b.IsRead);
    public int PendingCount => Books.Count(b => !b.IsRead);
    public string ReadPercentage => BooksCount > 0
        ? $"{Math.Round((double)ReadCount / BooksCount * 100, 1)}% leído"
        : "0% leído";

    // Commands
    public ICommand LoadAsyncCommand { get; }
    public ICommand ViewDetailsCommand { get; }
    public ICommand MarkAsReadCommand { get; }
    public ICommand AddBookCommand { get; }

    public LibraryViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadAsyncCommand = new RelayCommand(async () => await LoadAsync());
        ViewDetailsCommand = new RelayCommand<Book>(async (book) => await ViewDetails(book));
        MarkAsReadCommand = new RelayCommand<Book>(async (book) => await MarkAsRead(book));
        AddBookCommand = new RelayCommand(async () => await Shell.Current.GoToAsync("addbook"));
    }

    // LoadAsync → loads initial data
    public async Task LoadAsync()
    {
        var books = await _databaseService.GetBooksAsync();
        Books.Clear();
        foreach (var book in books)
            Books.Add(book);

        // Notify calculated properties
        OnPropertyChanged(nameof(BooksCount));
        OnPropertyChanged(nameof(ReadCount));
        OnPropertyChanged(nameof(PendingCount));
        OnPropertyChanged(nameof(ReadPercentage));
    }

    async Task ViewDetails(Book book)
    {
        if (book != null)
            await Shell.Current.GoToAsync($"bookdetail?bookId={book.ISBN}");
    }

    async Task MarkAsRead(Book book)
    {
        if (book != null)
        {
            book.IsRead = true;
            await _databaseService.SaveBookAsync(book);
            await LoadAsync();
            await Shell.Current.DisplayAlert("Éxito", $"'{book.Title}' marcado como leído!", "OK");
        }
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}