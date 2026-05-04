namespace Biblioteca;

public interface IAzar
{
    int ObtenerIndice(int max);
}

public class AzarRandom : IAzar
{
    private Random _random = new Random();
    public int ObtenerIndice(int max) => _random.Next(max);
}

public class AzarFijo : IAzar
{
    private int _indiceFijo;
    public AzarFijo(int indice) => _indiceFijo = indice;
    public int ObtenerIndice(int max) => _indiceFijo;
}