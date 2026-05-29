using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Settings")]
    public RoomType roomType;

    [Header("Connectors")]
    public Enter entranceNode;
    public Exit[] exitNodes;    
    private Room parentRoom = null;

    private void Awake()
    {
        if (entranceNode == null)
        {
            DebugHolder.LogWarning($"Room setup issue: '{gameObject.name}' is missing 'entranceNode'. Attempting auto-find in children.");
            entranceNode = GetComponentInChildren<Enter>();
        }

        if (exitNodes == null || exitNodes.Length == 0)
        {
            DebugHolder.LogWarning($"Room setup issue: '{gameObject.name}' has no configured 'exitNodes'. Attempting auto-find in children.");
            exitNodes = GetComponentsInChildren<Exit>();
        }
    }

    public Enter getEnterNode()
    {
        return entranceNode;
    }
    public Exit[] getExitNodes()
    {
        return exitNodes;
    }
    public void setParent(Room parent)
    {
        parentRoom = parent;
    }
    public Room getParent()
    {
        return parentRoom;
    }
    public bool isOverlapping()
    {
        BoxCollider bounds = GetComponent<BoxCollider>();
        if (bounds == null) return false;

        // Disable self so we don't detect our own collider
        bounds.enabled = false;

        Vector3 worldCenter = transform.TransformPoint(bounds.center);
        Collider[] hits = Physics.OverlapBox(
            worldCenter,
            bounds.size / 2,
            transform.rotation,
            Physics.AllLayers,
            QueryTriggerInteraction.Collide
        );

        bounds.enabled = true;

        foreach (Collider hit in hits)
        {
            Room hitRoom = hit.transform.root.GetComponent<Room>();
            if (hitRoom == null) continue;
            if (gameObject.name == hitRoom.gameObject.name) continue;
            if (hitRoom == parentRoom) continue; // ignore parent

            if (Generator.DebugModeStatic){
                DebugHolder.Log($"Placement blocked: '{gameObject.name}' overlaps '{hitRoom.gameObject.name}' via collider '{hit.gameObject.name}' type '{hit.gameObject.GetType().Name}'.", gameObject);
            }   
            return true;
        }

        return false;
    }


    private int depth = 0;
    public void SetDepth(int newDepth) { depth = newDepth; }
    public int GetDepth() { return depth; }


}
