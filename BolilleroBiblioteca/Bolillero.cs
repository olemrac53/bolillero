namespace Biblioteca;
using System.Collections.Generic;

public class Bolillero : ICloneable
{
    public int Cantidad { get; private set; }
    private List<int> _bolillas;
    private List<int> _bolillasFuera;
    private IAzar _azar;

    public Bolillero(int cantidad, IAzar azar)
    {
        Cantidad = cantidad;
        _azar = azar;
        ReiniciarBolillero();
    }

    public int SacarBolilla()
    {
        int index = _azar.ObtenerIndice(_bolillas.Count);
        int valor = _bolillas[index];

        _bolillas.RemoveAt(index);
        _bolillasFuera.Add(valor);

        return valor;
    }

    public void ReiniciarBolillero()
    {
        _bolillas = new List<int>();
        _bolillasFuera = new List<int>();

        for (int i = 0; i < Cantidad; i++)
        {
            _bolillas.Add(i);
        }
    }

    public void ReingresarBolillas()
    {
        _bolillas.AddRange(_bolillasFuera);
        _bolillasFuera.Clear();
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
            if (Jugar(jugada)) aciertos++;
        }
        return aciertos;
    }

    public int CantidadDentro() => _bolillas.Count;

    public int CantidadFuera() => _bolillasFuera.Count;

    // ICloneable: devuelve una copia independiente del bolillero con su propio estado
    public object Clone()
    {
        var clon = new Bolillero(Cantidad, _azar);
        clon._bolillas = new List<int>(_bolillas);
        clon._bolillasFuera = new List<int>(_bolillasFuera);
        return clon;
    }
}