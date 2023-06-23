using Model;

namespace Services;

public class SimuladorColasEsperaService : ISimuladorColasEsperaService
{
    private readonly IDistribucionesService DistribucionesService;
    private double u;
    public List<DatoEspera> TiemposPorAtraccion = new List<DatoEspera>();

    public SimuladorColasEsperaService(IDistribucionesService distribucionesService)
    {
        DistribucionesService = distribucionesService;
    }

    public async Task<(string, IEnumerable<DatoEspera>, double)> Simular(int ingresoEsperado)
    {
        TiemposPorAtraccion = await SimularTiemposDeEspera(ingresoEsperado);
        double tiempoEsperaPromedio = TiemposPorAtraccion.SelectMany(d => d.TiempoEspera.Values).Average();

        var respuesta = GetRespuesta(tiempoEsperaPromedio);
        return (respuesta, TiemposPorAtraccion, tiempoEsperaPromedio);
    }

    public IEnumerable<DatoEspera> GetDatoEsperas()
    {
        return TiemposPorAtraccion;
    }

    public async Task<List<DatoEspera>> SimularTiemposDeEspera(int ingresoEsperado)
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

        var datosEsperaRR = new DatoEspera { Nombre = "RR", TiempoEspera = new Dictionary<DateTime, double>() };
        var datosEsperaMF = new DatoEspera { Nombre = "MF", TiempoEspera = new Dictionary<DateTime, double>() };

        int año = DateTime.Now.Year;    
        int mes = DateTime.Now.Month;

        for (int dia = 0; dia <= 29; dia++)
        {
            u = DistribucionesService.GenerarNumeroAleatorio();
            var visitantesDiarios = -cantidadVisitantesDiariosPromedio * Math.Log(u);
            for (int hora = 0; hora <= 16; hora++)
            {
                var visitantesRR = 0; var visitantesMF = 0;
                u = DistribucionesService.GenerarNumeroAleatorio();
                int visitantesPorHora = (int)(-cantidadVisitantesPorHoraPromedio * Math.Log(u));
                
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
                    //await Task.Delay(1);
                }//visitantes++

                // tiempo de espera de cada hora de cada día
                // este contenido se tiene que mostrar en la tabla de la vista
                datosEsperaRR.TiempoEspera.Add(new DateTime(año, mes, dia + 1, hora, 0, 0), TiempoEspera(visitantesRR, tiempoServicioRR, capacidadRR));
                datosEsperaMF.TiempoEspera.Add(new DateTime(año, mes, dia + 1, hora, 0, 0), TiempoEspera(visitantesMF, tiempoServicioMF, capacidadMF));
            }
        }

        return new List<DatoEspera> { datosEsperaRR, datosEsperaMF };
    }
    public int TiempoEspera(int visitantes, double tiempoServicio, int capacidad)
    {
        int tiempo = (int)(visitantes / capacidad * tiempoServicio);
        return tiempo;
    }

    private string GetRespuesta(double tiempoEsperaPromedio)
    {
        if (tiempoEsperaPromedio <= 130)
        {
            return @"Cumple con el ingreso esperado y los clientes estan satisfechos con los tiempos de espera";
        }
        else if (tiempoEsperaPromedio <= 180)
        {
            return @"Cumple con el ingreso esperado y los tiempos de espera son razonables (estan entre los registrados en el pasado)";
        }
        else if (tiempoEsperaPromedio <= 360)
        {
            return @"La gente esta inconforme con el amontonamiento y los altos tiempos de espera";
        }
        else
        {
            return @"La capacidad del establecimiento es insuficiente para lograr el ingreso esperado";
        }
    }

    #region Metodo Obsoleto
    public Task<string> SimularObsoleto(Temporada temporada, int cantidadVisitantesMensuales, int ingresoEsperado)
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
    #endregion
}
