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
        if (enableInfoLevel > 0 || enableWarningLevel > 0 || enableErrorLevel > 0 || enableGizmos)
        {
            LogInfo("Cocoon Logger Initialized. Enable Info: " + enableInfoLevel + " | Enable Warning: " + enableWarningLevel + " | Enable Error: " + enableErrorLevel + " | Enable Gizmos: " + enableGizmos, 1, "Logger", "Initialization");
        }
    }

    public static void LogInfo(string message, byte importance = 1, string source = "General", string category = "General")
    {
        if (current != null && importance <= current.enableInfoLevel)
        {
            current.LogInfoInternal(message, importance, source, category);
        }
    }

    public static void LogWarning(string message, byte importance = 1, string source = "General", string category = "General")
    {
        if (current != null && importance <= current.enableWarningLevel)
        {
            current.LogWarningInternal(message, importance, source, category);
        }
    }

    public static void LogError(string message, byte importance = 1, string source = "General", string category = "General")
    {
        if (current != null && importance <= current.enableErrorLevel)
        {
            current.LogErrorInternal(message, importance, source, category);
        }
    }

    public static void LogException(System.Exception ex, byte importance = 5, string source = "General", string category = "General")
    {
        if (current != null && importance <= current.enableErrorLevel)
        {
            current.LogExceptionInternal(ex, importance, source, category);
        }
    }
    
    public static bool doDrawGizmos()
    {
        return current != null ? current.enableGizmos : false;
    }

    private void LogInfoInternal(string message, byte importance, string source, string category)
    {
        Debug.Log(BuildPrefix(importance, source, category) + message);
    }

    private void LogWarningInternal(string message, byte importance, string source, string category)
    {
        Debug.LogWarning(BuildPrefix(importance, source, category) + message);
    }

    private void LogErrorInternal(string message, byte importance, string source, string category)
    {
        Debug.LogError(BuildPrefix(importance, source, category) + message);
    }

    private void LogExceptionInternal(System.Exception ex, byte importance, string source, string category)
    {
        Debug.LogError(BuildPrefix(importance, source, category) + "Exception: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
    }

    private string BuildPrefix(byte importance, string source, string category)
    {
        return "<color=" + GetImportanceColor(importance) + ">[Cocoon Debug | I" + importance + " | Frame " + Time.frameCount + " | Time " + Time.timeSinceLevelLoad.ToString("F3") + " | Source " + source + " | Category " + category + "]</color> ";
    }

    private string GetImportanceColor(byte importance)
    {
        switch (importance)
        {
            case 1:
                return "#FF3B30";
            case 2:
                return "#FF9500";
            case 3:
                return "#FFD60A";
            case 4:
                return "#34C759";
            case 5:
                return "#8E8E93";
            default:
                return "#FFFFFF";
        }
    }

}