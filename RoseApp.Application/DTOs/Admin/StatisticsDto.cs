namespace RoseApp.Application.DTOs.Admin;

public class StatisticsDto
{
    public int TotalUsers { get; set; }
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalBlogs { get; set; }
    public int TotalReviews { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }
}