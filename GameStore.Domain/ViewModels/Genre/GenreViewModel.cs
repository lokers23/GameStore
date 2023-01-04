using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.ViewModels.Genre
{
    public class GenreViewModel
    {
        [Required(ErrorMessage = "Введите название")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина жанра должна быть от 1 до 50 символов")]
        public string? Name { get; set; }
    }
}
