using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca;

public class Simulacion
{
    private Bolillero bolillero;

    public Simulacion(Bolillero bolillero)
    {
        this.bolillero = bolillero;
    }

    public int JugarNVeces(List<int> jugada, int cantidadVeces)
    {
        int aciertos = 0;
        for (int i = 0; i < cantidadVeces; i++)
        {
            if (bolillero.Jugar(jugada))
            {
                aciertos++;
                
            }
        }
        return aciertos;
    }

    // Le cambiamos el nombre para sacar el "Parallel" y usar la terminología de clase
    public async Task<int> SimularAsincronico(List<int> jugada, int cantidadVeces)
    {
        // 1. Armamos una lista para ir guardando todas las simulaciones, 
        // similar a como guardaban hpTask, kodakTask, etc.
        List<Task<int>> tareas = new List<Task<int>>();

        for (int i = 0; i < cantidadVeces; i++)
        {
            // 2. Envolvemos la simulación en un Task y la agregamos a la lista
            tareas.Add(Task.Run(() =>
            {
                // Cada tarea necesita su propio bolillero para no mezclar sus bolillas con las de otra tarea
                var bolilleroLocal = new Bolillero(bolillero.Cantidad, new AzarRandom()); 

                if (bolilleroLocal.Jugar(jugada))
                {
                    return 1; // Si gana, esta tarea devuelve 1 acierto
                }
                return 0; // Si pierde, devuelve 0
            }));
        }

        // 3. Usamos await Task.WhenAll para esperar que terminen todas juntas, devolviendo un arreglo de enteros
        int[] resultados = await Task.WhenAll(tareas);

        // 4. Recorremos los resultados y los sumamos
        int totalAciertos = 0;
        foreach (int resultado in resultados)
        {
            totalAciertos += resultado;
        }

        return totalAciertos;
    }
}