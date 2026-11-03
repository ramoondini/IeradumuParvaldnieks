using IeradumuParvaldnieks.Application.Interfaces;
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IeradumuParvaldnieks.Infrastructure.Repositories
{
    public class SqlIeradumuRepository : IIeradumuRepository
    {
        private readonly IeradumuDbContext db;

        //Saņem datubāzes konktekstu
        public SqlIeradumuRepository(IeradumuDbContext db)
        {
            this.db = db;
        }
        //Iegūst visus ieradumus ar izpildes vēsturi
        public async Task<List<Ieradums>> IegutVisuAsync()
        {
            return await db.Ieradumi.Include(i => i.Izpildes).ToListAsync();
        }

        //Iegūst ieradumu pēc tā id
        public async Task<Ieradums?> IegutPecIdAsync(Guid id)
        {
            return await db.Ieradumi.Include(i => i.Izpildes).FirstOrDefaultAsync(i => i.Id == id);
        }

        //Pievieno jaunu ieradumu
        public async Task PievienotAsync(Ieradums ieradums)
        {
            await db.Ieradumi.AddAsync(ieradums);
            await db.SaveChangesAsync();
        }

        //Saglabā ieraduma izmaiņas
        public async Task AtjauninatAsync(Ieradums ieradums)
        {
            //Noskaidro jau saglabātās izmaiņas
            var saglabatieId = await db.IeradumuIzpildes.Where(i => i.IeradumaId == ieradums.Id).Select(i => i.Id).ToListAsync();
        
            //Pievieno jaunos izpildes ierakstus
            foreach (var izpilde in ieradums.Izpildes)
            {
                if (!saglabatieId.Contains(izpilde.Id))
                {
                    db.Entry(izpilde).State = EntityState.Added;
                }
            }

            //Saglabā izmaiņas datubāzē
            await db.SaveChangesAsync();
        }

    }
}