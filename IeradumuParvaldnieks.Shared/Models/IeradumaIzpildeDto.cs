using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieks.Shared.Models
{
    // DTO: ieraduma izpildes ieraksta lauki
    public class IeradumaIzpildeDto
    {
        public Guid Id { get; set; } // Konkrētā ieraduma ieraksta identifikātors
        public Guid IeradumaId { get; set; } // Norāda kuram ieradumam ieraksts pieder
        public DateTime Datums { get; set; } //Datums uz kuru attiecas ieraduma izpilde
        public bool IrPabeigts { get; set; }// True ja ir izpildits u False ja nav
        public string? Piezimes { get; set; } // Neobligaata piezime
    }
}
