using Model;

namespace Services;

public interface ISimuladorColasEsperaService
{
    event Action OnChange;

    public string Respuesta { get; set; }
    public IEnumerable<DatoEspera> TiemposDeEspera { get; }
    public double TiemposEsperaPromedio { get; set; }
    public double PrecioEntrada { get; set; }

    int TiempoEspera(int visitantes, double tiempoServicio, int capacidad);
    //(string Respuesta, IEnumerable<DatoEspera> TiemposDeEspera, double TiempoEsperaPromedio) 
    void Simular(int ingresoEsperado);
}
