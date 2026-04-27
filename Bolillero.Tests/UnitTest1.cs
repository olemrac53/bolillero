using Biblioteca;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestBolilleros;

public class SimulacionTests
{
    [Fact]
    public async Task SimularAsincronico_DeberiaDarCeroConJugadaImposible()
    {
        // Arrange
        var bolillero = new Bolillero(3, new AzarRandom()); // Bolillas del 0 al 2
        var simulacion = new Simulacion(bolillero); 
        var jugada = new List<int> { 5, 6, 7 }; 

        // Act
        var aciertos = await simulacion.SimularAsincronico(jugada, 1000);

        // Assert
        Assert.Equal(0, aciertos);
    }

    [Fact]
    public async Task SimularAsincronico_DeberiaDarMasDeCeroConJugadaProbable()
    {
        // Arrange
        var bolillero = new Bolillero(3, new AzarRandom()); // Bolillas: 0, 1, 2
        var simulacion = new Simulacion(bolillero); 
        var jugada = new List<int> { 0, 1, 2 };

        // Act
        var aciertos = await simulacion.SimularAsincronico(jugada, 10000);

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
        
        // Assert: Como le pasamos el índice 0, siempre debería sacar la bolilla en la posición 0
        Assert.Equal(0, resultado); 
    }
}