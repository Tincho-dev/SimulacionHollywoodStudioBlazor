using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using SimulacionHollywoodStudio;
using SimulacionHollywoodStudio.Shared;
using Radzen.Blazor;
using Services;

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
    const double PrecioEntrada = 109;
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
                TiempoEsperaPromedioRR = tiemposDeEsperaFiltrados.FirstOrDefault(a => a.Nombre == Atracciones.RiseOfTheResistance)?.TiempoEspera.Where(k => k.Key.Date == selectedDate.Date).DefaultIfEmpty().Average(t => t.Value) ?? 0;
                TiempoEsperaPromedioMF = tiemposDeEsperaFiltrados.FirstOrDefault(a => a.Nombre == Atracciones.MilleniumFalcom)?.TiempoEspera.Where(k => k.Key.Date == selectedDate.Date).DefaultIfEmpty().Average(t => t.Value) ?? 0;
                TiempoEsperaPromedio = (TiempoEsperaPromedioMF + TiempoEsperaPromedioRR);
                await JSRuntime.InvokeVoidAsync("plotFunction", TiempoEsperaPromedio);
            }
        }
    }

    void ChangeDate(int days)
    {
        var newDate = selectedDate.AddDays(days);
        selectedDate = days < 0 && selectedDate.Day == 1 ? new DateTime(selectedDate.Year, selectedDate.Month, DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month)) : (newDate.Month != selectedDate.Month ? (days > 0 ? new DateTime(selectedDate.Year, selectedDate.Month, 1) : new DateTime(selectedDate.Year, selectedDate.Month, DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month))) : newDate);
        OnDateChanged(selectedDate);
    }

    private async Task HandleButtonClickAsync()
    {
        Estado = "Simulando...";
        AlertMessage = string.Empty;
        // Validación de los ingresos esperados.
        if (!ValidarIngresosEsperados())
        {
            return;
        }

        // Simulación y asignación de los resultados
        SimulacionYAsignacionResultados();
        // Comprobación y manejo de los resultados
        Estado = string.IsNullOrEmpty(Respuesta) ? "Error: No se recibió respuesta" : "Simulacion completada.";
        // Actualización de la gráfica
        await JSRuntime.InvokeVoidAsync("plotFunction", TiempoEsperaPromedio);
        // Actualización de la fecha
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
        var simulacionResult = simulador.Simular((int)IngresosEsperados * 1000000, PrecioEntrada);
        Respuesta = simulacionResult.Respuesta;
        TiemposDeEspera = simulacionResult.TiemposDeEspera;
        TiempoEsperaPromedioMensual = TiempoEsperaPromedio = simulacionResult.TiempoEsperaPromedio;
        var tiempoEsperaRiseOfResistance = ObtenerTiempoEspera(Atracciones.RiseOfTheResistance);
        var tiempoEsperaMilleniumFalcom = ObtenerTiempoEspera(Atracciones.MilleniumFalcom);
        TiempoEsperaPromedioRRMensual = TiempoEsperaPromedioRR = tiempoEsperaRiseOfResistance;
        TiempoEsperaPromedioMFMensual = TiempoEsperaPromedioMF = tiempoEsperaMilleniumFalcom;
    }

    private double ObtenerTiempoEspera(Atracciones atraccion) => 
        TiemposDeEspera.FirstOrDefault(a => a.Nombre == atraccion)
        ?.TiempoEspera.Where(t => t.Value > 0).DefaultIfEmpty()
        .Average(t => t.Value) ?? 0;

    private async Task HandleButtonMensualClick()
    {
        // Calcular el promedio mensual de los tiempos de espera para todas las atracciones
        TiempoEsperaPromedioRR = TiempoEsperaPromedioRRMensual;
        TiempoEsperaPromedioMF = TiempoEsperaPromedioMFMensual;
        TiempoEsperaPromedio = TiempoEsperaPromedioMensual;
        // Actualizar el gráfico con el nuevo promedio mensual
        await JSRuntime.InvokeVoidAsync("plotFunction", TiempoEsperaPromedio);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JSRuntime.InvokeVoidAsync("plotFunction");
    }
}
