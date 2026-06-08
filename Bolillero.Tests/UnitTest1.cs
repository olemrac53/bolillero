using Biblioteca;

namespace TestBolilleros;

public class UnitTest1
{
    Bolillero _bolillero;
    Simulacion _simulacion;
    public UnitTest1()
    {
        _bolillero = new Bolillero(10, new AzarFijo(0));
        _simulacion = new Simulacion(_bolillero);
    }

    // ─── TP1: Tests del Bolillero ───────────────────────────────────────────────

    [Fact]
    public void SacarBolilla_DevuelveBolillaCero_YActualizaCantidades()
    {
        int resultado = _bolillero.SacarBolilla();

        Assert.Equal(0, resultado);
        Assert.Equal(9, _bolillero.CantidadDentro());
        Assert.Equal(1, _bolillero.CantidadFuera());
    }

    [Fact]
    public void ReingresarBolillas_DespuesDeSacarUna_VuelveA10()
    {
        _bolillero.SacarBolilla();
        _bolillero.ReingresarBolillas();

        Assert.Equal(10, _bolillero.CantidadDentro());
        Assert.Equal(0, _bolillero.CantidadFuera());
    }

    [Fact]
    public void Jugar_JugadaGanadora_0123_Gana()
    {
        List<int> jugada = new List<int> { 0, 1, 2, 3 };

        bool gano = _bolillero.Jugar(jugada);

        Assert.True(gano);
    }

    [Fact]
    public void Jugar_JugadaPerdedora_421_Pierde()
    {
        List<int> jugada = new List<int> { 4, 2, 1 };

        bool gano = _bolillero.Jugar(jugada);

        Assert.False(gano);
    }

    [Fact]
    public void JugarNVeces_Jugada01_1Vez_Gana1Vez()
    {
        List<int> jugada = new List<int> { 0, 1 };

        int aciertos = _bolillero.JugarNVeces(jugada, 1);

        Assert.Equal(1, aciertos);
    }

    // ─── TP2: Tests de Simulacion ───────────────────────────────────────────────

    [Fact]
    public void SimularSinHilos_JugadaSegura_DevuelveNAciertos()
    {
        List<int> jugada = new List<int> { 0, 1, 2 };

        long aciertos = _simulacion.SimularSinHilos(jugada, 100);

        Assert.Equal(100, aciertos);
    }

    [Fact]
    public void SimularConHilos_JugadaSegura_DevuelveNAciertos()
    {
        List<int> jugada = new List<int> { 0, 1, 2 };

        long aciertos = _simulacion.SimularConHilos(jugada, 100, 4);

        Assert.Equal(100, aciertos);
    }

    [Fact]
    public void Clone_BolilleroClonado_EsIndependienteDelOriginal()
    {
        Bolillero original = new Bolillero(10, new AzarFijo(0));
        Bolillero clon = (Bolillero)original.Clone();

        clon.SacarBolilla();

        Assert.Equal(10, original.CantidadDentro());
        Assert.Equal(9, clon.CantidadDentro());
    }

    // ─── TP3: Test de SimularConHilosAsync ─────────────────────────────────────

    [Fact]
    public async Task SimularConHilosAsync_JugadaSegura_DevuelveNAciertos()
    {
        List<int> jugada = new List<int> { 0, 1, 2 };

        long aciertos = await _simulacion.SimularConHilosAsync(jugada, 100, 4);

        Assert.Equal(100, aciertos);
    }

    // ─── TP4: Tests de SimularParallelAsync ────────────────────────────────────

    // Verifica que Parallel.For acumula correctamente los aciertos cuando la jugada siempre gana.
    [Fact]
    public async Task SimularParallelAsync_JugadaSegura_DevuelveNAciertos()
    {
        List<int> jugada = new List<int> { 0, 1, 2 };

        long aciertos = await _simulacion.SimularParallelAsync(jugada, 100);

        Assert.Equal(100, aciertos);
    }

    // Verifica que el método devuelve 0 cuando la jugada nunca puede ganar.
    [Fact]
    public async Task SimularParallelAsync_JugadaPerdedora_DevuelveCeroAciertos()
    {
        List<int> jugada = new List<int> { 9, 8, 7 };

        long aciertos = await _simulacion.SimularParallelAsync(jugada, 50);

        Assert.Equal(0, aciertos);
    }

    // Verifica el caso borde de una sola jugada ganadora.
    [Fact]
    public async Task SimularParallelAsync_UnaJugadaGanadora_DevuelveUno()
    {
        List<int> jugada = new List<int> { 0, 1 };

        long aciertos = await _simulacion.SimularParallelAsync(jugada, 1);

        Assert.Equal(1, aciertos);
    }
}