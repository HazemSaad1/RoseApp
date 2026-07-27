namespace RoseApp.Application.DTOs.Products;

public class ProductFilterDto
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 20;
    public string? Search { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? SubCategoryId { get; set; }
    public Guid? OccasionId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; } = "asc";
}