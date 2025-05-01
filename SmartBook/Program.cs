namespace SmartBook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new BookService();
            var app = new LibraryApp(service);
            app.Run();
        }
    }
}
