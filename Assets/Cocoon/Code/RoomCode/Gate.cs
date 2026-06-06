using UnityEngine;

[System.Serializable]
public class Gate : IWeighted
{
    [SerializeField] private GameObject GatePrefab;
    [SerializeField][Range(0, 255)] private byte Weight;

    public int getWeight()
    {
        return (int)Weight;
    }
    public GameObject getGatePrefab()
    {
        return GatePrefab;
    }
}