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
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Количество символов должно быть от 1 до 50")]
        public string? Name { get; set; }
    }
}
