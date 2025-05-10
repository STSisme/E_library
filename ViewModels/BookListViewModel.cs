using E_Library.Model;

namespace E_Library.ViewModels  
{
    public class BookListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
