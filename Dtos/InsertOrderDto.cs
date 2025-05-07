namespace Elibrary.Dtos
{
    public class InsertOrderDto
    {
        public Guid User_Id { get; set; }
        public List<InsertOrderItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}
