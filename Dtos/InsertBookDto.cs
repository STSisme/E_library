namespace E_Library.Dtos
{
    public class InsertBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}

