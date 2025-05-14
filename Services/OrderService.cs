using E_Library.Data;
using E_Library.Model;
using E_Library.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order GetOrderById(Guid orderId)
        {
            return _context.Orders.Include(o => o.OrderItems)  // Assuming you want to include order items
                                   .FirstOrDefault(o => o.Order_Id == orderId);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }

}
