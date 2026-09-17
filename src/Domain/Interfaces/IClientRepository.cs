using Domain;

namespace Domain.Interfaces;

// Una interfaz define un contrato: indica qué operaciones debe ofrecer un
// repositorio, pero no explica cómo se implementa cada operación.
public interface IClientRepository
{
    
    Cliente GetById(int id);


    List<Cliente> List();

  
    Cliente Add(Cliente entity);

   
    void Update(Cliente entity);


    void Delete(Cliente entity);
}

