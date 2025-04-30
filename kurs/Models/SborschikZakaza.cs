using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using kurs.Models;

namespace kurs.Models
{
    public partial class SborschikZakaza
    {
        public SborschikZakaza()
        {
            Zakaz = new HashSet<Zakaz>();
        }

        [Key]
        [Column("Id_sborschik_zakaza")]
        public int IdSborschikZakaza { get; set; }

        [DisplayName("Телефон сборщика заказа")]
        [Required(ErrorMessage = "Введите телефон сборщика заказа")]
        [Range(10000000000, 99999999999, ErrorMessage = "Телефон сборщика заказа должен быть 11-значным числом")]
        public long SborschikZakazaTelefon { get; set; }

        [DisplayName("Имя сборщика заказа")]
        [Required(ErrorMessage = "Введите имя сборщика заказа")]

        public string SborschikZakazaNames { get; set; } = null!;

        [DisplayName("Заказы")]
        public virtual ICollection<Zakaz> Zakaz { get; set; }
    }
}















//[StringLength(100, ErrorMessage = "Имя сборщика заказа не должно превышать 100 символов")]
