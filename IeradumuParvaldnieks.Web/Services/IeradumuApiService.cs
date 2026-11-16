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
    }
}
