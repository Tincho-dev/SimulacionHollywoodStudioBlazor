using Radzen;

namespace Model;

public class DatoEspera
{
    public string Nombre { get; set; } // El nombre de la atracción
    public GoogleMapPosition Posicion { get; set; } // Las coordenadas de la atracción
    public Dictionary<DateTime, double> TiempoEspera { get; set; } // El tiempo de espera en minutos asociado a cada fecha y hora
}

public static class TasasDeLlegadaData
{
    public static Dictionary<string, double> TasasPorHora = new Dictionary<string, double>
    {
        { "7", 3.15 },
        { "8", 7.2 },
        { "9", 4.35 },
        { "10", 3.4 },
        { "11", 2.95 },
        { "12", 2.9 },
        { "13", 2.9 },
        { "14", 3.1 },
        { "15", 2.7 },
        { "16", 2.7 },
        { "17", 2.75 },
        { "18", 2.85 },
        { "19", 2.45 },
        { "20", 2.1 },
        { "21", 1.75 },
        { "22", 4.55 },
        { "23", 1.25 }
    };

    public static Dictionary<string, double> TasasPorMes = new Dictionary<string, double>
    {
        { "1", 6.6 },
        { "2", 3.15 },
        { "3", 3.15 },
        { "4", 2.4 },
        { "5", 1.85 },
        { "6", 4.25 },
        { "7", 2.9 },
        { "8", 2.15 },
        { "9", 2.7 },
        { "10", 3.6 },
        { "11", 3.75 },
        { "12", 3.4 }
    };

    public static Dictionary<string, double> TasasPorDia = new Dictionary<string, double>
    {
        { "0", 1.9 },
        { "1", 3.65 },
        { "2", 3.25 },
        { "3", 2.55 },
        { "4", 3.1 },
        { "5", 2.9 },
        { "6", 2.7 }
    };
}


