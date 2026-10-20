using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

// El id lo genera la base, el código lo genera el servicio y el estado arranca
// en pendiente, por eso no se piden acá. Las habitaciones van por otro endpoint.
// El cliente tampoco se manda: es el usuario logueado (sale del token).
public record CreateReservationRequest(
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de inicio es inválido")]
    DateTime StartDate,

    [Required(ErrorMessage = "La fecha de fin es obligatoria")]
    [DataType(DataType.Date, ErrorMessage = "El formato de la fecha de fin es inválido")]
    DateTime EndDate,

    [Required(ErrorMessage = "El monto total es obligatorio")]
    decimal TotalAmount
);
