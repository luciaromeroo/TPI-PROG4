namespace Domain.Interfaces;

public interface IRepository<T> where T : class
{
    T? GetById(int id);
    IEnumerable<T> List();
    T Add(T entity);
    T Update(T entity);
    void Delete(T entity);
}
