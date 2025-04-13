// Controlador principal que conecta MazeGenerator y RoomRenderer
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DungeonGenerator;

public class DungeonManager : MonoBehaviour
{
    [Header("Configuración del Calabozo")]
    [SerializeField] private Vector2 tamañoCalabozo = new Vector2(5, 5);
    [SerializeField] private int celdaInicial = 0;

    [Header("Instanciación Visual")]
    [SerializeField] private GameObject[] prefabsHabitaciones;
    [SerializeField] private Vector2 separacion = new Vector2(10, 10);

    private List<Cell> tableroGenerado;

    void Start()
    {
        // Genera el laberinto lógicamente
        MazeGenerator generador = new MazeGenerator(tamañoCalabozo, celdaInicial);
        tableroGenerado = generador.Generate();

        // Renderiza las habitaciones en el mundo
        RoomRenderer renderizador = new RoomRenderer(prefabsHabitaciones, separacion, tamañoCalabozo);
        renderizador.Render(tableroGenerado);
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
