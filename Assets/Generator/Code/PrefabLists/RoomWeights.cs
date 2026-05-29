using UnityEngine;
[System.Serializable]
public class RoomWeights
{
    public GameObject prefab;
    [Range(0, 100)] public int weight = 50;

}