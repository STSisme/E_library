namespace E_Library.Dtos
{
    public class InsertReviewDto
    {
        public Guid User_Id { get; set; }
        public Guid Book_Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
