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
            TiempoEspera = new Dictionary<int, int> {
                {8,110 },
                {9,105 },
                {10,100 },
                {11,95 },
                {12,108 },
                {13,119 },
                {14,120 },
                {15,100 },
                {16,123 },
                {17,134 },
                {18,97 }
            }


        });
        

        return Task.FromResult(list);
    }
}
