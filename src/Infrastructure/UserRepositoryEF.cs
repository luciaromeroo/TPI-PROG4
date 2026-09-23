using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class UserRepositoryEF : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepositoryEF(ApplicationContext context)
    {
        _context = context;
    }

    public User GetById(int id)
    {
        return _context.Users.FirstOrDefault(user => user.Id == id)
            ?? throw new KeyNotFoundException($"User with id {id} not found.");
    }

    public List<User> List()
    {
        return _context.Users.ToList();
    }

    public User Add(User entity)
    {
        _context.Users.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Update(User entity)
    {
        if (!_context.Users.Any(user => user.Id == entity.Id))
        {
            throw new KeyNotFoundException($"User with id {entity.Id} not found.");
        }

        _context.Users.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(User entity)
    {
        var user = _context.Users.Find(entity.Id)
            ?? throw new KeyNotFoundException($"User with id {entity.Id} not found.");

        _context.Users.Remove(user);
        _context.SaveChanges();
    }

}