namespace Elibrary.Dtos
{
    public class InsertCartDto
    {
        public Guid User_Id { get; set; }
        public Guid Book_Id { get; set; }
        public int Quantity { get; set; }
    }
}
