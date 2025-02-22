using Restaurant.Models;

namespace Restaurant.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemDto>> ListOrderItems();
        Task<ServiceResponse> DeleteOrderItem(int id);
    }
}
