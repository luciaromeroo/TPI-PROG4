using Domain.Exceptions;

namespace Domain;

// Un Payment es UN pago parcial de una reserva (seña, transferencia, saldo abonado en el hotel, etc.).
// Una reserva puede registrar varios pagos, cada uno con su propio método y monto.
// Las propiedades tienen "private set": desde afuera se leen, pero el estado solo cambia por
// el constructor o por los métodos de negocio (ConfirmPayment), que validan las reglas.
public class Payment
{
    public int Id { get; private set; }
    public int TransactionNumber { get; private set; }
    public decimal Amount { get; private set; }
    public string PaymentMethod { get; private set; } = string.Empty;
    public bool PaymentState { get; private set; }      // false = pendiente de acreditación, true = acreditado
    public DateTime PaymentDate { get; private set; }

    public int ReservationId { get; private set; }
    public Reservation Reservation { get; private set; } = null!;

    // Constructor vacío requerido por Entity Framework (privado: solo lo usa EF al leer de la base).
    private Payment() { }

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

    // Marca el pago como acreditado. Es la única forma de pasar PaymentState a true.
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
