namespace ITI.Gymunity.FP.Application.DTOs
{
    public record Pagination<T>(int PageIndex, int PageSize, int Count, IEnumerable<T> Data);
}
