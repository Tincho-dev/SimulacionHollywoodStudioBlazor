using Model;

namespace Services;

public interface ISimuladorColasEsperaService
{
    int TiempoEspera(int visitantes, double tiempoServicio, int capacidad);
    Task<(string, IEnumerable<DatoEspera>, double)> Simular(int ingresoEsperado, double precioEntrada);
    IEnumerable<DatoEspera> GetDatoEsperas();
}
