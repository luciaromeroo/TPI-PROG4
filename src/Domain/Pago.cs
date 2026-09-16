namespace Domain;

public class Pago
{
    public int NumeroTransaccion { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public bool EstadoPago { get; set; }
}