using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.CompilerServices;

public static class DebugHolder
{
	public static void Log(
		object message,
		Object context = null,
		[CallerMemberName] string callerMember = "",
		[CallerFilePath] string callerFile = "",
		[CallerLineNumber] int callerLine = 0)
	{
		if (!Generator.DebugModeStatic) return;
		string payload = FormatPayload("LOG", message, context, callerMember, callerFile, callerLine);
		if (context != null)
		{
			Debug.Log(payload, context);
		}
		else
		{
			Debug.Log(payload);
		}
	}

	public static void LogWarning(
		object message,
		Object context = null,
		[CallerMemberName] string callerMember = "",
		[CallerFilePath] string callerFile = "",
		[CallerLineNumber] int callerLine = 0)
	{
		if (!Generator.DebugModeStatic) return;
		string payload = FormatPayload("WARN", message, context, callerMember, callerFile, callerLine);
		if (context != null)
		{
			Debug.LogWarning(payload, context);
		}
		else
		{
			Debug.LogWarning(payload);
		}
	}

	public static void LogError(
		object message,
		Object context = null,
		[CallerMemberName] string callerMember = "",
		[CallerFilePath] string callerFile = "",
		[CallerLineNumber] int callerLine = 0)
	{
		if (!Generator.DebugModeStatic) return;
		string payload = FormatPayload("ERROR", message, context, callerMember, callerFile, callerLine);
		if (context != null)
		{
			Debug.LogError(payload, context);
		}
		else
		{
			Debug.LogError(payload);
		}
	}

	private static string FormatPayload(
		string level,
		object message,
		Object context,
		string callerMember,
		string callerFile,
		int callerLine)
	{
		string sceneName = SceneManager.GetActiveScene().name;
		string contextPath = GetContextPath(context);
		string fileName = Path.GetFileName(callerFile);
		string timeInfo = Time.realtimeSinceStartup.ToString("F3") + "s";

		return $"[{level}] [t={timeInfo}] [frame={Time.frameCount}] [scene={sceneName}] [context={contextPath}] [at={fileName}:{callerLine}::{callerMember}] {message}";
	}

	private static string GetContextPath(Object context)
	{
		if (context == null)
		{
			return "none";
		}

		if (context is GameObject gameObject)
		{
			return GetTransformPath(gameObject.transform);
		}

		if (context is Component component)
		{
			return GetTransformPath(component.transform) + "/" + component.GetType().Name;
		}

		return context.name;
	}

	private static string GetTransformPath(Transform transform)
	{
		if (transform == null)
		{
			return "none";
		}

		string path = transform.name;
		while (transform.parent != null)
		{
			transform = transform.parent;
			path = transform.name + "/" + path;
		}

		return path;
	}
}
