using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Response;
using Microsoft.Extensions.Logging;

namespace GameStore.Domain.Helpers
{
    public static class Catcher
    {
        public static Response<T> CatchError<T, K>(Exception exception, ILogger<K> logger)
        {
            logger.LogError(exception, exception.Message);

            var response = new Response<T>()
            {
                Message = MessageError.ServerError,
                Status = HttpStatusCode.ServerError
            };

            return response;
        }
    }
}
