using Domain;
using Domain.Interfaces;

namespace Infrastructure.Data;

// Igual que PaymentRepository y ReservationRepository: lo básico viene de Repository<User>.
// Si no encuentra nada devuelve null y el servicio decide qué hacer (NotFoundException -> 404).
public class UserRepositoryEF : Repository<User>, IUserRepository
{
    public UserRepositoryEF(ApplicationContext context) : base(context) { }

    public User? GetByMail(string mail)
    {
        return _dbSet.FirstOrDefault(user => user.Mail == mail);
    }
}
