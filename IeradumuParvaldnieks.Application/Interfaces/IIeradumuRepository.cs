using IeradumuParvaldnieks.Domain.Entities;

namespace IeradumuParvaldnieks.Application.Interfaces
{
    public interface IIeradumuRepository { //Nosaka kādas darbības var veikt ar ieradumu glabātuvi

        Task<List<Ieradums>> IegutVisuAsync(); // Iegūst visus ieradumus

        Task<Ieradums?> IegutPecIdAsync(Guid id); // Atrod ieradumu pēc ID

        Task PievienotAsync(Ieradums ieradums); // Pievieno jaunu ieradumu

        Task AtjauninatAsync(Ieradums ieradums); // Saglabā izmaiņas esošā ieradumā

    }
}
