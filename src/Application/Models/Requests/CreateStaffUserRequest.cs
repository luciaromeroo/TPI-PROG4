using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.Models.Requests;

// Alta de usuarios con rol elegido. Solo lo puede usar un SuperAdmin
// (lo controla [Authorize] en UserController).
public record CreateStaffUserRequest(
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no debe superar los 50 caracteres")]
    string Name,

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(50, ErrorMessage = "El apellido no debe superar los 50 caracteres")]
    string LastName,

    [Required(ErrorMessage = "El mail es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del mail es inválido")]
    string Mail,

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    string Password,

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(10, ErrorMessage = "El DNI no debe superar los 10 caracteres")]
    string Dni,

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres")]
    string Phone,

    [EnumDataType(typeof(Role), ErrorMessage = "El rol no es válido")]
    Role Role
);
