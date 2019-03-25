using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerLog : MonoBehaviour
{
	// Private VARS
	private List<string> Eventlog = new List<string>();
	private new string guiText = "";
	[SerializeField]
	private Text eventLog;


	// Public VARS
	public int maxLines = 8;


	public void AddEvent(string eventString)
	{
		Eventlog.Add(eventString);

		if (Eventlog.Count >= maxLines)
		{
			Eventlog.RemoveAt(0);
		}

		guiText = "";
		foreach (string logEvent in Eventlog)
		{
			guiText += logEvent;
			guiText += "\n";
		}
		eventLog.text = guiText;
	}

	public void Start()
	{
		eventLog = GameObject.Find("ConsoleWindowText").GetComponent<Text>();
	}

}