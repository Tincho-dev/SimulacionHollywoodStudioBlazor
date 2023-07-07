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
        TiemposPorAtraccion = SimularTiemposDeEspera(ingresoEsperado);
        double tiempoEsperaPromedio = TiemposPorAtraccion.SelectMany(d => d.TiempoEspera.Values).Average();

        var respuesta = GetRespuesta(tiempoEsperaPromedio);
        return (respuesta, TiemposPorAtraccion, tiempoEsperaPromedio);
    }

    public IEnumerable<DatoEspera> GetDatoEsperas()
    {
        return TiemposPorAtraccion;
    }

    public List<DatoEspera> SimularTiemposDeEspera(int ingresoEsperado)
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

        for (int dia = 0; dia <= 30; dia++)
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
}
