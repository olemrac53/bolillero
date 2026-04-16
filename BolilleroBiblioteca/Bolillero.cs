namespace Biblioteca;

public class Bolillero
{
    public int Cantidad { get; private set; }
    private List<int> bolillasDentro;
    private List<int> bolillasFuera;
    private IAzar azar;

    public Bolillero(int cantidad, IAzar azar)
    {
        this.Cantidad = cantidad;
        this.azar = azar;
        this.bolillasDentro = Enumerable.Range(0, cantidad).ToList();
        this.bolillasFuera = new List<int>();
    }

    public int SacarBolilla()
    {
        int index = azar.ObtenerIndice(bolillasDentro.Count);
        int valor = bolillasDentro[index];
        
        bolillasDentro.RemoveAt(index);
        bolillasFuera.Add(valor);
        
        return valor;
    }

    public void ReingresarBolillas()
    {
        bolillasDentro.AddRange(bolillasFuera);
        bolillasFuera.Clear();
    }

    public bool Jugar(List<int> jugada)
    {
        ReingresarBolillas();
        foreach (var numero in jugada)
        {
            if (SacarBolilla() != numero)
                return false;
        }
        return true;
    }

    public int JugarNVeces(List<int> jugada, int cantidad)
    {
        int aciertos = 0;
        for (int i = 0; i < cantidad; i++)
        {
            if (this.Jugar(jugada)) aciertos++;
        }
        return aciertos;
    }

    public int CantidadDentro() => bolillasDentro.Count;
    
    public int CantidadFuera() => bolillasFuera.Count;
}