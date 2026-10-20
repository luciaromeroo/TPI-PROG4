using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

// El id lo genera la base, el código lo genera el servicio, el estado arranca en pendiente
// y el total lo calcula el sistema (regla "Independencia de tarifa"): nada de eso se pide acá.
// La reserva sí se crea con sus habitaciones en la misma operación.
public record CreateReservationRequest(
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de inicio es inválido")]
    DateTime StartDate,

    [Required(ErrorMessage = "La fecha de fin es obligatoria")]
    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de fin es inválido")]
    DateTime EndDate,

    [Required(ErrorMessage = "Debe especificarse el cliente")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del cliente debe ser válido")]
    int UserId,

    [Required(ErrorMessage = "Debe especificarse al menos una habitación")]
    [MinLength(1, ErrorMessage = "La reserva debe incluir al menos una habitación")]
    List<ReservationRoomRequest> Rooms
);
