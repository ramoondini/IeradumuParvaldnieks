using IeradumuParvaldnieks.Application.Interfaces;
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.Enums;
using System.Security.Cryptography.X509Certificates;

namespace IeradumuParvaldnieks.Application.Services
{

    public class IeradumuAppicationService //Serviss ar galvenajām ieradumu darbībām
    {
        private readonly IIeradumuRepository repository;

        public IeradumuAppicationService(IIeradumuRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Ieradums>> IegutVisuAsync(string lietotajaid) // Iegūst konkrēta lietotāja ieradumus
        {
            List<Ieradums> ieradumi = await repository.IegutVisuAsync();

            return ieradumi.Where(i => i.LietotajaId == lietotajaid).ToList();
        }

        public async Task<Ieradums> IegutPecIdAsync(Guid id) // Atrod ieradumu pēc ID
        {
            Ieradums? ieradums = await repository.IegutPecIdAsync(id);

            if (ieradums == null)
            
                throw new KeyNotFoundException("Ieradums nav atrasts.");

                return ieradums;

        }

        public async Task<Ieradums> IzveidotAsync(string lietotajaId, string nosaukums, string? apraksts, Atkartosana? atkartosana)
        {//Izveido jaunu ieradumu

            ParbauditNosaukumu(nosaukums);

            if (string.IsNullOrWhiteSpace(lietotajaId))
                throw new ArgumentException("Lietotāja ID nedrīkst būt tukšs");

            Ieradums ieradums = new Ieradums
            {
                Id = Guid.NewGuid(),
                LietotajaId = lietotajaId,
                Nosaukums = nosaukums.Trim(),
                Apraksts = apraksts,
                Statuss = IeradumaStatuss.Aktivs,
                Izveidots = DateTime.Now,
                Atjauninats = DateTime.Now,
                Atkartosana = atkartosana
            };

            await repository.PievienotAsync(ieradums);

            return ieradums;
        }

        public async Task<Ieradums> LabotAsync(Guid id, string nosaukums, string? apraksts, Atkartosana? atkartosana) // Labo esošu ieradumu
        {
            ParbauditNosaukumu(nosaukums);

            Ieradums ieradums = await IegutPecIdAsync(id);

            ieradums.Nosaukums = nosaukums.Trim();
            ieradums.Apraksts = apraksts;
            ieradums.Atjauninats = DateTime.Now;
            ieradums.Atkartosana = atkartosana;

            await repository.AtjauninatAsync(ieradums);

            return ieradums;
        }

        public async Task ArhivetAsync(Guid id) //Arhivē ieradumu
        {
            Ieradums ieradums = await IegutPecIdAsync(id);

            ieradums.Statuss = IeradumaStatuss.Arhivets;
            ieradums.Atjauninats = DateTime.Now;

            await repository.AtjauninatAsync(ieradums);
        }

        public async Task AtjaunotAsync(Guid id) //Atjaunot arhivētu ieradumu
        {
            Ieradums ieradums = await IegutPecIdAsync(id);

            ieradums.Statuss = IeradumaStatuss.Aktivs;
            ieradums.Atjauninats = DateTime.Now;

            await repository.AtjauninatAsync(ieradums);
        }

        public async Task AtzimetIzpildiAsync(Guid id, DateTime datums) //Atzīme kā izpildītu izvēlētajā datumā
        {
            Ieradums ieradums = await IegutPecIdAsync(id);

            ieradums.AtzimetKaIzpilditu(datums);

            await repository.AtjauninatAsync(ieradums);
        }

        public async Task<List<IeradumaIzpilde>> IegutIzpildesAsync(Guid id) //Iegūst izpildes ierakstus
        {
            Ieradums ieradums = await IegutPecIdAsync(id);

            return ieradums.Izpildes;
        }

        private static void ParbauditNosaukumu(string nosaukums) // Pārbauda nosaukuma derīgumu
        {
            if (string.IsNullOrWhiteSpace(nosaukums))
                throw new ArgumentException("Ieraduma nosaukums nedrīkst būt tukšs.");

            if (nosaukums.Length > 100)
                throw new ArgumentException("Ieraduma nosaukums nedrīkst pārsniegt 100 rakstzīmes.");

        }
    }
}