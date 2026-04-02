using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ProyectoFinal.Models
{
    [Table("books")]
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(20)]
        public string ISBN { get; set; }
        public int Year { get; set; }

        [MaxLength(50)]
        public string Genre { get; set; }
        public int Pages { get; set; }

        [MaxLength(500)]
        public string CoverUrl { get; set; }
        public bool IsRead { get; set; }
        public int Rating { get; set; } // 1-5 estrellas

        [MaxLength(1000)]
        public string Notes { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
