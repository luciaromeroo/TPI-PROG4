using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

// Un item de la reserva: qué habitación se aparta y cuántos huéspedes duermen en ella.
// El precio no va acá porque se lo saca a la Room, y la cantidad tampoco por sobre su capacidad.
public record ReservationRoomRequest(
    [Required(ErrorMessage = "Debe especificarse la habitación")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la habitación debe ser válido")]
    int RoomId,

    [Required(ErrorMessage = "La cantidad de huéspedes es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de huéspedes debe ser mayor a cero")]
    int GuestsNumber
);
