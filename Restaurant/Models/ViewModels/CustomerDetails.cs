using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models.ViewModels
{
    public class CustomerDetails
    {
        public CustomerDto Customer { get; set; }
        public List<OrderDto> Order { get; set; }

    }
}
