using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
   public class Book
    {
        public string Title { get; set; } 
        public string Author { get; set; }
        public string ISBN { get; set; } 
        public string Category { get; set; } 
        public string SourceLibrary { get; set; }
        public bool IsAvailable { get; set; } 

        public Book(string title, string author, string isbn, string category, string sourceLibrary)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Category = category;
            SourceLibrary = sourceLibrary;
            IsAvailable = true;
        }

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author},ISBN: {ISBN}, Category: {Category}, Status: {(IsAvailable ? "Available" : "Checked Out")}";
        }
    }
}
