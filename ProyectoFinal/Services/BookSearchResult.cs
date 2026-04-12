using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Models;

public class BookSearchResult
{
#pragma warning disable 8618
    public string Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ThumbnailUrl { get; set; }
#pragma warning restore 8618
}

