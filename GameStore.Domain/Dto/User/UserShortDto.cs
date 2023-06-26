using GameStore.Domain.Enums;

namespace GameStore.Domain.Dto.User;

public class UserShortDto
{
    public int? Id { get; set; }
    public string? Login { get; set; }
    public string? Mail { get; set; }
    public decimal? Balance { get; set; }
    //
    public AccessRole? Role { get; set; }
}