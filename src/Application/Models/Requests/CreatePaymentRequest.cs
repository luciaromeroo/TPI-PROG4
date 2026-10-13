using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

// Sin Id (lo genera la base), sin PaymentState (nace en false), sin PaymentDate (la pone el servidor)
// y sin TransactionNumber (lo genera el servicio).
public record CreatePaymentRequest(
    [Required(ErrorMessage = "La reserva es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la reserva debe ser válido")]
    int ReservationId,

    // Que sea mayor a 0 lo valida la entidad Payment (PAYMENT_AMOUNT_MUST_BE_POSITIVE).
    [Required(ErrorMessage = "El monto es obligatorio")]
    decimal Amount,

    [Required(ErrorMessage = "El método de pago es obligatorio")]
    [StringLength(50, ErrorMessage = "El método de pago no debe superar los 50 caracteres")]
    string PaymentMethod
);
