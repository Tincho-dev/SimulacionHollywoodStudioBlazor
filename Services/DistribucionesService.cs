namespace Services;

public class DistribucionesService : IDistribucionesService
{
    private readonly IGeneradorService GeneradorService;
    private int Semilla, ConstanteAditiva,
        ConstanteMultiplicativa, Modulo, Digitos;

    public DistribucionesService(IGeneradorService generadorService)
    {
        GeneradorService = generadorService;
        Semilla = 12930;
        ConstanteAditiva = 234;
        ConstanteMultiplicativa = 232;
        Modulo = 567;
        Digitos = 2;
    }

    public double GenerarNumeroAleatorio()
        => GeneradorService.CongruencialMixto(
            Semilla++,
            ConstanteAditiva,
            ConstanteMultiplicativa,
            Modulo,
            Digitos);
}
