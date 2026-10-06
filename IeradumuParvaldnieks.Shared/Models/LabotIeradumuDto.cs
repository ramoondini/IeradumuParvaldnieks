using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieks.Shared.Models
{
    // DTO: lauki ieraduma rediģēšanai
    public class LabotIeradumuDto
    {
        public string Nosaukums { get; set; } = string.Empty;
        public string? Apraksts { get; set; }
        public AtkartosanasDto? Atkartosanas { get; set; }
    }
}
