using System.ComponentModel.DataAnnotations;
using IeradumuParvaldnieks.Domain.Enums;

namespace IeradumuParvaldnieks.Web.Models
{
    public class IeradumaIzveidesModelis
    {
        [Required(ErrorMessage = "Ievadi ieraduma nosaukumu.")]
        [StringLength(100, ErrorMessage = "Nosaukums nedrīkst būt garāks par 100 simboliem.")]
        public string Nosaukums { get; set; } = "";

        public string? Apraksts { get; set; }

        public AtkartojumaBiezums Biezums { get; set; }
    }
}
