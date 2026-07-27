using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Domain.Enums
{
    public enum OrderStatus
    {
        Pending=1,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }
}
