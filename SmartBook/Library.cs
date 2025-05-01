using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
  public class Library
    {
        public List<Book> Books { get; set; } = new List<Book>();

        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            Books.Remove(book);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return Books;
        }

        public Book? GetBookByISBN(string isbn)
        {
            return Books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public IEnumerable<Book> SearchBooks(string term)
        {
            return Books.Where(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                         b.Author.Contains(term, StringComparison.OrdinalIgnoreCase) || b.ISBN.Contains(term, StringComparison.OrdinalIgnoreCase));
        }
    }
}
