using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using kurs.Models;
using Microsoft.EntityFrameworkCore;

namespace kurs.Models
{
    public class Zakaz
    {
        public Zakaz()
        {
            Tovar = new HashSet<Tovar>();
            ZakazData = DateTime.UtcNow;
        }

        [Key]
        [Column("id_zakaz")]
        public int IdZakaz { get; set; }

        
        [DisplayName("Дата заказа")]
        [Required(ErrorMessage = "Введите дату заказа")]
        [Column("zakaz_data")]
        public DateTime ZakazData { get; set; } = DateTime.UtcNow;

        [DisplayName("Сумма заказа")]
        [Required(ErrorMessage = "Введите сумму заказа")]
        [Column("zakaz_summa")]
        [Precision(5)]
        public /*decimal*/ int ZakazSumma { get; set; }

        [DisplayName("ID покупателя")]
        [Required(ErrorMessage = "Введите ID покупателя")]
        [Column("id_pokupatel")]
        public int IdPokupatel { get; set; }

        [DisplayName("ID сборщика заказа")]
        [Required(ErrorMessage = "Введите ID сборщика заказа")]
        [Column("id_sborschik_zakaza")]
        public int IdSborschikZakaza { get; set; }

        [DisplayName("ID доставки")]
        [Required(ErrorMessage = "Введите ID доставки")]
        [Column("id_dostavka")]
        public int IdDostavka { get; set; }

        [ForeignKey("IdPokupatel")]
        [ValidateNever]
        public virtual Pokupatel IdPokupatelNavigation { get; set; }

        [ForeignKey("IdSborschikZakaza")]
        [ValidateNever]
        public virtual SborschikZakaza IdSborschikZakazaNavigation { get; set; }

        [ForeignKey("IdDostavka")]
        [ValidateNever]
        public virtual Dostavka IdDostavkaNavigation { get; set; }

        [DisplayName("Товар")]
        public virtual ICollection<Tovar> Tovar { get; set; }
    }
}






//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations.Schema;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
//using kurs.Models;

//namespace kurs.Models
//{
//    public class Zakaz
//    {
//        public Zakaz()
//        {
//            Tovar = new HashSet<Tovar>();
//        }

//        [Key]
//        [Column("Id_zakaz")]
//        public int IdZakaz { get; set; }

//        [DisplayName("Дата заказа")]
//        public DateOnly? ZakazData { get; set; }

//        [DisplayName("Сумма заказа")]
//        [Range(0, 99999.99, ErrorMessage = "Сумма заказа должна быть в диапазоне от 0 до 99999.99")]
//        public decimal? ZakazSumma { get; set; }

//        [DisplayName("ID покупателя")]
//        public int? IdPokupatel { get; set; }

//        [DisplayName("ID сборщика заказа")]
//        public int? IdSborschikZakaza { get; set; }

//        [DisplayName("ID доставки")]
//        public int? IdDostavka { get; set; }

//        [ForeignKey("IdDostavka")]
//        [DisplayName("Доставка")]
//        public virtual Dostavka? IdDostavkaNavigation { get; set; }

//        [ForeignKey("IdPokupatel")]
//        [DisplayName("Покупатель")]
//        public virtual Pokupatel? IdPokupatelNavigation { get; set; }

//        [ForeignKey("IdSborschikZakaza")]
//        [DisplayName("Сборщик заказа")]
//        public virtual SborschikZakaza? IdSborschikZakazaNavigation { get; set; }

//        [DisplayName("Товары")]
//        public virtual ICollection<Tovar> Tovar { get; set; }
//    }
//}
