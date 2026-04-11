using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Models;

public class BookSearchResult
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ThumbnailUrl { get; set; }
}

public class BookDetail
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN {  get; set; }
    public int PublishedYear { get; set; }
    public int PageCount { get; set; }
    public string Description {  get; set; }
    public string CoverUrl { get; set; }
    public List<string> Categories { get; set; }

}
