namespace Biblioteca;

public class Simulacion
{
    private Bolillero _bolillero;

    public Simulacion(Bolillero bolillero) => _bolillero = bolillero;

    // Simula jugando n veces de forma secuencial, sin hilos
    public long SimularSinHilos(List<int> jugada, int cantidadVeces)
    {
        long aciertos = 0;
        for (int i = 0; i < cantidadVeces; i++)
        {
            // Cada iteración trabaja con un clon independiente para no contaminar el estado
            var clon = (Bolillero)_bolillero.Clone();
            if (clon.Jugar(jugada))
                aciertos++;
        }
        return aciertos;
    }

    // Simula jugando n veces repartiendo el trabajo en varios hilos
    public long SimularConHilos(List<int> jugada, int cantidadVeces, int cantidadHilos)
    {
        // Dividimos la carga entre los hilos disponibles
        int jugadasPorHilo = cantidadVeces / cantidadHilos;
        int resto = cantidadVeces % cantidadHilos;

        var tareas = new Task<long>[cantidadHilos];

        for (int i = 0; i < cantidadHilos; i++)
        {
            // El último hilo absorbe el resto si la división no es exacta
            int jugadasDeEsteHilo = jugadasPorHilo + (i == cantidadHilos - 1 ? resto : 0);

            tareas[i] = Task.Run(() =>
            {
                long aciertosLocales = 0;
                for (int j = 0; j < jugadasDeEsteHilo; j++)
                {
                    // Cada hilo trabaja con su propio clon, sin concurrencia sobre el bolillero
                    var clon = (Bolillero)_bolillero.Clone();
                    if (clon.Jugar(jugada))
                        aciertosLocales++;
                }
                return aciertosLocales;
            });
        }

        // Esperamos que todos los hilos terminen y sumamos los resultados
        Task.WaitAll(tareas);

        long totalAciertos = 0;
        foreach (var tarea in tareas)
            totalAciertos += tarea.Result;

        return totalAciertos;
    }
}