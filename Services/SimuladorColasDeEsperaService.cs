using Model;
using Newtonsoft.Json;
using Radzen;
using System;

namespace Services;

public class SimuladorColasEsperaService : ISimuladorColasEsperaService
{
    private readonly IDistribucionesService DistribucionesService;

    public SimuladorColasEsperaService(IDistribucionesService distribucionesService)
    {
        DistribucionesService = distribucionesService;
    }

    public DatoEspera CrearDatoEspera(string nombreAtraccion)
    {
        var tasasDeLlegada = new Dictionary<DateTime, double>();
        var year = 2023;

        for (int mes = 1; mes <= 12; mes++)
        {
            for (int dia = 1; dia <= DateTime.DaysInMonth(year, mes); dia++)
            {
                var fecha = new DateTime(year, mes, dia);
                for (int hora = 8; hora <= 23; hora++)
                {
                    var tasaHora = TasasDeLlegadaData.TasasPorHora.ContainsKey(hora.ToString()) ? TasasDeLlegadaData.TasasPorHora[hora.ToString()] : 0;
                    var tasaMes = TasasDeLlegadaData.TasasPorMes.ContainsKey(mes.ToString()) ? TasasDeLlegadaData.TasasPorMes[mes.ToString()] : 0;
                    var tasaDia = TasasDeLlegadaData.TasasPorDia.ContainsKey(((int)fecha.DayOfWeek).ToString()) ? TasasDeLlegadaData.TasasPorDia[((int)fecha.DayOfWeek).ToString()] : 0;

                    var tasaFinal = (tasaHora + tasaMes + tasaDia) / 3;

                    tasasDeLlegada.Add(new DateTime(year, mes, dia, hora, 0, 0), 313/tasaFinal);
                }
            }
        }

        return new DatoEspera
        {
            Nombre = nombreAtraccion,
            Posicion = new GoogleMapPosition(), // Aquí puedes asignar la posición real
            TiempoEspera = tasasDeLlegada
        };
    }


    #region Visitantes y tiempos de espera
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

    public double CalcularTiempoDeEspera(int numPersonasCola, double tasaLlegada)
    {
        // Convertir la tasa de llegada a personas por minuto
        double tasaLlegadaMinuto = tasaLlegada;

        // Utilizar la fórmula de Little para calcular el tiempo de espera promedio
        double tiempoEspera = numPersonasCola / tasaLlegadaMinuto;

        return tiempoEspera;
    }

    public Task<List<DatoEspera>> ObtenerDatosEspera(int numGente)
    {
        List<DatoEspera> list = new()
        {
            CrearDatoEspera("Rise of the Resistance")
        //CrearDatoEspera("Rise of the Resistance");
        };
        return Task.FromResult(list);
    }
    #endregion
}
