using UnityEngine;

public class RoomFactory : IRoomFactory
{
    private GameObject[] roomPrefabs;

    public RoomFactory(GameObject[] roomPrefabs)
    {
        this.roomPrefabs = roomPrefabs;
    }

    public GameObject CreateRoom(Vector3 position, bool[] status, string roomName)
    {
        int randomIndex = Random.Range(0, roomPrefabs.Length);
        GameObject roomInstance = Object.Instantiate(roomPrefabs[randomIndex], position, Quaternion.identity);
        roomInstance.name = roomName;

        // Se actualiza el comportamiento de la sala.
        RoomBehaviour rb = roomInstance.GetComponent<RoomBehaviour>();
        if (rb != null)
        {
            rb.UpdateRoom(status);
        }
        return roomInstance;
    }
}
