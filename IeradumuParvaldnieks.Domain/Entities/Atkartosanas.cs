using IeradumuParvaldnieks.Domain.Enums;

namespace IeradumuParvaldnieks.Domain.Entities
{
    // Entītija: atkārtošanās konfigurācija ieradumam
    public class Atkartosana
    {
        public AtkartojumaBiezums Biezums { get; set; } // Ieraduma atkārtojuma biežums
        public int Intervals { get; set; } = 1;// Cik bieži atkārtojas
        public DayOfWeek[]? NedelasDienas { get; set; }//Dienas, kad atkārtojas
        public DateTime? BeiguDatums { get; set; }//Datums, kad atkārtošanās beidzas
    }
}
