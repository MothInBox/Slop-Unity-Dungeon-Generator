using UnityEngine;
[System.Serializable]
public class RoomWeights
{
    public GameObject prefab;
    [Range(0, 100)] public int weight = 50;
    public int limit = 0; 
    //dont show to editor
    [HideInInspector]public int count = 0;

}