using ProyectoFinal.Models;
using ProyectoFinal.Services;
using SQLite;

namespace ProyectoFinal.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;
    private const string DatabaseFilename = "biblioteca.db3";

    public DatabaseService()
    {
#pragma warning disable CA1416
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Book>().Wait();
#pragma warning restore CA1416
    }

    // Get all books
    public Task<List<Book>> GetBooksAsync()
    {
        return _database.Table<Book>().ToListAsync();
    }

    // Get one book by ID
    public Task<Book> GetBookDetailAsync(int id)
    {
        return _database.Table<Book>()
                        .Where(b => b.Id == id)
                        .FirstOrDefaultAsync();
    }

    // Add or update a book
    public Task<int> SaveBookAsync(Book book)
    {
        if (book.Id != 0)
            return _database.UpdateAsync(book);
        else
            return _database.InsertAsync(book);
    }

    // Delete a book
    public Task<int> DeleteBookAsync(Book book)
    {
        return _database.DeleteAsync(book);
    }

    // Get statistics
    public async Task<(int total, int read, int pending, int totalPages)> GetStatsAsync()
    {
        var books = await GetBooksAsync();
        int total = books.Count;
        int read = books.Count(b => b.IsRead);
        int pending = total - read;
        int totalPages = books.Where(b => b.IsRead).Sum(b => b.Pages);
        return (total, read, pending, totalPages);
    }
}
