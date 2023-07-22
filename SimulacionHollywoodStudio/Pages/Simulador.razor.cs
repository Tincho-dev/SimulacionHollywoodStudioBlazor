using Microsoft.JSInterop;
using Model;
using Radzen;

namespace SimulacionHollywoodStudio.Pages;

public partial class Simulador
{
    public double IngresosEsperados { get; set; }
    public string Respuesta { get; set; } = String.Empty;
    public string Estado { get; set; } = "Esperando...";
    public string? AlertMessage { get; set; }
    //Promedio Mensual
    public double TiempoEsperaPromedioMensual { get; set; }
    public IEnumerable<DatoEspera> TiemposDeEsperaPromedioMensual { get; set; } = new List<DatoEspera>();
    public double TiempoEsperaPromedioRRMensual { get; set; }
    public double TiempoEsperaPromedioMFMensual { get; set; }
    //Promedio por dia
    public IEnumerable<DatoEspera> TiemposDeEspera { get; set; } = new List<DatoEspera>();
    public double TiempoEsperaPromedio { get; set; }
    public double TiempoEsperaPromedioRR { get; set; }
    public double TiempoEsperaPromedioMF { get; set; }
    //Cambio de dia
    DateTime selectedDate = DateTime.Today;
    IEnumerable<DatoEspera>? tiemposDeEsperaFiltrados;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        simulador.OnChange += StateHasChanged;
        OnDateChanged(selectedDate);
    }

    double GetTiempoPromedio(Atracciones name)
    => tiemposDeEsperaFiltrados.FirstOrDefault(a => a.Nombre == name)
        ?.TiempoEspera.Where(k => k.Key.Date == selectedDate.Date).DefaultIfEmpty()
        .Average(t => t.Value) ?? 0;

    async void OnDateChanged(DateTime? date)
    {
        if (date.HasValue)
        {
            selectedDate = date.Value;
            tiemposDeEsperaFiltrados = TiemposDeEspera.Where(d => d.TiempoEspera.Keys.Any(k => k.Date == selectedDate.Date));
            if (tiemposDeEsperaFiltrados.Any())
            {
                TiempoEsperaPromedioRR = GetTiempoPromedio(Atracciones.RiseOfTheResistance);
                TiempoEsperaPromedioMF = GetTiempoPromedio(Atracciones.MilleniumFalcom);
                TiempoEsperaPromedio = (TiempoEsperaPromedioMF + TiempoEsperaPromedioRR);
                await JSRuntime.InvokeVoidAsync("plotFunction", TiempoEsperaPromedio);
            }
        }
    }

    void ChangeDate(int days)
    {
        var newDate = selectedDate.AddDays(days);
        selectedDate = days < 0 && selectedDate.Day == 1
            ? new DateTime(
                selectedDate.Year,
                selectedDate.Month,
                DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month))
            : (
                newDate.Month != selectedDate.Month
                    ? (
                        days > 0
                        ? new DateTime(selectedDate.Year, selectedDate.Month, 1)
                        : new DateTime(
                            selectedDate.Year,
                            selectedDate.Month,
                            DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month)))
                    : newDate
              );
        OnDateChanged(selectedDate);
    }

    private async Task HandleButtonClickAsync()
    {
        Estado = "Simulando...";
        AlertMessage = string.Empty;
        if (!ValidarIngresosEsperados())
            return;
        SimulacionYAsignacionResultados();
        Estado = string.IsNullOrEmpty(Respuesta) ? "Error: No se recibió respuesta" : "Simulacion completada.";
        await JSRuntime.InvokeVoidAsync("plotFunction", TiempoEsperaPromedio);
        OnDateChanged(selectedDate);
    }

    private bool ValidarIngresosEsperados()
    {
        if (IngresosEsperados > 175 || IngresosEsperados < 0)
        {
            AlertMessage = "El ingreso esperado supera el límite. Por favor ingrese un valor menor.";
            Estado = "Error en la entrada.";
            return false;
        }
        return true;
    }

    private void SimulacionYAsignacionResultados()
    {
        simulador.Simular((int)IngresosEsperados * 1000000);
        TiemposDeEspera = simulador.TiemposDeEspera;
        TiempoEsperaPromedioMensual = TiempoEsperaPromedio = simulador.TiemposEsperaPromedio;
        tiemposDeEsperaFiltrados = simulador.TiemposDeEspera;
        var tiempoEsperaRiseOfResistance = ObtenerTiempoEspera(Atracciones.RiseOfTheResistance);
        var tiempoEsperaMilleniumFalcom = ObtenerTiempoEspera(Atracciones.MilleniumFalcom);
        TiempoEsperaPromedioRRMensual = TiempoEsperaPromedioRR = tiempoEsperaRiseOfResistance;
        TiempoEsperaPromedioMFMensual = TiempoEsperaPromedioMF = tiempoEsperaMilleniumFalcom;
    }

    private double ObtenerTiempoEspera(Atracciones atraccion) =>
        simulador.TiemposDeEspera.FirstOrDefault(a => a.Nombre == atraccion)
        ?.TiempoEspera.Where(t => t.Value > 0).DefaultIfEmpty()
        .Average(t => t.Value) ?? 0;

    private async Task HandleButtonMensualClick()
    {
        // Calcular el promedio mensual de los tiempos de espera para todas las atracciones
        TiempoEsperaPromedioRR = TiempoEsperaPromedioRRMensual;
        TiempoEsperaPromedioMF = TiempoEsperaPromedioMFMensual;
        // Actualizar el gráfico con el nuevo promedio mensual
        await JSRuntime.InvokeVoidAsync("plotFunction", simulador.TiemposEsperaPromedio);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JSRuntime.InvokeVoidAsync("plotFunction");
    }
}
