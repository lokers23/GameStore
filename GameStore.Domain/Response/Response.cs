using GameStore.Domain.Enums;

namespace GameStore.Domain.Response
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public HttpStatusCode Status { get; set; }
        public bool? HasNextPage { get; set; }
        public bool? HasPreviousPage { get; set; }
    }
}
