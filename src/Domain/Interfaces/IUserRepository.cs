namespace Domain.Interfaces;

// El CRUD (GetById, List, Add, Update, Delete) ya viene del genérico.
// Acá solo lo propio de usuarios: buscar por mail para no repetirlo.
public interface IUserRepository : IRepository<User>
{
    User? GetByMail(string mail);
}
