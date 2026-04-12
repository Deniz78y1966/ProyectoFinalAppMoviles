using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProyectoFinal.ViewModels;

public class AddBookViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;
    private string _title;
    private string _author;
    private string _isbn;
    private string _year;
    private string _pages;
    private string _description;

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    public string Author
    {
        get => _author;
        set { _author = value; OnPropertyChanged(); }
    }

    public string ISBN
    {
        get => _isbn;
        set { _isbn = value; OnPropertyChanged(); }
    }

    public string Year
    {
        get => _year;
        set { _year = value; OnPropertyChanged(); }
    }

    public string Pages
    {
        get => _pages;
        set { _pages = value; OnPropertyChanged(); }
    }

    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    // Commands
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddBookViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        SaveCommand = new RelayCommand(async () => await SaveBook());
        CancelCommand = new RelayCommand(async () => await Shell.Current.GoToAsync(".."));
    }

    async Task SaveBook()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Author))
        {
#pragma warning disable CS0618
            await Shell.Current.DisplayAlert("Error", "Título y autor son obligatorios", "OK");
#pragma warning restore CS0618
            return;
        }

        var book = new Book
        {
            Title = Title,
            Author = Author,
            ISBN = ISBN ?? "",
            Year = int.TryParse(Year, out int year) ? year : 0,
            Pages = int.TryParse(Pages, out int pages) ? pages : 0,
            Description = Description ?? "",
            IsRead = false,
            Rating = 0,
            Notes = "",
            DateAdded = DateTime.Now
        };

        await _databaseService.SaveBookAsync(book);
#pragma warning disable CS0618
        await Shell.Current.DisplayAlert("Éxito", $"'{book.Title}' agregado!", "OK");
#pragma warning restore CS0618
        await Shell.Current.GoToAsync("..");
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}