using System;
using System.Collections.Generic;
using System.Text;

namespace IeradumuParvaldnieks.Domain.Entities
{
    // Entītija: ieraduma izpildes ieraksts
    public class IeradumaIzpilde
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Konkrētā ieraduma ieraksta identifikātors
        public Guid IeradumaId { get; set; } // Norāda kuram ieradumam ieraksts pieder
        public DateTime Datums { get; set; } //Datums uz kuru attiecas ieraduma izpilde
        public bool IrPabeigts { get; set; }// True ja ir izpildits u False ja nav
        public string? Piezimes { get; set; } // Neobligaata piezime
    }
}
