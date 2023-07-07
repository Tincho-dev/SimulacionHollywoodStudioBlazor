namespace Model;

public class DatoEspera
{
    public string Nombre { get; set; } // El nombre de la atracción
    public Dictionary<DateTime, double> TiempoEspera { get; set; } // El tiempo de espera en minutos asociado a cada fecha y hora
}
