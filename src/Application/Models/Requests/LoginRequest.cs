using System.ComponentModel.DataAnnotations;

namespace Application.Models.Requests;

// Se inicia sesión con mail y contraseña (regla del TFI). El rol NO se manda:
// sale del usuario guardado en la base.
public record LoginRequest(
    [Required(ErrorMessage = "El mail es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del mail es inválido")]
    string Mail,

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    string Password
);
