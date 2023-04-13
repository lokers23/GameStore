using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Order;

public class OrderViewModel
{
    public int UserId;
    public List<int>? GameIds { get; set; }
} 