using UnityEngine;

public class CocoonLogger : MonoBehaviour
{
    [SerializeField]public bool enableInfo = false; //enable for logging.
    [SerializeField]public bool enableWarning = false; //enable for logging.
    [SerializeField]public bool enableError = false; //enable for logging.

    private static CocoonLogger current;

    private void Awake()
    {
        if (current != null && current != this){
            Debug.LogWarning("Multiple instances of CocoonLogger detected. Only one instance should be active at a time.");
            return;
        }
        if (enableInfo || enableWarning || enableError)
        {
            LogInfo("Cocoon Logger Initialized. Enable Info: " + enableInfo + " | Enable Warning: " + enableWarning + " | Enable Error: " + enableError);
            current = this;
        }
    }

    public static void LogInfo(string message)
    {
        if (current != null && current.enableInfo)
        {
            current.LogInfoInternal(message);
        }
    }

    public static void LogWarning(string message)
    {
        if (current != null && current.enableWarning)
        {
            current.LogWarningInternal(message);
        }
    }

    public static void LogError(string message)
    {
        if (current != null && current.enableError)
        {
            current.LogErrorInternal(message);
        }
    }

    public static void LogException(string message, System.Exception ex)
    {
        if (current != null && current.enableError)
        {
            current.LogExceptionInternal(message, ex);
        }
    }

    private void LogInfoInternal(string message)
    {
        Debug.Log("[Cocoon Debug] : " + message);
    }

    private void LogWarningInternal(string message)
    {
        Debug.LogWarning("[Cocoon Debug] : " + message);
    }

    private void LogErrorInternal(string message)
    {
        Debug.LogError("[Cocoon Debug] :  " + message);
    }

    private void LogExceptionInternal(string message, System.Exception ex)
    {
        Debug.LogError("[Cocoon Debug] : " + message + " | Exception: " + ex.Message);
    }
}