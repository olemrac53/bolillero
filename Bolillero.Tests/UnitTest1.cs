using Biblioteca;
using Sim;
using System.Collections.Generic;
using Xunit;

namespace TestBolilleros
{
    public class UnitTest1
    {
        // ─── TP1: Tests del Bolillero ───────────────────────────────────────────────

        [Fact]
        public void SacarBolilla_DevuelveBolillaCero_YActualizaCantidades()
        {
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));

            int resultado = miBolillero.SacarBolilla();

            Assert.Equal(0, resultado);
            Assert.Equal(9, miBolillero.CantidadDentro());
            Assert.Equal(1, miBolillero.CantidadFuera());
        }

        [Fact]
        public void ReingresarBolillas_DespuesDeSacarUna_VuelveA10()
        {
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));

            miBolillero.SacarBolilla();
            miBolillero.ReingresarBolillas();

            Assert.Equal(10, miBolillero.CantidadDentro());
            Assert.Equal(0, miBolillero.CantidadFuera());
        }

        [Fact]
        public void Jugar_JugadaGanadora_0123_Gana()
        {
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));
            List<int> jugada = new List<int> { 0, 1, 2, 3 };

            bool gano = miBolillero.Jugar(jugada);

            Assert.True(gano);
        }

        [Fact]
        public void Jugar_JugadaPerdedora_421_Pierde()
        {

            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));
            List<int> jugada = new List<int> { 4, 2, 1 };

            bool gano = miBolillero.Jugar(jugada);

            Assert.False(gano);
        }

        [Fact]
        public void JugarNVeces_Jugada01_1Vez_Gana1Vez()
        {
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));
            List<int> jugada = new List<int> { 0, 1 };

            int aciertos = miBolillero.JugarNVeces(jugada, 1);

            Assert.Equal(1, aciertos);
        }

        // ─── TP2: Tests de Simulacion ───────────────────────────────────────────────

        [Fact]
        public void SimularSinHilos_JugadaSegura_DevuelveNAciertos()
        {
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));
            Simulacion sim = new Simulacion(miBolillero);
            List<int> jugada = new List<int> { 0, 1, 2 };

            long aciertos = sim.SimularSinHilos(jugada, 100);

            Assert.Equal(100, aciertos);
        }
        
        [Fact]
        public void SimularConHilos_JugadaSegura_DevuelveNAciertos()
        {
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));
            Simulacion sim = new Simulacion(miBolillero);
            List<int> jugada = new List<int> { 0, 1, 2 };

            long aciertos = sim.SimularConHilos(jugada, 100, 4);

            Assert.Equal(100, aciertos);
        }

        [Fact]
        public void Clone_BolilleroClonado_EsIndependienteDelOriginal()
        {
            Bolillero original = new Bolillero(10, new AzarFijo(0));
            Bolillero clon = (Bolillero)original.Clone();

            // Sacar bolillas del clon no debe afectar al original
            clon.SacarBolilla();

            Assert.Equal(10, original.CantidadDentro());
            Assert.Equal(9, clon.CantidadDentro());
        }
    }
}