using UnityEngine;
[System.Serializable]
public class RandomPrefabsTypeEntry
{
    public RoomType roomType;
    [Range(0, 100)] public int weight = 50;
    public RoomWeights[] prefabs;
    [Tooltip("Maximum number of this room type to be placed. 0 for unlimited.")]
    public int limit = 0; 
    //dont show to editor
    [HideInInspector]public int count = 0;
}