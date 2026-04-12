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

    // Commands
    public ICommand LoadBooksCommand { get; }
    public ICommand ViewDetailsCommand { get; }
    public ICommand MarkAsReadCommand { get; }
    public ICommand AddBookCommand { get; }

    public LibraryViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadBooksCommand = new RelayCommand(async () => await LoadBooks());
        ViewDetailsCommand = new RelayCommand<Book>(async (book) => await ViewDetails(book));
        MarkAsReadCommand = new RelayCommand<Book>(async (book) => await MarkAsRead(book));
        AddBookCommand = new RelayCommand(async () => await Shell.Current.GoToAsync("addbook"));
    }

    public async Task LoadBooks()
    {
        var books = await _databaseService.GetBooksAsync();
        Books.Clear();
        foreach (var book in books)
            Books.Add(book);
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
            await LoadBooks();
            await Shell.Current.DisplayAlert("Éxito", $"'{book.Title}' marcado como leído!", "OK");
        }
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}