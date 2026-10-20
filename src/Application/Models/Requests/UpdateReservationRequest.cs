using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

// Solo se pueden cambiar las fechas, y siempre con la reserva pendiente: el monto no se toca
// porque lo recalcula el sistema. Todo opcional porque en el update se manda solo lo que cambia.
public class UpdateReservationRequest
{
    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de inicio es inválido")]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de fin es inválido")]
    public DateTime? EndDate { get; set; }
}
