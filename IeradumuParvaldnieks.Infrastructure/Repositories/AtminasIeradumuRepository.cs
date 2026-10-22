using IeradumuParvaldnieks.Application.Interfaces;
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.TestData;

namespace IeradumuParvaldnieks.Infrastructure.Repositories
{

    public class AtminasIeradumuRepository : IIeradumuRepository// Glabā ieradumus atmiņā, kamēr API darbojas
    {
        private readonly List<Ieradums> ieradumi = TestaDati.IzveidotIeradumus(); //Izmantojam testa datus

        public Task<List<Ieradums>> IegutVisuAsync() //Visa ieradumu sarakstu
        {
            return Task.FromResult(ieradumi.ToList());
        }

        public Task<Ieradums?> IegutPecIdAsync(Guid id)//Atrod ieradumu pec id
        {
            Ieradums? ieradums = ieradumi.FirstOrDefault(i => i.Id == id);

            return Task.FromResult(ieradums);
        }

        public Task PievienotAsync (Ieradums ieradums)//Pievieno ieradumu sarakstam
        {
            ieradumi.Add(ieradums);

            return Task.CompletedTask;
        }

        public Task AtjauninatAsync(Ieradums ieradums)//Saglabā izmaiņas ieradumā
        {
            int indekss = ieradumi.FindIndex(i => i.Id == ieradums.Id);

            if (indekss == -1)
                throw new KeyNotFoundException("Ieradums nav atrasts.");

            ieradumi[indekss] = ieradums;

            return Task.CompletedTask;
        }
    }
}