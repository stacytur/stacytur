

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace kurs.Models
{
    public partial class Dostavka
    {
        public Dostavka()
        {
            Zakaz = new HashSet<Zakaz>();
            DataPolucheniya = DateTime.UtcNow.Date;
            OjdaemayaData = DateTime.UtcNow.Date;
        }

        [Key]
        [Column("Id_dostavka")]
        public int IdDostavka { get; set; }

        [DisplayName("Дата получения")]
        public DateTime DataPolucheniya { get; set; }

        [DisplayName("Ожидаемая дата")]
        public DateTime OjdaemayaData { get; set; }

        [DisplayName("Статус доставки")]
        public string? StatusDostavki { get; set; }

        [DisplayName("Заказы")]
        public virtual ICollection<Zakaz> Zakaz { get; set; }
    }
}


















//[StringLength(20, ErrorMessage = "Статус доставки не должен превышать 20 символов")]