using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
namespace IeradumuParvaldnieks.Domain.TestData
{
    // Testa datu ģenerators: atgriež piemēra ieradumu sarakstu lietošanai konsolē vai testos
    public static class TestaDati
    {
        public static List<Ieradums> IzveidotIeradumus()
        {
            List<Ieradums> ieradumi = new List<Ieradums>();
            Ieradums MacibuIeradums = new Ieradums
            {
                Id = Guid.NewGuid(),
                LietotajaId = "user1",
                Nosaukums = "Mācīties .NET",
                Apraksts = "Mācīties .NET vismaz 1 stundu",
                Statuss = IeradumaStatuss.Aktivs,
                Izveidots = DateTime.Now,
                Atjauninats = DateTime.Now,
                Atkartosana = new Atkartosana
                {
                    Biezums = AtkartojumaBiezums.KatruDienu,
                    Intervals = 1
                }
            };

            Ieradums TreninaIeradums = new Ieradums
            {
                Id = Guid.NewGuid(),
                LietotajaId = "user2",
                Nosaukums = "Vingrināties",
                Apraksts = "Darīt vingrinājumus vismaz 30 minūtes.",
                Statuss = IeradumaStatuss.Aktivs,
                Izveidots = DateTime.Now,
                Atjauninats = DateTime.Now,
                Atkartosana = new Atkartosana
                {
                    Biezums = AtkartojumaBiezums.KatruNedelu,
                    Intervals = 1,
                    NedelasDienas = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
                }
            };

            Ieradums RelaxingIeradums = new Ieradums
            {
                Id = Guid.NewGuid(),
                LietotajaId = "user1",
                Nosaukums = "Relaksēties",
                Apraksts = "Ieguldīt laiku, lai atvienotos un atjaunotos.",
                Statuss = IeradumaStatuss.Arhivets,
                Izveidots = DateTime.Now,
                Atjauninats = DateTime.Now,
                Atkartosana = new Atkartosana
                {
                    Biezums = AtkartojumaBiezums.KatruDienu,
                    Intervals = 1
                }
            };

            MacibuIeradums.Izpildes.Add(new IeradumaIzpilde
            {
                    Id = Guid.NewGuid(),
                    IeradumaId = MacibuIeradums.Id,
                    Datums = DateTime.Today,
                    IrPabeigts = true,
                    Piezimes = "Izpildīts"
            });

            MacibuIeradums.Izpildes.Add(new IeradumaIzpilde
            {
                Id = Guid.NewGuid(),
                IeradumaId = MacibuIeradums.Id,
                Datums = DateTime.Today.AddDays(-1),
                IrPabeigts = true,
                Piezimes = "Izpildīts vakar"
            });

            TreninaIeradums.Izpildes.Add(new IeradumaIzpilde
             {
                    Id = Guid.NewGuid(),
                    IeradumaId = TreninaIeradums.Id,
                    Datums = DateTime.Today.AddDays(-4),
                    IrPabeigts = false,
                    Piezimes = "Nav izpildīts",
             });

            ieradumi.Add(MacibuIeradums);
            ieradumi.Add(TreninaIeradums);
            ieradumi.Add(RelaxingIeradums);

            return ieradumi;
        }
    }
}
