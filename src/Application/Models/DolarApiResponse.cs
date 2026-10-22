namespace Application.Models;

// Forma del JSON que devuelve DolarApi (https://dolarapi.com/v1/dolares/oficial), por ejemplo:
// { "moneda": "USD", "casa": "oficial", "nombre": "Oficial",
//   "compra": 1495, "venta": 1545, "fechaActualizacion": "2026-10-20T18:00:00.000Z" }
// GetFromJsonAsync no distingue mayúsculas, así que "venta" se carga en Venta.
public class DolarApiResponse
{
    public string Moneda { get; set; } = string.Empty;
    public string Casa { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Compra { get; set; }
    public decimal Venta { get; set; }
    public DateTime FechaActualizacion { get; set; }
}
