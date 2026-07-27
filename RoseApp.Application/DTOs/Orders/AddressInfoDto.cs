using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.DTOs.Orders
{
    public class AddressInfoDto
    {
        public string Label { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
