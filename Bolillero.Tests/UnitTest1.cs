using Biblioteca;
using System.Collections.Generic;
using Xunit;

namespace TestBolilleros
{
    public class UnitTest1 
    {


        [Fact]
        public void Prueba_SacarBolilla_AzarFijo()
        {
            // Verificamos que sacar una bolilla funcione con el IAzar
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0)); 
            
            int resultado = miBolillero.SacarBolilla();
            
            Assert.Equal(0, resultado); 
        }

        [Fact]
        public void Prueba_Jugada_Ganadora()
        {
            // Arrange: Bolillero que siempre saca la posición 0.
            // Si las bolillas son 0, 1, 2... siempre sacará el 0.
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0)); 
            
            // Si apostamos al 0, deberíamos ganar.
            List<int> jugada = new List<int> { 0 }; 

            // Act
            bool gano = miBolillero.Jugar(jugada);

            // Assert
            Assert.True(gano);
        }

        [Fact]
        public void Prueba_Jugada_Perdedora()
        {
            // Arrange
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0)); 
            
            // Si siempre sale el 0, y apostamos al 5, deberíamos perder.
            List<int> jugada = new List<int> { 5 }; 

            // Act
            bool gano = miBolillero.Jugar(jugada);

            // Assert       
            Assert.False(gano);
        }

        [Fact]
        public void Prueba_VolverAColocarBolillas()
        {
            // Arrange
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0)); 
            
            // Act: Sacamos una bolilla y luego las volvems a colocar
            miBolillero.SacarBolilla();
            miBolillero.ReiniciarBolillero();

            // Assert: Al volver a jugar, el bolillero debería estar lleno de nuevo.
            int bolilla = miBolillero.SacarBolilla();
            Assert.Equal(0, bolilla);
        }

        [Fact]
        public void Prueba_JugarNVeces_Bolillero()
        {
            // Arrange
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0)); 
            List<int> jugada = new List<int> { 0 }; // Jugada ganadora segura
            
            // Act: Jugamos 5 veces
            long aciertos = miBolillero.JugarNVeces(jugada, 5);

            // Assert: Deberíamos haber acertado las 5 veces, nota. Con el sort en el bolillero.cs, volveria a la posicion original
            Assert.Equal(5, aciertos);
        }
    }
}