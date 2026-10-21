using Application.Models;

namespace Application.Interfaces;

// Contrato para obtener la cotización del dólar. La implementación (DolarApiService)
// vive en Infrastructure y llama a la API externa con el HttpClient Factory.
public interface IDolarApiService
{
    Task<DolarApiResponse?> GetDolarOficialAsync();
}
