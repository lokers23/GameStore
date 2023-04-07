using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.ViewModels.Order;

public class OrderViewModel
{
    [Required(ErrorMessage = "Введите дату оплаты")]
    //[StringLength(100, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 100")]
    public DateTime? PayOn { get; set; }
    
    [Required(ErrorMessage = "Введите стоимость")]
    [Range(0, 9999999999999999.99, ErrorMessage = "Стоимость не может быть меньше нуля и содержать больше 18 цифр")]
    public decimal? Amount { get; set; }
    
    [Required(ErrorMessage = "В заказе должны быть игры")]
    public List<Domain.Models.Game>? Games { get; set; }
}