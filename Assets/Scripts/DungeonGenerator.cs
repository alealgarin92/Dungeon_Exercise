using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] Vector2 dungeonSize;
    [SerializeField] int startPos = 0;
    [SerializeField] GameObject[] rooms; // Prefabs de habitaciones.
    [SerializeField] Vector2 offset;

    private IRoomFactory roomFactory;
    private MazeGenerator mazeGenerator;
    private List<Cell> board;

    void Start()
    {
        // Inyección de dependencias: se crea la factory con los prefabs asignados.
        roomFactory = new RoomFactory(rooms);
        mazeGenerator = new MazeGenerator(dungeonSize, startPos);
        board = mazeGenerator.GenerateMaze();
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        int width = (int)dungeonSize.x;
        int height = (int)dungeonSize.y;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = Mathf.FloorToInt(i + j * width);

                if (index < board.Count)
                {
                    Cell currentCell = board[index];

                    if (currentCell.visited)
                    {
                        Vector3 roomPos = new Vector3(i * offset.x, 0f, -j * offset.y);
                        string roomName = "Room " + i + "-" + j;

                        // Utilizamos el Factory para instanciar la habitación.
                        roomFactory.CreateRoom(roomPos, currentCell.status, roomName);
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        float w = Screen.width / 2;
        float h = Screen.height - 80;
        if (GUI.Button(new Rect(w, h, 250, 50), "Regenerate Dungeon"))
        {
            RegenerateDungeon();
        }
    }

    void RegenerateDungeon()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
