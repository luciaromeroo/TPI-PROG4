using Domain;

namespace Domain.Interfaces;

// Una interfaz define un contrato: indica qué operaciones debe ofrecer un
// repositorio, pero no explica cómo se implementa cada operación.
public interface IUserRepository
{
    
    User GetById(int id);


    List<User> List();

  
    User Add(User entity);

   
    void Update(User entity);


    void Delete(User entity);
}

