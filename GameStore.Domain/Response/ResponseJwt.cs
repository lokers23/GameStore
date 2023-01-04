using GameStore.Domain.Enums;

namespace GameStore.Domain.Response
{
    public class ResponseJwt
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
