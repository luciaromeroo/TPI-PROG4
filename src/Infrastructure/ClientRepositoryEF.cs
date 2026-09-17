using Domain.Interfaces;
using Domain;

namespace Infrastructure.Data;

public class ClientRepositoryEF : IClientRepository
{
    private static List<Cliente> _clients = [];

    public Cliente GetById(int id)
    {
        return _clients.FirstOrDefault(client => client.Id == id)
            ?? throw new KeyNotFoundException($"Client with id {id} not found.");
    }

    public List<Cliente> List()
    {
        return _clients;
    }

    public Cliente Add(Cliente entity)
    {
        _clients.Add(entity);
        return entity;
    }

    public void Update(Cliente entity)
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

    public void Delete(Cliente entity)
    {
          _clients.RemoveAll(client => client.Id == entity.Id);
    }

}