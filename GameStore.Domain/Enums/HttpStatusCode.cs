namespace GameStore.Domain.Enums
{
    public enum HttpStatusCode
    {
        Ok = 200,
        Created = 201,
        NoContent = 204,
        AuthorizeError = 401,
        ForbiddenError = 403,
        NotFound = 404,
        Conflict = 409,
        ValidationError = 422,
        ServerError = 500,
    }
}
