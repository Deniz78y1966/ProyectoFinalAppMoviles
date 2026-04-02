using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using ProyectoFinal.Models;

namespace ProyectoFinal.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        public DatabaseService()
        {
            // Ruta de la BD.
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "library.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            // Constructor con el CreateTableAsync
            _database.CreateTableAsync<Book>().Wait();
        }

        // 1. SaveBookAsync (Smart Insert/Update)
        public async Task<int> SaveBookAsync(Book book)
        {
            if (book.Id != 0)
            {
                return await _database.UpdateAsync(book);
            }
            else
            {
                book.DateAdded = DateTime.Now; // Set creation date for new books
                return await _database.InsertAsync(book);
            }
        }
    
        //2. GetBookAsync 
        public async Task<List<Book>> GetBookAsync(int id)
        {
            return await _database.Table<Book>().ToListAsync();
        }

        // 3. GetBooksByGenreAsync (Filter by genre)
        public async Task<List<Book>> GetBooksByGenreAsync(string genre)
        {
            return await _database.Table<Book>()
                                 .Where(b => b.Genre == genre)
                                 .ToListAsync();
        }

        // 4. GetReadBooksAsync (Only read books)
        public async Task<List<Book>> GetReadBooksAsync()
        {
            return await _database.Table<Book>()
                                 .Where(b => b.IsRead == true)
                                 .ToListAsync();
        }

        // 5. GetUnreadBooksAsync (Pending books)
        public async Task<List<Book>> GetUnreadBooksAsync()
        {
            return await _database.Table<Book>()
                                 .Where(b => b.IsRead == false)
                                 .ToListAsync();
        }

        // 6. DeleteBookAsync
        public async Task<int> DeleteBookAsync(Book book)
        {
            return await _database.DeleteAsync(book);
        }

        // 7. UpdateReadStatusAsync
        public async Task<int> UpdateReadStatusAsync(int id, bool isRead)
        {
            var book = await _database.Table<Book>().Where(b => b.Id == id).FirstOrDefaultAsync();
            if (book != null)
            {
                book.IsRead = isRead;
                return await _database.UpdateAsync(book);
            }
            return 0;
        }

        // 8. GetStatisticsAsync (Returns Totals, Read, Pending)
        public async Task<(int Total, int Read, int Unread)> GetStatisticsAsync()
        {
            var allBooks = await _database.Table<Book>().ToListAsync();

            int total = allBooks.Count;
            int read = allBooks.Count(b => b.IsRead);
            int unread = total - read;

            return (total, read, unread);
        }

    }
}
