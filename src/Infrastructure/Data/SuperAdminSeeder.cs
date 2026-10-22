using Application.Interfaces;
using Domain;
using Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data;

// Datos del primer SuperAdmin. Se leen de appsettings.json, sección "SuperAdminSeed" (mail y contraseña desde variables de entorno).
public class SuperAdminSeedOptions
{
    public const string SectionName = "SuperAdminSeed";

    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

// Problema del huevo y la gallina: solo un SuperAdmin puede crear administradores,
// pero alguien tiene que crear al primero. Al arrancar la API, si no hay ningún
// SuperAdmin, se crea uno con los datos de la configuración.
public class SuperAdminSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly SuperAdminSeedOptions _options;
    private readonly IUnitOfWork _unitOfWork;

    public SuperAdminSeeder(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IOptions<SuperAdminSeedOptions> options,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _options = options.Value;
        _unitOfWork = unitOfWork;
    }

    public void Seed()
    {
        // Sin mail o contraseña configurados no se crea nada.
        if (string.IsNullOrWhiteSpace(_options.Mail) || string.IsNullOrWhiteSpace(_options.Password))
            return;

        if (_userRepository.List().Any(user => user.Type == Role.SuperAdmin))
            return;

        if (_userRepository.GetByMail(_options.Mail.Trim().ToLowerInvariant()) is not null)
            return;

        var superAdmin = new User(
            _options.Name,
            _options.LastName,
            _options.Mail,
            _passwordHasher.Hash(_options.Password),
            _options.Dni,
            _options.Phone,
            Role.SuperAdmin
        );

        _userRepository.Add(superAdmin);
        _unitOfWork.SaveChanges();
    }
}
