﻿using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string Phone { get; set; }

        //one customer can have many orders
        public ICollection<Order> Orders { get; set; }


    }


    public class CustomerDto
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateOnly? LastOrderDate { get; set; }

        public float LastOrderPrice { get; set; }
    }

    public class AUCustomerDto
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string Phone { get; set; }
    }
}
