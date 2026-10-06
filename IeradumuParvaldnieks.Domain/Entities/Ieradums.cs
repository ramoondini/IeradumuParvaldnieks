using IeradumuParvaldnieks.Domain.Enums;

namespace IeradumuParvaldnieks.Domain.Entities
{
    // Entītija: ieraduma pamatinformācija
    public class Ieradums
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LietotajaId { get; set; } = string.Empty;
        public string Nosaukums { get; set; } = string.Empty;
        public string? Apraksts { get; set; }
        public IeradumaStatuss Statuss { get; set; }

        public DateTime Izveidots { get; set; } = DateTime.Now;
        public DateTime Atjauninats { get; set; } = DateTime.Now;

        public Atkartosana? Atkartosana { get; set; }

        public List<IeradumaIzpilde> Izpildes { get; set; } = new();


        // Pārbauda, vai ieradums konkrētajā datumā ir izpildīts
        public bool IrPabeigts(DateTime datums)
        {
            foreach (IeradumaIzpilde izpilde in Izpildes)
            {
                if (izpilde.Datums.Date == datums.Date &&
                    izpilde.IrPabeigts)
                {
                    return true;
                }
            }

            return false;
        }


        // Atzīmē ieradumu kā izpildītu
        public IeradumaIzpilde AtzimetKaIzpilditu(
            DateTime datums,
            string? piezimes = null)
        {
            if (Statuss == IeradumaStatuss.Arhivets)
            {
                throw new InvalidOperationException(
                    "Arhivētu ieradumu nevar atzīmēt kā izpildītu.");
            }

            if (IrPabeigts(datums))
            {
                throw new InvalidOperationException(
                    "Ieradums šajā datumā jau ir atzīmēts kā izpildīts.");
            }

            IeradumaIzpilde izpilde = new IeradumaIzpilde
            {
                Id = Guid.NewGuid(),
                IeradumaId = Id,
                Datums = datums.Date,
                IrPabeigts = true,
                Piezimes = piezimes
            };

            Izpildes.Add(izpilde);

            Atjauninats = DateTime.Now;

            return izpilde;
        }
    }
}