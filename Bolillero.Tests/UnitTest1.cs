using Biblioteca;

namespace TestBolilleros;

public class SimulacionTests
{
    [Fact]
    public async Task SimularParallelAsync_DeberiaDarCeroConJugadaImposible()
    {
        // Arrange
        var bolillero = new Biblioteca.Bolillero(3, new AzarRandom()); // Bolillas del 0 al 2
        
        var simulacion = new Simulacion(bolillero); 
        var jugada = new List<int> { 5, 6, 7 }; 

        // Act
        var aciertos = await simulacion.SimularParallelAsync(jugada, 1000);

        // Assert
        Assert.Equal(0, aciertos);
    }

    [Fact]
    public async Task SimularParallelAsync_DeberiaDarMasDeCeroConJugadaProbable()
    {
        // Arrange
        var bolillero = new Biblioteca.Bolillero(3, new AzarRandom()); // Bolillas: 0, 1, 2
        var simulacion = new Simulacion(bolillero); 
        var jugada = new List<int> { 0, 1, 2 };

        // Act
        var aciertos = await simulacion.SimularParallelAsync(jugada, 10_000);

        // Assert
        Assert.True(aciertos > 0, "Esperábamos al menos un acierto.");
    }

    [Fact]
    public void SacarBolilla_ConAzarFijo_DeberiaDarSiempreElMismoResultado()
    {
        // Arrange: Forzamos a que el azar siempre devuelva el índice 0
        var bolillero = new Bolillero(10, new AzarFijo(0)); 
        
        // Act
        var resultado = bolillero.SacarBolilla();
        
        // Assert: Como le pasamos el índice 0, siempre debería sacar la bolilla 0
        Assert.Equal(0, resultado); 
    }
}