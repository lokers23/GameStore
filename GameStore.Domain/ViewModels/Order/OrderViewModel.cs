using GameStore.Domain.Dto.Game;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Order;

public class OrderViewModel
{
    public List<GameCountDto>? GameCounts { get; set; }
} 