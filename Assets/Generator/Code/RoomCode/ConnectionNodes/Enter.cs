using UnityEngine;

public class Enter : MonoBehaviour
{
    [Header("Settings")]
    public bool isConnected = false;
    private Exit connectedExit = null;

    //Getters and Setters
    public void Connect(Exit exit)
    {
        isConnected = true;
        connectedExit = exit;
    }
    public void Disconnect()
    {
        isConnected = false;
        connectedExit = null;
    }
    public Exit GetConnectedExit()
    {
        return connectedExit;
    }

    void OnDrawGizmos()
    {
        if (!Generator.showGizmosStatic) return;
        Gizmos.color = isConnected ? Color.blue : Color.green;

        // Draw a sphere at the connector position
        Gizmos.DrawSphere(transform.position, 0.3f);

        // Draw an arrow showing which direction the connector faces
        Gizmos.DrawRay(transform.position, transform.forward * 1.5f);
    }
}