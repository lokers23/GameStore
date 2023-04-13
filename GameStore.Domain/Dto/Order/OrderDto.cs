using GameStore.Domain.Dto.Key;
using GameStore.Domain.Dto.User;
using GameStore.Domain.Models;

namespace GameStore.Domain.Dto.Order;

public class OrderDto
{
    public int? Id { get; set; }
    public DateTime? PayOn { get; set; }
    public decimal? Amount { get; set; }
    public UserShortDto? User { get; set; }
    public List<KeyDto> Keys { get; set; }
}