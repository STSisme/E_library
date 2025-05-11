using E_Library.Model;

namespace E_Library.ViewModels  
{
    public class BookListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public string SearchQuery { get; set; }
        public string SortOrder { get; set; }
        public string GenreFilter { get; set; }

        public IEnumerable<string> Genres { get; set; }
    }
}
