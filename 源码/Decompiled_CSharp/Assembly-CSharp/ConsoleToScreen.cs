using UnityEngine;

public class ConsoleToScreen : MonoBehaviour
{
	private const int maxLines = 50;

	private const int maxLineLength = 120;

	private string _logStr = "";

	private Vector2 _scrollPosition;

	public int fontSize = 15;

	private void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		string[] array = logString.Split('\n');
		foreach (string text in array)
		{
			if (text.Length <= 120)
			{
				_logStr = _logStr + text + "\n";
				continue;
			}
			for (int j = 0; j < text.Length; j += 120)
			{
				int length = Mathf.Min(120, text.Length - j);
				_logStr = _logStr + text.Substring(j, length) + "\n";
			}
		}
		if (_logStr.Split('\n').Length > 50)
		{
			_ = _logStr.Split('\n').Length;
			int num = _logStr.IndexOf('\n');
			_logStr = _logStr.Remove(0, num + 1);
		}
	}

	private void Update()
	{
		_scrollPosition.y = float.PositiveInfinity;
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(10f, 10f, (float)Screen.width - 20f, (float)Screen.height - 20f));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUIStyle.none, GUIStyle.none);
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.fontSize = fontSize;
		GUILayout.Label(_logStr, gUIStyle);
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}
