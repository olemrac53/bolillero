using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca;

namespace Sim
{
    public class Simulacion(Biblioteca.Bolillero bolillero)
    {
        private Biblioteca.Bolillero bolillero = bolillero;

        // 1. PRIMER MÉTODO
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

        // 2. SEGUNDO MÉTODO
        public async Task<int> SimularParallelAsync(List<int> jugada, int cantidadVeces)
        {
            int aciertos = 0;
            object lockObj = new object();
            int totalBolillas = bolillero.CantidadDentro() + bolillero.CantidadFuera();

            await Task.Run(() =>
            {
                Parallel.For(0, cantidadVeces, i =>
                {
                    // SOLUCIÓN: Pasamos new AzarRandom() directamente como segundo parámetro del constructor
                    var bolilleroLocal = new Biblioteca.Bolillero(totalBolillas, new AzarRandom()); 

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

        // 3. TERCER MÉTODO
        public async Task<int> SimularConHilosAsync(List<int> jugada, int cantidadVeces)
        {
            int aciertos = 0;
            object lockObj = new object();
            int totalBolillas = bolillero.CantidadDentro() + bolillero.CantidadFuera();

            int cantidadHilos = Environment.ProcessorCount;
            int iteracionesPorHilo = cantidadVeces / cantidadHilos;
            int iteracionesSobrantes = cantidadVeces % cantidadHilos;

            var tareas = new List<Task>();

            for (int i = 0; i < cantidadHilos; i++)
            {
                int iteraciones = iteracionesPorHilo + (i == 0 ? iteracionesSobrantes : 0);
                
                tareas.Add(Task.Run(() =>
                {
                    int aciertosLocales = 0;
                    
                    // SOLUCIÓN: Pasamos new AzarRandom() directamente como segundo parámetro del constructor
                    var bolilleroLocal = new Biblioteca.Bolillero(totalBolillas, new AzarRandom());

                    for (int j = 0; j < iteraciones; j++)
                    {
                        if (bolilleroLocal.Jugar(jugada))
                        {
                            aciertosLocales++;
                        }
                    }

                    lock (lockObj)
                    {
                        aciertos += aciertosLocales;
                    }
                }));
            }

            await Task.WhenAll(tareas);
            
            return aciertos;
        }
    }
}