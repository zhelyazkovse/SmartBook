using Xunit;
using System.Collections.Generic;
using System.Linq;
using SmartBook;

namespace SmartBook.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void AddBook_ShouldAddBookToList()
        {
            var service = new BookService();
            service.Books = new List<Book>();

            var book = new Book("Test Title", "Test Author", "1234567890123", "Fiction", "Library");

            service.AddBook(book);

            Assert.Contains(book, service.Books);
        }

        [Fact]
        public void GetSearchResults_ShouldReturnMatchingBooks()
        {
            var service = new BookService();
            service.Books = new List<Book>
            {
                new Book("Memories of Ice", "Steven Erikson", "9780765300304", "Fantasy", "Library"),
                new Book("The Name of the Wind", "Patrick Rothfuss", "9780756404741", "Fantasy", "Library")
            };

            var results = service.GetSearchResults("Memories");

            Assert.Single(results);
            Assert.Equal("Memories of Ice", results.First().Title);
        }

        [Fact]
        public void GetSearchResults_ShouldReturnEmptyList_WhenNoMatchesFound()
        {
            var service = new BookService();
            service.Books = new List<Book>
            {
                new Book("Memories of Ice", "Steven Erikson", "9780765300304", "Fantasy", "Library"),
                new Book("The Name of the Wind", "Patrick Rothfuss", "9780756404741", "Fantasy", "Library")
            };

            var results = service.GetSearchResults("Nonexistent");

            Assert.Empty(results);
        }

        [Fact]
        public void RemoveBook_ShouldRemoveBookFromList()
        {
            var service = new BookService();
            var book = new Book("Test Title", "Test Author", "1234567890123", "Fiction", "Library");
            service.Books = new List<Book> { book };

            service.RemoveBook(book.ISBN);

            Assert.DoesNotContain(book, service.Books);
        }

    }
}
