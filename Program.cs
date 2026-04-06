using Biblioteca;

class Program
{
    static async Task Main(string[] args)
    {
        var bolillero = new Bolillero(10);
        var simulacion = new Simulacion(bolillero);
        var jugada = new List<int> { 0,1,2 };

        int aciertos = await simulacion.SimularParallelAsync(jugada, 1000);

        Console.WriteLine($"Aciertos en 1000 intentos: {aciertos}");
    }
}