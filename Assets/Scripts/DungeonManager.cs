// Controlador principal que conecta MazeGenerator y RoomRenderer
using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class DungeonManager : MonoBehaviour
{
    [Header("Configuraci�n del Calabozo")]
    [SerializeField] private Vector2 tama�oCalabozo = new Vector2(5, 5);
    [SerializeField] private int celdaInicial = 0;

    [Header("Instanciaci�n Visual")]
    [SerializeField] private GameObject[] prefabsHabitaciones;
    [SerializeField] private Vector2 separacion = new Vector2(10, 10);

    private List<Cell> tableroGenerado;

    void Start()
    {
        // Genera el laberinto l�gicamente
        MazeGenerator generador = new MazeGenerator(tama�oCalabozo, celdaInicial);
        tableroGenerado = generador.Generate();

        // Renderiza las habitaciones en el mundo
        RoomRenderer renderizador = new RoomRenderer(prefabsHabitaciones, separacion, tama�oCalabozo);
        renderizador.Render(tableroGenerado);
    }
}
