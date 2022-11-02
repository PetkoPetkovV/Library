﻿using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();

    }
}

