// Clase responsable de instanciar las habitaciones (visualización del calabozo)
using System.Collections.Generic;
using UnityEngine;

public class RoomRenderer
{
    private GameObject[] roomPrefabs;
    private Vector2 offset;
    private Vector2 dungeonSize;

    public RoomRenderer(GameObject[] rooms, Vector2 spacing, Vector2 size)
    {
        roomPrefabs = rooms;
        offset = spacing;
        dungeonSize = size;
    }

    public void Render(List<Cell> board)
    {
        for (int i = 0; i < dungeonSize.x; i++)
        {
            for (int j = 0; j < dungeonSize.y; j++)
            {
                int index = Mathf.FloorToInt(i + j * dungeonSize.x);
                Cell currentCell = board[index];

                if (currentCell.visited)
                {
                    int roomIndex = Random.Range(0, roomPrefabs.Length);
                    Vector3 position = new Vector3(i * offset.x, 0f, -j * offset.y);

                    GameObject newRoom = GameObject.Instantiate(roomPrefabs[roomIndex], position, Quaternion.identity);
                    newRoom.name += $" {i}-{j}";

                    RoomBehaviour rb = newRoom.GetComponent<RoomBehaviour>();
                    rb.UpdateRoom(currentCell.status);
                }
            }
        }
    }
}
