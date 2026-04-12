using System.Collections.Generic;

namespace ProyectoFinal.Models;

public class BookDetail
{
#pragma warning disable 8618
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int PublishedYear { get; set; }
    public int PageCount { get; set; }
    public string Description { get; set; }
    public string CoverUrl { get; set; }
    public List<string> Categories { get; set; }
#pragma warning restore 8618
}
