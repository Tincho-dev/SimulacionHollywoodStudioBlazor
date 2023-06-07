using Model;

namespace Services;

public class SimuladorColasEsperaService : ISimuladorColasEsperaService
{
    private readonly IDistribucionesService DistribucionesService;

    public SimuladorColasEsperaService(IDistribucionesService distribucionesService)
    {
        DistribucionesService = distribucionesService;
    }

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

    public int TiempoDeEspera(double tasaDeLlegada, double tasaDeServicio)
    {
        double u = DistribucionesService.GenerarNumeroAleatorio();
        tasaDeLlegada = -tasaDeLlegada * Math.Log(u);

        double wq = tasaDeLlegada / (tasaDeServicio * (tasaDeServicio - tasaDeLlegada));

        return (int)wq;
    }

    public Dictionary<int, double> TiemposDeEspera(double tasaDeLlegada, double tasaDeServicio)
    {
        var tiemposDeEspera = new Dictionary<int, double>();
        for (int i = 8; i < 23; i++)
        {
            tiemposDeEspera.Add(i, CalcularTiempoDeEspera( 300, tasaDeServicio));
        }
        return tiemposDeEspera;
    }

    public Task<List<DatoEspera>> ObtenerDatosEspera(int numGente)
    {
        List<DatoEspera> list = new();

        list.Add(new DatoEspera
        {
            Nombre = "Rise of the Resistance",
            TiempoEspera = TiemposDeEspera(0.057, 0.056)
        });
        list.Add(
        new DatoEspera
        {
            Nombre = "Millenium Falcon",
            TiempoEspera = TiemposDeEspera(0.045, 0.022)
        });
        return Task.FromResult(list);
    }
}
