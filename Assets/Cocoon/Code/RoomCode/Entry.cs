using UnityEngine;
public class Entry : MonoBehaviour
{
    public (Quaternion, Vector3) GetTransform()
    {
        return (transform.rotation, transform.position);
    }
}