namespace Domain.Interfaces;

// Unidad de trabajo: confirma en la base, de una sola vez, todo lo que los repositorios
// agregaron, modificaron o borraron durante un caso de uso.
// Los repositorios NO guardan: el servicio decide cuándo termina la operación y llama a SaveChanges.
// Así una operación que toca varias entidades (por ejemplo, acreditar un pago y confirmar la
// reserva) se guarda completa o no se guarda nada: el límite de la transacción queda a la vista.
public interface IUnitOfWork
{
    int SaveChanges();
}
