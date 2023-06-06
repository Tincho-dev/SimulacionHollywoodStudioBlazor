using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public class SimuladorColasEsperaService : ISimuladorColasEsperaService 
{

    public int CantidadVisitantesMensuales(Temporada temporada)
    {
        int visitantes;
        if (temporada == Temporada.Alta)
        {
            visitantes = 100000;
        }
        else
        {
            visitantes = 90000;
        }
        return visitantes;
    }

    public int TiempoDeEspera(int visitantesEnCola, double tiempoDeServicio) 
    { 
        throw new NotImplementedException(); 
    }

    public Task<List<DatoEspera>> ObtenerDatosEspera(int numGente)
    {
        List<DatoEspera> list = new List<DatoEspera>();

        list.Add(new DatoEspera{ 
            Nombre="Millenium Falcon",
            TiempoEspera = new List<int> { 110,112,112,90,120,80 }
        });
        

        return Task.FromResult(list);
    }
}
