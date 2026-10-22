using System.Net.Http.Json;
using Application.Interfaces;
using Application.Models;

namespace Infrastructure.Services;

// Consume DolarApi (servicio de terceros, gratis y sin clave) para obtener la cotización del dólar.
// Igual que PokemonAPIService del material de la cátedra: se inyecta el IHttpClientFactory y se
// crea el cliente "dolarApiHttpClient", que está registrado con su BaseAddress en Program.cs.
public class DolarApiService : IDolarApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _dolarApiClient;

    public DolarApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _dolarApiClient = _httpClientFactory.CreateClient("dolarApiHttpClient");
    }

    // GET https://dolarapi.com/v1/dolares/oficial
    public async Task<DolarApiResponse?> GetDolarOficialAsync()
    {
        return await _dolarApiClient.GetFromJsonAsync<DolarApiResponse>("dolares/oficial");
    }
}
