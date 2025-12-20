namespace ITI.Gymunity.FP.Infrastructure.DTOs
{
    public record Pagination<T>(int PageIndex, int PageSize, int Count, IEnumerable<T> Data);
}
