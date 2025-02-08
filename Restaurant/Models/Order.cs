﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    public class Order
    {

        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; }


        // one order belongs to one customer
        [ForeignKey("Customers")]

        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        //one order can have many order items
        public ICollection<OrderItem> OrderItems { get; set; }

    }

    public class OrderDto
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; }

        public string CustomerName { get; set; }

        public float TotalOrderPrice { get; set; }

    }


    public class AUOrderDto
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; }

        public int? CustomerId { get; set; }

    }


    public class OrderWithItemsDto
    {
        public int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string CustomerName { get; set; }
        public float TotalOrderPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

}
