using UnityEngine;

[System.Serializable]
public class Gate
{
    [SerializeField] private GameObject GatePrefab;
    [SerializeField][Range(0, 255)] private byte Weight;

    public byte getWeight()
    {
        return Weight;
    }
    public GameObject getGatePrefab()
    {
        return GatePrefab;
    }
}