namespace Biblioteca;
using System.Collections.Generic; // No te olvides de esto para las List

public class Bolillero
{
    public int Cantidad { get; private set; }
    private readonly int _cantidadInicial;
    private List<int> _bolillas = new();
    private List<int> bolillasFuera = new();
    private IAzar azar;

    public Bolillero(int cantidad, IAzar azar)
    {
        _cantidadInicial = cantidad;
        this.azar = azar;
        ReiniciarBolillero();
    }

    public int SacarBolilla()
    {
        int index = azar.ObtenerIndice(_bolillas.Count);
        int valor = _bolillas[index];
        
        _bolillas.RemoveAt(index);
        bolillasFuera.Add(valor); 
        
        return valor;
    }

    public void ReiniciarBolillero()
    {
        _bolillas = new List<int>();
        bolillasFuera = new List<int>(); 

        for (int i = 0; i < _cantidadInicial; i++)
        {
            _bolillas.Add(i);
        }
    }

    public void ReingresarBolillas()
    {
        _bolillas.AddRange(bolillasFuera);
        bolillasFuera.Clear();
        
        _bolillas.Sort();
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

    

    public int CantidadDentro() => _bolillas.Count;
    
    public int CantidadFuera() => bolillasFuera.Count;
}