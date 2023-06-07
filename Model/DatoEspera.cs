using Radzen;

namespace Model
{
    public class DatoEspera
    {
        public string Nombre { get; set; } // El nombre de la atracción
        public GoogleMapPosition Posicion { get; set; } // Las coordenadas de la atracción
        public Dictionary<int,int> TiempoEspera { get; set; } // El tiempo de espera en minutos
    }
}
