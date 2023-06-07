using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public interface ISimuladorColasEsperaService
{
    int TiempoDeEspera(double tasaDeLlegada, double tasaDeServicio);
    int CantidadVisitantesMensuales(Temporada temporada);
    Task<List<DatoEspera>> ObtenerDatosEspera(int numGente);
}
