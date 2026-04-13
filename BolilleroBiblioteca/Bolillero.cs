namespace Biblioteca;
public class Bolillero
{
    public int Cantidad {get;private set;}
    public List<int> bolillas;
    public List<int> bolillasExtraidas;
    public Random random;

    public Bolillero(int cantidad)
    {
        Cantidad = cantidad;
        bolillas = Enumerable.Range(0, cantidad).ToList();
        bolillasExtraidas = new List<int>();
        random = new Random();
    }

    public int SacarBolilla()
    {
        int index = random.Next(bolillas.Count);
        int valor = bolillas[index];
        bolillasExtraidas.Add(valor);
        return valor;
    }

    public void Reiniciar()
    {
        bolillas.AddRange(bolillasExtraidas);
        bolillasExtraidas.Clear();
    }

    public bool Jugar(List<int> jugada)
    {
        Reiniciar();
        foreach (var numero in jugada)
        {
            if (SacarBolilla() != numero)
                return false;
        }
        return true;
    }
}