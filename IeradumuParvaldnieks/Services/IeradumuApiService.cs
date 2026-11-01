using System.Net.Http.Json;
using IeradumuParvaldnieks.Domain.Entities;

namespace IeradumuParvaldnieks.Services
{
    public class IeradumuApiService
    {
        private readonly HttpClient httpClient;

        public IeradumuApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Ieradums>> IegutVisuAsync(string lietotajaId)
        {
            return await httpClient.GetFromJsonAsync<List<Ieradums>>($"ieradumi?lietotajaId={lietotajaId}") ?? new List<Ieradums>();
        }

        public async Task AtjaunotAsync(Guid id)
        {
            using HttpResponseMessage atbilde = await httpClient.PutAsync($"ieradumi/{id}/atjaunot", null);

            atbilde.EnsureSuccessStatusCode();
        }

        public async Task AtzimetIzpildiAsync(Guid id, DateTime datums)
        {
            using HttpResponseMessage atbilde = await httpClient.PostAsJsonAsync($"ieradumi/{id}/izpildes", new { datums });

            if (!atbilde.IsSuccessStatusCode)
            {
                string kluda = await atbilde.Content.ReadAsStringAsync();

                throw new InvalidOperationException(
                    string.IsNullOrWhiteSpace(kluda)
                    ? "Neizdevās atzīmēt ieradumu"
                    : kluda.Trim('"'));
            }
        }

        // Izveido jaunu ieradumu caur API
        public async Task IzveidotAsync(
            string nosaukums,
            string? apraksts,
            Atkartosana? atkartosana)
        {
            using HttpResponseMessage atbilde =
                await httpClient.PostAsJsonAsync(
                    "ieradumi",
                    new
                    {
                        lietotajaId = "user1",
                        nosaukums,
                        apraksts,
                        atkartosana
                    });

            if (!atbilde.IsSuccessStatusCode)
            {
                string kluda = await atbilde.Content.ReadAsStringAsync();
                throw new InvalidOperationException(kluda.Trim('"'));
            }
        }

        // Labo esošu ieradumu caur API
        public async Task LabotAsync(
            Guid id,
            string nosaukums,
            string? apraksts,
            Atkartosana? atkartosana)
        {
            using HttpResponseMessage atbilde =
                await httpClient.PutAsJsonAsync(
                    $"ieradumi/{id}",
                    new
                    {
                        nosaukums,
                        apraksts,
                        atkartosana
                    });

            if (!atbilde.IsSuccessStatusCode)
            {
                string kluda = await atbilde.Content.ReadAsStringAsync();
                throw new InvalidOperationException(kluda.Trim('"'));
            }
        }

        // Arhivē ieradumu caur API
        public async Task ArhivetAsync(Guid id)
        {
            using HttpResponseMessage atbilde =
                await httpClient.PutAsync(
                    $"ieradumi/{id}/arhivet", null);

            atbilde.EnsureSuccessStatusCode();
        }
    }
}