using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ProyectoFinal.Services;

public class BookApiService
{
    private readonly HttpClient httpClient;
    private const string API_KEY = "AIzaSyAw9L0GMDzKa4aqTNtVBDDhGHn4Jk9Fbjc";
    private const string API_URL = "https://www.googleapis.com/books/v1/volumes";
    public BookApiService()
    {
        httpClient = new HttpClient();
    }
    public async Task<List<BookSearchResult>> SearchBooksAsync(string query)
    {
        var url = $"{API_URL}?q={Uri.EscapeDataString(query)}&maxResults=20&key={API_KEY}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var root = JsonDocument.Parse(json).RootElement;

        var results = new List<BookSearchResult>();

        if (!root.TryGetProperty("items", out var items))
            return results; // No results found

        foreach (var item in items.EnumerateArray())
        {
            var info = item.GetProperty("volumeInfo");

            results.Add(new BookSearchResult
            {
#pragma warning disable CS8601
                Id = item.GetProperty("id").GetString(),
                Title = info.TryGetProperty("title", out var t) ? t.GetString() : "Unknown",
                Author = info.TryGetProperty("authors", out var a)
                                   ? string.Join(", ", a.EnumerateArray().Select(x => x.GetString()))
                                   : "Unknown",
                ThumbnailUrl = info.TryGetProperty("imageLinks", out var img) &&
                               img.TryGetProperty("thumbnail", out var thumb)
                                   ? thumb.GetString()
                                   : null
#pragma warning restore CS8601
            });        
        }
        return results;
    }

    public async Task<BookDetail> GetBookDetailAsync(string bookId)
    {
        var url = $"{API_URL}/{bookId}?key={API_KEY}";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var root = JsonDocument.Parse(json).RootElement;
        var info = root.GetProperty("volumeInfo");

        return new BookDetail
        {
#pragma warning disable CS8601
            Title = info.TryGetProperty("title", out var t) ? t.GetString() : "Unknown",
            Author = info.TryGetProperty("authors", out var a)
                          ? string.Join(", ", a.EnumerateArray().Select(x => x.GetString()))
                          : "Unknown",
            ISBN = info.TryGetProperty("industryIdentifiers", out var ids)
                        ? ids.EnumerateArray()
                        .Where(x => x.GetProperty("type").GetString() == "ISBN_13")
                        .Select(x => x.GetProperty("identifier").GetString())
                        .FirstOrDefault()
                   : null,
           PublishedYear = info.TryGetProperty("publishedDate", out var d) 
                && d.GetString() != null
                && d.GetString()!.Length >= 4
                 && int.TryParse(d.GetString()!.Substring(0, 4), out int year)
                     ? year
                     : 0,
            PageCount = info.TryGetProperty("pageCount", out var p) ? p.GetInt32() : 0,
            Description = info.TryGetProperty("description", out var desc) ? desc.GetString() : null,
            CoverUrl = info.TryGetProperty("imageLinks", out var img) &&
                            img.TryGetProperty("thumbnail", out var thumb)
                                ? thumb.GetString()
                                : null,
            Categories = info.TryGetProperty("categories", out var cats)
                             ? cats.EnumerateArray()
                                .Select(x => x.GetString())
                                .Where(x => x != null)
                                .Select(x => x!)
                                .ToList()
                             : new List<string>()
#pragma warning restore CS8601
        };  
    }
}