using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biblioteca;

namespace Sim
{
    public class Simulacion
    {
        private Biblioteca.Bolillero bolillero;

        public Simulacion(Biblioteca.Bolillero bolillero)
        {
            this.bolillero = bolillero;
        }

        // Método esperado por el test SimularSinHilos_JugadaSegura_DevuelveNAciertos
        public long SimularSinHilos(List<int> jugada, long cantidadVeces)
        {
            long aciertos = 0;
            for (long i = 0; i < cantidadVeces; i++)
            {
                if (bolillero.Jugar(jugada))
                    aciertos++;
            }
            return aciertos;
        }

        // Método esperado por el test SimularConHilos_JugadaSegura_DevuelveNAciertos
        // Nota: Toma "hilos" como tercer parámetro y no es Async en la firma de tus tests
        public long SimularConHilos(List<int> jugada, long cantidadVeces, int cantidadHilos)
        {
            long aciertos = 0;
            object lockObj = new object();

            long iteracionesPorHilo = cantidadVeces / cantidadHilos;
            long iteracionesSobrantes = cantidadVeces % cantidadHilos;

            var tareas = new List<Task>();

            for (int i = 0; i < cantidadHilos; i++)
            {
                long iteraciones = iteracionesPorHilo + (i == 0 ? iteracionesSobrantes : 0);
                
                // Usamos el Clone() para que cada hilo trabaje con su propio bolillero en memoria sin pisarse
                var bolilleroLocal = (Biblioteca.Bolillero)bolillero.Clone();

                tareas.Add(Task.Run(() =>
                {
                    long aciertosLocales = 0;
                    
                    for (long j = 0; j < iteraciones; j++)
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

            Task.WaitAll(tareas.ToArray());
            
            return aciertos;
        }
    }
}