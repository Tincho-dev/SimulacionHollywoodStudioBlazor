﻿using Model;

namespace Services;

public interface ISimuladorColasEsperaService
{
    DatoEspera CrearDatoEspera(string nombreAtraccion);
    Task<List<DatoEspera>> ObtenerDatosEspera(int numGente);
    double CalcularTiempoDeEspera(int numPersonasCola, double tasaLlegada);
    Task<string> Simular(Temporada temporada, int cantidadVisitantesMensuales, int ingresoEsperado);
}
