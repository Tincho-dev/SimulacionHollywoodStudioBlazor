using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public interface ISimuladorColasEsperaService
{
    int TiempoDeEspera(int visitantesEnCola, double tiempoDeServicio);
    int CantidadVisitantesMensuales(Temporada temporada);
    Task<List<DatoEspera>> ObtenerDatosEspera(int numGente);
}
