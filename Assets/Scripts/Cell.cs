// Clase base que representa una celda del calabozo
public class Cell
{
    public bool visited = false; // Marca si fue recorrida
    public bool[] status = new bool[4]; // 0=Arriba, 1=Abajo, 2=Derecha, 3=Izquierda
}