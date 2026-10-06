using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieks.Shared.Models
{
    // DTO: nepieciešamie lauki ieraduma izveidei
    public class IzveidotIeradumuDto
    {
        public string LietotajaId { get; set; } = string.Empty;
        public string Nosaukums { get; set; } = string.Empty;
        public string? Apraksts { get; set; }
        public AtkartosanasDto? Atkartosanas { get; set; }
    }
}
