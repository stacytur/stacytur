using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace kurs.Models
{
    public partial class Pokupatel
    {
        public Pokupatel()
        {
            Zakaz = new HashSet<Zakaz>();
        }

        [Key]
        [Column("Id_pokupatel")]
        public int IdPokupatel { get; set; }

        [DisplayName("ФИО Покупателя")]
        [Required(ErrorMessage = "Введите ФИО покупателя")]
        public string PokupatelFio { get; set; } = null!;

        [DisplayName("Телефон Покупателя")]
        [Required(ErrorMessage = "Введите телефон покупателя")]
        [Range(10000000000, 99999999999, ErrorMessage = "Телефон покупателя должен быть 11-значным числом")]
        public long PokupatelTelefon { get; set; }

        [DisplayName("Адрес доставки Покупателя")]
        [Required(ErrorMessage = "Введите адрес доставки покупателя")]

        public string PokupatelAdresDostavki { get; set; } = null!;

        [DisplayName("Заказы")]
        public virtual ICollection<Zakaz> Zakaz { get; set; }
    }
}












//[StringLength(40, ErrorMessage = "ФИО покупателя не должно превышать 40 символов")]







//[StringLength(50, ErrorMessage = "Адрес доставки не должен превышать 50 символов")]

