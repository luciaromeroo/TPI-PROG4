using Domain.Interfaces;

namespace Infrastructure.Data;

// Implementación con EF Core: todos los repositorios del request comparten el mismo
// ApplicationContext (están registrados como Scoped), así que un único SaveChanges
// guarda todos los cambios juntos, dentro de una transacción.
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationContext _context;

    public UnitOfWork(ApplicationContext context)
    {
        _context = context;
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}
