using Model;

namespace Services;

public interface ISimuladorColasEsperaService
{
    int TiempoEspera(int visitantes, double tiempoServicio, int capacidad);
    Task<(string, IEnumerable<DatoEspera>)> Simular(int ingresoEsperado);
    IEnumerable<DatoEspera> GetDatoEsperas();
}
