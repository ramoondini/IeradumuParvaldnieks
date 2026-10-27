using IeradumuParvaldnieks.Domain.Entities;

namespace IeradumuParvaldnieks.Api.Models
{

    public class IzveidotIeradumuPieprasijumu
    {
        public string LietotajaId { get; set; } = "";
        public string Nosaukums { get; set; } = "";
        public string? Apraksts { get; set; }
        public Atkartosana? Atkartosana { get; set; }
    }
}