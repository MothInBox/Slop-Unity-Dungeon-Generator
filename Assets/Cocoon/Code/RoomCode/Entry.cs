using UnityEngine;
public class Entry : MonoBehaviour
{
    [SerializeField] private EntryType entryType;
    private bool isConnected;
    private Exit myExitNode;

    public EntryType GetEntryType()
    {
        return entryType;
    }
    public (Quaternion, Vector3) GetTransform()
    {
        return (transform.rotation, transform.position);
    }

    private void OnDrawGizmos()
    {
        if (!CocoonLogger.doDrawGizmos()){return;}
        //Green if not connected, blue if connected
        Gizmos.color = isConnected ? Color.blue : Color.green;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
    public bool IsConnected()
    {
        return isConnected;
    }
    public void Connect(Exit exitNode)
    {
        if (entryType == EntryType.Spawn)
        {
            CocoonLogger.LogWarning("Attempting to connect to a spawn entry. This should not happen. Ignoring.");
            return;
        }
        if(isConnected){return;}
        myExitNode = exitNode;
        myExitNode.Connect(this);
        isConnected = true;

    }
    public void Disconnect()
    {
        if(!isConnected){return;}
        myExitNode.Disconnect();
        isConnected = false;
    }
}