using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static SmartBook.Helpers;

namespace SmartBook
{
    class LibraryApp(BookService bookService)
    {
        private readonly BookService bookService = bookService;


        public void Run()
        {
            while (true)
            {
                Console.WriteLine(Environment.NewLine + "Smartbook menu: ");
                Console.WriteLine("1. Add a book");
                Console.WriteLine("2. Remove a book");
                Console.WriteLine("3. List books");
                Console.WriteLine("4. Search for a book");
                Console.WriteLine("5. Toggle availability (available/checked out");
                Console.WriteLine("6. Add book(s) to library");
                Console.WriteLine("7. Load saved library");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");
                string choice = ReadInput();

                switch (choice)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        RemoveBook();
                        break;
                    case "3":
                        bookService.ListBooks();
                        break;
                    case "4":
                        SearchBook();
                        break;
                    case "5":
                        ChangeAvailability();
                        break;
                    case "6":
                        bookService.SaveToFile();
                        break;
                    case "7":
                        LoadSavedLibrary();
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void AddBook()
        {
            string title, author, isbn, category, sourceLibrary;
            Console.Write("Title (or 0 to exit): ");
            title = ReadInput();
            if (title == "0")
                return;
            while (true)
            {
                Console.Write("Author (or 0 to exit): ");
                author = ReadInput();
                if (author == "0")
                    return;

                if (ValidationService.IsValidText(author))
                    break;
                Console.WriteLine("Invalid Author (cannot be empty or include numbers");
            }

            while (true)
            {
                Console.Write("ISBN(13 characters or 0 to exit): ");
                isbn = ReadInput();
                if (isbn == "0")
                    return;
                if (ValidationService.IsValidISBN(isbn) && !ValidationService.IsDuplicate(bookService.Books, isbn))
                    break;
                Console.WriteLine("Invalid ISBN (must be 13 characters and unique).");
            }
            while (true)
            {
                Console.Write("Category (or 0 to exit): ");
                category = ReadInput();
                if (category == "0")
                    return;
                if (ValidationService.IsValidText(category))
                    break;
                Console.WriteLine("Invalid Category (cannot be empty or include numbers");
            }
            Console.Write("Library (leave empty if no library is to be assigned or enter 0 to exit): ");
            sourceLibrary = ReadInput();
            if (sourceLibrary == "0")
                return;
            if (string.IsNullOrWhiteSpace(sourceLibrary))
                sourceLibrary = "Unassigned";
            var book = new Book(title, author, isbn, category, sourceLibrary);
            bookService.AddBook(book);

        }

        private void RemoveBook()
{
    while (true)
    {
        Console.Write("Enter ISBN of the book to remove (or 0 to exit): ");
        string input = ReadInput();
        if (input == "0")
            return;

        var book = bookService.Books.FirstOrDefault(b => b.ISBN == input);
        if (book != null)
        {
            bookService.RemoveBook(book.ISBN);
            Console.WriteLine($"Book with ISBN {input} removed.");
            return;
        }
        else
        {
            Console.WriteLine("No book found with that ISBN.");
        }
    }
}
        private void SearchBook()
        {
            while (true)
            { 
            Console.Write("Enter title, author or ISBN to search (or 0 to exit): ");
            string term = ReadInput();
                if (term == "0")
                    return;
                bookService.SearchBooks(term);
            
            }
        }

        private void ChangeAvailability()
        {
            while (true)
            {
            Console.Write("Enter the ISBN of the book (or 0 to exit): ");
                string isbn = ReadInput();
                if (isbn == "0")
                    return;
        bookService.ToggleAvailability(isbn);

            }    
        }

        private void LoadSavedLibrary()
        {

            var savedFolderPath = GetSavedFolderPath();

            if (!Directory.Exists(savedFolderPath))
            {
                Console.WriteLine("No saved libraries available.");
                return;
            }

            var savedFiles = Directory.GetFiles(savedFolderPath, "*.json").ToList();
            
            if (savedFiles.Count == 0)
            {
                Console.WriteLine("No saved library files found.");
                return;
            }

            Console.WriteLine("Available saved libraries: ");
            var fileNames = savedFiles.Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
            for (int i = 0; i < fileNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {fileNames[i]}");
            }

            Console.Write("Select a library to load: ");
            string userInput = ReadInput();

            if (int.TryParse(userInput, out int index) && index > 0 && index <= savedFiles.Count)
            {
                string selectedFile = savedFiles[index - 1];
                var json = File.ReadAllText(selectedFile);
                var deserializedBooks = JsonSerializer.Deserialize<List<Book>>(json);

                if (deserializedBooks != null)
                {
                    string libraryName = Path.GetFileNameWithoutExtension(selectedFile);

                    foreach (var book in deserializedBooks)
                    {
                        book.SourceLibrary = libraryName;
                    }
                    bookService.Books = deserializedBooks;
                    Console.WriteLine($"Library loaded from {selectedFile}");

                } else
                {
                    Console.WriteLine("Failed to load library from the selected file");
                }
            }
            else
            { 
                Console.WriteLine("Invalid choice.");
            }
        }
    }
}
