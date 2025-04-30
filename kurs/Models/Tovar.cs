using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using kurs.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace kurs.Models
{
    public class Tovar
    {

        [Key]
        [Column("Id_tovar")]
        public int IdTovar { get; set; }

        [DisplayName("Цена")]
        [Range(0, 9999999999.99, ErrorMessage = "Цена должна быть в диапазоне от 0 до 9999999999.99")]
        public decimal? Cena { get; set; }

        [DisplayName("Количество")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество должно быть неотрицательным числом")]
        public int? Kolichestvo { get; set; }

        [DisplayName("Наименование товара")]
        [StringLength(100, ErrorMessage = "Наименование товара не должно превышать 100 символов")]
        public string? TovarNaimenovanie { get; set; }

        [DisplayName("IDZakaz")]
        [Required(ErrorMessage = "Введите ID заказа")]
        [Column("id_zakaz")]
        public int IdZakaz { get; set; }

        [ForeignKey("IdZakaz")]
        [ValidateNever]
        public virtual Zakaz IdZakazNavigation { get; set; }
    }
}
