using RoseApp.Domain.Abstractions;
using RoseApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Domain.Entities.Commerce
{
    public class Payment : BaseEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string StripePaymentIntentId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}
