namespace IeradumuParvaldnieks.Shared.Models
{
    public class AtkartosanasDto
    {
        public string Biezums { get; set; } = string.Empty;
        public int Intervals { get; set; } = 1;
        public DayOfWeek[]? NedelasDienas { get; set; }
        public DateTime? BeiguDatums { get; set; }
    }
}