namespace Elibrary.Dtos
{
    public class InsertOrderItemDto
    {
        public Guid Book_Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
