using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] private Exit[] exits;
    [SerializeField] private Entry entry;
}