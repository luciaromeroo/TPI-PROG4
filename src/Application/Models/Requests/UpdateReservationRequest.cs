using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

// Todo opcional porque en el update se manda solo lo que cambia.
public class UpdateReservationRequest
{
    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de inicio es inválido")]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de fin es inválido")]
    public DateTime? EndDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El monto total no puede ser negativo")]
    public decimal? TotalAmount { get; set; }
}
