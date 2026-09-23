using Domain.Interfaces;
using Domain;

namespace Infrastructure.Data;

public class UserRepositoryEF : IUserRepository
{
    private static List<User> _clients = [];

    public User GetById(int id)
    {
        return _clients.FirstOrDefault(client => client.Id == id)
            ?? throw new KeyNotFoundException($"Client with id {id} not found.");
    }

    public List<User> List()
    {
        return _clients;
    }

    public User Add(User entity)
    {
        _clients.Add(entity);
        return entity;
    }

    public void Update(User entity)
    {
        var index = _clients.FindIndex(client => client.Id == entity.Id);
        if (index >= 0)
        {
            _clients[index] = entity;
        }
        else
        {
            throw new KeyNotFoundException($"Client with id {entity.Id} not found.");
        }
    }

    public void Delete(User entity)
    {
          _clients.RemoveAll(client => client.Id == entity.Id);
    }

}