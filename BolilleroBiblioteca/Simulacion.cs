namespace Biblioteca;

public class Simulacion
{
    private readonly Bolillero _bolillero;

    public Simulacion(Bolillero bolillero) => _bolillero = bolillero;

    // Ejecuta la jugada N veces de forma secuencial y cuenta los aciertos.
    public long SimularSinHilos(List<int> jugada, int cantidadVeces)
    {
        long aciertos = 0;

        for (int i = 0; i < cantidadVeces; i++)
        {
            // Usamos un clon para que cada jugada empiece con el bolillero en su estado inicial.
            var clon = (Bolillero)_bolillero.Clone();
            if (clon.Jugar(jugada))
                aciertos++;
        }

        return aciertos;
    }

    // Reparte las jugadas entre varios hilos (Tasks) y suma los resultados al final.
    public long SimularConHilos(List<int> jugada, int cantidadVeces, int cantidadHilos)
    {
        int jugadasPorHilo = cantidadVeces / cantidadHilos;
        int resto = cantidadVeces % cantidadHilos;

        var tareas = new Task<long>[cantidadHilos];

        for (int i = 0; i < cantidadHilos; i++)
        {
            int indiceHilo = i;
            // El último hilo absorbe el resto si la división no es exacta.
            int jugadasDeEsteHilo = jugadasPorHilo + (indiceHilo == cantidadHilos - 1 ? resto : 0);

            tareas[indiceHilo] = Task.Run(() =>
            {
                long aciertosLocales = 0;
                for (int j = 0; j < jugadasDeEsteHilo; j++)
                {
                    var clon = (Bolillero)_bolillero.Clone();
                    if (clon.Jugar(jugada))
                        aciertosLocales++;
                }
                return aciertosLocales;
            });
        }

        // Esperamos que todos los hilos terminen antes de sumar.
        Task.WaitAll(tareas);

        long totalAciertos = 0;
        foreach (var tarea in tareas)
            totalAciertos += tarea.Result;

        return totalAciertos;
    }

    // Igual que SimularConHilos pero usando async/await para no bloquear el hilo llamador.
    public async Task<long> SimularConHilosAsync(List<int> jugada, int cantidadVeces, int cantidadHilos)
    {
        int jugadasPorHilo = cantidadVeces / cantidadHilos;
        int resto = cantidadVeces % cantidadHilos;

        var tareas = new Task<long>[cantidadHilos];

        for (int i = 0; i < cantidadHilos; i++)
        {
            int indiceHilo = i;
            int jugadasDeEsteHilo = jugadasPorHilo + (indiceHilo == cantidadHilos - 1 ? resto : 0);

            tareas[indiceHilo] = Task.Run(() =>
            {
                long aciertosLocales = 0;
                for (int j = 0; j < jugadasDeEsteHilo; j++)
                {
                    var clon = (Bolillero)_bolillero.Clone();
                    if (clon.Jugar(jugada))
                        aciertosLocales++;
                }
                return aciertosLocales;
            });
        }

        // await libera el hilo llamador mientras espera, a diferencia de Task.WaitAll que lo bloquea.
        long[] resultadosParciales = await Task.WhenAll(tareas);
        return resultadosParciales.Sum();
    }

    // Usa Parallel.For para distribuir las jugadas entre los núcleos disponibles de forma asincrónica.
    public async Task<long> SimularParallelAsync(List<int> jugada, int cantidadVeces)
    {
        long totalAciertos = 0;

        // Task.Run evita que el bloqueo de Parallel.For afecte al hilo llamador.
        await Task.Run(() =>
        {
            Parallel.For(0, cantidadVeces, _ =>
            {
                var clon = (Bolillero)_bolillero.Clone();

                if (clon.Jugar(jugada))
                    // Interlocked.Increment suma de forma atómica para evitar condiciones de carrera entre hilos.
                    Interlocked.Increment(ref totalAciertos);
            });
        });

        return totalAciertos;
    }
}