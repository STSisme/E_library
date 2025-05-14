using E_Library.Model;
using System.Collections.Generic;

namespace E_Library.ViewModels
{
    public class HomePageViewModel
    {
        public List<string> Genres { get; set; }
        public Dictionary<string, List<Book>> CategorizedBooks { get; set; }
    }

}
