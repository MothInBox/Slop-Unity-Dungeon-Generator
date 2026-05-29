using UnityEngine;
[System.Serializable]
public class RoomWeights
{
    public GameObject prefab;
    [Range(0, 100)] public int weight = 50;
    [Tooltip("Maximum number of this room prefab to be placed. 0 for unlimited.")]
    public int limit = 0; 
    //dont show to editor
    [HideInInspector]public int count = 0;

}