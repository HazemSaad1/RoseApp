using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Domain.Entities.Commerce
{
    public class OrderItem : BaseEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
