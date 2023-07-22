using Model;

namespace Services;

public interface ISimuladorColasEsperaService
{
    int TiempoEspera(int visitantes, double tiempoServicio, int capacidad);
    (string Respuesta, IEnumerable<DatoEspera> TiemposDeEspera, double TiempoEsperaPromedio) Simular(int ingresoEsperado, double precioEntrada);
    IEnumerable<DatoEspera> GetDatoEsperas();
}
