using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.DTOs.Cart
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public List<CartItemDto> Items { get; set; } = [];
        public decimal TotalPrice { get; set; }
    }
}
