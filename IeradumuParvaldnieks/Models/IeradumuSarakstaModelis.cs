using IeradumuParvaldnieks.Domain.Entities;
namespace IeradumuParvaldnieks.Models
{
    public class IeradumsSarakstaModelis //paligmodelis ieradumu attelosanai saraksta
    {
        public Ieradums Ieradums { get; set; } // saglaba ieradumu objektu
        public string Nosaukums => Ieradums.Nosaukums;
        public string? Apraksts => Ieradums.Apraksts; //dati kurus parada saraksta
        public string Statuss => Ieradums.Statuss.ToString();

        public bool SodienIzpildits => Ieradums.IrPabeigts(DateTime.Today); //parbauda vai ieradums ir izpildits sodien

        public string IzpildesTeksts => SodienIzpildits ? "Izpildīts" : "Nav izpildīts"; //izpildes statuss
        public bool VarAtzimet => !SodienIzpildits; // vai drikst ieradumu atzimet ka sodien izpilditu
        public IeradumsSarakstaModelis(Ieradums ieradums) //sanem ieradumu un saglaba to modeli
        {
            Ieradums = ieradums;
        }
    }
}
