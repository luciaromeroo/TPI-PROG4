using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.Models.Requests;

public record RoomRequest(
    [Range(1, int.MaxValue, ErrorMessage = "El número de habitación debe ser mayor a cero")]
    int Number,

    [EnumDataType(typeof(RoomType), ErrorMessage = "El tipo de habitación no es válido")]
    RoomType Type,

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio por noche debe ser mayor a cero")]
    decimal PricePerNight,

    [Required(ErrorMessage = "El estado de la habitación es obligatorio")]
    bool? State,

    [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser al menos 1")]
    int Capacity
);