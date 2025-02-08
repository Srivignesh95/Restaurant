using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required]
        public string MName { get; set; }

        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        // one menu item can be in many order items
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class MenuItemDto
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required]
        public string MName { get; set; }

        [Required]
        public float Price { get; set; }
    }

    public class AUMenuItemDto
    {
        [Key]
        public int MenuItemId { get; set; }

        [Required]
        public string MName { get; set; }

        public string Description { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
