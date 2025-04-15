using UnityEngine;

public interface IRoomFactory
{
    /// <summary>
    /// Crea e instancia una habitaci�n (room) en la posici�n y con los par�metros indicados.
    /// </summary>
    GameObject CreateRoom(Vector3 position, bool[] status, string roomName);
}
