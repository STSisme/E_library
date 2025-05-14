using E_Library.Model;

namespace E_Library.Services.Interface
{
    public interface IOrderService
    {
        Order GetOrderById(Guid orderId);
        void UpdateOrder(Order order);
    }

}
