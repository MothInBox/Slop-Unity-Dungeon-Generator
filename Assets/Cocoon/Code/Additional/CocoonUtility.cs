using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CocoonUtility
{
    public void placeRoom(GameObject roomPrefab, Vector3 position, Quaternion rotation)
    {
        GameObject.Instantiate(roomPrefab, position, rotation);
    }
    public void DestroyRoom(GameObject room)
    {
        GameObject.Destroy(room);
    }
}