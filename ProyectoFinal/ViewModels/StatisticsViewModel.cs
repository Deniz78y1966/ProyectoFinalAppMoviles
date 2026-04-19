using ProyectoFinal.Drawables;
using ProyectoFinal.Helpers;
using ProyectoFinal.Models;
using ProyectoFinal.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProyectoFinal.ViewModels;

public class StatisticsViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;
    private int _totalBooks;
    private int _totalPages;
    private string _mostReadCategory;
    private double _averagePages;
    private int _booksRead;
    private int _booksPending;

    public int TotalBooks
    {
        get => _totalBooks;
        set { _totalBooks = value; OnPropertyChanged(); OnPropertyChanged(nameof(ReadPercentage)); }
    }

    public int TotalPages
    {
        get => _totalPages;
        set { _totalPages = value; OnPropertyChanged(); }
    }

    public string MostReadCategory
    {
        get => _mostReadCategory;
        set { _mostReadCategory = value; OnPropertyChanged(); }
    }

    public double AveragePages
    {
        get => _averagePages;
        set { _averagePages = value; OnPropertyChanged(); }
    }

    public int BooksRead
    {
        get => _booksRead;
        set { _booksRead = value; OnPropertyChanged(); OnPropertyChanged(nameof(ReadPercentage)); }
    }

    public int BooksPending
    {
        get => _booksPending;
        set { _booksPending = value; OnPropertyChanged(); }
    }

    // Calculated properties
    public string ReadPercentage => TotalBooks > 0
        ? $"{Math.Round((double)BooksRead / TotalBooks * 100, 1)}% leído"
        : "0% leído";

    public string BooksCount => $"{TotalBooks} libros en total";

    public ObservableCollection<Book> Books { get; set; } = new();
    public StatisticsDrawable ChartDrawable { get; set; } = new();

    public ICommand LoadAsyncCommand { get; }

    public StatisticsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadAsyncCommand = new RelayCommand(async () => await LoadAsync());
    }

    public async Task LoadAsync()
    {
        var (total, read, pending, totalPages) = await _databaseService.GetStatsAsync();

        TotalBooks = total;
        BooksRead = read;
        BooksPending = pending;
        TotalPages = totalPages;
        AveragePages = total > 0 ? Math.Round((double)totalPages / total, 1) : 0;

        var books = await _databaseService.GetBooksAsync();

        Books.Clear();
        foreach (var book in books)
            Books.Add(book);

        MostReadCategory = books
            .Where(b => b.IsRead && !string.IsNullOrEmpty(b.Genre))
            .GroupBy(b => b.Genre)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault() ?? "N/A";

        ChartDrawable.BooksRead = BooksRead;
        ChartDrawable.BooksPending = BooksPending;

        OnPropertyChanged(nameof(ChartDrawable));
        OnPropertyChanged(nameof(BooksCount));
        OnPropertyChanged(nameof(ReadPercentage));
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}