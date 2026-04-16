namespace Biblioteca;

public class Simulacion
{
    private Bolillero bolillero;

    public Simulacion(Bolillero bolillero) => this.bolillero = bolillero;

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
        int aciertos = 0;
        object lockObj = new object();

        await Task.Run(() =>
        {
            Parallel.For(0, cantidadVeces, i =>
            {
                // Inyectamos AzarRandom para que cada instancia tenga su propio motor de azar
                var bolilleroLocal = new Biblioteca.Bolillero(bolillero.Cantidad, new AzarRandom()); 

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