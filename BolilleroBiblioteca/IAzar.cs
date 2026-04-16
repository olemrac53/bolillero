using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biblioteca;


namespace Biblioteca;

public interface IAzar
{
    int ObtenerIndice(int max);
}

public class AzarRandom : IAzar
{
    private Random random = new Random();
    public int ObtenerIndice(int max) => random.Next(max);
}

public class AzarFijo : IAzar
{
    private int indiceFijo;
    public AzarFijo(int indice) => this.indiceFijo = indice;
    public int ObtenerIndice(int max) => indiceFijo;
}