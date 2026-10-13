using Domain.Exceptions;

namespace Domain;

// Un Payment es UN pago parcial de una reserva (seña, transferencia, saldo abonado en el hotel, etc.).
// Una reserva puede registrar varios pagos, cada uno con su propio método y monto.
public class Payment
{
    public int Id { get; set; }
    public int TransactionNumber { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public bool PaymentState { get; set; }      // false = pendiente de acreditación, true = acreditado
    public DateTime PaymentDate { get; set; }

    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    // Constructor vacío requerido por Entity Framework.
    public Payment() { }

    // Constructor de negocio: el Id lo genera la base, el pago siempre nace sin acreditar
    // (regla "Ciclo de Vida de los Pagos") y la fecha la pone el servidor.
    public Payment(int transactionNumber, decimal amount, string paymentMethod, int reservationId)
    {
        if (amount <= 0)
            throw new AppValidationException("PAYMENT_AMOUNT_MUST_BE_POSITIVE");

        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new AppValidationException("PAYMENT_METHOD_REQUIRED");

        TransactionNumber = transactionNumber;
        Amount = amount;
        PaymentMethod = paymentMethod.Trim();
        ReservationId = reservationId;
        PaymentState = false;
        PaymentDate = DateTime.Now;
    }

    // Marca el pago como acreditado.
    public void ConfirmPayment()
    {
        if (PaymentState)
            throw new AppValidationException("PAYMENT_ALREADY_CONFIRMED");

        PaymentState = true;
    }

    // Un pago acreditado es historial contable: no se puede borrar.
    public void EnsureCanBeDeleted()
    {
        if (PaymentState)
            throw new AppValidationException("CANNOT_DELETE_CONFIRMED_PAYMENT");
    }
}
