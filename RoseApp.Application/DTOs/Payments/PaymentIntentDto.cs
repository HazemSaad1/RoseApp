namespace RoseApp.Application.DTOs.Payments;

public class PaymentIntentDto
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "usd";
}