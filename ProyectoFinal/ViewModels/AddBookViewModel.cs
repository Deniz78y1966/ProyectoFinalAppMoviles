using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

#pragma warning disable CS0618
#pragma warning disable CS8618
#pragma warning disable CS8625
#pragma warning disable CS8612

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
        set { _title = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsFormValid)); OnPropertyChanged(nameof(FormStatus)); }
    }

    public string Author
    {
        get => _author;
        set { _author = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsFormValid)); OnPropertyChanged(nameof(FormStatus)); }
    }

    public string ISBN
    {
        get => _isbn;
        set
        {
            _isbn = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsISBNValid));
            OnPropertyChanged(nameof(ISBNError));
            OnPropertyChanged(nameof(IsFormValid));
            OnPropertyChanged(nameof(FormStatus));
        }
    }

    public string Year
    {
        get => _year;
        set { _year = value; OnPropertyChanged(); OnPropertyChanged(nameof(ParsedYear)); }
    }

    public string Pages
    {
        get => _pages;
        set { _pages = value; OnPropertyChanged(); OnPropertyChanged(nameof(ParsedPages)); }
    }

    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    // Calculated properties
    public bool IsISBNValid
    {
        get
        {
            if (string.IsNullOrWhiteSpace(ISBN))
                return true;
            var cleanISBN = ISBN.Replace("-", "").Replace(" ", "");
            return cleanISBN.Length == 10 || cleanISBN.Length == 13;
        }
    }

    public string ISBNError => IsISBNValid ? "" : "ISBN debe tener 10 o 13 dígitos";

    public bool IsFormValid =>
        !string.IsNullOrWhiteSpace(Title) &&
        !string.IsNullOrWhiteSpace(Author) &&
        IsISBNValid;

    public string FormStatus => !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Author)
        ? IsISBNValid
            ? "✓ Listo para guardar"
            : "⚠️ ISBN incorrecto"
        : "* Título y autor son obligatorios";

    public int ParsedYear => int.TryParse(Year, out int y) ? y : 0;
    public int ParsedPages => int.TryParse(Pages, out int p) ? p : 0;

    // Commands
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand LoadAsyncCommand { get; }

    public AddBookViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        SaveCommand = new RelayCommand(async () => await SaveBook());
        CancelCommand = new RelayCommand(async () => await Shell.Current.GoToAsync(".."));
        LoadAsyncCommand = new RelayCommand(async () => await LoadAsync());
    }

    public async Task LoadAsync()
    {
        Title = string.Empty;
        Author = string.Empty;
        ISBN = string.Empty;
        Year = string.Empty;
        Pages = string.Empty;
        Description = string.Empty;
        await Task.CompletedTask;
    }

    async Task SaveBook()
    {
        if (!IsFormValid)
        {
            await Shell.Current.DisplayAlert("Error",
                !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Author)
                    ? "ISBN incorrecto. Debe tener 10 o 13 dígitos"
                    : "Título y autor son obligatorios",
                "OK");
            return;
        }

        var book = new Book
        {
            Title = Title,
            Author = Author,
            ISBN = ISBN ?? "",
            Year = ParsedYear,
            Pages = ParsedPages,
            Description = Description ?? "",
            IsRead = false,
            Rating = 0,
            Notes = "",
            DateAdded = DateTime.Now
        };

        await _databaseService.SaveBookAsync(book);
        await Shell.Current.DisplayAlert("Éxito", $"'{book.Title}' agregado!", "OK");
        await Shell.Current.GoToAsync("..");
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

#pragma warning restore CS0618
#pragma warning restore CS8618
#pragma warning restore CS8625
#pragma warning restore CS8612