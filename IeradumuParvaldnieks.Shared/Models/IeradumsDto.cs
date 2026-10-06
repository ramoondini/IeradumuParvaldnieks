namespace IeradumuParvaldnieks.Shared.Models
{
    public class IeradumsDto
    {
        public Guid Id { get; set; }

        public string LietotajaId { get; set; } = string.Empty;

        public string Nosaukums { get; set; } = string.Empty;

        public string? Apraksts { get; set; }

        public string Statuss { get; set; } = string.Empty;

        public DateTime Izveidots { get; set; }

        public DateTime Atjauninats { get; set; }

        public AtkartosanasDto? Atkartosana { get; set; }

        public List<IeradumaIzpildeDto> Izpildes { get; set; } = new();
    }
}