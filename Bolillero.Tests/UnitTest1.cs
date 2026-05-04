using Biblioteca;
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
            // AzarFijo(0) siempre saca el índice 0: con el bolillero ordenado [0,1,2,3...] saca 0, luego 1, luego 2, luego 3
            Bolillero miBolillero = new Bolillero(10, new AzarFijo(0));
            List<int> jugada = new List<int> { 0, 1, 2, 3 };

            bool gano = miBolillero.Jugar(jugada);

            Assert.True(gano);
        }

        [Fact]
        public void Jugar_JugadaPerdedora_421_Pierde()
        {
            // AzarFijo(0) saca siempre el primero de la lista ordenada: 0, 1, 2...
            // La jugada empieza por 4, nunca va a matchear con 0
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