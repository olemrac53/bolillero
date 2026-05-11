using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca;

namespace Sim
{
    public class Simulacion(Biblioteca.Bolillero bolillero)
    {
        private Biblioteca.Bolillero bolillero = bolillero;

        public int JugarNVeces(List<int> jugada, int n)
        {
            int aciertos = 0;
            for (int i = 0; i < n; i++)
            {
                if (bolillero.Jugar(jugada))
                    aciertos++;
            }
            return aciertos;
        }

        public async Task<int> SimularParallelAsync(List<int> jugada, int cantidadVeces)
        {
            return await SimularConHilosAsync(jugada, cantidadVeces);
        }

        public async Task<int> SimularConHilosAsync(List<int> jugada, int cantidadVeces)
        {
            int aciertos = 0;
            object lockObj = new object();

            [cite_start]
            int totalBolillas = bolillero.CantidadDentro() + bolillero.CantidadFuera();

            await Task.Run(() =>
            {
                Parallel.For(0, cantidadVeces, i =>
                {
                    var bolilleroLocal = new Biblioteca.Bolillero(totalBolillas);
                    
                    [cite_start]
                    bolilleroLocal.SetAzar(new AzarRandom());

                    if (bolilleroLocal.Jugar(jugada))
                    {
                        lock (lockObj)
                        {
                            aciertos++;
                        }
                    }
                });
            });

            return aciertos;
        }
    }
}