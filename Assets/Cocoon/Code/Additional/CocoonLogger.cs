using UnityEngine;

public class CocoonLogger : MonoBehaviour
{
    [SerializeField][Range(0, 5)]public byte enableInfoLevel = 0; //enable for logging.
    [SerializeField][Range(0, 5)]public byte enableWarningLevel = 0; //enable for logging.
    [SerializeField][Range(0, 5)]public byte enableErrorLevel = 0; //enable for logging.
    [SerializeField]public bool enableGizmos = false; //enable for logging.

    private static CocoonLogger current;

    private void Awake()
    {
        if (current != null && current != this){
            Debug.LogWarning("Multiple instances of CocoonLogger detected. Only one instance should be active at a time.");
            return;
        }
        current = this;
        if (enableInfoLevel || enableWarningLevel || enableErrorLevel || enableGizmos)
        {
            LogInfo("Cocoon Logger Initialized. Enable Info: " + enableInfoLevel + " | Enable Warning: " + enableWarningLevel + " | Enable Error: " + enableErrorLevel + " | Enable Gizmos: " + enableGizmos, 1);
        }
    }

    public static void LogInfo(string message, byte importance = 1)
    {
        if (current != null && importance <= current.enableInfoLevel)
        {
            current.LogInfoInternal(message, importance);
        }
    }

    public static void LogWarning(string message, byte importance = 1)
    {
        if (current != null && importance <= current.enableWarningLevel)
        {
            current.LogWarningInternal(message, importance);
        }
    }

    public static void LogError(string message, byte importance = 1)
    {
        if (current != null && importance <= current.enableErrorLevel)
        {
            current.LogErrorInternal(message, importance);
        }
    }

    public static void LogException(System.Exception ex, byte importance = 5)
    {
        if (current != null && importance <= current.enableErrorLevel)
        {
            current.LogExceptionInternal(ex, importance);
        }
    }
    
    public static bool doDrawGizmos()
    {
        return current != null ? current.enableGizmos : false;
    }

    private void LogInfoInternal(string message, byte importance)
    {
        Debug.Log(BuildPrefix(importance) + message);
    }

    private void LogWarningInternal(string message, byte importance)
    {
        Debug.LogWarning(BuildPrefix(importance) + message);
    }

    private void LogErrorInternal(string message, byte importance)
    {
        Debug.LogError(BuildPrefix(importance) + message);
    }

    private void LogExceptionInternal(System.Exception ex, byte importance)
    {
        Debug.LogError(BuildPrefix(importance) + "Exception: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
    }

    private string BuildPrefix(byte importance)
    {
        return "[Cocoon Debug | Importance " + importance + " | Frame " + Time.frameCount + " | Time " + Time.timeSinceLevelLoad.ToString("F3") + "] ";
    }

}