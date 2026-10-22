using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

// Los métodos de escritura solo marcan los cambios en el DbContext; no los guardan.
// Quien confirma es el servicio, con IUnitOfWork.SaveChanges(), al terminar el caso de uso.
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public virtual IEnumerable<T> List()
    {
        return _dbSet.ToList();
    }

    public virtual T Add(T entity)
    {
        _dbSet.Add(entity);
        return entity;
    }

    public virtual T Update(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
