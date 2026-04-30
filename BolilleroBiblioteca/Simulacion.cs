namespace Biblioteca;

public class Simulacion
{
    private Bolillero bolillero;

    public Simulacion(Bolillero bolillero) => this.bolillero = bolillero;

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

    public async Task<long> SimularConHilosAsync(List<int> jugada, int cantidadVeces, int cantidadHilos)
    // TODO: PASAR EL ASYNC A LONG EN UN VECTOR, PARA QUE DEVUELVA LA CANTIDAD DE ACERTOS EN CADA HILO, Y LUEGO SUMARLOS
    {
        // 1. Armamos una lista para ir guardando todas las simulaciones, 
        var tareas = new Task<int>[cantidadHilos];

        for (int i = 0; i < cantidadVeces; i++)
        {
            // 2. Envolvemos la simulación en un Task y la agregamos a la lista
            tareas[i] = Task.Run(() =>
            {
                var clon = new Bolillero(bolillero.Cantidad, new AzarRandom());

                if (clon.Jugar(jugada))
                {
                    return 1; // Si gana, esta tarea devuelve 1 acierto
                }
                return 0; // Si pierde, devuelve 0
            });
    }

    // 3. Usamos await Task.WhenAll para esperar que terminen todas juntas, devolviendo un arreglo de enteros
    int[] resultados = await Task.WhenAll(tareas);

    //TODO: ESTE AWAIT NECESITO QUE FUNCIONE CON EL METODO LONG QUE NO TIENE ASYNC, PARA QUE DEVUELVA UN VECTOR DE LONGS DE FORMA CORRECTA

    // 4. Recorremos los resultados y los sumamos
    int totalAciertos = 0;
        foreach (int resultado in resultados)
        {
            totalAciertos += resultado;
        }

        return totalAciertos;
    }
}