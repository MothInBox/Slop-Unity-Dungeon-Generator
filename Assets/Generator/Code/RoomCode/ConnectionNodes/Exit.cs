using UnityEngine;

public class Exit : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] allowedWallPrefabs;
    public RoomType[] allowedTypePrefabs;
    public bool isConnected = false;
    private Enter connectedEnter = null;
    private bool isDeadEnd = false;


    //Getters and Setters
    public void Connect(Enter enter)
    {
        isConnected = true;
        connectedEnter = enter;
    }
    public void Disconnect()
    {
        isConnected = false;
        connectedEnter = null;
    }
    public void SetDeadEnd(bool value)
    {
        isDeadEnd = value;
    }
    public bool GetIsConnected()
    {        return isConnected;
    }
    public Enter GetConnectedEnter()
    {        return connectedEnter;
    }
    public bool GetIsDeadEnd()
    {        return isDeadEnd;
    }




    //Debug Gizmos
    void OnDrawGizmos()
    {
        if (!Generator.showGizmosStatic) return;
        Gizmos.color = isConnected ? Color.blue : Color.red;
        Gizmos.color = isDeadEnd ? Color.purple : Gizmos.color;

        // Draw a sphere at the connector position
        Gizmos.DrawSphere(transform.position, 0.3f);

        // Draw an arrow showing which direction the connector faces
        Gizmos.DrawRay(transform.position, transform.forward * 1.5f);
    }

}