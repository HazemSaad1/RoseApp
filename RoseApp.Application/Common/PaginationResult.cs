namespace RoseApp.Application.Common
{
    public class PaginationResult<T>
    {
        public IReadOnlyList<T> Data { get; set; } = [];
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public static PaginationResult<T> Create(
            IReadOnlyList<T> data,
            int count,
            int pageNumber,
            int pageSize)
        {
            return new PaginationResult<T>
            {
                Data = data,
                TotalCount = count,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
        }
    }
}
