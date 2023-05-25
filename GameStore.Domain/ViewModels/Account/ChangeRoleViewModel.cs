using System.ComponentModel.DataAnnotations;
using GameStore.Domain.Enums;

namespace GameStore.Domain.ViewModels.Account;

public class ChangeRoleViewModel
{
    [Required(ErrorMessage = "Укажите роль")]
    public AccessRole? Role { get; set; }
    
    [Required(ErrorMessage = "Укажите пользователя")]
    public int? UserId { get; set; }
}