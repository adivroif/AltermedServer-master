using AltermedManager.Models.DummyHealthProviderModels;

using System.Text.Json;

namespace AltermedManager.Services
    {
    public class DummyHealthProviderService
        {
        private readonly HttpClient _httpClient;

        public DummyHealthProviderService(IHttpClientFactory httpClientFactory)
            {
            _httpClient = httpClientFactory.CreateClient("DummyProvider");
            }

        public async Task<HealthProviderPatient?> GetPatientByIdAsync(string idNum)
            {
            var response = await _httpClient.GetAsync($"/healthapi/PatientDummy/{idNum}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HealthProviderPatient>(json, new JsonSerializerOptions
                {
                PropertyNameCaseInsensitive = true
                });
            }
        }

    }
