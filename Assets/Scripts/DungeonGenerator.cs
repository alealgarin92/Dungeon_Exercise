// Refactor del código original con comentarios explicativos
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    // Clase que representa cada celda del calabozo
    public class Cell
    {
        public bool visited = false; // Indica si la celda ya fue recorrida durante la generación
        public bool[] status = new bool[4]; // Estado de las paredes: 0=Arriba, 1=Abajo, 2=Derecha, 3=Izquierda
    }

    [SerializeField] private Vector2 _dungeonSize; // Tamaño del calabozo en celdas (ancho x alto)
    [SerializeField] private int _startPos = 0; // Celda de inicio para comenzar a generar el laberinto
    [SerializeField] private GameObject[] _rooms; // Prefabs de habitaciones posibles a instanciar
    [SerializeField] private Vector2 offset; // Separación entre habitaciones al colocarlas en el mundo

    private List<Cell> _board; // Lista que representa el tablero del calabozo

    void Start()
    {
        GenerateMaze(); // Comienza la generación del laberinto al iniciar el juego
    }

    void GenerateMaze()
    {
        CreateBoard(); // Inicializa el tablero con celdas vacías

        int currentCell = _startPos; // Celda inicial
        Stack<int> path = new Stack<int>(); // Pila para guardar el camino recorrido

        int steps = 0;
        while (steps < 1000) // Límite de pasos para evitar bucles infinitos
        {
            steps++;
            _board[currentCell].visited = true; // Marca la celda como visitada

            // Si se llegó a la última celda, se detiene la generación
            if (currentCell == _board.Count - 1) break;

            List<int> neighbors = CheckNeighbors(currentCell); // Revisa vecinos disponibles

            if (neighbors.Count == 0)
            {
                // Si no hay vecinos, retrocede en el camino
                if (path.Count == 0) break;
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell); // Guarda la celda actual en la pila
                int newCell = neighbors[Random.Range(0, neighbors.Count)]; // Elige un vecino aleatorio

                SetPassage(currentCell, newCell); // Abre pasaje entre celdas
                currentCell = newCell; // Avanza a la nueva celda
            }
        }

        GenerateRooms(); // Crea visualmente las habitaciones en el mundo
    }

    void CreateBoard()
    {
        _board = new List<Cell>();
        int totalCells = Mathf.FloorToInt(_dungeonSize.x * _dungeonSize.y); // Calcula el número total de celdas
        for (int i = 0; i < totalCells; i++)
        {
            _board.Add(new Cell()); // Añade una nueva celda al tablero
        }
    }

    void SetPassage(int current, int next)
    {
        // Determina la dirección entre dos celdas y abre las paredes en consecuencia
        if (next > current)
        {
            if (next - 1 == current) // Derecha
            {
                _board[current].status[2] = true;
                _board[next].status[3] = true;
            }
            else // Abajo
            {
                _board[current].status[1] = true;
                _board[next].status[0] = true;
            }
        }
        else
        {
            if (next + 1 == current) // Izquierda
            {
                _board[current].status[3] = true;
                _board[next].status[2] = true;
            }
            else // Arriba
            {
                _board[current].status[0] = true;
                _board[next].status[1] = true;
            }
        }
    }

    void GenerateRooms()
    {
        for (int i = 0; i < _dungeonSize.x; i++)
        {
            for (int j = 0; j < _dungeonSize.y; j++)
            {
                int index = Mathf.FloorToInt(i + j * _dungeonSize.x);
                Cell currentCell = _board[index];

                if (currentCell.visited)
                {
                    int randomRoom = Random.Range(0, _rooms.Length); // Selecciona una habitación aleatoria
                    Vector3 position = new Vector3(i * offset.x, 0f, -j * offset.y); // Calcula la posición de la habitación

                    GameObject newRoom = Instantiate(_rooms[randomRoom], position, Quaternion.identity); // Instancia la habitación
                    newRoom.name += $" {i}-{j}"; // Le asigna un nombre con su posición

                    RoomBehaviour rb = newRoom.GetComponent<RoomBehaviour>();
                    rb.UpdateRoom(currentCell.status); // Actualiza las paredes según el laberinto
                }
            }
        }
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // Verifica si la celda de arriba está disponible
        int up = Mathf.FloorToInt(cell - _dungeonSize.x);
        if (up >= 0 && !_board[up].visited) neighbors.Add(up);

        // Verifica abajo
        int down = Mathf.FloorToInt(cell + _dungeonSize.x);
        if (down < _board.Count && !_board[down].visited) neighbors.Add(down);

        // Verifica derecha
        if ((cell + 1) % _dungeonSize.x != 0 && !_board[cell + 1].visited) neighbors.Add(cell + 1);

        // Verifica izquierda
        if (cell % _dungeonSize.x != 0 && !_board[cell - 1].visited) neighbors.Add(cell - 1);

        return neighbors; // Devuelve la lista de vecinos disponibles
    }

    private void OnGUI()
    {
        // Botón en pantalla para regenerar el calabozo
        float w = Screen.width / 2;
        float h = Screen.height - 80;
        if (GUI.Button(new Rect(w, h, 250, 50), "Regenerate Dungeon"))
        {
            RegenerateDungeon();
        }
    }

    void RegenerateDungeon()
    {
        // Recarga la escena actual para generar un nuevo calabozo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
