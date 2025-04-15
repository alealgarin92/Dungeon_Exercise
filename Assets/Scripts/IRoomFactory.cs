using UnityEngine;

public interface IRoomFactory
{
    /// <summary>
    /// Crea e instancia una habitación (room) en la posición y con los parámetros indicados.
    /// </summary>
    GameObject CreateRoom(Vector3 position, bool[] status, string roomName);
}
