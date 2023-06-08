using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public interface ISimuladorColasEsperaService
{
    DatoEspera CrearDatoEspera(string nombreAtraccion);
    Task<List<DatoEspera>> ObtenerDatosEspera(int numGente);
    double CalcularTiempoDeEspera(int numPersonasCola, double tasaLlegada);
}
