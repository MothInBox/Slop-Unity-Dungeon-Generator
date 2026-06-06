using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] private Exit[] exits;
    [SerializeField] private Entry entry;

    public Exit[] getExits()
    {
        return exits;
    }
    public Entry getEntry()
    {
        return entry;
    }
}