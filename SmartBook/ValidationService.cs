using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook
{
   static class ValidationService
    {
        public static bool IsValidText(string text)
        {
            return !string.IsNullOrWhiteSpace(text) && !text.Any(char.IsDigit);
        }
        public static bool IsValidISBN(string isbn)
        {
            return isbn.Length == 13;
        }
        public static bool IsDuplicate(List<Book> books, string isbn)
        {
            return books.Any(b => b.ISBN == isbn);
        }
    }
}
