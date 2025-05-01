using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static SmartBook.Helpers;

namespace SmartBook
{
    class BookService
    {
        private string filePath = Path.Combine("Saved", "library.json");
        public List<Book> Books { get; set; } = new List<Book>();

        public BookService()
        {
            LoadFromFile();
        }

        public void AddBook(Book book)
        {
            if (!ValidationService.IsValidISBN(book.ISBN))

            {
                Console.WriteLine("Invalid ISBN (must be 13 characters).");
                return;
            }

            if (ValidationService.IsDuplicate(Books, book.ISBN))
            {
                Console.WriteLine("Book with this ISBN already exists.");
                return;
            }
           
           

            Books.Add(book);
            Console.WriteLine("Book added successfully.");

            if (!string.IsNullOrEmpty(book.SourceLibrary))
            {
                SaveLibraryToFile(book.SourceLibrary);  
            } 
            
        }
        public void SaveLibraryToFile(string libraryName)
        {
            string folderPath = Helpers.GetSavedFolderPath();
            Directory.CreateDirectory(folderPath); 
            string filePath = Path.Combine(folderPath, $"{libraryName}.json");

            List<Book> existingBooks = new List<Book>();
            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                var deserialized = JsonSerializer.Deserialize<List<Book>>(existingJson);
                if (deserialized != null)
                    existingBooks = deserialized;
            }

           
            var newBooks = Books
                .Where(b => b.SourceLibrary == libraryName && !existingBooks.Any(e => e.ISBN == b.ISBN))
                .ToList();

            existingBooks.AddRange(newBooks);

            var json = JsonSerializer.Serialize(existingBooks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            Console.WriteLine($"Library '{libraryName}' saved to {Path.GetFullPath(filePath)}");
        }


        public void RemoveBook(string isbn)
        {
            var book = Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                Books.Remove(book);
                Console.WriteLine("Book removed successfully.");
            } else
            {
                Console.WriteLine("Book not found.");
            }
        }

        public void ListBooks()
        {
            Console.WriteLine("Current session list: " + Environment.NewLine);

            if (Books.Count == 0)
            {
                Console.WriteLine("No books available.");
            } else
            {
                foreach (var book in Books)
                {
                    string libraryName = string.IsNullOrEmpty(book.SourceLibrary) ? "None" : book.SourceLibrary;
                    Console.WriteLine($"{book} | Library: {libraryName}");
                }
            }
        }

        public IEnumerable<Book> GetSearchResults(string term)
        {
            return Books.Where(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                    b.Author.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                    b.ISBN.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        public void SearchBooks(string term)
        {
            var results = GetSearchResults(term);
            if (!results.Any())
            {
                Console.WriteLine("No books found.");
            }

            foreach (var book in results)
            {
                Console.WriteLine(book);
            }
        }

        public void ToggleAvailability(string isbn)
        {
            var book = Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                book.IsAvailable = !book.IsAvailable;
                Console.WriteLine($"Status updated to: {(book.IsAvailable ? "Available" : "Checked out")}");
            } else
            {
                Console.WriteLine("No book with matching ISBN found.");
            }
        }

        public void SaveToFile()
        {
            Console.Write("Enter a filename(no extension): ");
            string userInput = ReadInput();
            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Invalid filename.");
                return;
            }

            Console.WriteLine("Current session books:");
            for (int i = 0; i < Books.Count; i++)
            {
                var book = Books[i];
                string libraryName = string.IsNullOrEmpty(book.SourceLibrary) ? "None" : book.SourceLibrary;
                Console.WriteLine($"{i + 1}. {book} | Library: {libraryName}");
            }

            Console.Write("Enter the number(s) or title(s) of the book(s) to save (comma-separated, no space): ");
            string input = ReadInput();
            var selectedBooks = new List<Book>();
            var tokens = input.Split(',');

            foreach (var token in tokens)
            {
                var trimmed = token.Trim();
                if (int.TryParse(trimmed, out int index))
                {
                var book = Books[index - 1];
                    if (index >= 1 && index <= Books.Count)
                    { 
                if (!selectedBooks.Any(b => b.ISBN == book.ISBN))
                {
                    selectedBooks.Add(book);
                }
            }
        }
        else
        {
        var matches = Books.Where(b => b.Title.Equals(trimmed, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var match in matches)
            {
            if (!selectedBooks.Any(b => b.ISBN == match.ISBN))
            {
            selectedBooks.Add(match);
            }
}
}
}

            if (selectedBooks.Count == 0)
{
    Console.WriteLine("No valid books selected.");
    return;
}

            string rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            string savedFolderPath = Path.Combine(rootDirectory, "Saved");
            if (!Directory.Exists(savedFolderPath))
            {
                Directory.CreateDirectory(savedFolderPath);
            }
            string filePath = Path.Combine(savedFolderPath, userInput + ".json");

            List<Book> existingBooks = new List<Book>();
            if (File.Exists(filePath))
            {
                var existingJson = File.ReadAllText(filePath);
                var deserialized = JsonSerializer.Deserialize<List<Book>>(existingJson);
                if (deserialized != null)
                {
                    existingBooks = deserialized;
                }
            }
            foreach (var book in selectedBooks)
            {
                if (!existingBooks.Any(b => b.ISBN == book.ISBN))
                {
                    existingBooks.Add(book);
                }
            }
            var json = JsonSerializer.Serialize(existingBooks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Library saved to {Path.GetFullPath(filePath)}");

        }

        
        public void LoadFromFile()
        {
            if (!File.Exists(filePath)) return;

            var json = File.ReadAllText(filePath);
            var deserializedBooks = JsonSerializer.Deserialize<List<Book>>(json);

            if (deserializedBooks != null)
            {
                Books = deserializedBooks;
            }
            else
            {
                Books = new List<Book>();
            }
        }

        public void LoadFromSpecificFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            var json = File.ReadAllText(filePath);
            var deserializedBooks = JsonSerializer.Deserialize<List<Book>>(json);

            if (deserializedBooks != null)
            {
                foreach (var book in deserializedBooks)
                {
                    if (!Books.Any(b => b.ISBN == book.ISBN))
                    {
                        Books.Add(book);
                    }
                }
                Console.WriteLine($"Library loaded from {Path.GetFileName(filePath)}");
            } else
            {
                Console.WriteLine("Failed to load library.");
            }
        }

        //public void CheckoutBook(string isbn, User user)
        //{
        //    var book = Books.FirstOrDefault(b => b.ISBN == isbn);
        //    if (book != null && book.IsAvailable)
        //    {
        //        book.IsAvailable = false;
        //        book.CheckedOutBy = user;
        //        user.CheckedOutBooks.Add(book);
        //        Console.WriteLine($"Book '{book.Title}' checked out by {user.Name}");
        //    } else
        //    {
        //        Console.WriteLine("Book not available for checkout.");
        //    }
        //}

        //public void ReturnBook(string isbn, User user)
        //{
        //    var book = user.CheckedOutBooks.FirstOrDefault(b => b.ISBN == isbn);
        //    if (book != null)
        //    {
        //        book.IsAvailable = true;
        //        book.CheckedOutBy = null;
        //        user.CheckedOutBooks.Remove(book);
        //        Console.WriteLine($"Book '{book.Title}' returned by {user.Name}");
        //    } else
        //    {
        //        Console.WriteLine("This user hasn't checked out this book.");
        //    }
        //}
    }
}
