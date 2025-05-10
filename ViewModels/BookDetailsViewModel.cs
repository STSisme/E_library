using E_Library.Entities;
using E_Library.Model;

namespace E_Library.ViewModels  
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
