using IeradumuParvaldnieks.Domain.Entities;
using System.Net.Http.Json;

namespace IeradumuParvaldnieks.Web.Services
{
    public class IeradumuApiService
    {
        private readonly HttpClient httpClient;

        public IeradumuApiService(IHttpClientFactory factory) // Iegust savienojumu ar API
        {
            httpClient = factory.CreateClient("IeradumuApi");
        }

        //Ielade lietotaja ieradumus caur API
        public async Task<List<Ieradums>> IegutVisuAsync(string lietotajaId)
        {
            return await httpClient.GetFromJsonAsync<List<Ieradums>>($"ieradumi?lietotajaId={lietotajaId}") ?? new();
        }

        //Iegut vienu ieradumu pec ID
        public async Task<Ieradums?> IegutPecIdAsync(Guid id)
        {
            var atbilde = await httpClient.GetAsync($"ieradumi/{id}");

            if (atbilde.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            atbilde.EnsureSuccessStatusCode();

            return await atbilde.Content.ReadFromJsonAsync<Ieradums>();
        }

        //Izveido jaunu ieradumu izmantojot API
        public async Task IzveidotAsync(Ieradums ieradums)
        {
            var atbilde = await httpClient.PostAsJsonAsync("ieradumi", ieradums);
            atbilde.EnsureSuccessStatusCode();
        }
    }
}
