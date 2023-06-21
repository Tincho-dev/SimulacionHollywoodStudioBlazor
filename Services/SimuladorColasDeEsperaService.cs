using Model;
using Newtonsoft.Json;
using Radzen;
using System;
using System.Net.WebSockets;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace Services;

public class SimuladorColasEsperaService : ISimuladorColasEsperaService
{
    private readonly IDistribucionesService DistribucionesService;
    private double u;

    public SimuladorColasEsperaService(IDistribucionesService distribucionesService)
    {
        DistribucionesService = distribucionesService;
    }



    public Task<string> Simular(Temporada temporada, int cantidadVisitantesMensuales, int ingresoEsperado)
    {
        var TEVMF = 0;
        var TEVRR = 0;
        var TEH = 0;
        var TED = 0;
        var Indicador = 0;
        var PE = 109;

        var VRR = 0;
        double TEHRR = 0;
        var VMF = 0;
        double TEHMF = 0;
        var respuesta = string.Empty;

        for (int i = 0; i < 30; i++)
        {
            u = DistribucionesService.GenerarNumeroAleatorio();
            var CVD = 22600 * (22700 - 22600) * u;
            for (int j = 8; j < 15; j++)
            {
                u = DistribucionesService.GenerarNumeroAleatorio();
                var CVH = -(1620) * Math.Log(u);
                for (int visitantes = 0; visitantes < CVH; visitantes++)
                {
                    u = DistribucionesService.GenerarNumeroAleatorio();
                    if (u <= 0.65)
                    {
                        VRR++;
                        u = DistribucionesService.GenerarNumeroAleatorio();
                        TEHRR = (-110) * Math.Log(u);
                    }
                    else
                    {
                        VMF++;
                        u = DistribucionesService.GenerarNumeroAleatorio();
                        TEHMF = (-55) * Math.Log(u);
                    }
                }//al cerrar el for aumenta en 1 los visitantes
                TEH = TEH + TEVRR + TEVMF;
            }//al terminar el for aumenta en 1 las horas
            TED += TEH;
        }//al terminar el for aumenta en 1 el dia a simular

        var FunciónCalculoIngresos = ingresoEsperado / cantidadVisitantesMensuales * PE;
        Indicador = TED * FunciónCalculoIngresos;

        if (Indicador == ingresoEsperado)
        {
            if (TED <= 150)
            {
                respuesta = "Cumple con el ingreso esperado";
            }
            else
            {
                respuesta = @"La capacidad del establecimiento es 
                            insuficiente para lograr el ingreso esperado";
            }
        }
        else if (Indicador <= ingresoEsperado / 2)
        {
            respuesta = @"No cumple con el ingreso
                        esperado, se recomienda
                        incorporar nuevas
                        atracciones";
        }
        else
        {
            respuesta = @"No cumple con el
                        ingreso esperado, se
                        recomienda establecer
                        promociones";
        }

        return Task.FromResult(respuesta);
    }

    public Task<string> SimularV2(int ingresoEsperado)
    {
        var precioEntrada = 109;
        int[,] tiempoEsperaRR = new int[30, 17];
        int[,] tiempoEsperaMF = new int[30, 17];
        var tiempoServicioRR = 18;
        var tiempoServicioMF = 4.5;
        var capacidadRR = 60;
        var capacidadMF = 42;

        var cantidadVisitantesMensuales = ingresoEsperado / precioEntrada;
        var cantidadVisitantesDiariosPromedio = cantidadVisitantesMensuales / 30;
        var cantidadVisitantesPorHoraPromedio = cantidadVisitantesDiariosPromedio / 16;

        for (int dia = 0; dia <= 29; dia++)
        {
            u = DistribucionesService.GenerarNumeroAleatorio();
            var visitantesDiarios = -cantidadVisitantesDiariosPromedio * Math.Log(u);
            for (int hora = 0; hora <= 16; hora++)
            {
                var visitantesRR = 0; var visitantesMF = 0;
                u = DistribucionesService.GenerarNumeroAleatorio();
                var visitantesPorHora = -cantidadVisitantesPorHoraPromedio * Math.Log(u);
                for (int visitantes = 0; visitantes < visitantesPorHora; visitantes++)
                {
                    u = DistribucionesService.GenerarNumeroAleatorio();
                    if (u <= 0.75)
                    {
                        u = DistribucionesService.GenerarNumeroAleatorio();
                        if (u <= 0.3)
                        {
                            visitantesRR++;
                        }
                        else
                        {
                            visitantesMF++;
                        }
                    }
                }//visitantes++
                // tiempo deespera de cada hora de cada día
                // este contenido se tiene que mostrar en la tabla de la vista
                tiempoEsperaRR[dia, hora] = TiempoEspera(visitantesRR, tiempoServicioRR, capacidadRR);
                tiempoEsperaMF[dia, hora] = TiempoEspera(visitantesMF, tiempoServicioMF, capacidadMF);
                //visitantesDiarios = 0;
            }
        }
        // Tiempo de Espera Promedio
        int TEP = (int)tiempoEsperaMF.Cast<int>()
            .Zip(tiempoEsperaMF.Cast<int>(), (a, b) => a + b)
            .Average();
        var respuesta = string.Empty;
        if (TEP <= 180)
        {
            if (TEP <= 130)
            {
                respuesta = @"Cumple con el ingreso
                    esperado y los clientes estan
                    satisfechos con los tiempos de espera";
            }
            else
            {
                respuesta = @"Cumple con el ingreso
                    esperado y los tiempos de
                    espera son razonables
                    (estan entre los registrados en el pasado)";
            }
        }
        else if (TEP <= 360)
        {
            respuesta = @"La gente esta inconforme
                con el amontonamiento y
                los altos tiempos de
                espera";
        }
        else
        {
            respuesta = @"La capacidad del establecimiento es
                 insuficiente para lograr el ingreso esperado";

        }
        return Task.FromResult(respuesta);
    }

    private int TiempoEspera(int visitantes, double tiempoServicio, int capacidad)
    {
        return (int)(visitantes / capacidad * tiempoServicio);
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

                    tasasDeLlegada.Add(new DateTime(year, mes, dia, hora, 0, 0), 313 / tasaFinal);
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
    public Task<int> VisitantesEstimados(DateTime fecha)
    {
        int visitantes;

        // Determinar la temporada basándose en el mes
        if (fecha.Month >= 12 || fecha.Month <= 3) // Diciembre a Marzo = Temporada Alta
        {
            visitantes = 100000;
        }
        else if (fecha.Month >= 8 && fecha.Month <= 10) // Agosto a Octubre = Temporada Baja
        {
            visitantes = 90000;
        }
        else
        {
            // Para los meses restantes, puedes elegir un valor por defecto o calcular de alguna manera
            visitantes = 95000;
        }

        return Task.FromResult( visitantes);
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
