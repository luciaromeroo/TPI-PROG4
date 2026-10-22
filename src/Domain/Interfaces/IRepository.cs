namespace Domain.Interfaces;

// Add, Update y Delete solo registran el cambio: para guardarlo en la base hay que
// llamar a IUnitOfWork.SaveChanges() (lo hace el servicio al final del caso de uso).
public interface IRepository<T> where T : class
{
    T? GetById(int id);
    IEnumerable<T> List();
    T Add(T entity);
    T Update(T entity);
    void Delete(T entity);
}
